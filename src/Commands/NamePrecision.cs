using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=16
    /// </remarks>
    public class NamePrecision : Command
    {
        public int Precision { get; set; }

        public NamePrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 16, container))
        {

        }

        public NamePrecision(CGMFile container, int precision)
            : this(container)
        {
            Precision = precision;
            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(Precision == 8 || Precision == 16 || Precision == 24 || Precision == 32, $"unsupported NAME PRECISION {Precision}");
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.NamePrecision = Precision;

            AssertPrecision();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.NamePrecision = Precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            var val = Math.Pow(2, Precision) / 2;
            writer.WriteLine($" NAMEPREC -{val}, {val - 1} % {Precision} binary bits %;");
        }

        public override string ToString()
        {
            return "NamePrecision " + Precision;
        }
    }
}
