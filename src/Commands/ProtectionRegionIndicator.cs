using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=17
    /// </summary>
    public class ProtectionRegionIndicator : Command
    {
        public int Index { get; set; }
        public int Indicator { get; set; }

        public ProtectionRegionIndicator(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 17, container))
        {
            
        }

        public ProtectionRegionIndicator(CGMFile container, int index, int indicator)
            :this(container)
        {
            Index = index;
            Indicator = indicator;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            Indicator = reader.ReadIndex();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            writer.WriteIndex(Indicator);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" PROTREGION {WriteIndex(Index)} {WriteIndex(Indicator)};");
        }
    }
}