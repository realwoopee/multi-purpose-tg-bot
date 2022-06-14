using System;
using WordCounterBot.BLL.Common;
using Xunit;

namespace WordCounterBot.Tests
{
    public class WordCounterUtilTest
    {
        [Fact]
        public void Test()
        {
            var text = "Красная собака поймала синюю (лису) ( ).\n The red dog catched a blue fox.";

            Assert.Equal(12, WordCounterUtil.CountWords(text));
        }
    }
}
