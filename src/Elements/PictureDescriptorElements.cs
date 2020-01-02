using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class PictureDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((PictureDescriptorElement)elementId)
            {
                case PictureDescriptorElement.SCALING_MODE:
                    return new ScalingMode(container);
                case PictureDescriptorElement.COLOUR_SELECTION_MODE:
                    return new ColourSelectionMode(container);
                case PictureDescriptorElement.LINE_WIDTH_SPECIFICATION_MODE:
                    return new LineWidthSpecificationMode(container);
                case PictureDescriptorElement.MARKER_SIZE_SPECIFICATION_MODE:
                    return new MarkerSizeSpecificationMode(container);
                case PictureDescriptorElement.EDGE_WIDTH_SPECIFICATION_MODE:
                    return new EdgeWidthSpecificationMode(container);
                case PictureDescriptorElement.VDC_EXTENT:
                    return new VDCExtent(container);
                case PictureDescriptorElement.BACKGROUND_COLOUR:
                    return new BackgroundColour(container);
                case PictureDescriptorElement.DEVICE_VIEWPORT:
                    return new DeviceViewport(container);
                case PictureDescriptorElement.DEVICE_VIEWPORT_SPECIFICATION_MODE:
                    return new DeviceViewportSpecificationMode(container);
                case PictureDescriptorElement.DEVICE_VIEWPORT_MAPPING:
                    return new DeviceViewportMapping(container);
                case PictureDescriptorElement.LINE_REPRESENTATION:
                    return new LineRepresentation(container);
                case PictureDescriptorElement.MARKER_REPRESENTATION:
                    return new MarkerRepresentation(container);
                case PictureDescriptorElement.TEXT_REPRESENTATION:
                    return new TextRepresentation(container);
                case PictureDescriptorElement.FILL_REPRESENTATION:
                    return new FillRepresentation(container);
                case PictureDescriptorElement.EDGE_REPRESENTATION:
                    return new EdgeRepresentation(container);
                case PictureDescriptorElement.INTERIOR_STYLE_SPECIFICATION_MODE:
                    return new InteriorStyleSpecificationMode(container);
                case PictureDescriptorElement.LINE_AND_EDGE_TYPE_DEFINITION:
                    return new LineAndEdgeTypeDefinition(container);
                case PictureDescriptorElement.HATCH_STYLE_DEFINITION:
                    return new HatchStyleDefinition(container);
                case PictureDescriptorElement.GEOMETRIC_PATTERN_DEFINITION:
                    return new GeometricPatternDefinition(container);
                case PictureDescriptorElement.APPLICATION_STRUCTURE_DIRECTORY:
                    return new ApplicationStructureDirectory(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);

            }
        }
    }
}
