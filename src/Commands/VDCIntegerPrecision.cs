using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=3, ElementId=1
    /// </remarks>
    public class VDCIntegerPrecision : Command
    {
        private int _precision;       

        public VDCIntegerPrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 1, container))
        {
           
        }

        public VDCIntegerPrecision(CGMFile container, int precision)
            :this(container)
        {
            _precision = precision;
            AssertPrecision();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _precision = reader.ReadInt();
            _container.VDCIntegerPrecision = _precision;

            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(_precision == 16 || _precision == 24 || _precision == 32, "unsupported VDCINTEGER PRECISION");
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(_precision);
            _container.VDCIntegerPrecision = _precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            var val = Math.Pow(2, _precision) / 2;
            writer.WriteLine($" VDCINTEGERPREC -{val}, {val - 1} % {_precision} binary bits %;");
        }

        public override string ToString()
        {
            return "VDCIntegerPrecision " + _precision;
        }

        public int Precision => _precision;    
    }
}
