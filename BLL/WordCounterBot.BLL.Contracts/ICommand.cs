using System;
using System.Collections.Generic;
using System.Text;

namespace WordCounterBot.BLL.Core
{
    public interface ICommand
    {
        public void Execute();
    }
}
