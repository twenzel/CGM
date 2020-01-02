namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=25
    /// </summary>
    public class PatternIndex : GenericIndexCommand
    {
        public PatternIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 25, container), "PATINDEX")
        {
        }

        public PatternIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
