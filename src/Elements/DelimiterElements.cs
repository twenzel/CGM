using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class DelimiterElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CgmFile container)
        {
            return ((DelimiterElement)elementId) switch
            {
                DelimiterElement.NO_OP => new NoOp(container),
                DelimiterElement.BEGIN_METAFILE => new BeginMetafile(container),
                DelimiterElement.END_METAFILE => new EndMetafile(container),
                DelimiterElement.BEGIN_PICTURE => new BeginPicture(container),
                DelimiterElement.BEGIN_PICTURE_BODY => new BeginPictureBody(container),
                DelimiterElement.END_PICTURE => new EndPicture(container),
                DelimiterElement.BEGIN_SEGMENT => new BeginSegment(container),
                DelimiterElement.END_SEGMENT => new EndSegment(container),
                DelimiterElement.BEGIN_FIGURE => new BeginFigure(container),
                DelimiterElement.END_FIGURE => new EndFigure(container),
                DelimiterElement.BEGIN_PROTECTION_REGION => new BeginProtectionRegion(container),
                DelimiterElement.END_PROTECTION_REGION => new EndProtectionRegion(container),
                DelimiterElement.BEGIN_COMPOUND_LINE => new BeginCompoundLine(container),
                DelimiterElement.END_COMPOUND_LINE => new EndCompoundLine(container),
                DelimiterElement.BEGIN_COMPOUND_TEXT_PATH => new BeginCompoundTextPath(container),
                DelimiterElement.END_COMPOUND_TEXT_PATH => new EndCompoundTextPath(container),
                DelimiterElement.BEGIN_TILE_ARRAY => new BeginTileArray(container),
                DelimiterElement.END_TILE_ARRAY => new EndTileArray(container),
                DelimiterElement.BEGIN_APPLICATION_STRUCTURE => new BeginApplicationStructure(container),
                DelimiterElement.BEGIN_APPLICATION_STRUCTURE_BODY => new BeginApplicationStructureBody(container),
                DelimiterElement.END_APPLICATION_STRUCTURE => new EndApplicationStructure(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
