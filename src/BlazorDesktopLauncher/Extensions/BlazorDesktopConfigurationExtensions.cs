/*
	MIT License

	Copyright (c) 2020 Y56380X

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlazorDesktopLauncher.Extensions
{
	public static class BlazorDesktopConfigurationExtensions
	{
		private static readonly Dictionary<PlatformID, string[]> PlatformBrowsers = new Dictionary<PlatformID, string[]>
		{
			{ PlatformID.Win32NT, new []{ @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe" }},
			{ PlatformID.Unix, new []{ @"/usr/bin/chromium", @"/snap/bin/chromium", @"/usr/bin/vivaldi" }}
		};
		
		public static void UseLocalBrowser(this IBlazorDesktopConfiguration config)
		{
			static bool TryGetExecutablePath(out string? path)
			{
				path = PlatformBrowsers.TryGetValue(Environment.OSVersion.Platform, out var paths)
					? paths.FirstOrDefault(File.Exists)
					: null;
				
				return path != null;
			}

			config.BrowserExecutablePath = TryGetExecutablePath(out var path) ? path : string.Empty;
		}
	}
}