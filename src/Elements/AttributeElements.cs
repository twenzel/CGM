using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class AttributeElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            return ((AttributeElement)elementId) switch
            {
                AttributeElement.LINE_BUNDLE_INDEX => new LineBundleIndex(container),
                AttributeElement.LINE_TYPE => new LineType(container),
                AttributeElement.LINE_WIDTH => new LineWidth(container),
                AttributeElement.LINE_COLOUR => new LineColour(container),
                AttributeElement.MARKER_BUNDLE_INDEX => new MarkerBundleIndex(container),
                AttributeElement.MARKER_TYPE => new MarkerType(container),
                AttributeElement.MARKER_SIZE => new MarkerSize(container),
                AttributeElement.MARKER_COLOUR => new MarkerColour(container),
                AttributeElement.TEXT_BUNDLE_INDEX => new TextBundleIndex(container),
                AttributeElement.TEXT_FONT_INDEX => new TextFontIndex(container),
                AttributeElement.TEXT_PRECISION => new TextPrecision(container),
                AttributeElement.CHARACTER_EXPANSION_FACTOR => new CharacterExpansionFactor(container),
                AttributeElement.CHARACTER_SPACING => new CharacterSpacing(container),
                AttributeElement.TEXT_COLOUR => new TextColour(container),
                AttributeElement.CHARACTER_HEIGHT => new CharacterHeight(container),
                AttributeElement.CHARACTER_ORIENTATION => new CharacterOrientation(container),
                AttributeElement.TEXT_PATH => new TextPath(container),
                AttributeElement.TEXT_ALIGNMENT => new TextAlignment(container),
                AttributeElement.CHARACTER_SET_INDEX => new CharacterSetIndex(container),
                AttributeElement.ALTERNATE_CHARACTER_SET_INDEX => new AlternateCharacterSetIndex(container),
                AttributeElement.FILL_BUNDLE_INDEX => new FillBundleIndex(container),
                AttributeElement.INTERIOR_STYLE => new InteriorStyle(container),
                AttributeElement.FILL_COLOUR => new FillColour(container),
                AttributeElement.HATCH_INDEX => new HatchIndex(container),
                AttributeElement.PATTERN_INDEX => new PatternIndex(container),
                AttributeElement.EDGE_BUNDLE_INDEX => new EdgeBundleIndex(container),
                AttributeElement.EDGE_TYPE => new EdgeType(container),
                AttributeElement.EDGE_WIDTH => new EdgeWidth(container),
                AttributeElement.EDGE_COLOUR => new EdgeColour(container),
                AttributeElement.EDGE_VISIBILITY => new EdgeVisibility(container),
                AttributeElement.FILL_REFERENCE_POINT => new FillReferencePoint(container),
                AttributeElement.PATTERN_TABLE => new PatternTable(container),
                AttributeElement.PATTERN_SIZE => new PatternSize(container),
                AttributeElement.COLOUR_TABLE => new ColourTable(container),
                AttributeElement.ASPECT_SOURCE_FLAGS => new AspectSourceFlags(container),
                AttributeElement.PICK_IDENTIFIER => new PickIdentifier(container),
                AttributeElement.LINE_CAP => new LineCap(container),
                AttributeElement.LINE_JOIN => new LineJoin(container),
                AttributeElement.LINE_TYPE_CONTINUATION => new LineTypeContinuation(container),
                AttributeElement.LINE_TYPE_INITIAL_OFFSET => new LineTypeInitialOffset(container),
                AttributeElement.TEXT_SCORE_TYPE => new TextScoreType(container),
                AttributeElement.RESTRICTED_TEXT_TYPE => new RestrictedTextType(container),
                AttributeElement.INTERPOLATED_INTERIOR => new InterpolatedInterior(container),
                AttributeElement.EDGE_CAP => new EdgeCap(container),
                AttributeElement.EDGE_JOIN => new EdgeJoin(container),
                AttributeElement.EDGE_TYPE_CONTINUATION => new EdgeTypeContinuation(container),
                AttributeElement.EDGE_TYPE_INITIAL_OFFSET => new EdgeTypeInitialOffset(container),
                AttributeElement.SYMBOL_LIBRARY_INDEX => new SymbolLibraryIndex(container),
                AttributeElement.SYMBOL_COLOUR => new SymbolColour(container),
                AttributeElement.SYMBOL_SIZE => new SymbolSize(container),
                AttributeElement.SYMBOL_ORIENTATION => new SymbolOrientation(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
