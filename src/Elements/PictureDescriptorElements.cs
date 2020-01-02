using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class PictureDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            return ((PictureDescriptorElement)elementId) switch
            {
                PictureDescriptorElement.SCALING_MODE => new ScalingMode(container),
                PictureDescriptorElement.COLOUR_SELECTION_MODE => new ColourSelectionMode(container),
                PictureDescriptorElement.LINE_WIDTH_SPECIFICATION_MODE => new LineWidthSpecificationMode(container),
                PictureDescriptorElement.MARKER_SIZE_SPECIFICATION_MODE => new MarkerSizeSpecificationMode(container),
                PictureDescriptorElement.EDGE_WIDTH_SPECIFICATION_MODE => new EdgeWidthSpecificationMode(container),
                PictureDescriptorElement.VDC_EXTENT => new VDCExtent(container),
                PictureDescriptorElement.BACKGROUND_COLOUR => new BackgroundColour(container),
                PictureDescriptorElement.DEVICE_VIEWPORT => new DeviceViewport(container),
                PictureDescriptorElement.DEVICE_VIEWPORT_SPECIFICATION_MODE => new DeviceViewportSpecificationMode(container),
                PictureDescriptorElement.DEVICE_VIEWPORT_MAPPING => new DeviceViewportMapping(container),
                PictureDescriptorElement.LINE_REPRESENTATION => new LineRepresentation(container),
                PictureDescriptorElement.MARKER_REPRESENTATION => new MarkerRepresentation(container),
                PictureDescriptorElement.TEXT_REPRESENTATION => new TextRepresentation(container),
                PictureDescriptorElement.FILL_REPRESENTATION => new FillRepresentation(container),
                PictureDescriptorElement.EDGE_REPRESENTATION => new EdgeRepresentation(container),
                PictureDescriptorElement.INTERIOR_STYLE_SPECIFICATION_MODE => new InteriorStyleSpecificationMode(container),
                PictureDescriptorElement.LINE_AND_EDGE_TYPE_DEFINITION => new LineAndEdgeTypeDefinition(container),
                PictureDescriptorElement.HATCH_STYLE_DEFINITION => new HatchStyleDefinition(container),
                PictureDescriptorElement.GEOMETRIC_PATTERN_DEFINITION => new GeometricPatternDefinition(container),
                PictureDescriptorElement.APPLICATION_STRUCTURE_DIRECTORY => new ApplicationStructureDirectory(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
