using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class SegmentControlElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            return ((SegmentControlElement)elementId) switch
            {
                SegmentControlElement.COPY_SEGMENT => new CopySegment(container),
                SegmentControlElement.INHERITANCE_FILTER => new InheritanceFilter(container),
                SegmentControlElement.CLIP_INHERITANCE => new ClipInheritance(container),
                SegmentControlElement.SEGMENT_TRANSFORMATION => new SegmentTransformation(container),
                SegmentControlElement.SEGMENT_HIGHLIGHTING => new SegmentHighlighting(container),
                SegmentControlElement.SEGMENT_DISPLAY_PRIORITY => new SegmentDisplayPriority(container),
                SegmentControlElement.SEGMENT_PICK_PRIORITY => new SegmentPickPriority(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
