using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=13
    /// </remarks>
    public class BeginProtectionRegion : Command
    {
        private int _regionIndex;

        public BeginProtectionRegion(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 13, container))
        {
            
        }

        public BeginProtectionRegion(CGMFile container, int index)
            :this(container)
        {
            _regionIndex = index;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _regionIndex = reader.ReadIndex();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(_regionIndex);           
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGPROTREGION {WriteIndex(_regionIndex)};");
        }

        public int RegionIndex => _regionIndex;
    }
}