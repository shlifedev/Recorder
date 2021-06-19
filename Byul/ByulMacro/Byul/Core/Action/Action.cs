using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core.Action
{
    public abstract class Action : ILogic
    {
        public abstract bool Execute();
    }
}
