namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=21
    /// </summary>
    public class FillBundleIndex : GenericIndexCommand
    {
        public FillBundleIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 21, container), "FILLINDEX")
        {
        }

        public FillBundleIndex(CGMFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}