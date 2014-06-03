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
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Arma2Net.Addins
{
	public class AsyncAddinInvocationMethod : IAddinInvocationMethod
	{
		readonly Addin addin;
		readonly bool storeResults;
		ConcurrentQueue<string> results = new ConcurrentQueue<string>();
		Exception exception;
		readonly object syncRoot = new object();

		public AsyncAddinInvocationMethod(Addin addin, bool storeResults = true)
		{
			this.addin = addin;
			this.storeResults = storeResults;
		}

		public string Invoke(string args, int maxResultSize)
		{
			lock (syncRoot)
			{
				if (exception != null)
				{
					var e = exception;
					exception = null;
					throw e;
				}
			}
			if (args != null)
			{
				if (args.Equals("getresult", StringComparison.OrdinalIgnoreCase))
				{
					string result;
					results.TryDequeue(out result);
					return result;
				}
				if (args.Equals("clearresults", StringComparison.OrdinalIgnoreCase))
				{
					results = new ConcurrentQueue<string>();
					return null;
				}
			}
			var task = new Task(() =>
			{
				var result = addin.Invoke(args, maxResultSize);
				if (storeResults)
					results.Enqueue(result);
			});
			task.ContinueWith(t =>
			{
				t.Exception.Handle(e =>
				{
					lock (syncRoot)
						exception = e;
					return true;
				});
			}, TaskContinuationOptions.OnlyOnFaulted);
			task.Start();
			return null;
		}
	}
}