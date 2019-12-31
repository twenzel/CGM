using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=2
    /// </summary>
    public class ColourSelectionMode : Command
    {
        public enum Type { INDEXED, DIRECT }     

        public ColourSelectionMode(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 2, container))
        {
            
        }

        public ColourSelectionMode(CGMFile container, Type mode)
            :this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int e = reader.ReadEnum();
            if (e == 0)
                Mode = Type.INDEXED;
            else if (e == 1)
                Mode = Type.DIRECT;
            else
            {
                Mode = Type.INDEXED;
                reader.Unsupported("color selection mode " + e);
            }

            _container.ColourSelectionMode = Mode;            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.ColourSelectionMode = Mode;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  colrmode {WriteEnum(Mode)};");
        }

        public override string ToString()
        {
            return $"ColourSelectionMode {Mode}";
        }

        public Type Mode { get; set; }
    }
}