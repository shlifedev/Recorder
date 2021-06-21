using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Byul.Core
{
    public abstract class Command
    {
        /// <summary>
        /// 해당 커맨드에 대한 랜더러를 초기화 해야한다.
        /// </summary>
        public GUI.Command.ICommandRenderer commandRenderer;
        /// <summary>
        /// 커맨드는 순서대로 실행되므로, 인덱스가 곧 리스트이 순서가 된다
        /// </summary>
        public int index = 0;

 
    }
}
