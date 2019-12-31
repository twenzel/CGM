using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class AttributeElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((AttributeElement)elementId)
            {
                case AttributeElement.LINE_BUNDLE_INDEX: 
                    return new LineBundleIndex(container);
                case AttributeElement.LINE_TYPE: 
                    return new LineType(container);
                case AttributeElement.LINE_WIDTH: 
                    return new LineWidth(container);
                case AttributeElement.LINE_COLOUR: 
                    return new LineColour(container);
                case AttributeElement.MARKER_BUNDLE_INDEX: 
                    return new MarkerBundleIndex(container);
                case AttributeElement.MARKER_TYPE: 
                    return new MarkerType(container);
                case AttributeElement.MARKER_SIZE: 
                    return new MarkerSize(container);
                case AttributeElement.MARKER_COLOUR: 
                    return new MarkerColour(container);
                case AttributeElement.TEXT_BUNDLE_INDEX: 
                    return new TextBundleIndex(container);
                case AttributeElement.TEXT_FONT_INDEX: 
                    return new TextFontIndex(container);
                case AttributeElement.TEXT_PRECISION: 
                    return new TextPrecision(container);
                case AttributeElement.CHARACTER_EXPANSION_FACTOR:
                    return new CharacterExpansionFactor(container);
                case AttributeElement.CHARACTER_SPACING:
                    return new CharacterSpacing(container);
                case AttributeElement.TEXT_COLOUR: 
                    return new TextColour(container);
                case AttributeElement.CHARACTER_HEIGHT: 
                    return new CharacterHeight(container);
                case AttributeElement.CHARACTER_ORIENTATION: 
                    return new CharacterOrientation(container);
                case AttributeElement.TEXT_PATH: 
                    return new TextPath(container);
                case AttributeElement.TEXT_ALIGNMENT: 
                    return new TextAlignment(container);
                case AttributeElement.CHARACTER_SET_INDEX: 
                    return new CharacterSetIndex(container);
                case AttributeElement.ALTERNATE_CHARACTER_SET_INDEX: 
                    return new AlternateCharacterSetIndex(container);
                case AttributeElement.FILL_BUNDLE_INDEX:
                    return new FillBundleIndex(container);
                case AttributeElement.INTERIOR_STYLE: 
                    return new InteriorStyle(container);
                case AttributeElement.FILL_COLOUR: 
                    return new FillColour(container);
                case AttributeElement.HATCH_INDEX: 
                    return new HatchIndex(container);
                case AttributeElement.PATTERN_INDEX:
                    return new PatternIndex(container);
                case AttributeElement.EDGE_BUNDLE_INDEX: 
                    return new EdgeBundleIndex(container);
                case AttributeElement.EDGE_TYPE: 
                    return new EdgeType(container);
                case AttributeElement.EDGE_WIDTH: 
                    return new EdgeWidth(container);
                case AttributeElement.EDGE_COLOUR: 
                    return new EdgeColour(container);
                case AttributeElement.EDGE_VISIBILITY: 
                    return new EdgeVisibility(container);
                case AttributeElement.FILL_REFERENCE_POINT:
                    return new FillReferencePoint(container);
                case AttributeElement.PATTERN_TABLE:
                    return new PatternTable(container);
                case AttributeElement.PATTERN_SIZE: 
                    return new PatternSize(container);
                case AttributeElement.COLOUR_TABLE: 
                    return new ColourTable(container);
                case AttributeElement.ASPECT_SOURCE_FLAGS:
                    return new AspectSourceFlags(container);
                case AttributeElement.PICK_IDENTIFIER: 
                    return new PickIdentifier(container);
                case AttributeElement.LINE_CAP: 
                    return new LineCap(container);
                case AttributeElement.LINE_JOIN: 
                    return new LineJoin(container);
                case AttributeElement.LINE_TYPE_CONTINUATION:
                    return new LineTypeContinuation(container);
                case AttributeElement.LINE_TYPE_INITIAL_OFFSET:
                    return new LineTypeInitialOffset(container);
                case AttributeElement.TEXT_SCORE_TYPE:
                    return new TextScoreType(container);
                case AttributeElement.RESTRICTED_TEXT_TYPE: 
                    return new RestrictedTextType(container);
                case AttributeElement.INTERPOLATED_INTERIOR: 
                    return new InterpolatedInterior(container);
                case AttributeElement.EDGE_CAP: 
                    return new EdgeCap(container);
                case AttributeElement.EDGE_JOIN:
                    return new EdgeJoin(container);
                case AttributeElement.EDGE_TYPE_CONTINUATION:
                    return new EdgeTypeContinuation(container);
                case AttributeElement.EDGE_TYPE_INITIAL_OFFSET:
                    return new EdgeTypeInitialOffset(container);
                case AttributeElement.SYMBOL_LIBRARY_INDEX:
                    return new SymbolLibraryIndex(container);
                case AttributeElement.SYMBOL_COLOUR:
                    return new SymbolColour(container);
                case AttributeElement.SYMBOL_SIZE:
                    return new SymbolSize(container); 
                case AttributeElement.SYMBOL_ORIENTATION:
                    return new SymbolOrientation(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
