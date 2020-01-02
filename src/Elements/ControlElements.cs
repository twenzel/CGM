using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ControlElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CgmFile container)
        {
            return ((ControlElement)elementId) switch
            {
                ControlElement.VDC_INTEGER_PRECISION => new VDCIntegerPrecision(container),
                ControlElement.VDC_REAL_PRECISION => new VDCRealPrecision(container),
                ControlElement.AUXILIARY_COLOUR => new AuxiliaryColour(container),
                ControlElement.TRANSPARENCY => new Transparency(container),
                ControlElement.CLIP_RECTANGLE => new ClipRectangle(container),
                ControlElement.CLIP_INDICATOR => new ClipIndicator(container),
                ControlElement.LINE_CLIPPING_MODE => new LineClipping(container),
                ControlElement.MARKER_CLIPPING_MODE => new MarkerClipping(container),
                ControlElement.EDGE_CLIPPING_MODE => new EdgeClipping(container),
                ControlElement.NEW_REGION => new NewRegion(container),
                ControlElement.SAVE_PRIMITIVE_CONTEXT => new SavePrimitiveContext(container),
                ControlElement.RESTORE_PRIMITIVE_CONTEXT => new RestorePrimitiveContext(container),
                ControlElement.PROTECTION_REGION_INDICATOR => new ProtectionRegionIndicator(container),
                ControlElement.GENERALIZED_TEXT_PATH_MODE => new GeneralizedTextPathMode(container),
                ControlElement.MITRE_LIMIT => new MitreLimit(container),
                ControlElement.TRANSPARENT_CELL_COLOUR => new TransparentCellColour(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
