using System;
using Xunit;
using WordCounterBot.BLL.Common.Helpers;

namespace WordCounterBot.Tests
{
    public class StringHelperTests
    {
        /// <summary>
        /// Обычная строка без спецсимволов.
        /// Приём: Классы хороших данных.
        /// </summary>
        [Fact]
        public void Escape_RegularStringWithoutSpecialChars_ReturnsUnchanged()
        {
            // Arrange
            var input = "Hello World";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("Hello World", result);
        }

        /// <summary>
        /// Строка с буквами разных алфавитов.
        /// Приём: Классы хороших данных.
        /// </summary>
        [Fact]
        public void Escape_UnicodeString_ReturnsUnchanged()
        {
            // Arrange
            var input = "Привет мир 你好世界";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("Привет мир 你好世界", result);
        }

        /// <summary>
        /// Пустая строка - граничный случай.
        /// Приём: Анализ граничных условий.
        /// </summary>
        [Fact]
        public void Escape_EmptyString_ReturnsEmpty()
        {
            // Arrange
            var input = "";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("", result);
        }

        /// <summary>
        /// Строка из одного символа (минимальная длина > 0).
        /// Приём: Анализ граничных условий.
        /// </summary>
        [Fact]
        public void Escape_SingleCharString_ReturnsCorrectly()
        {
            // Arrange
            var input = "a";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("a", result);
        }
        
        /// <summary>
        /// Строка из пробелов.
        /// Приём: Анализ граничных условий.
        /// </summary>
        [Fact]
        public void Escape_WhitespaceOnly_ReturnsUnchanged()
        {
            // Arrange
            var input = "   \t\n";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("   \t\n", result);
        }

        /// <summary>
        /// HTML-тег - типичный сценарий инъекции.
        /// Приём: Угадывание ошибок - проверка XSS-защиты.
        /// </summary>
        [Fact]
        public void Escape_HtmlTag_ReturnsFullyEscaped()
        {
            // Arrange
            var input = "<script>alert('xss')</script>";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Contains("&lt;", result);
            Assert.Contains("&gt;", result);
            Assert.DoesNotContain("<script>", result);
        }

        /// <summary>
        /// Проверка корректного порядка замен при смешанных спецсимволах.
        /// Приём: Угадывание ошибок - в первоначальном коде был баг.
        /// </summary>
        [Fact]
        public void Escape_MixedSpecialCharacters_AmpersandReplacedFirst()
        {
            // Arrange
            var input = "A & B < C";
    
            // Act
            var result = input.HtmlEscape();
    
            // Assert
            Assert.Equal("A &amp; B &lt; C", result);
        }

        /// <summary>
        /// Экранирование символа &lt;.
        /// Приём: Классы эквивалентности (каждый спецсимвол - отдельный класс входных данных)
        /// </summary>
        [Fact]
        public void Escape_LessThanChar_ReturnsEscaped()
        {
            // Arrange
            var input = "<";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&lt;", result);
        }

        /// <summary>
        /// Экранирование символа "больше".
        /// Приём: Классы эквивалентности (каждый спецсимвол - отдельный класс входных данных)
        /// </summary>
        [Fact]
        public void Escape_GreaterThanChar_ReturnsEscaped()
        {
            // Arrange
            var input = ">";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&gt;", result);
        }

        /// <summary>
        /// Экранирование амперсанда.
        /// Приём: Классы эквивалентности (каждый спецсимвол - отдельный класс входных данных)
        /// </summary>
        [Fact]
        public void Escape_AmpersandChar_ReturnsEscaped()
        {
            // Arrange
            var input = "&";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&amp;", result);
        }

        /// <summary>
        /// Экранирование двойной кавычки.
        /// Приём: Классы эквивалентности (каждый спецсимвол - отдельный класс входных данных)
        /// </summary>
        [Fact]
        public void Escape_DoubleQuoteChar_ReturnsEscaped()
        {
            // Arrange
            var input = "\"";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&quot;", result);
        }

        /// <summary>
        /// Экранирование апострофа.
        /// Приём: Классы эквивалентности (каждый спецсимвол - отдельный класс входных данных)
        /// </summary>
        [Fact]
        public void Escape_ApostropheChar_ReturnsEscaped()
        {
            // Arrange
            var input = "'";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&apos;", result);
        }
        
        /// <summary>
        /// Строка со спецсимволами в начале.
        /// Приём: Классы эквивалентности - позиция спецсимвола.
        /// </summary>
        [Fact]
        public void Escape_SpecialCharAtStart_Escaped()
        {
            // Arrange
            var input = "<hello";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.StartsWith("&lt;", result);
        }

        /// <summary>
        /// Строка со спецсимволами в конце.
        /// Приём: Классы эквивалентности - позиция спецсимвола.
        /// </summary>
        [Fact]
        public void Escape_SpecialCharAtEnd_Escaped()
        {
            // Arrange
            var input = "hello>";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.EndsWith("&gt;", result);
        }
        
        /// <summary>
        /// Комбинация всех спецсимволов.
        /// Приём: Классы эквивалентности - комбинированный ввод
        /// </summary>
        [Fact]
        public void Escape_AllSpecialCharsCombined_AllEscaped()
        {
            // Arrange
            var input = "<>&\"'";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.DoesNotContain("<", result);
            Assert.DoesNotContain(">", result);
            Assert.DoesNotContain("\"", result);
            Assert.DoesNotContain("'", result);
        }

        /// <summary>
        /// Множественные вхождения одного символа.
        /// Приём: Классы эквивалентности - комбинированный ввод.
        /// </summary>
        [Fact]
        public void Escape_MultipleOccurrences_AllReplaced()
        {
            // Arrange
            var input = "<<<>>>";
            
            // Act
            var result = input.HtmlEscape();
            
            // Assert
            Assert.Equal("&lt;&lt;&lt;&gt;&gt;&gt;", result);
        }
    }
}