using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy.Configuration;
using Normaleezie.Tests.Test_Classes;
using Normaleezie;
using Normaleezie.NormalizedData;
using Normaleezie.NormalizedStructure;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace nEZ.Unit
{
    #region Normalize
    public class When_Calling_Normalize_With_A_Null_Object
    {
        private readonly List<List<List<object>>> normalizedForm;
        private readonly Normalizer normalizer;

        public When_Calling_Normalize_With_A_Null_Object()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.Normalize<List<object>>(null)).CallsBaseMethod();

            normalizedForm = normalizer.Normalize<List<object>>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_List()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
        }

        [Fact]
        public void It_Should_NOT_Call_ConvertToNormalizedForm()
        {
            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<object>>.Ignored))
                .MustNotHaveHappened();
        }
    }
    public class When_Calling_Normalize_With_An_Empty_List
    {
        private readonly List<List<List<object>>> normalizedForm;
        private readonly Normalizer normalizer;

        public When_Calling_Normalize_With_An_Empty_List()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.Normalize(A<List<object>>.Ignored)).CallsBaseMethod();

            normalizedForm = normalizer.Normalize(new List<object>());
        }

        [Fact]
        public void It_Should_Return_An_Empty_List()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
        }

        [Fact]
        public void It_Should_NOT_Call_ConvertToNormalizedForm()
        {
            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<object>>.Ignored))
                .MustNotHaveHappened();
        }
    }

    public class When_Calling_Normalize_With_A_List
    {
        private readonly List<List<List<object>>> normalizedForm;
        private readonly Normalizer normalizer;
        private readonly List<Animal> animals;
        private readonly List<List<List<object>>> convertToNormalizedFormReturnValue;

        public When_Calling_Normalize_With_A_List()
        {
            normalizer = A.Fake<Normalizer>();

            A.CallTo(() => normalizer.Normalize(A<List<Animal>>.Ignored)).CallsBaseMethod();

            convertToNormalizedFormReturnValue = new List<List<List<object>>>()
            {
                new List<List<object>>()
                {
                    new List<object>()
                    {
                        "denormaleezie"
                    }
                }
            };

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored)).Returns(convertToNormalizedFormReturnValue);

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                }
            };

            normalizedForm = normalizer.Normalize(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_In_Normalized_Form()
        {
            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.NotEmpty(normalizedForm);
        }

        [Fact]
        public void It_Should_Return_The_Value_Returned_From_ConvertToNormalizedForm()
        {
            Assert.Equal(convertToNormalizedFormReturnValue, normalizedForm);
        }

        [Fact]
        public void It_Should_Call_ConvertToNormalizedForm()
        {
            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
    #endregion

    #region ConvertToNormalizedForm

    public class When_Calling_ConvertToNormalizedForm
    {
        private readonly NormalizedDataManager normalizedDataManager;
        private readonly NormalizedStructureManager normalizedStructureManager;
        private readonly List<object> animals;
        private readonly List<List<List<object>>> normalizedForm;
        private readonly List<List<object>> normalizedDataList;
        private readonly List<List<object>> normalizedStructureList;

        public When_Calling_ConvertToNormalizedForm()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            Normalizer normalizer = new Normalizer(normalizedDataManager, normalizedStructureManager);

            normalizedDataList = new List<List<object>>()
                {
                    new List<object>() { "Id" },
                    new List<object>() { "Name", "Tony", "Tania", "Talia" },
                    new List<object>() { "Age", 21, 1, 3 },
                    new List<object>() { "Type", "Tiger" }
                };

            normalizedStructureList = new List<List<object>>()
                {
                    new List<object>() {1, 1, 1, 1 },
                    new List<object>() {2, 2, 2, 1 },
                    new List<object>() {3, 3, 3, 1 }
                };

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataList(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(normalizedDataList);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureList(A<List<object>>.Ignored, A<List<List<object>>>.Ignored))
                .Returns(normalizedStructureList);

            animals = new List<object>()
                {
                    new Animal()
                    {
                        AnimalId = 1,
                        Type = "Tiger",
                        Age = 21,
                        Name = "Tony"
                    }
                };

            normalizedForm = normalizer.ConvertToNormalizedForm(animals);
        }

        [Fact]
        public void It_Should_Return_The_NormalizedForm()
        {
            Assert.Equal(normalizedDataList, normalizedForm[0]);
            Assert.Equal(normalizedStructureList, normalizedForm[1]);
        }

        [Fact]
        public void It_Should_Call_CreateNormalizedDataList()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedDataList(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Call_CreateNormalizedStructureList()
        {
            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureList(A<List<object>>.Ignored, A<List<List<object>>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == normalizedDataList)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    #endregion

    //    #region CreateNormalizedDataList

    //    public class When_Calling_CreateNormalizedDataList
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> animals;
    //        private readonly List<List<object>> normalizedDataList;
    //        private readonly List<List<object>> getNormalizedDataForListReturnValue;

    //        public When_Calling_CreateNormalizedDataList()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.CreateNormalizedDataList(A<List<object>>.Ignored)).CallsBaseMethod();

    //            getNormalizedDataForListReturnValue = new List<List<object>>()
    //            {
    //                new List<object>() {"Name" },
    //                new List<object>() {"AnimalId" },
    //                new List<object>() {"Age", 21, 12 },
    //                new List<object>() {"Type", "Tiger" }
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored,  A<string>.Ignored))
    //                .Returns(getNormalizedDataForListReturnValue.Select(i => i).ToList());

    //            animals = new List<object>()
    //            {
    //                new Animal()
    //                {
    //                    AnimalId = 1,
    //                    Type = "Tiger",
    //                    Age = 21,
    //                    Name = "Tony"
    //                },
    //                new Animal(),
    //                new Animal()
    //            };

    //            normalizedDataList = normalizer.CreateNormalizedDataList(animals);
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedDataForList()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == animals)
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Data_From_GetNormalizedDataForList_Sorted_By_DataName()
    //        {
    //            Assert.Equal(new List<List<object>>()
    //            {
    //                getNormalizedDataForListReturnValue[2],
    //                getNormalizedDataForListReturnValue[1],
    //                getNormalizedDataForListReturnValue[0],
    //                getNormalizedDataForListReturnValue[3],
    //            }, normalizedDataList);
    //        }
    //    }
    //    #endregion

    //    #region GetNormalizedDataForProperty

    //    public class When_Calling_GetNormalizedDataForProperty_With_A_Null_Object_List
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedDataForProperty_With_A_Null_Object_List()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedDataForProperty(null, typeof(Animal).GetProperty("AnimalId")));
    //        }
    //    }
    //    public class When_Calling_GetNormalizedDataForProperty_With_A_Null_PropertyInfo
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedDataForProperty_With_A_Null_PropertyInfo()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedDataForProperty(new List<object>(), null));
    //        }
    //    }

    //    public class When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_True
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> animals;
    //        private readonly PropertyInfo propInfo;
    //        private readonly string propertyNamePrefix;
    //        private readonly List<List<object>> normalizedDataForProperty;
    //        private readonly List<List<object>> getNormalizedDataForSimplePropertyReturnValue;

    //        public When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_True()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(true);

    //            propertyNamePrefix = "This is a property name prefix";

    //            getNormalizedDataForSimplePropertyReturnValue = new List<List<object>>()
    //            {
    //                new List<object>() { "This is the list returned from GetNormalizedDataForSimpleProperty." }
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedDataForSimpleProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .Returns(getNormalizedDataForSimplePropertyReturnValue);

    //            animals = new List<object>()
    //            {
    //                new Animal()
    //                {
    //                    AnimalId = 1,
    //                    Type = "Tiger",
    //                    Age = 21,
    //                    Name = "Tony"
    //                },
    //                new Animal(),
    //                new Animal()
    //            };

    //            propInfo = typeof(Animal).GetProperty("AnimalId");

    //            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(animals, propInfo, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Call_IsSimpleType()
    //        {
    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
    //                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
    //                .MustHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedDataForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForSimpleProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == animals && (PropertyInfo)args[1] == propInfo && args[2].ToString() == propertyNamePrefix)
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForComplexProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForListProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Return_Only_The_Data_From_GetNormalizedDataForSimpleProperty()
    //        {
    //            Assert.Equal(getNormalizedDataForSimplePropertyReturnValue, normalizedDataForProperty);
    //        }
    //    }

    //    public class When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_False_And_The_Property_Is_A_List
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> zoos;
    //        private readonly PropertyInfo propInfo;
    //        private readonly string propertyNamePrefix;
    //        private readonly List<List<object>> normalizedDataForProperty;
    //        private readonly List<List<object>> getNormalizedDataForListPropertyReturnValue;

    //        public When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_False_And_The_Property_Is_A_List()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(false);

    //            propertyNamePrefix = "This is a property name prefix";

    //            getNormalizedDataForListPropertyReturnValue = new List<List<object>>()
    //            {
    //                new List<object>() { "This is the list returned from GetNormalizedDataForSimpleProperty." }
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedDataForListProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .Returns(getNormalizedDataForListPropertyReturnValue);

    //            zoos = new List<object>()
    //            {
    //                new Zoo(),
    //                new Zoo(),
    //                new Zoo()
    //            };

    //            propInfo = typeof(Zoo).GetProperty("Animals");

    //            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(zoos, propInfo, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Call_IsSimpleType()
    //        {
    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
    //                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
    //                .MustHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForSimpleProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForComplexProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedDataForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForListProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == zoos && (PropertyInfo)args[1] == propInfo && args[2].ToString() == propertyNamePrefix)
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_Return_Only_The_Data_From_GetNormalizedDataForListProperty()
    //        {
    //            Assert.Equal(getNormalizedDataForListPropertyReturnValue, normalizedDataForProperty);
    //        }
    //    }

    //    public class When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_False_And_The_Property_Is_NOT_A_List
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> zoos;
    //        private readonly PropertyInfo propInfo;
    //        private readonly string propertyNamePrefix;
    //        private readonly List<List<object>> normalizedDataForProperty;
    //        private readonly List<List<object>> getNormalizedDataForComplexPropertyReturnValue;

    //        public When_Calling_GetNormalizedDataForProperty_When_IsSimpleType_Is_False_And_The_Property_Is_NOT_A_List()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored)).Returns(false);

    //            propertyNamePrefix = "This is a property name prefix";

    //            getNormalizedDataForComplexPropertyReturnValue = new List<List<object>>()
    //            {
    //                new List<object>() { "This is the list returned from GetNormalizedDataForSimpleProperty." }
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedDataForComplexProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .Returns(getNormalizedDataForComplexPropertyReturnValue);

    //            zoos = new List<object>()
    //            {
    //                new Zoo(),
    //                new Zoo(),
    //                new Zoo()
    //            };

    //            propInfo = typeof(Zoo).GetProperty("Name");

    //            normalizedDataForProperty = normalizer.GetNormalizedDataForProperty(zoos, propInfo, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Call_IsSimpleType()
    //        {
    //            A.CallTo(() => normalizer.IsSimpleType(A<Type>.Ignored))
    //                .WhenArgumentsMatch(args => (Type)args[0] == propInfo.PropertyType)
    //                .MustHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForSimpleProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedDataForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForComplexProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == zoos && (PropertyInfo)args[1] == propInfo && args[2].ToString() == propertyNamePrefix)
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForListProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Return_Only_The_Data_From_GetNormalizedDataForListProperty()
    //        {
    //            Assert.Equal(getNormalizedDataForComplexPropertyReturnValue, normalizedDataForProperty);
    //        }
    //    }

    //    #endregion

    //    #region GetNormalizedDataForList

    //    public class When_Calling_GetNormalizedDataForList_With_A_Null_Object_List
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedDataForList_With_A_Null_Object_List()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof (ArgumentException), () => normalizer.GetNormalizedDataForList(null));
    //        }
    //    }

    //    public class When_Calling_GetNormalizedDataForList_With_An_Empty_List
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<List<object>> normalizedDataForList;
    //        private readonly string propertyNamePrefix;

    //        public When_Calling_GetNormalizedDataForList_With_An_Empty_List()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            propertyNamePrefix = "prefix";

    //            normalizedDataForList = normalizer.GetNormalizedDataForList(new List<object>(), propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Return_A_List_With_Only_The_PropertyNamePrefix()
    //        {
    //            Assert.Equal(new List<List<object>>() {new List<object>() {propertyNamePrefix}}, normalizedDataForList);
    //        }
    //    }
    //    public class When_Calling_GetNormalizedDataForList_With_A_List_Of_Nulls
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<List<object>> normalizedDataForList;
    //        private readonly string propertyNamePrefix;

    //        public When_Calling_GetNormalizedDataForList_With_A_List_Of_Nulls()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            propertyNamePrefix = "prefix";

    //            normalizedDataForList = normalizer.GetNormalizedDataForList(new List<object>() {null, null}, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Return_A_List_With_Only_The_PropertyNamePrefix_And_Null()
    //        {
    //            Assert.Equal(new List<List<object>>() { new List<object>() { propertyNamePrefix, null } }, normalizedDataForList);
    //        }
    //    }
    //    public class When_Calling_GetNormalizedDataForList_With_A_List_Of_Lists
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> denormalizedData;
    //        private readonly List<List<object>> normalizedDataForList;
    //        private readonly List<List<object>> recursiveGetNormalizedDataForListReturnValue;
    //        private readonly string propertyNamePrefix;

    //        public When_Calling_GetNormalizedDataForList_With_A_List_Of_Lists()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            recursiveGetNormalizedDataForListReturnValue = new List<List<object>>() {new List<object>() {"Returned Object"} };

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, "prefix~"))
    //                .ReturnsLazily(() => recursiveGetNormalizedDataForListReturnValue);

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, "prefix")).CallsBaseMethod();

    //            propertyNamePrefix = "prefix";

    //            denormalizedData = new List<object>() {new List<object>() {"This", "Is"}, new List<object>() {"A", "List"}};

    //            normalizedDataForList = normalizer.GetNormalizedDataForList(denormalizedData, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_Recursivly_Call_GetNormalizedDataForList()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, "prefix~"))
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Value_From_The_Recursive_Call_To_GetNormalizedDataForList()
    //        {
    //            Assert.Equal(recursiveGetNormalizedDataForListReturnValue, normalizedDataForList);
    //        }
    //    }
    //    public class When_Calling_GetNormalizedDataForList_With_A_List_Of_Simple_Objects
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> denormalizedData;
    //        private readonly List<List<object>> normalizedDataForList;
    //        private readonly string propertyNamePrefix;

    //        public When_Calling_GetNormalizedDataForList_With_A_List_Of_Simple_Objects()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            propertyNamePrefix = "prefix";

    //            denormalizedData = new List<object>() { "This", "Is", "Some", "Data" };

    //            normalizedDataForList = normalizer.GetNormalizedDataForList(denormalizedData, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForList_Recursively()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        //[Fact]
    //        //public void It_Should_Return_The_Value_From_The_Call_To_GetNormalizedDataForProperty()
    //        //{
    //        //    Assert.Equal(getNormalizedDataForPropertyReturnValues.SelectMany(i => i), normalizedDataForList);
    //        //}
    //    }
    //    public class When_Calling_GetNormalizedDataForList_With_A_List_Of_Complex_Objects
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> denormalizedData;
    //        private readonly List<List<object>> normalizedDataForList;
    //        private readonly string propertyNamePrefix;
    //        private readonly List<List<List<object>>> getNormalizedDataForPropertyReturnValues;

    //        public When_Calling_GetNormalizedDataForList_With_A_List_Of_Complex_Objects()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored)).CallsBaseMethod();

    //            getNormalizedDataForPropertyReturnValues = new List<List<List<object>>>()
    //            {
    //                new List<List<object>>() { new List<object>() { "Age" } },
    //                new List<List<object>>() { new List<object>() { "AnimalId" } },
    //                new List<List<object>>() { new List<object>() { "Name" } },
    //                new List<List<object>>() { new List<object>() { "Type" } }
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .ReturnsNextFromSequence(getNormalizedDataForPropertyReturnValues.ToArray());

    //            propertyNamePrefix = "prefix";

    //            denormalizedData = new List<object>() { new Animal() {}, new Animal() };

    //            normalizedDataForList = normalizer.GetNormalizedDataForList(denormalizedData, propertyNamePrefix);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedDataForList_Recursively()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForList(A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Values_From_The_Calls_To_GetNormalizedDataForProperty()
    //        {
    //            Assert.Equal(getNormalizedDataForPropertyReturnValues.SelectMany(i => i), normalizedDataForList);
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedDataForProperty_For_Every_Property_In_The_List_Type()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedDataForProperty(A<List<object>>.Ignored, A<PropertyInfo>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == denormalizedData && args[2] == propertyNamePrefix)
    //                .MustHaveHappened(Repeated.Exactly.Times(typeof(Animal).GetProperties().Length));
    //        }
    //    }

    //    #endregion

    //    #region GetNormalizedDataForListProperty

    //    #endregion

    //    #region GetNormalizedDataForComplexProperty

    //    #endregion

    //    #region GetNormalizedDataForSimpleProperty

    //    #endregion

    //    #region CreateNormalizedData

    //    #endregion


    //    #region GetUniquePropertyValues
    //    public class When_Calling_GetUniquePropertyValues_With_A_Null_Object_List
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetUniquePropertyValues_With_A_Null_Object_List()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetUniquePropertyValues(null, typeof(Animal).GetProperty("AnimalId")));
    //        }
    //    }
    //    public class When_Calling_GetUniquePropertyValues_With_A_Null_PropertyInfo
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetUniquePropertyValues_With_A_Null_PropertyInfo()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetUniquePropertyValues(new List<object>(), null));
    //        }
    //    }

    //    public class When_Calling_GetUniquePropertyValues_For_A_Property_With_No_Duplicates
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> animals;
    //        private readonly List<object> uniquePropertyValues;

    //        public When_Calling_GetUniquePropertyValues_For_A_Property_With_No_Duplicates()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<object>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

    //            animals = new List<object>()
    //            {
    //                new Animal()
    //                {
    //                    AnimalId = 1,
    //                    Type = "Tiger",
    //                    Age = 21,
    //                    Name = "Tony"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 2,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tania"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 3,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tali"
    //                }
    //            };

    //            PropertyInfo propInfo = typeof(Animal).GetProperty("AnimalId");

    //            uniquePropertyValues = normalizer.GetUniquePropertyValues(animals, propInfo);
    //        }

    //        [Fact]
    //        public void It_Should_Return_A_String_For_Every_ListItem()
    //        {
    //            Assert.Equal(animals.Count, uniquePropertyValues.Count);
    //        }

    //        [Fact]
    //        public void The_List_Should_Contain_The_Property_Values()
    //        {
    //            Assert.Equal(1, (int)uniquePropertyValues[0]);
    //            Assert.Equal(2, (int)uniquePropertyValues[1]);
    //            Assert.Equal(3, (int)uniquePropertyValues[2]);
    //        }
    //    }
    //    public class When_Calling_GetUniquePropertyValues_For_A_Property_With_Duplicates
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> uniquePropertyValues;

    //        public When_Calling_GetUniquePropertyValues_For_A_Property_With_Duplicates()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetUniquePropertyValues(A<List<object>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

    //            List<object> animals = new List<object>()
    //            {
    //                new Animal()
    //                {
    //                    AnimalId = 1,
    //                    Type = "Tiger",
    //                    Age = 21,
    //                    Name = "Tony"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 2,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tania"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 3,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tali"
    //                }
    //            };

    //            PropertyInfo propInfo = typeof(Animal).GetProperty("Type");

    //            uniquePropertyValues = normalizer.GetUniquePropertyValues(animals, propInfo);
    //        }

    //        [Fact]
    //        public void It_Should_Return_A_String_For_Every_Unique_ListItem()
    //        {
    //            Assert.Equal(1, uniquePropertyValues.Count);
    //        }

    //        [Fact]
    //        public void The_List_Should_Contain_Only_The_Unique_Property_Values()
    //        {
    //            Assert.Equal("Tiger", (string)uniquePropertyValues[0]);
    //        }
    //    }
    //    #endregion

    //    #region GetUniqueValues

    //    #endregion

    //    #region CreateNormalizedStructureList
    //    public class When_Calling_CreateNormalizedStructureList_With_A_Null_DenormalizedList
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_CreateNormalizedStructureList_With_A_Null_DenormalizedList()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureList(null, new List<List<object>>()));
    //        }
    //    }
    //    public class When_Calling_CreateNormalizedStructureList_With_A_Null_NormalizedDataList
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_CreateNormalizedStructureList_With_A_Null_NormalizedDataList()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureList(new List<object>(), null));
    //        }
    //    }

    //    public class When_Calling_CreateNormalizedStructureList
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<object> animals;
    //        private readonly List<List<object>> normalizedStructureList;
    //        private readonly List<List<object>> createNormalizedStructureItemReturnValues;

    //        public When_Calling_CreateNormalizedStructureList()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.CreateNormalizedStructureList(A<List<object>>.Ignored, A<List<List<object>>>.Ignored)).CallsBaseMethod();

    //            createNormalizedStructureItemReturnValues = new List<List<object>>()
    //            {
    //                new List<object>() { 1, 2, 3, 4 },
    //                new List<object>() { 5, 6, 7, 8 },
    //                new List<object>() { 9, 9, 9, 9 }
    //            };

    //            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored))
    //                .ReturnsNextFromSequence(createNormalizedStructureItemReturnValues.ToArray());

    //            animals = new List<object>()
    //            {
    //                new Animal()
    //                {
    //                    AnimalId = 1,
    //                    Type = "Tiger",
    //                    Age = 21,
    //                    Name = "Tony"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 2,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tania"
    //                },
    //                new Animal()
    //                {
    //                    AnimalId = 3,
    //                    Type = "Tiger",
    //                    Age = 12,
    //                    Name = "Tali"
    //                }
    //            };

    //            List<List<object>> normalizedDataList = new List<List<object>>()
    //            {
    //                new List<object>() { "AnimalId" },
    //                new List<object>() { "Type" },
    //                new List<object>() { "Age" },
    //                new List<object>() { "Name" }
    //            };

    //            normalizedStructureList = normalizer.CreateNormalizedStructureList(animals, normalizedDataList);
    //        }

    //        [Fact]
    //        public void It_Should_Call_CreateNormalizedStructureItem_Once_For_Each_Item_In_The_NormalizedDataList()
    //        {
    //            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored))
    //                .MustHaveHappened(Repeated.Exactly.Times(animals.Count));
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Data_From_CreateNormalizedStructureItem()
    //        {
    //            foreach (var createNormalizedStructureItemReturnValue in createNormalizedStructureItemReturnValues)
    //            {
    //                Assert.Contains(normalizedStructureList, dsl => dsl == createNormalizedStructureItemReturnValue);
    //            }
    //        }
    //    }
    //    #endregion

    //    #region CreateNormalizedStructureItem
    //    public class When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedList
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedList()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureItem(null, new List<List<object>>()));
    //        }
    //    }
    //    public class When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedData
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_CreateNormalizedStructureItem_With_A_Null_DenormalizedData()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.CreateNormalizedStructureItem(new Animal(), null));
    //        }
    //    }

    //    public class When_Calling_CreateNormalizedStructureItem
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly List<List<object>> normalizedDataList;
    //        private readonly List<object> normalizedStructureItem;
    //        private readonly List<object> getNormalizedItemPropertyObjectReturnValues;

    //        public When_Calling_CreateNormalizedStructureItem()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored)).CallsBaseMethod();

    //            getNormalizedItemPropertyObjectReturnValues = new List<object>()
    //            {
    //                101, 1, 1, "Tony"
    //            };

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored))
    //                .ReturnsNextFromSequence(getNormalizedItemPropertyObjectReturnValues.ToArray());

    //            Animal animal = new Animal()
    //            {
    //                AnimalId = 101,
    //                Type = "Tiger",
    //                Age = 21,
    //                Name = "Tony"
    //            };

    //            normalizedDataList = new List<List<object>>()
    //            {
    //                new List<object>() { "AnimalId" },
    //                new List<object>() { "Type", "Tiger", "Hippo" },
    //                new List<object>() { "Age", 21 },
    //                new List<object>() { "Name"}
    //            };

    //            normalizedStructureItem = normalizer.CreateNormalizedStructureItem(animal, normalizedDataList);
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedItemPropertyObject_Once_For_Each_Item_In_The_DenormalizedData()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored))
    //                .MustHaveHappened(Repeated.Exactly.Times(normalizedDataList.Count));
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Data_From_GetNormalizedItemPropertyObject()
    //        {
    //            foreach (var getNormalizedItemPropertyObjectReturnValue in getNormalizedItemPropertyObjectReturnValues)
    //            {
    //                Assert.Contains(normalizedStructureItem, dataStructureObject => dataStructureObject == getNormalizedItemPropertyObjectReturnValue);
    //            }
    //        }
    //    }
    //    #endregion

    //    #region GetNormalizedItemPropertyObject
    //    public class When_Calling_GetNormalizedItemPropertyObject_With_A_Null_DenormalizedItem
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedItemPropertyObject_With_A_Null_DenormalizedItem()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedItemPropertyObject(null, new List<object>()));
    //        }
    //    }

    //    public class When_Calling_GetNormalizedItemPropertyObject_With_A_Null_NormalizedPropertyData
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedItemPropertyObject_With_A_Null_NormalizedPropertyData()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedItemPropertyObject(new Animal(), null));
    //        }
    //    }
    //    public class When_Calling_GetNormalizedItemPropertyObject_With_An_Empty_NormalizedPropertyData
    //    {
    //        private readonly Normalizer normalizer;

    //        public When_Calling_GetNormalizedItemPropertyObject_With_An_Empty_NormalizedPropertyData()
    //        {
    //            normalizer = new Normalizer();
    //        }

    //        [Fact]
    //        public void It_Should_Throw_An_Exception()
    //        {
    //            Assert.Throws(typeof(ArgumentException), () => normalizer.GetNormalizedItemPropertyObject(new Animal(), new List<object>()));
    //        }
    //    }

    //    public class When_Calling_GetNormalizedItemPropertyObject_For_A_Simple_Property
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly Animal animal;
    //        private readonly List<object> normalizedPropertyData;
    //        private readonly object dataStructureObject;
    //        private readonly object simplePropertyReturnObject = 4;

    //        public When_Calling_GetNormalizedItemPropertyObject_For_A_Simple_Property()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForSimpleProperty(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .Returns(simplePropertyReturnObject);

    //            animal = new Animal()
    //            {
    //                AnimalId = 101,
    //                Type = "Tiger",
    //                Age = 21,
    //                Name = "Tony"
    //            };

    //            normalizedPropertyData = new List<object>() { "Name" };

    //            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Value_From_GetNormalizedItemPropertyObjectForSimpleProperty()
    //        {
    //            Assert.Equal(simplePropertyReturnObject, dataStructureObject);
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedItemPropertyObjectForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForSimpleProperty(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == animal && args[1] == normalizedPropertyData && args[2] == normalizedPropertyData[0])
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForComplexProperty(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForListProperty(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }
    //    }

    //    public class When_Calling_GetNormalizedItemPropertyObject_For_A_PropertyName_That_Contains_The_Complex_Symbol_First
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly BookMark bookMark;
    //        private readonly List<object> normalizedPropertyData;
    //        private readonly object dataStructureObject;
    //        private readonly object simplePropertyReturnObject = 4;

    //        public When_Calling_GetNormalizedItemPropertyObject_For_A_PropertyName_That_Contains_The_Complex_Symbol_First()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<BookMark>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForComplexProperty(A<BookMark>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .Returns(simplePropertyReturnObject);

    //            bookMark = new BookMark()
    //            {

    //            };

    //            normalizedPropertyData = new List<object>() { "Book.Authors~Name" };

    //            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(bookMark, normalizedPropertyData);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Value_From_GetNormalizedItemPropertyObjectForComplexProperty()
    //        {
    //            Assert.Equal(simplePropertyReturnObject, dataStructureObject);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForSimpleProperty(A<BookMark>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedItemPropertyObjectForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForComplexProperty(A<BookMark>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == bookMark && args[1] == normalizedPropertyData && args[2] == normalizedPropertyData[0])
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForListProperty(A<BookMark>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }
    //    }

    //    public class When_Calling_GetNormalizedItemPropertyObject_For_A_PropertyName_That_Contains_The_List_Symbol_First
    //    {
    //        private readonly Normalizer normalizer;
    //        private readonly Zoo zoo;
    //        private readonly List<object> normalizedPropertyData;
    //        private readonly object dataStructureObject;
    //        private readonly object simplePropertyReturnObject = 4;

    //        public When_Calling_GetNormalizedItemPropertyObject_For_A_PropertyName_That_Contains_The_List_Symbol_First()
    //        {
    //            normalizer = A.Fake<Normalizer>();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Zoo>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForListProperty(A<Zoo>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .Returns(simplePropertyReturnObject);

    //            zoo = new Zoo();

    //            normalizedPropertyData = new List<object>() { "Animals~Name.FirstName" };

    //            dataStructureObject = normalizer.GetNormalizedItemPropertyObject(zoo, normalizedPropertyData);
    //        }

    //        [Fact]
    //        public void It_Should_Return_The_Value_From_GetNormalizedItemPropertyObjectForListProperty()
    //        {
    //            Assert.Equal(simplePropertyReturnObject, dataStructureObject);
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForSimpleProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForSimpleProperty(A<Zoo>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_NOT_Call_GetNormalizedItemPropertyObjectForComplexProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForComplexProperty(A<Zoo>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .MustNotHaveHappened();
    //        }

    //        [Fact]
    //        public void It_Should_Call_GetNormalizedItemPropertyObjectForListProperty()
    //        {
    //            A.CallTo(() => normalizer.GetNormalizedItemPropertyObjectForListProperty(A<Zoo>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
    //                .WhenArgumentsMatch(args => args[0] == zoo && args[1] == normalizedPropertyData && args[2] == normalizedPropertyData[0])
    //                .MustHaveHappened(Repeated.Exactly.Once);
    //        }
    //    }

    //    #endregion

    //    #region GetNormalizedItemPropertyObjectForSimpleProperty

    //    //public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_No_NormalizedPropertyData_Other_Than_The_PropertyName
    //    //{
    //    //    private readonly Normalizer normalizer;
    //    //    private readonly object dataStructureObject;

    //    //    public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_No_NormalizedPropertyData_Other_Than_The_PropertyName()
    //    //    {
    //    //        normalizer = A.Fake<Normalizer>();

    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //    //        Animal animal = new Animal()
    //    //        {
    //    //            AnimalId = 101,
    //    //            Type = "Tiger",
    //    //            Age = 21,
    //    //            Name = "Tony"
    //    //        };

    //    //        List<object> normalizedPropertyData = new List<object>() { "Name" };

    //    //        dataStructureObject = normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData);
    //    //    }

    //    //    [Fact]
    //    //    public void It_Should_Return_The_Property_Value()
    //    //    {
    //    //        Assert.Equal("Tony", (string)dataStructureObject);
    //    //    }
    //    //}

    //    //public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_Missing_In_The_NormalizedPropertyData
    //    //{
    //    //    private readonly Normalizer normalizer;
    //    //    private readonly Animal animal;
    //    //    private readonly List<object> normalizedPropertyData;

    //    //    public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_Missing_In_The_NormalizedPropertyData()
    //    //    {
    //    //        normalizer = A.Fake<Normalizer>();

    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //    //        animal = new Animal()
    //    //        {
    //    //            AnimalId = 101,
    //    //            Type = "Tiger",
    //    //            Age = 21,
    //    //            Name = "Tony"
    //    //        };

    //    //        normalizedPropertyData = new List<object>() { "Type", "Hippo" };
    //    //    }

    //    //    [Fact]
    //    //    public void It_Should_Throw_An_Exception()
    //    //    {
    //    //        Assert.Throws(typeof(InvalidOperationException), () => normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData));
    //    //    }
    //    //}

    //    //public class When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_In_The_NormalizedPropertyData
    //    //{
    //    //    private readonly Normalizer normalizer;
    //    //    private readonly object dataStructureObject;

    //    //    public When_Calling_GetNormalizedItemPropertyObject_For_A_Property_With_Its_Value_In_The_NormalizedPropertyData()
    //    //    {
    //    //        normalizer = A.Fake<Normalizer>();

    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<Animal>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //    //        Animal animal = new Animal()
    //    //        {
    //    //            AnimalId = 101,
    //    //            Type = "Tiger",
    //    //            Age = 21,
    //    //            Name = "Tony"
    //    //        };

    //    //        List<object> normalizedPropertyData = new List<object>() { "Type", "Tiger", "Hippo" };

    //    //        dataStructureObject = normalizer.GetNormalizedItemPropertyObject(animal, normalizedPropertyData);
    //    //    }

    //    //    [Fact]
    //    //    public void It_Should_Return_The_Index_Of_The_Property_Value()
    //    //    {
    //    //        Assert.Equal(1, (int)dataStructureObject);
    //    //    }
    //    //}

    //    //public class When_Calling_GetNormalizedItemPropertyObject_For_A_SubProperty_From_Another_Property
    //    //{
    //    //    private readonly Normalizer normalizer;
    //    //    private readonly Book book;
    //    //    private readonly object dataStructureObject;

    //    //    public When_Calling_GetNormalizedItemPropertyObject_For_A_SubProperty_From_Another_Property()
    //    //    {
    //    //        normalizer = A.Fake<Normalizer>();

    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<BookMark>.Ignored, A<List<object>>.Ignored)).CallsBaseMethod();

    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<object>.Ignored, A<List<object>>.Ignored))
    //    //            .Returns(2);

    //    //        book = new Book()
    //    //        {
    //    //            Title = "Changes",
    //    //            Author = "Jim Butcher",
    //    //            PublishDate = new DateTime(2010, 4, 6),
    //    //            Series = "The Dresden Files",
    //    //            PurchaseYear = 2016,
    //    //            PurchaseLocation = "Amazon",
    //    //            HasRead = false
    //    //        };

    //    //        BookMark bookMark = new BookMark()
    //    //        {
    //    //            CurrentPage = 300,
    //    //            Book = book
    //    //        };

    //    //        List<object> normalizedPropertyData = new List<object>() { "Book.Title.Test" };

    //    //        dataStructureObject = normalizer.GetNormalizedItemPropertyObject(bookMark, normalizedPropertyData);
    //    //    }

    //    //    [Fact]
    //    //    public void It_Should_Call_GetNormalizedItemPropertyObject_For_The_Subproperty()
    //    //    {
    //    //        A.CallTo(() => normalizer.GetNormalizedItemPropertyObject(A<object>.Ignored, A<List<object>>.Ignored))
    //    //            .WhenArgumentsMatch(args =>  args[0] == book && ((List<object>)args[1])[0].ToString() == "Title.Test")
    //    //            .MustHaveHappened(Repeated.Exactly.Once);
    //    //    }

    //    //    [Fact]
    //    //    public void It_Should_Return_The_Value_From_Calling_GetNormalizedItemPropertyObject()
    //    //    {
    //    //        Assert.Equal(2, dataStructureObject);
    //    //    }
    //    //}
    //    #endregion

    //    #region GetNormalizedItemPropertyObjectForComplexProperty

    //    #endregion

    //    #region GetNormalizedItemPropertyObjectForListProperty

    //    #endregion
}
