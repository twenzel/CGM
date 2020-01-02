using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ControlElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((ControlElement)elementId)
            {
                case ControlElement.VDC_INTEGER_PRECISION:
                    return new VDCIntegerPrecision(container);
                case ControlElement.VDC_REAL_PRECISION:
                    return new VDCRealPrecision(container);
                case ControlElement.AUXILIARY_COLOUR:
                    return new AuxiliaryColour(container);
                case ControlElement.TRANSPARENCY:
                    return new Transparency(container);
                case ControlElement.CLIP_RECTANGLE:
                    return new ClipRectangle(container);
                case ControlElement.CLIP_INDICATOR:
                    return new ClipIndicator(container);
                case ControlElement.LINE_CLIPPING_MODE:
                    return new LineClipping(container);
                case ControlElement.MARKER_CLIPPING_MODE:
                    return new MarkerClipping(container);
                case ControlElement.EDGE_CLIPPING_MODE:
                    return new EdgeClipping(container);
                case ControlElement.NEW_REGION:
                    return new NewRegion(container);
                case ControlElement.SAVE_PRIMITIVE_CONTEXT:
                    return new SavePrimitiveContext(container);
                case ControlElement.RESTORE_PRIMITIVE_CONTEXT:
                    return new RestorePrimitiveContext(container);
                case ControlElement.PROTECTION_REGION_INDICATOR:
                    return new ProtectionRegionIndicator(container);
                case ControlElement.GENERALIZED_TEXT_PATH_MODE:
                    return new GeneralizedTextPathMode(container);
                case ControlElement.MITRE_LIMIT:
                    return new MitreLimit(container);
                case ControlElement.TRANSPARENT_CELL_COLOUR:
                    return new TransparentCellColour(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
