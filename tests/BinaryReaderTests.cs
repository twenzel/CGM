using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    class BinaryReaderTests : CgmTest
    {
        [Test]
        public void ReadBinaryFiles()
        {
            var assembly = this.GetType().Assembly;

            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(".cgm", StringComparison.OrdinalIgnoreCase))
                {
                    BinaryCgmFile binaryFile = ReadBinaryFile(name, assembly);

                    Assert.AreEqual(0, binaryFile.Messages.Count(), "Messages: " + string.Join("\r\n", binaryFile.Messages.Select(m => m.ToString())));
                }
            }
        }
 
    }
}
