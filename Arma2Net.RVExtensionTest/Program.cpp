/*
* Copyright 2013 Arma2NET Developers
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

#include <iostream>
#include <string>
#include "..\Arma2Net\RVExtension.h"

#define OUTPUT_SIZE 20000

int main(void)
{
	while (true)
	{
		std::string functionString;
		std::cout << "Enter function name: ";
		std::getline(std::cin, functionString);
		if (functionString.length() == 0)
			continue;

		const char* function = functionString.c_str();
		char output[OUTPUT_SIZE] = { 0 };
		RVExtension(output, OUTPUT_SIZE, function);

		std::cout << output << std::endl;
	}
}