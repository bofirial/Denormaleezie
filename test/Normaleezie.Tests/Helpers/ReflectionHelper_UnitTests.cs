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

namespace Unit.Helpers
{
    #region ConvertList

    public class ConvertList
    {
        private readonly ReflectionHelper reflectionHelper;

        public ConvertList()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void Should_Throw_An_ArgumentException_When_The_List_Is_Null()
        {
            Assert.Throws<ArgumentException>(() => reflectionHelper.ConvertList<object>(null));
        }
        
        [Fact]
        public void Should_Throw_An_InvalidCastException_When_Every_Object_In_The_List_Does_Not_Match_The_Type()
        {
            List<object> objects = new List<object>()
            {
                new Animal() {Name = "Reggie"},
                new Person() {Name = "Ronnie"},
                new Book() {Title = "Ralphie"}
            };

            Assert.Throws<InvalidCastException>(() => reflectionHelper.ConvertList<Animal>(objects));
        }

        [Fact]
        public void Should_Return_A_List_Of_The_Generic_Type()
        {
            List<object> animals = new List<object>()
            {
                new Animal() {Name = "Reggie"},
                new Animal() {Name = "Ronnie"},
                new Animal() {Name = "Ralphie"}
            };

            List<Animal> returnedAnimals = reflectionHelper.ConvertList<Animal>(animals);

            Assert.IsType(typeof(List<Animal>), returnedAnimals);
        }
    }

    #endregion

    #region IsSimpleType

    public class IsSimpleType
    {
        private readonly ReflectionHelper reflectionHelper;
        public IsSimpleType()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void Should_Throw_An_Exception_When_The_Type_Is_Null()
        {
            Assert.Throws(typeof(ArgumentException), () => reflectionHelper.IsSimpleType(null));
        }
        
        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(float))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(char))]
        [InlineData(typeof(decimal))]
        public void Should_Return_True_When_The_Type_Is_A_Simple_Type(Type type)
        {
            bool isSimpleType = reflectionHelper.IsSimpleType(type);

            Assert.True(isSimpleType);
        }

        [Theory]
        [InlineData(typeof(Book))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(Tuple<string>))]
        [InlineData(typeof(object))]
        public void Should_Return_False_When_The_Type_Is_A_Complex_Type(Type type)
        {
            bool isSimpleType = reflectionHelper.IsSimpleType(type);

            Assert.False(isSimpleType);
        }
    }

    #endregion

    #region IsIEnumerableType

    public class IsIEnumerableType
    {
        private readonly ReflectionHelper reflectionHelper;

        public IsIEnumerableType()
        {
            reflectionHelper = new ReflectionHelper();
        }

        [Fact]
        public void Should_Throw_An_Exception_When_The_Type_Is_Null()
        {
            Assert.Throws(typeof(ArgumentException), () => reflectionHelper.IsIEnumerableType(null));
        }
        
        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<Book>))]
        [InlineData(typeof(Array))]
        public void Should_Return_True_When_The_Type_Is_An_IEnumerable(Type type)
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(type);

            Assert.True(isSimpleType);
        }

        [Theory]
        [InlineData(typeof(Book))]
        [InlineData(typeof(Tuple<string>))]
        [InlineData(typeof(object))]
        public void Should_Return_False_When_The_Type_Is_Not_IEnumerable(Type type)
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(type);

            Assert.False(isSimpleType);
        }

        [Fact]
        public void Should_Return_False_When_The_Type_Is_String()
        {
            bool isSimpleType = reflectionHelper.IsIEnumerableType(typeof(string));

            Assert.False(isSimpleType);
        }
    }
    #endregion
}
