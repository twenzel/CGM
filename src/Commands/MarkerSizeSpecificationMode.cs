﻿using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=4
    /// </summary>
    public class MarkerSizeSpecificationMode : Command
    {
        public SpecificationMode Mode { get; set; }

        public MarkerSizeSpecificationMode(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 4, container))
        {
            
        }

        public MarkerSizeSpecificationMode(CGMFile container, SpecificationMode mode)
            :this(container)
        {
            Mode = mode;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int mode = reader.ReadEnum();
            Mode = SpecificationModeTools.GetMode(mode);
            _container.MarkerSizeSpecificationMode = Mode;            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Mode);
            _container.MarkerSizeSpecificationMode = Mode;
        }

        public override string ToString()
        {
            return $"MarkerSizeSpecificationMode {Mode}";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" MARKERSIZEMODE  {WriteEnum(Mode)};");
        }       
    }
}