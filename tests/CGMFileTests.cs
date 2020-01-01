using FluentAssertions;
using NUnit.Framework;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    class CGMFileTests : CGMTest
    {
        [Ignore("Not yet ready")]
        [TestCase("col_disassembly.cgm", "C1352151M101901MU00")]        
        public void GetGraphicName(string fileName, string expectedName)
        {
            var binaryFile = ReadBinaryFile(fileName);
            var actualName = binaryFile.GetGraphicName();

            Assert.AreEqual(expectedName, actualName);
        }

        [Ignore("Not yet ready")]
        [TestCase("items.cgm", new[] { "1", "2", "3", "4", "5" })]        
        public void GetFigureItemTexts(string fileName, string[] expectedItems)
        {
            var binaryFile = ReadBinaryFile(fileName);
            var actualItems = binaryFile.GetFigureItemTexts(true);
            actualItems.Sort();

            actualItems.Should().Equal(expectedItems);            
        }
    }
}
