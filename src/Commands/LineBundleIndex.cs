namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=1
    /// </summary>
    public class LineBundleIndex : GenericIndexCommand
    {
        public LineBundleIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 1, container), "LINEINDEX")
        {

        }

        public LineBundleIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
