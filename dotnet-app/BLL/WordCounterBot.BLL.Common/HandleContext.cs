using System;
using System.Collections.Generic;

namespace WordCounterBot.BLL.Common
{
    public class HandleContext
    {
        private readonly List<string> _handledBy;
        public IReadOnlyList<string> HandledBy => _handledBy;

        public HandleContext(List<string> handledBy)
        {
            _handledBy = handledBy ?? throw new ArgumentNullException(nameof(handledBy));
        }
    }
}
