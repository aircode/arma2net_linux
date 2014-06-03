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
using System.Reflection;

namespace Arma2Net.Addins
{
	public static class Mono
	{
		static Dictionary<string, Assembly> assemblyCache = new Dictionary<string, Assembly>();

		static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			var requestedAssembly = new AssemblyName(e.Name);
			Utils.Log("Resolving assembly {0}", requestedAssembly.Name);

			Assembly assembly;
			if (assemblyCache.TryGetValue(requestedAssembly.FullName, out assembly))
			{
				Utils.Log("Retrieved assembly {0} from assembly cache", requestedAssembly.Name);
				return assembly;
			}

			var extensions = new[] { ".dll", ".exe" };

			var directories = new List<string>();
			directories.Add(Utils.BaseDirectory); // for our own assemblies
			directories.AddRange(Directory.EnumerateDirectories(Utils.AddinDirectory)); // for a dependency in an addin folder

			foreach (var directory in directories)
			{
				foreach (var extension in extensions)
				{
					try
					{
						var filename = Path.Combine(directory, requestedAssembly.Name + extension);
						Utils.Log("Loading assembly {0} from {1}", requestedAssembly.Name, filename);
						assembly = Assembly.LoadFrom(filename);
						assemblyCache.Add(requestedAssembly.FullName, assembly);
						return assembly;
					}
					catch { }
				}
			}
			return null;
		}

		static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject;
			Utils.Log("Fatal unhandled exception of type {0}", ex.GetType());
			Utils.Log(ex.ToString());
		}

		public static void Initialize()
		{
			Utils.Log("Arma2NET initializing on CLR {0}", Environment.Version);
			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
			AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
			Utils.Log("Loading addins");
			try
			{
				AddinManager.LoadAddins();
			}
			catch (Exception e)
			{
				Utils.Log("Failed to load addins");
				Utils.Log(e.ToString());
			}
		}
	}
}