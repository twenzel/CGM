using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class CharacterOrientation : Command
    {
        private double _xUp;
        private double _yUp;
        private double _xBase;
        private double _yBase;

        public CharacterOrientation(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 16, container))
        {
            
        }

        public CharacterOrientation(CGMFile container, double xUp, double yUp, double xBase, double yBase)
            :this(container)
        {
            _xUp = xUp;
            _yUp = yUp;
            _xBase = xBase;
            _yBase = yBase;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _xUp = reader.ReadVdc();
            _yUp = reader.ReadVdc();
            _xBase = reader.ReadVdc();
            _yBase = reader.ReadVdc();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(_xUp);
            writer.WriteVdc(_yUp);
            writer.WriteVdc(_xBase);
            writer.WriteVdc(_yBase);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  CHARORI {WriteVDC(_xUp)} {WriteVDC(_yUp)}, {WriteVDC(_xBase)} {WriteVDC(_yBase)};");
        }

        public override string ToString()
        {
            return $"CharacterOrientation xUp = {_xUp}";
        }

        public bool IsDownToUp()
        {
            return _xUp == -1 && _yUp == 0 && _xBase == 0 && _yBase == 1;
        }

        public bool IsLeftToRight()
        {
            return _xUp == 0 && _yUp == 1 && _xBase == 1 && _yBase == 0;
        }

        public double Xup => _xUp;
        public double yup => _yUp;
        public double Xbase => _xBase;
        public double Ybase => _yBase;
    }
}