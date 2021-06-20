using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core
{
    public class CommandExecutor
    {
        public List<ICommand> commandList = new List<ICommand>();
        public void UpdateAndExecute()
        {
            foreach(var logic in commandList)
            {
                var result = logic.Execute();
                if(result == false)
                {
                    
                }
            }
        }
    }
}
