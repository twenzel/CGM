using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=15
    /// </remarks>
    public class CharacterCodingAnnouncer : Command
    {
        public enum Type
        {
            BASIC_7_BIT,
            BASIC_8_BIT,
            EXTENDED_7_BIT,
            EXTENDED_8_BIT,
        }

        public Type Value { get; private set; }

        public CharacterCodingAnnouncer(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 15, container))
        {

        }

        public CharacterCodingAnnouncer(CgmFile container, Type type)
            : this(container)
        {
            Value = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var type = reader.ReadEnum();
            switch (type)
            {
                case 0:
                    Value = Type.BASIC_7_BIT;
                    break;
                case 1:
                    Value = Type.BASIC_8_BIT;
                    break;
                case 2:
                    Value = Type.EXTENDED_7_BIT;
                    break;
                case 3:
                    Value = Type.EXTENDED_8_BIT;
                    break;
                default:
                    reader.Unsupported("unsupported character coding type " + type);
                    Value = Type.BASIC_7_BIT;
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            switch (Value)
            {
                case Type.BASIC_7_BIT:
                    writer.WriteLine($" charcoding BASIC7BIT;");
                    break;
                case Type.BASIC_8_BIT:
                    writer.WriteLine($" charcoding BASIC8BIT;");
                    break;
                case Type.EXTENDED_7_BIT:
                    writer.WriteLine($" charcoding EXTD7BIT;");
                    break;
                case Type.EXTENDED_8_BIT:
                    writer.WriteLine($" charcoding EXTD8BIT;");
                    break;
                default:
                    throw new NotSupportedException($"CharacterCoding {Value} not supported.");
            }
        }

        public override string ToString()
        {
            return $"CharacterCodingAnnouncer type={Value}";
        }
    }
}