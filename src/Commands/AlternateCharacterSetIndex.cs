using System.Collections.Generic;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// ClassId=5, ElementId=20
    /// </remarks>
    public class AlternateCharacterSetIndex : GenericIndexCommand
    {        
        public AlternateCharacterSetIndex(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 20, container), "altcharsetindex")
        {
           
        }

        public AlternateCharacterSetIndex(CGMFile container, int index)
            : this(container)
        {
            Index = index;
        }      
       
    }
}