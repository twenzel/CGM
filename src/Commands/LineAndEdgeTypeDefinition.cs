using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=17
    /// </remarks>
    public class LineAndEdgeTypeDefinition : Command
    {
        public int LineType { get; set; }
        public double DashCycleRepeatLength { get; set; }
        public int[] DashElements { get; set; }

        public LineAndEdgeTypeDefinition(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 17, container))
        {
            
        }

        public LineAndEdgeTypeDefinition(CGMFile container, int lineType, double dashCycleRepeat, int[] elements)
            : this(container)
        {
            LineType = lineType;
            DashCycleRepeatLength = dashCycleRepeat;
            DashElements = elements;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            LineType = reader.ReadIndex();
            Assert(LineType <= 0, $"invalid lineType {LineType}. Should be negative.");

            DashCycleRepeatLength = System.Math.Abs(reader.ReadSizeSpecification(_container.LineWidthSpecificationMode));
            DashElements = new int[(reader.Arguments.Length - reader.CurrentArg) / reader.SizeOfInt()];

            for (int i = 0; i < DashElements.Length; i++)
            {
                DashElements[i] = reader.ReadInt();
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(LineType);
            writer.WriteSizeSpecification(DashCycleRepeatLength, _container.LineWidthSpecificationMode);
            foreach (var val in DashElements)
                writer.WriteInt(val);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("LineAndEdgeTypeDefinition");
            sb.Append(" lineType=").Append(LineType);
            sb.Append(" dashCycleRepeatLength=").Append(DashCycleRepeatLength);
            sb.Append(" [");
            for (int i = 0; i < DashElements.Length; i++)
            {
                sb.Append(DashElements[i]).Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" LINEEDGETYPEDEF {WriteInt(LineType)} {WriteVDC(DashCycleRepeatLength)}");

            foreach (var val in DashElements)
            {
                writer.Write($" {WriteInt(val)}");
            }

            writer.WriteLine(";");
        }
    }
}