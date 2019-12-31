using codessentials.CGM.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Elements
{ 
    public static class SegmentControlElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((SegmentControlElement)elementId)
            {
                case SegmentControlElement.COPY_SEGMENT: 
                    return new CopySegment(container);
                case SegmentControlElement.INHERITANCE_FILTER: 
                    return new InheritanceFilter(container);
                case SegmentControlElement.CLIP_INHERITANCE:
                    return new ClipInheritance(container);
                case SegmentControlElement.SEGMENT_TRANSFORMATION:
                    return new SegmentTransformation(container);
                case SegmentControlElement.SEGMENT_HIGHLIGHTING:
                    return new SegmentHighlighting(container);
                case SegmentControlElement.SEGMENT_DISPLAY_PRIORITY:
                    return new SegmentDisplayPriority(container);
                case SegmentControlElement.SEGMENT_PICK_PRIORITY:
                    return new SegmentPickPriority(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
