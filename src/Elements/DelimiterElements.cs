using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class DelimiterElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch((DelimiterElement)elementId)
            {
                case DelimiterElement.NO_OP:
                    return new NoOp(container);                
                case DelimiterElement.BEGIN_METAFILE:
                    return new BeginMetafile(container);
                case DelimiterElement.END_METAFILE:
                    return new EndMetafile(container);
                case DelimiterElement.BEGIN_PICTURE:
                    return new BeginPicture(container);
                case DelimiterElement.BEGIN_PICTURE_BODY:
                    return new BeginPictureBody(container);
                case DelimiterElement.END_PICTURE:
                    return new EndPicture(container);
                case DelimiterElement.BEGIN_SEGMENT:
                    return new BeginSegment(container);
                case DelimiterElement.END_SEGMENT:
                    return new EndSegment(container);
                case DelimiterElement.BEGIN_FIGURE:
                    return new BeginFigure(container);
                case DelimiterElement.END_FIGURE:
                    return new EndFigure(container);
                case DelimiterElement.BEGIN_PROTECTION_REGION:
                    return new BeginProtectionRegion(container);
                case DelimiterElement.END_PROTECTION_REGION:
                    return new EndProtectionRegion(container);
                case DelimiterElement.BEGIN_COMPOUND_LINE:
                    return new BeginCompoundLine(container);
                case DelimiterElement.END_COMPOUND_LINE:
                    return new EndCompoundLine(container);
                case DelimiterElement.BEGIN_COMPOUND_TEXT_PATH:
                    return new BeginCompoundTextPath(container);
                case DelimiterElement.END_COMPOUND_TEXT_PATH:
                    return new EndCompoundTextPath(container);
                case DelimiterElement.BEGIN_TILE_ARRAY:
                    return new BeginTileArray(container);
                case DelimiterElement.END_TILE_ARRAY:
                    return new EndTileArray(container);
                case DelimiterElement.BEGIN_APPLICATION_STRUCTURE:
                    return new BeginApplicationStructure(container);
                case DelimiterElement.BEGIN_APPLICATION_STRUCTURE_BODY:
                    return new BeginApplicationStructureBody(container);
                case DelimiterElement.END_APPLICATION_STRUCTURE:
                    return new EndApplicationStructure(container);
                default:                    
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
