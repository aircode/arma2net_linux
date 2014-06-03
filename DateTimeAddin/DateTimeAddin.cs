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

using System.Globalization;
using Arma2Net.Addins;
using SDateTime = System.DateTime;

namespace DateTimeAddin
{
	[Addin("DateTime", Author = "Scott_NZ", Description = "Date and time utilities")]
	public class DateTime : Addin
	{
		public override string Invoke(string args, int maxResultSize)
		{
			var split = (args ?? "").Split(',');
			var dateTime = split[0] == "now" ? SDateTime.Now : SDateTime.UtcNow;
			var format = split.Length > 1 ? split[1] : "G";
			return dateTime.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
