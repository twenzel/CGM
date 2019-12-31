using FluentAssertions;
using codessentials.CGM.Commands;
using codessentials.CGM.Export;
using codessentials.CGM.Import;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    public class BinaryWriterReaderTests
    {
        protected DefaultBinaryReader _reader;
        protected DefaultBinaryWriter _writer;
        protected MemoryStream _stream;
        protected Mock<ICommandFactory> _commandFactory;
        
        [SetUp]
        public void Setup()
        {
            _stream = new MemoryStream();
            var cgm = new BinaryCGMFile();
            _commandFactory = new Mock<ICommandFactory>();

            _writer = new DefaultBinaryWriter(_stream, cgm);
            _reader = new DefaultBinaryReader(_stream, cgm, _commandFactory.Object);
        } 

        [Test]
        public void Bool()
        {
            Test(w => w.WriteBool(true), r => _reader.ReadBool().Should().Be(true));
            Test(w => w.WriteBool(false), r => _reader.ReadBool().Should().Be(false));
        }

        [Test]
        public void FixedString()
        {
            Test(w => w.WriteFixedString("test"), r => _reader.ReadFixedString().Should().Be("test"));            
        }

        [Test]
        public void FixedString_Long()
        {
            var testString = "".PadLeft(10000, 'a');
            Test(w => w.WriteFixedString(testString), r => {
                var actual = _reader.ReadFixedString();
                actual.Length.Should().Be(testString.Length);
                actual.Should().Be(testString);
            });
        }

        //[Test]
        //public void FixedString_Very_Long()
        //{
        //    var testString = "".PadLeft(10000, 'a').PadLeft(20000, 'b').PadLeft(30000, 'c').PadLeft(40000, 'd').PadLeft(50000, 'e').PadLeft(60000, 'f'); 
        //    Test(w => w.WriteFixedString(testString), r => {
        //        var actual = _reader.ReadFixedString();
        //        actual.Length.Should().Be(testString.Length);
        //        actual.Should().Be(testString);
        //    });
        //}

        [Test]
        public void String()
        {
            Test(w => w.WriteString("test"), r => _reader.ReadString().Should().Be("test"));
        }

        [Test]
        public void String_Long()
        {
            var testString = "".PadLeft(10000, 'a');
            Test(w => w.WriteString(testString), r => {
                var actual = _reader.ReadString();
                actual.Length.Should().Be(testString.Length);
                actual.Should().Be(testString);
            });
        }

        //[Test]
        //public void String_Very_Long()
        //{
        //    var testString = "".PadLeft(10000, 'a').PadLeft(20000, 'b').PadLeft(30000, 'c').PadLeft(40000, 'd').PadLeft(50000, 'e').PadLeft(60000, 'f'); 
        //    Test(w => w.WriteString(testString), r => {
        //        var actual = _reader.ReadString();
        //        actual.Length.Should().Be(testString.Length);
        //        actual.Should().Be(testString);
        //    });
        //}

        [Test]
        public void UInt1_1()
        {
            Test(w => w.WriteUInt(1, 1), r => _reader.ReadUInt(1).Should().Be(1));            
        }

        [Test]
        public void UInt1_0()
        {            
            Test(w => w.WriteUInt(0, 1), r => _reader.ReadUInt(1).Should().Be(0));
        }

        [Test]
        public void UInt2()
        {
            Test(w => w.WriteUInt(2, 2), r => _reader.ReadUInt(2).Should().Be(2));            
        }

        [Test]
        public void UInt2_1()
        {            
            Test(w => w.WriteUInt(1, 2), r => _reader.ReadUInt(2).Should().Be(1));
        }

        [Test]
        public void UInt4_14()
        {
            Test(w => w.WriteUInt(14, 4), r => { _reader.ReadUInt(4).Should().Be(14); });
        }

        [Test]
        public void UInt4_13()
        {           
            Test(w => w.WriteUInt(13, 4), r => { _reader.ReadUInt(4).Should().Be(13); });            
        }

        [Test]
        public void UInt4_5()
        {            
            Test(w => w.WriteUInt(5, 4), r => { _reader.ReadUInt(4).Should().Be(5); });
        }

        [Test]
        public void UInt8()
        {
            Test(w => w.WriteUInt(55, 8), r => _reader.ReadUInt(8).Should().Be(55));
        }

        [Test]
        public void UInt16()
        {
            Test(w => w.WriteUInt(55, 16), r => _reader.ReadUInt(16).Should().Be(55));
        }

        [Test]
        public void UInt24()
        {
            Test(w => w.WriteUInt(55, 24), r => _reader.ReadUInt(24).Should().Be(55));
        }

        [Test]
        public void UInt32()
        {
            Test(w => w.WriteUInt(55, 32), r => _reader.ReadUInt(32).Should().Be(55));
            Test(w => w.WriteUInt(0, 32), r => _reader.ReadUInt(32).Should().Be(0));
            Test(w => w.WriteUInt(System.Int32.MaxValue, 32), r => _reader.ReadUInt(32).Should().Be(System.Int32.MaxValue));
        }

        private void Test(Action<IBinaryWriter> writerAction, Action<IBinaryReader> readerAction)
        {
            _stream.SetLength(0);
            var command = new TestCommand(writerAction, readerAction);
            _writer.WriteCommand(command);
            _stream.Position = 0;

            _commandFactory.Setup(c => c.CreateCommand(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CGMFile>())).Returns(command);

            _reader.ReadCommands();
            if (_reader.Messages.Any())
                Assert.Fail(System.String.Join("\r\n", _reader.Messages.Select(m => m.ToString()).ToArray()));
        }
    }

    public class TestCommand : Command
    {
        private Action<IBinaryWriter> _writerAction;
        private Action<IBinaryReader> _readerAction;

        public TestCommand(Action<IBinaryWriter> writerAction, Action<IBinaryReader> readerAction)
            :base(new CommandConstructorArguments(ClassCode.AttributeElements, 1, null))
        {
            _writerAction = writerAction;
            _readerAction = readerAction;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            _writerAction(writer);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _readerAction(reader);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
