using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=6
    /// </remarks>
    public class IndexPrecision : Command
    {
        public int Precision { get; set; }

        public IndexPrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 6, container))
        {

        }

        public IndexPrecision(CGMFile container, int precision)
            : this(container)
        {
            Precision = precision;
            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(Precision == 8 || Precision == 16 || Precision == 24 || Precision == 32, "unsupported INDEX PRECISION");
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.IndexPrecision = Precision;

            AssertPrecision();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.IndexPrecision = Precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            var val = Math.Pow(2, Precision) / 2;
            writer.WriteLine($" indexprec -{val}, {val - 1} % {Precision} binary bits %;");
        }

        public override string ToString()
        {
            return "IndexPrecision " + Precision;
        }
    }
}
