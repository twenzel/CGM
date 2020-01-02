using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=3, ElementId=2
    /// </remarks>
    public class VDCRealPrecision : RealPrecisionBase
    {
        public VDCRealPrecision(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 2, container))
        {

        }

        public VDCRealPrecision(CgmFile container, Precision precision)
            : this(container)
        {
            SetValue(precision);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);
            _container.VDCRealPrecision = Value;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            _container.VDCRealPrecision = Value;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            if (Value == Precision.Floating_32)
                writer.WriteLine($"  vdcrealprec -511.0000, 511.0000, 7 % 10 binary bits %;");
            else
                throw new NotSupportedException($"VDCReal Precision {Value} is currently not supported.");
        }

        public override string ToString()
        {
            return "VDCRealPrecision " + Value;
        }
    }
}
