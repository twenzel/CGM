using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=20
    /// </summary>
    public class CircularArcCentreReversed : CircularArcCentre
    {
        public CircularArcCentreReversed(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 20, container))
        {

        }

        public CircularArcCentreReversed(CgmFile container, CgmPoint center, double startDeltaX, double startDeltaY, double endDeltaX, double endDeltaY, double radius)
           : this(container)
        {
            SetValues(center, startDeltaX, startDeltaY, endDeltaX, endDeltaY, radius);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write("  ARCCTRREV");
            WriteValues(writer);
            writer.WriteLine(" ;");
        }
    }
}