using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class HatchIndex : Command
    {
        public enum HatchType
        {
            HORIZONTAL_LINES = 1,
            VERTICAL_LINES = 2,
            POSITIVE_SLOPE_LINES = 3,
            NEGATIVE_SLOPE_LINES = 4,
            HORIZONTAL_VERTICAL_CROSSHATCH = 5,
            POSITIVE_NEGATIVE_CROSSHATCH = 6
        }

        public HatchType Type { get; set; } = HatchType.HORIZONTAL_LINES;

        public HatchIndex(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 24, container))
        {
           
        }

        public HatchIndex(CGMFile container, HatchType type)
            :this(container)
        {
            Type = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int indexValue = reader.ReadIndex();
            switch (indexValue)
            {
                case 1:
                    Type = HatchType.HORIZONTAL_LINES;
                    break;
                case 2:
                    Type = HatchType.VERTICAL_LINES;
                    break;
                case 3:
                    Type = HatchType.POSITIVE_SLOPE_LINES;
                    break;
                case 4:
                    Type = HatchType.NEGATIVE_SLOPE_LINES;
                    break;
                case 5:
                    Type = HatchType.HORIZONTAL_VERTICAL_CROSSHATCH;
                    break;
                case 6:
                    Type = HatchType.POSITIVE_NEGATIVE_CROSSHATCH;
                    break;
                default:
                    reader.Unsupported("hatch style: " + indexValue);
                    break;
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" HATCHINDEX {WriteInt((int)Type)};");
        }
    }
}