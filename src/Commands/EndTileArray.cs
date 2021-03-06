﻿namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=20
    /// </remarks>
    public class EndTileArray : Command
    {
        public EndTileArray(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 20, container))
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {

        }

        public override string ToString()
        {
            return "EndTileArray";
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" ENDTILEARRAY;");
        }
    }
}