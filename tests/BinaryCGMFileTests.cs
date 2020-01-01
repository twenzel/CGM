using codessentials.CGM.Commands;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    class BinaryCGMFileTests : CGMTest
    {
        [Ignore("Not yet ready")]
        [Test]        
        public void ReadBinary_WriteAsBinary_ReadBinaryAgain_And_Compare()
        {
            var assembly = GetType().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var name in resourceNames)
            {
                if (name.EndsWith(".cgm", StringComparison.OrdinalIgnoreCase))
                {
                    var binaryFile = ReadBinaryFile(name, assembly);
                    var expected = ConvertToClearText(binaryFile);

                    using var stream = new MemoryStream(binaryFile.GetContent());
                    var newBinary = new BinaryCGMFile(stream);
                    var actual = ConvertToClearText(newBinary);

                    actual.Should().Be(expected, "In file " + name);
                }
            }
        }

        [Test]
        public void TestDedicatedFile()
        {
            var assembly = GetType().Assembly;

            var binaryFile = ReadBinaryFile("1STPRIZE.CGM", assembly);
            var expected = ConvertToClearText(binaryFile);

            using (var stream = new MemoryStream(binaryFile.GetContent()))
            {
                var newBinary = new BinaryCGMFile(stream);
                var actual = ConvertToClearText(newBinary);

                actual.Should().Be(expected);
            }
        }

        [TestCase("Any")]
        [TestCase("LessFour")]
        [TestCase("LessEight")]
        [TestCase("LessEight1")]
        [TestCase("MoreDescribingText")]
        [TestCase("ALittleMoreTextThanTheTestBefore")]
        [TestCase("ATextWithAVeryLongContentByDoublingAllSentencesLikeATextWithAVeryLongContentBy")]
        public void Floating_String_Test(string data)
        {
            var writer = new CGMWriter(FileFormat.Binary, "");            
            writer.SetDescription(data);
            writer.Finish();

            var binaryFile = new BinaryCGMFile(new MemoryStream(writer.GetContent()));
            var actual = ConvertToClearText(binaryFile);
            var expected = new StringBuilder();
            expected.AppendLine("BEGMF '';");
            expected.AppendLine(" mfversion 1;");
            expected.AppendLine($" mfdesc '{data}';");
            expected.AppendLine("ENDMF;");

            Assert.AreEqual(expected.ToString().Replace("\r\n", "\n"), actual);
        }

        [Test]
        public void ComposeTest()
        {
            var writer = new CGMWriter(FileFormat.Binary, "");           
            writer.SetDescription("Created By UnitTest");
            writer.SetElementList("DRAWINGPLUS");
            writer.SetFontList(new[] { "Arial", "Arial Bold" });
            writer.SetCharacterSetList(new[] { new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._94_CHAR_G_SET, "B"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._96_CHAR_G_SET, "A"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "I"), new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "L") });
            writer.SetVDCType(VDCType.Type.Real);
            writer.Finish();

            var binaryFile = new BinaryCGMFile(new MemoryStream(writer.GetContent()));
            var actual = ConvertToClearText(binaryFile);
            var expected = new StringBuilder();
            expected.AppendLine("BEGMF '';");
            expected.AppendLine(" mfversion 1;");
            expected.AppendLine(" mfdesc 'Created By UnitTest';");
            expected.AppendLine(" mfelemlist 'DRAWINGPLUS';");
            expected.AppendLine(" fontlist 'Arial', 'Arial Bold';");
            expected.AppendLine(" charsetlist STD94 'B' STD96 'A' COMPLETECODE 'I' COMPLETECODE 'L';");
            expected.AppendLine(" vdctype real;");
            //expected.AppendLine(" colrprec 255;");
            //expected.AppendLine(" colrindexprec 127;");
            //expected.AppendLine(" colrvalueext 0 0 0, 255 255 255;");
            //expected.AppendLine(" maxcolrindex 255;");
            //expected.AppendLine(" integerprec - 32768, 32767 % 16 binary bits %;");
            //expected.AppendLine(" realprec - 511.0000, 511.0000, 7 % 10 binary bits %;");
            //expected.AppendLine(" charcoding BASIC8BIT;");
            expected.AppendLine("ENDMF;");

            Assert.AreEqual(expected.ToString().Replace("\r\n", "\n"), actual);
        }
    }
}
