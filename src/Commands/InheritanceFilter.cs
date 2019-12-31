using System;
using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=8, ElementId=2
    /// </summary>
    public class InheritanceFilter : Command
    {
        public enum Filter
        {
            LINEINDEX,
                LINETYPE ,
            LINEWIDTH ,
            LINECOLR,
            LINECLIPMODE,
            MARKERINDEX,
            MARKERTYPE,
            MARKERSIZE,
            MARKERCOLR,
            MARKERCLIPMODE,
            TEXTINDEX,
            TEXTFONTINDEX,
            TEXTPREC,
            CHAREXPAN,
            CHARSPACE,
            TEXTCOLR,
            CHARHEIGHT,
            CHARORI,
            TEXTPATH,
            TEXTALIGN,
            FILLINDEX,
            INTSTYLE,
            FILLCOLR,
            HATCHINDEX,
            PATINDEX,
            EDGEINDEX,
            EDGETYPE,
            EDGEWIDTH,
            EDGECOLR,
            EDGEVIS,
            EDGECLIPMODE,
            FILLREFPT,
            PATSIZE,
            AUXCOLR,
            TRANSPARENCY,
            LINEATTR,
            MARKERATTR,
            TEXPRESANDPLACEMATTR,
            TEXTPLACEMANDORIATTR,
            FILLATTR,
            EDGEATTR,
            PATATTR,
            OUTPUTCTRL,
            PICKID,
            ALLATTRCTRL,
            ALLINH,
            LINETYPEASF,
            LINEWIDTHASF,
            LINECOLRASF,
            MARKERTYPEASF,
            MARKERSIZEASF,
            MARKERCOLRASF,
            TEXTFONTINDEXASF,
             TEXTPRECASF,
            CHAREXPANASF,
            CHARSPACEASF,
            TEXTCOLRASF,
            INTSTYLEASF,
            FILLCOLRASF,
            HATCHINDEXASF,
            PATINDEXASF,
            EDGETYPEASF,
            EDGEWIDTHASF,
            EDGECOLRASF,
            ALLLINE,
            ALLMARKER,
            ALLTEXT,
            ALLFILL,
            ALLEDGE,
            ALL,
            MITRELIMIT,
            LINECAP,
            LINEJOIN,
            LINETYPECONT,
            LINETYPEINITOFFSET,
            TEXTSCORETYPE,
            RESTRTEXTTYPE,
            INTERPOLATEDINTERIOR,
            EDGECAP,
            EDGEJOIN,
            EDGETYPECONT,
            EDGETYPEINITOFFSET,
            SYMBOLLIBINDEX,
            SYMBOLCOLR,
            SYMBOLSIZE,
            SYMBOLORI,
            SYMBOLATTR
        }

        public Filter[] Values { get; set; }
        public int Setting { get; set; }

        public InheritanceFilter(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.SegmentControlandSegmentAttributeElements, 2, container))
        {
           
        }

        public InheritanceFilter(CGMFile container, Filter[] values, int setting)
            :this(container)
        {
            Values = values;
            Setting = setting;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            int n = (reader.Arguments.Length - 1) / reader.SizeOfEnum();

            Values = new Filter[n];
            for (int i = 0; i < n; i++)
            {
                Values[i] = (Filter)reader.ReadEnum();
            }

            Setting = reader.ReadEnum();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var val in Values)
                writer.WriteEnum((int)val);
            writer.WriteEnum(Setting);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  INHFILTER");

            foreach(int val in Values)
            {
                writer.Write($" {WriteEnum(val)}");
            }

            if (Setting == 0)
                writer.Write(" stlist");
            else
                writer.Write(" seg");

            writer.WriteLine(";");
        }
    }
}