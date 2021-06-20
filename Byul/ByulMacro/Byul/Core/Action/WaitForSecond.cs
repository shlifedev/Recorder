using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core.Action
{
    public class WaitForSecond : Action
    {
        public int waitTime = 0; 
        public WaitForSecond(int waitTime) 
        {
            this.waitTime = waitTime;
        }
         
        public override bool Execute()
        {
            System.Threading.Thread.Sleep(waitTime);
            return true;
        }
    }
}
