using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=3, ElementId=6
    /// </summary>
    public class ClipIndicator : Command
    {
        private bool _flag;

        public ClipIndicator(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.ControlElements, 6, container))
        {
           
        }

        public ClipIndicator(CGMFile container, bool flag)
            :this(container)
        {
            _flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _flag = reader.ReadBool();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteBool(_flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  clip {WriteBool(_flag)};");           
        }

        public override string ToString()
        {
            return $"ClipIndicator {_flag}";
        }

        public bool Flag => _flag;
    }
}