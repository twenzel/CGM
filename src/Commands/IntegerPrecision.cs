using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, ElementId=4
    /// </remarks>
    public class IntegerPrecision : Command
    {
        public int Precision { get; set; }

        public IntegerPrecision(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 4, container))
        {

        }

        public IntegerPrecision(CgmFile container, int precision)
            : this(container)
        {
            Precision = precision;
            AssertPrecision();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.IntegerPrecision = Precision;

            AssertPrecision();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.IntegerPrecision = Precision;
        }

        private void AssertPrecision()
        {
            Assert(Precision == 8 || Precision == 16 || Precision == 24 || Precision == 32, "unsupported INTEGER PRECISION");
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            var val = Math.Pow(2, Precision) / 2;
            writer.WriteLine($" integerprec -{val}, {val - 1} % {Precision} binary bits %;");
        }

        public override string ToString()
        {
            return "IntegerPrecision " + Precision;
        }
    }
}
