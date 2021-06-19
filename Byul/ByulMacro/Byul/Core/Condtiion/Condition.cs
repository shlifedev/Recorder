using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core.Condtiion
{
    public abstract class Condition : ILogic
    {
        public abstract bool Execute();
    }
}
