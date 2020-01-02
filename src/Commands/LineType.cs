﻿namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=2
    /// </summary>
    public class LineType : DashTypeCommand
    {
        public LineType(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 2, container))
        {
        }

        public LineType(CGMFile container, DashType type)
            : this(container)
        {
            SetValue(type);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  linetype {WriteDashType()};");
        }

        public override string ToString()
        {
            return $"LineType {Type}";
        }
    }
}
