/*
* Copyright 2014 Arma2NET Developers
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

#include <stdbool.h>
#include <stdlib.h>
#include <string.h>
#include "Arma2Net.h"
#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/debug-helpers.h>

static MonoDomain *domain = NULL;
static MonoImage *image = NULL;

typedef MonoString *(*InvokeAddinThunk)(MonoString *str, int *maxResultSize);
static InvokeAddinThunk invokeAddin = NULL;

static bool initialized = false;

static void Initialize(void)
{
	domain = mono_jit_init_version("Default", "v4.0.30319");
	if (!domain)
	{
		printf("mono_jit_init_version failed\n");
		exit(10001);
	}

	MonoAssembly *assembly = mono_domain_assembly_open(domain, "Arma2Net.Addins.dll");
	if (!assembly)
	{
		printf("mono_domain_assembly_open failed\n");
		exit(10002);
	}

	image = mono_assembly_get_image(assembly);
	if (!image)
	{
		printf("mono_assembly_get_image failed\n");
		exit(10003);
	}

	MonoMethodDesc *desc = mono_method_desc_new("Mono:Initialize()", false);
	if (!desc)
	{
		printf("mono_method_desc_new failed for Mono:Initialize\n");
		exit(10004);
	}

	MonoMethod *method = mono_method_desc_search_in_image(desc, image);
	if (!method)
	{
		printf("mono_method_desc_search_in_image failed for Mono:Initialize\n");
		exit(10005);
	}

	MonoObject *exception = NULL;
	/*MonoObject *result = */mono_runtime_invoke(method, NULL, NULL, &exception);
	if (exception)
	{
		printf("mono_runtime_invoke failed for Mono:Initialize due to an exception\n");
		exit(10006);
	}

	mono_method_desc_free(desc);

	desc = mono_method_desc_new("AddinManager:InvokeAddin(string,int)", false);
	if (!desc)
	{
		printf("mono_method_desc_new failed for AddinManager:InvokeAddin\n");
		exit(10007);
	}

	method = mono_method_desc_search_in_image(desc, image);
	if (!method)
	{
		printf("mono_method_desc_search_in_image failed for AddinInManager:InvokeAddin\n");
		exit(10008);
	}

	invokeAddin = mono_method_get_unmanaged_thunk(method);
}

__attribute__((__visibility__("default"))) void RVExtension(char *output, int outputSize, const char *function)
{
	if (!initialized)
	{
		initialized = true;
		Initialize();
	}

	MonoString *str = mono_string_new(domain, function);
	int maxResultSize = outputSize - 1;

	MonoString *result = invokeAddin(str, &maxResultSize);

	if (result != NULL)
	{
		char *resultChars = mono_string_to_utf8(result);
		strncpy(output, resultChars, outputSize);
		free(resultChars);
	}
}

