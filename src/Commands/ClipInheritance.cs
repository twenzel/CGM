using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=3
    /// </summary>
    public class ClipInheritance : Command
    {
        public enum Value
        {
            STLIST = 0,
            INTERSECTION
        }

        private Value _value;

        public ClipInheritance(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 3, container))
        {
           
        }

        public ClipInheritance(CGMFile container, Value value)
            :this(container)
        {
            _value = value;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _value = (Value)reader.ReadEnum();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)_value);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" CLIPINH {WriteEnum(_value)};");
        }

        public Value Data => _value;
    }
}