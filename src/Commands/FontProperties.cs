using codessentials.CGM.Classes;
using System.Collections.Generic;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=21
    /// </summary>
    public class FontProperties : Command
    {
        public class FontInfo
        {
            public int PropertyIndicator { get; set; }
            public int Priority { get; set; }
            public StructuredDataRecord Value { get; set; }
        }

        public List<FontInfo> Infos { get; set; } = new List<FontInfo>();

        public FontProperties(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 21, container))
        {
           
        }

        public FontProperties(CGMFile container, FontInfo[] infos)
            :this(container)
        {
            Infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new FontInfo();
                info.PropertyIndicator = reader.ReadIndex();
                info.Priority = reader.ReadInt();
                info.Value = reader.ReadSDR();

                Infos.Add(info);
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var info in Infos)
            {
                writer.WriteIndex(info.PropertyIndicator);
                writer.WriteInt(info.Priority);
                writer.WriteSDR(info.Value);
            }
            
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" FONTPROP");

            foreach(var info in Infos)
            {
                writer.Write($" {WriteIndex(info.PropertyIndicator)} {WriteInt(info.Priority)}");

                WriteSDR(writer, info.Value);                
            }

            writer.WriteLine(";");
        }
    }
}