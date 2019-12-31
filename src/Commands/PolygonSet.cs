using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=8
    /// </summary>
    public class PolygonSet : Command
    {
        public enum EdgeFlag
        {
            INVIS = 0,
            VIS = 1,
            CLOSEINVIS = 2,
            CLOSEVIS = 3,
        }

        public List<KeyValuePair<EdgeFlag, CGMPoint>> Set { get; set; } = new List<KeyValuePair<EdgeFlag, CGMPoint>>();

        public PolygonSet(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 8, container))
        {
            
        }

        public PolygonSet(CGMFile container, IEnumerable<KeyValuePair<EdgeFlag, CGMPoint>> values)
            :this(container)
        {
            Set.AddRange(values);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Assert(reader.Arguments.Length % (reader.SizeOfPoint() + reader.SizeOfEnum()) == 0, "Invalid amount of arguments");
            int n = reader.Arguments.Length / (reader.SizeOfPoint() + reader.SizeOfEnum());

            for (int i = 0; i < n; i++)
            {
                EdgeFlag edgeOutFlag = (EdgeFlag)reader.ReadEnum();
                var p = reader.ReadPoint();
                Set.Add(new KeyValuePair<EdgeFlag, CGMPoint>(edgeOutFlag, p));
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var pair in Set)
            {
                writer.WriteEnum((int)pair.Key);
                writer.WritePoint(pair.Value);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" POLYGONSET");

            foreach(var pair in Set)
                writer.Write($" {WritePoint(pair.Value)} {WriteEnum(pair.Key)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return "PolygonSet";
        }
    }
}