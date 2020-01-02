using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=3, ElementId=1
    /// </remarks>
    public class VDCIntegerPrecision : Command
    {
        public int Precision { get; private set; }

        public VDCIntegerPrecision(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 1, container))
        {

        }

        public VDCIntegerPrecision(CgmFile container, int precision)
            : this(container)
        {
            Precision = precision;
            AssertPrecision();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.VDCIntegerPrecision = Precision;

            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(Precision == 16 || Precision == 24 || Precision == 32, "unsupported VDCINTEGER PRECISION");
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.VDCIntegerPrecision = Precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            var val = Math.Pow(2, Precision) / 2;
            writer.WriteLine($" VDCINTEGERPREC -{val}, {val - 1} % {Precision} binary bits %;");
        }

        public override string ToString()
        {
            return "VDCIntegerPrecision " + Precision;
        }
    }
}
