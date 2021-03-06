#region Copyright (c) 2007 Ryan Williams <drcforbin@gmail.com>
/// <copyright>
/// Copyright (c) 2007 Ryan Williams <drcforbin@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// </copyright>
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Obfuscar;

namespace ObfuscarTests
{
	[TestFixture]
	public class PathFailureTests
	{
		internal const string BadPath = "Q:\\Does\\Not\\Exist";

		[Test]
		public void CheckBadPathIsBad ()
		{
			// if badPath exists, the other tests here are no good
			Assert.IsFalse (System.IO.Directory.Exists (BadPath), "Didn't expect BadPath to exist.");
		}

		[Test]
		public void CheckBadProjectPath ()
		{
            var exception = Assert.Throws<ObfuscarException>(() => { new Obfuscator(BadPath); });
            Assert.AreEqual("Unable to read specified project file:  Q:\\Does\\Not\\Exist", exception.Message);
		}

		[Test]
		public void CheckBadModuleFile ()
		{
			string xml = String.Format (
				             @"<?xml version='1.0'?>" +
				             @"<Obfuscator>" +
				             @"<Module file='{0}\ObfuscarTests.dll' />" +
				             @"</Obfuscator>", BadPath);
            var exception = Assert.Throws<ObfuscarException>(() => { Obfuscator.CreateFromXml(xml); });
            Assert.AreEqual("Unable to find assembly:  Q:\\Does\\Not\\Exist\\ObfuscarTests.dll", exception.Message);
		}

		[Test]
		public void CheckBadInPath ()
		{
			string xml = String.Format (
				             @"<?xml version='1.0'?>" +
				             @"<Obfuscator>" +
				             @"<Var name='InPath' value='{0}' />" +
				             @"</Obfuscator>", BadPath);
            var exception = Assert.Throws<ObfuscarException>(() => { Obfuscator.CreateFromXml(xml); });
            Assert.AreEqual("Path specified by InPath variable must exist:Q:\\Does\\Not\\Exist", exception.Message);
		}
	}
}
