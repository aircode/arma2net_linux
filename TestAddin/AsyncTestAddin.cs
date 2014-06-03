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

using System;
using Arma2Net.Addins;

namespace TestAddin
{
	[Addin("AsyncTest")]
	public class AsyncTestAddin : Addin
	{
		public AsyncTestAddin()
		{
			InvocationMethod = new AsyncAddinInvocationMethod(this);
		}

		public override string Invoke(string args, int maxResultSize)
		{
			if (args != null)
			{
				if (args.StartsWith("return ", StringComparison.OrdinalIgnoreCase))
					return args.Substring(args.IndexOf(' ') + 1);

				if (args.Equals("throw", StringComparison.OrdinalIgnoreCase))
					throw new Exception("Test");
			}

			return "Test";
		}
	}
}
