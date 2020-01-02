using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    class ClearTextTests : CGMTest
    {
        [Test]
        public void CompareFiles()
        {
            var assembly = this.GetType().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var name in resourceNames)
            {
                if (name.EndsWith(".cgm", StringComparison.OrdinalIgnoreCase))
                {
                    var comparisionFile = GetComparisionFileName(name);

                    if (resourceNames.Contains(comparisionFile))
                    {
                        var binaryFile = ReadBinaryFile(name, assembly);
                        var actual = ConvertToClearText(binaryFile);
                        var expected = ReadExpectedTextContent(comparisionFile, assembly);

                        Assert.AreEqual(0, binaryFile.Messages.Count());


                        //StringAssert.AreEqualIgnoringCase(RemoveCompareExceptions(expected), RemoveCompareExceptions(actual), $"File {name} differs!");
                    }
                }
            }
        }

        [Ignore("Not yet ready")]
        [Test]
        public void ConvertFiles_ToClearText()
        {
            var assembly = this.GetType().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var name in resourceNames)
            {
                if (name.EndsWith(".cgm", StringComparison.OrdinalIgnoreCase))
                {
                    var binaryFile = ReadBinaryFile(name, assembly);
                    var content = ConvertToClearText(binaryFile);

                    Assert.IsNotEmpty(content, $"File {name} is could not be converted!");
                }
            }
        }

        private string RemoveCompareExceptions(string value)
        {
            return value.Replace("\n", "").Replace(" ", "").Replace("-0.0000", "0.0000");
        }

        private string ReadExpectedTextContent(string resourceName, Assembly assembly)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private string GetComparisionFileName(string resourceName)
        {
            return Path.ChangeExtension(resourceName, ".txt");
        }
    }
}
