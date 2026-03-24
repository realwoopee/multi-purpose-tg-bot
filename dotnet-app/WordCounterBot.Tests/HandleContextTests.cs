using System;
using System.Collections.Generic;
using Xunit;
using WordCounterBot.BLL.Common;

namespace WordCounterBot.Tests
{
    public class HandleContextTests
    {
        /// <summary>
        /// Передача null вместо коллекции в конструктор.
        /// Приём: Классы плохих данных.
        /// </summary>
        [Fact]
        public void Constructor_WithNullList_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new HandleContext(null!)
            );
            
            Assert.Equal("handledBy", exception.ParamName);
        }

        /// <summary>
        /// Передача корректного непустого списка в конструктор.
        /// Приём: Классы хороших данных.
        /// </summary>
        [Fact]
        public void Constructor_WithPopulatedList_PreservesAllElements()
        {
            // Arrange
            var handledBy = new List<string> { "WordCounter", "CommandExecutor" };

            // Act
            var context = new HandleContext(handledBy);

            // Assert
            Assert.Equal(2, context.HandledBy.Count);
            Assert.Equal("WordCounter", context.HandledBy[0]);
            Assert.Equal("CommandExecutor", context.HandledBy[1]);
        }

        /// <summary>
        /// Проверка доступа к элементам списка по индексу.
        /// Приём: Классы хороших данных.
        /// </summary>
        [Fact]
        public void HandledBy_AccessByIndex_ReturnsCorrectElement()
        {
            // Arrange
            var handledBy = new List<string> { "First", "Second", "Third" };
            var context = new HandleContext(handledBy);

            // Act & Assert
            Assert.Equal("First", context.HandledBy[0]);
            Assert.Equal("Second", context.HandledBy[1]);
            Assert.Equal("Third", context.HandledBy[2]);
        }

        /// <summary>
        /// Передача пустого списка (минимальный валидный ввод).
        /// Приём: Анализ граничных условий.
        /// </summary>
        [Fact]
        public void Constructor_WithEmptyList_CreatesContextSuccessfully()
        {
            // Arrange
            var handledBy = new List<string>();

            // Act
            var context = new HandleContext(handledBy);

            // Assert
            Assert.NotNull(context.HandledBy);
            Assert.Empty(context.HandledBy);
        }

        /// <summary>
        /// Передача списка с одним элементом.
        /// Приём: Анализ граничных условий.
        /// </summary>
        [Fact]
        public void Constructor_WithSingleElement_WorksCorrectly()
        {
            // Arrange
            var handledBy = new List<string> { "OnlyHandler" };

            // Act
            var context = new HandleContext(handledBy);

            // Assert
            Assert.Single(context.HandledBy);
            Assert.Equal("OnlyHandler", context.HandledBy[0]);
        }

        /// <summary>
        /// Многократный доступ к свойству HandledBy.
        /// Приём: Угадывание ошибок.
        /// </summary>
        [Fact]
        public void HandledBy_CalledMultipleTimes_ReturnsConsistentData()
        {
            // Arrange
            var handledBy = new List<string> { "Handler1" };
            var context = new HandleContext(handledBy);

            // Act
            var first = context.HandledBy;
            var second = context.HandledBy;

            // Assert
            Assert.Equal(first.Count, second.Count);
            Assert.Equal(first[0], second[0]);
            Assert.Same(first, second);
        }

        /// <summary>
        /// Передача списка с null-элементами.
        /// Приём: Угадывание ошибок.
        /// </summary>
        [Fact]
        public void Constructor_WithNullElements_AcceptsThem()
        {
            // Arrange
            var handledBy = new List<string> { "Handler1", null!, "Handler2" };

            // Act
            var context = new HandleContext(handledBy);

            // Assert
            Assert.Equal(3, context.HandledBy.Count);
            Assert.Null(context.HandledBy[1]);
        }

        /// <summary>
        /// Добавление элемента во внешний список после создания контекста.
        /// Приём: Тестирование, основанное на потоках данных.
        /// </summary>
        [Fact]
        public void HandledBy_WhenOwnerAddsElement_ReflectsChange()
        {
            // Arrange
            var handledBy = new List<string>();
            var context = new HandleContext(handledBy);

            // Act
            handledBy.Add("WordCounter");

            // Assert
            Assert.Single(context.HandledBy);
            Assert.Equal("WordCounter", context.HandledBy[0]);
        }

        /// <summary>
        /// Последовательное добавление нескольких обработчиков.
        /// Приём: Тестирование, основанное на потоках данных.
        /// </summary>
        [Fact]
        public void HandledBy_SimulatesUpdateRouterUsage_TracksAllHandlers()
        {
            // Arrange
            var handledBy = new List<string>();
            var context = new HandleContext(handledBy);

            // Act
            handledBy.Add("SystemMessageHandler");
            handledBy.Add("WordCounter");
            handledBy.Add("CommandExecutor");

            // Assert
            Assert.Equal(3, context.HandledBy.Count);
            Assert.Contains("SystemMessageHandler", context.HandledBy);
            Assert.Contains("WordCounter", context.HandledBy);
            Assert.Contains("CommandExecutor", context.HandledBy);
        }
    }
}