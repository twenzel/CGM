using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=35
    /// </summary>
    public class AspectSourceFlags : Command
    {
        public enum ASFType
        {
            linetype = 0,
            linewidth,
            linecolour,
            markertype,
            markersize,
            markercolour,
            textfontindex,
            textprecision,
            characterexpansionfactor,
            characterspacing,
            textcolour,
            interiorstyle,
            fillcolour,
            hatchindex,
            patternindex,
            edgetype,
            edgewidth,
            edgecolour
        }

        public enum ASFValue
        {
            INDIV,
            BUNDLED
        }

        public class ASFInfo
        {
            public ASFType Type { get; set; }
            public ASFValue Value { get; set; }
        }

        public List<ASFInfo> Infos { get; } = new List<ASFInfo>();

        public AspectSourceFlags(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 35, container))
        {

        }

        public AspectSourceFlags(CgmFile container, ASFInfo[] infos)
            : this(container)
        {
            Infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new ASFInfo()
                {
                    Type = (ASFType)reader.ReadEnum(),
                    Value = (ASFValue)reader.ReadEnum()
                };
                Infos.Add(info);
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var info in Infos)
            {
                writer.WriteEnum((int)info.Type);
                writer.WriteEnum((int)info.Value);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" ASF");

            foreach (var info in Infos)
                writer.Write($" {WriteEnum(info.Type)} {WriteEnum(info.Value)}");

            writer.WriteLine(";");
        }
    }
}
