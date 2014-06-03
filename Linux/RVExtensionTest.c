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
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "Arma2Net.h"

#define OUTPUT_SIZE 20000

int main(void)
{
	while (true)
	{
		char *function = NULL;
		size_t length = 0;

		printf("Enter function name: ");
		if (getline(&function, &length, stdin) == -1)
			return 0;

		for (size_t i = 0; i < length; i++)
			if (function[i] == '\n')
				function[i] = 0;

		length = strlen(function);

		if (length > 0)
		{
			char output[OUTPUT_SIZE] = { 0 };
			RVExtension(output, OUTPUT_SIZE, function);

			printf("%s\n", output);
		}

		free(function);
	}
	return 0;
}
