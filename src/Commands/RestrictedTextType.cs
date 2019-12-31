using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Commands
{
    public class RestrictedTextType : Command
    {
        public enum Type
        {
            BASIC = 1,
            BOXED_CAP,
            BOXED_ALL,
            ISOTROPIC_CAP,
            ISOTROPIC_ALL,
            JUSTIFIED
        }

        public Type Value { get; set; }

        public RestrictedTextType(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 42, container))
        {
            
        }

        public RestrictedTextType(CGMFile container, Type type)
            :this(container)
        {
            Value = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int indexValue = reader.ReadIndex();
            switch (indexValue)
            {
                case 1:
                    Value = Type.BASIC;
                    break;
                case 2:
                    Value = Type.BOXED_CAP;
                    break;
                case 3:
                    Value = Type.BOXED_ALL;
                    break;
                case 4:
                    Value = Type.ISOTROPIC_CAP;
                    break;
                case 5:
                    Value = Type.ISOTROPIC_ALL;
                    break;
                case 6:
                    Value = Type.JUSTIFIED;
                    break;
                default:
                    Value = Type.BASIC;
                    reader.Unsupported("unsupported text type " + indexValue);
                    break;
            }

            _container.RestrictedTextType = Value;            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Value);
            _container.RestrictedTextType = Value;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" RESTRTEXTTYPE {WriteInt((int)Value)};");
        }       
    }
}
