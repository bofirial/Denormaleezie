using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Normaleezie;
using Normaleezie.Helpers;
using Normaleezie.Tests.Test_Classes;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace nEZ.Unit.Helpers
{
    #region ConvertList
    public class When_Calling_ConvertList_With_A_Null_Object
    {
        private readonly ReflectionHelper reflectionHelper;

        public When_Calling_ConvertList_With_A_Null_Object()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void It_Should_Throw_An_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => reflectionHelper.ConvertList<object>(null));
        }
    }

    public class When_Calling_ConvertList
    {
        private readonly List<Animal> returnedAnimals;

        public When_Calling_ConvertList()
        {
            ReflectionHelper reflectionHelper = new ReflectionHelper();

            List<object> animals = new List<object>()
            {
                new Animal() {Name = "Reggie"},
                new Animal() {Name = "Ronnie"},
                new Animal() {Name = "Ralphie"}
            };

            returnedAnimals = reflectionHelper.ConvertList<Animal>(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_Of_The_Generic_Type()
        {
            Assert.IsType(typeof(List<Animal>), returnedAnimals);
        }
    }

    public class When_Calling_ConvertList_With_A_Mixed_List
    {
        private readonly ReflectionHelper reflectionHelper;
        private readonly List<object> objects;

        public When_Calling_ConvertList_With_A_Mixed_List()
        {
            reflectionHelper = new ReflectionHelper();

            objects = new List<object>()
            {
                new Animal() {Name = "Reggie"},
                new Person() {Name = "Ronnie"},
                new Book() {Title = "Ralphie"}
            };
        }

        [Fact]
        public void It_Should_Throw_An_InvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => reflectionHelper.ConvertList<Animal>(objects));
        }
    }

    #endregion

    #region IsSimpleType
    public class When_Calling_IsSimpleType_With_A_Null_Type
    {
        private readonly ReflectionHelper reflectionHelper;

        public When_Calling_IsSimpleType_With_A_Null_Type()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => reflectionHelper.IsSimpleType(null));
        }
    }

    public class When_Calling_IsSimpleType
    {
        private readonly ReflectionHelper reflectionHelper;

        public When_Calling_IsSimpleType()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(float))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(char))]
        [InlineData(typeof(decimal))]
        public void With_A_Simple_Type_It_Should_Return_True(Type type)
        {
            bool isSimpleType = reflectionHelper.IsSimpleType(type);

            Assert.True(isSimpleType);
        }

        [Theory]
        [InlineData(typeof(Book))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(Tuple<string>))]
        [InlineData(typeof(object))]
        public void With_A_Complex_Type_It_Should_Return_False(Type type)
        {
            bool isSimpleType = reflectionHelper.IsSimpleType(type);
            
            Assert.False(isSimpleType);
        }
    }
    #endregion

    #region IsIEnumerableType
    public class When_Calling_IsIEnumerableType_With_A_Null_Type
    {
        private readonly ReflectionHelper reflectionHelper;

        public When_Calling_IsIEnumerableType_With_A_Null_Type()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => reflectionHelper.IsIEnumerableType(null));
        }
    }

    public class When_Calling_IsIEnumerableType
    {
        private readonly ReflectionHelper reflectionHelper;

        public When_Calling_IsIEnumerableType()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<Book>))]
        [InlineData(typeof(Array))]
        public void With_An_IEnumerable_Type_It_Should_Return_True(Type type)
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(type);

            Assert.True(isSimpleType);
        }

        [Theory]
        [InlineData(typeof(Book))]
        [InlineData(typeof(Tuple<string>))]
        [InlineData(typeof(object))]
        public void With_A_Type_That_Is_Not_IEnumerable_Type_It_Should_Return_False(Type type)
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(type);

            Assert.False(isSimpleType);
        }

        [Fact]
        public void With_The_String_Type_It_Should_Return_False()
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(typeof(string));

            Assert.False(isSimpleType);
        }
    }
    #endregion
}
