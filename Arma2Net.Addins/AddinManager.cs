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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arma2Net.Addins
{
	public static class AddinManager
	{
		static readonly Dictionary<string, Addin> loadedAddins = new Dictionary<string, Addin>(StringComparer.OrdinalIgnoreCase);

		static IEnumerable<string> EnumerateAddinFiles()
		{
			foreach (var directory in Directory.EnumerateDirectories(Utils.AddinDirectory))
			{
				foreach (var file in Directory.EnumerateFiles(directory, "*.dll"))
					yield return file;
			}
		}

		static List<AddinInfo> FindAddins(Assembly assembly)
		{
			var addins = new List<AddinInfo>();
			foreach (var type in assembly.GetTypes())
			{
				var attributes = type.GetCustomAttributes(typeof(AddinAttribute), true);
				if (!attributes.Any())
					continue;

				addins.Add(new AddinInfo { Type = type, Attribute = (AddinAttribute)attributes[0] });
			}
			return addins;
		}

		public static void LoadAddins()
		{
			foreach (var file in EnumerateAddinFiles())
			{
				Utils.Log("Loading addin {0}", file);
				var assembly = Assembly.LoadFile(file);
				var addins = FindAddins(assembly);

				foreach (var addin in addins)
				{
					Utils.Log("Found addin {0}", addin.Type);

					var a = addin.Attribute;
					if (a.Name == null)
					{
						Utils.Log("No addin name for {0} specified; ignoring", addin.Type);
						continue;
					}

					Utils.Log("Name = {0}, Version = {1}, Author = {2}, Description = {3}", a.Name, a.Version, a.Author, a.Description);

					try
					{
						var obj = (Addin)Activator.CreateInstance(addin.Type);
						loadedAddins.Add(a.Name, obj);
					}
					catch (Exception e)
					{
						Utils.Log("Failed to load addin {0}", a.Name);
						Utils.Log(e.ToString());
					}
				}
			}
		}

		public static string InvokeAddin(string str, int maxResultSize)
		{
			if (string.IsNullOrEmpty(str))
				return null;

			var split = str.Split(new[] { ' ' }, 2);

			var name = split[0];
			var args = split.Length > 1 ? split[1] : null;

			Addin addin;
			if (!loadedAddins.TryGetValue(name, out addin))
				throw new InvalidOperationException(string.Format("Unable to locate addin {0}", name));

			if (addin.InvocationMethod == null)
				throw new InvalidOperationException(string.Format("No invocation method object set for addin {0}", name));

			string result = null;
			try
			{
				result = addin.InvocationMethod.Invoke(args, maxResultSize);
				if (result == null)
					return null;
			}
			catch (Exception e)
			{
				Utils.Log("Failed to invoke addin {0}", str);
				Utils.Log(e.ToString());
				return string.Format("throw \"{0}\"", e.GetType());
			}

			var resultSize = Encoding.UTF8.GetByteCount(result);
			if (resultSize > maxResultSize)
			{
				Utils.Log("Failed to return a result for addin {0} because it is too long ({0} > {1})", str, resultSize, maxResultSize);
				return "throw \"ResultTooLong\"";
			}

			return result;
		}
	}
}