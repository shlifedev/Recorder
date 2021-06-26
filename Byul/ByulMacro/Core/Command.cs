using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByulMacro.Core
{

    public interface ICommand
    {   
        string CommandTitle { get; }
        void Execute(); 
    }


    public enum CommandType
    {
        FindImageAndClick = 0, //왼,우 지정가능
        FindImageAndDoubleClick ,
        FindImageAndKeyEvent ,
        WaitForSecond , // ~초동안 기다리기  
        FindImageAndBackgroundClick,
        Goto, // 특정 순서로 가기 
    } 
    public abstract class Command : ICommand
    {
        /// <summary>
        /// 해당 커맨드에 대한 랜더러를 초기화 해야한다.
        /// </summary>
        public GUI.Command.ICommandRenderer commandRenderer;
        /// <summary>
        /// 커맨드는 순서대로 실행되므로, 인덱스가 곧 리스트이 순서가 된다
        /// </summary>
        public int index = 0;

        public abstract string CommandTitle { get;} 
        public abstract void Execute();
    }

    public class CommandImageFindAndClick : Command
    {
        public override string CommandTitle => "해당 이미지를 찾은 후 클릭"; 
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

}
