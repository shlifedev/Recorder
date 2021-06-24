using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Attributes
{
    public class Run : System.Attribute
    {
        public object o = null;
        public Run() { }
        public Run(object obj) { this.o = obj; } 
    }
}
