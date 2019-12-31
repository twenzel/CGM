using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=3
    /// </remarks>
    public class VDCType : Command
    {
        public enum Type
        {
            Integer,
            Real
        }

        private Type _type = Type.Integer;

        public VDCType(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 3, container))
        {
           
        }

        public VDCType(CGMFile container, Type type)
            :this(container)
        {
            _type = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int p1 = reader.ReadEnum();
            if (p1 == 0)
                _type = Type.Integer;
            else if (p1 == 1)
                _type = Type.Real;
            else
                reader.Unsupported("VDC Type " + p1);

            _container.VDCType = _type;            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)_type);
            _container.VDCType = _type;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            if (_type == Type.Integer)
            {
                writer.Info("Writing vdctype = real instead of integer (as read by the binary file) because of some problems using integer. If the CGM file could not be opened in any viewer please edit file and change vdctype.");
                writer.WriteLine($" vdctype {WriteEnum(Type.Real)};");
            }
            else
                writer.WriteLine($" vdctype {WriteEnum(_type)};");
        }

        public override string ToString()
        {
            return $"VDCType [{_type}]";
        }

        public Type Value => _type;
    }
}
