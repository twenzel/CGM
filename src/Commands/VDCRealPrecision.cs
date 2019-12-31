using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=3, ElementId=2
    /// </remarks>
    public class VDCRealPrecision : RealPrecisionBase
    {    

        public VDCRealPrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 2, container))
        {            
                    
        }

        public VDCRealPrecision(CGMFile container, Precision precision)
            :this(container)
        {
            SetValue(precision);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            base.ReadFromBinary(reader);
            _container.VDCRealPrecision = _precision;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            base.WriteAsBinary(writer);
            _container.VDCRealPrecision = _precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            if (_precision == Precision.Floating_32)
                writer.WriteLine($"  vdcrealprec -511.0000, 511.0000, 7 % 10 binary bits %;");
            else
                throw new NotSupportedException($"VDCReal Precision {_precision} is currently not supported.");
        }

        public override string ToString()
        {
            return "VDCRealPrecision " + _precision;
        }
    }
}
