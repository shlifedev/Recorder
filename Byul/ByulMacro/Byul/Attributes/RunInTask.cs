using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Attributes
{
    public class RunInTask : System.Attribute
    {
        public object o = null;
        public RunInTask() { }
        public RunInTask(object obj) { this.o = obj; } 
    }
}
