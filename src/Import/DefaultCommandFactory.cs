using codessentials.CGM.Commands;
using codessentials.CGM.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Import
{
    public class DefaultCommandFactory: ICommandFactory
    {
        /// <summary>
        /// Create a new command instance
        /// </summary>
        /// <param name="elementId">The command Id</param>
        /// <param name="elementClass">The command class</param>
        /// <param name="container">The parent container</param>
        /// <returns></returns>
        public Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            ClassCode classCode = (ClassCode)elementClass;

            switch (classCode)
            {

                case ClassCode.DelimiterElement:
                    return DelimiterElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.MetafileDescriptorElements:
                    return MetaFileDescriptorElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.PictureDescriptorElements:
                    return PictureDescriptorElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.ControlElements:
                    return ControlElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.GraphicalPrimitiveElements:
                    return GraphicalPrimitiveElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.AttributeElements:
                    return AttributeElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.EscapeElement:
                    return new Escape(container);

                case ClassCode.ExternalElements:
                    return ExternalElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.SegmentControlandSegmentAttributeElements:
                    return SegmentControlElements.CreateCommand(elementId, elementClass, container);

                case ClassCode.ApplicationStructureDescriptorElements:
                    return ApplicationStructureDescriptorElements.CreateCommand(elementId, elementClass, container);

                default:
                    Command.Assert(10 <= elementClass && elementClass <= 15, "unsupported element class " + elementClass);
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
