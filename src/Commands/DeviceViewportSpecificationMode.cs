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
    /// Class=2, Element=9
    /// </remarks>
    public class DeviceViewportSpecificationMode : Command
    {
        public enum Mode
        {
            FRACTION, //FractionOfDrawingSurface,
            MM, //MillimetersWithScaleFactor,
            PHYDEVCOORD //PhysicalDeviceCoordinates
        }

        private Mode _mode;
        private double _metricScaleFactor;

        public DeviceViewportSpecificationMode(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 9, container))
        {
            
        }

        public DeviceViewportSpecificationMode(CGMFile container, Mode mode, double factor)
            :this(container)
        {
            _mode = mode;
            _metricScaleFactor = factor;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int enumValue = reader.ReadEnum();
            switch (enumValue)
            {
                case 0:
                    _mode = Mode.FRACTION;
                    break;
                case 1:
                    _mode = Mode.MM;
                    break;
                case 2:
                    _mode = Mode.PHYDEVCOORD;
                    break;
                default:
                    reader.Unsupported("unsupported mode " + enumValue);
                    break;
            }

            if (_container.RealPrecisionProcessed)
                _metricScaleFactor = reader.ReadReal();
            else
                _metricScaleFactor = reader.ReadFloatingPoint32();

            _container.DeviceViewportSpecificationMode = _mode;            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {    
            writer.WriteEnum((int)_mode);
            _container.DeviceViewportSpecificationMode = _mode;

            if (_container.RealPrecisionProcessed)
                writer.WriteReal(_metricScaleFactor);
            else
                writer.WriteFloatingPoint32(_metricScaleFactor);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" DEVVPMODE {WriteEnum(_mode)} {WriteReal(_metricScaleFactor)}");
        }

        public override string ToString()
        {
            return $"DeviceViewportSpecificationMode [mode={_mode}, metricScaleFactor={_metricScaleFactor}]";
        }

        public Mode Value => _mode;
        public double MetricScaleFactor => _metricScaleFactor;
    }
}
