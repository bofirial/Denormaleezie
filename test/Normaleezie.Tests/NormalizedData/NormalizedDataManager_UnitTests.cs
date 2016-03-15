using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Normaleezie.Helpers;
using Normaleezie.NormalizedData;
using Normaleezie.NormalizedStructure;
using Normaleezie.Tests.Test_Classes;
using Xunit;
using Xunit.Extensions;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Unit_NormalizedDataManager
{
    #region CreateNormalizedData

    public class CreateNormalizedData
    {
        private readonly NormalizedDataManager normalizedDataManager;
        private readonly ReflectionHelper reflectionHelper;

        public CreateNormalizedData()
        {
            reflectionHelper = A.Fake<ReflectionHelper>();
            normalizedDataManager = A.Fake<NormalizedDataManager>(x => x.WithArgumentsForConstructor(() => new NormalizedDataManager(reflectionHelper)));

        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_List_Is_Null()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            Assert.Throws<ArgumentNullException>(() => normalizedDataManager.CreateNormalizedData<object>(null));
        }

        [Fact]
        public void Should_Return_Normalized_Data_Containing_Only_The_DataName_When_The_List_Is_Empty()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedData(new List<object>());

            Assert.Equal(new List<object>() { string.Empty }, normalizedData[0]);
        }

        [Fact]
        public void Should_Check_For_Circular_References()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedData(new List<object>() {"test"});

            List<string> ignoredStringList = A<List<string>>.Ignored;

            A.CallTo(() => normalizedDataManager.CheckForCircularReferences(ref ignoredStringList, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_Call_CreateNormalizedDataForListOfIEnumerableType_When_IsIEnumerableType_Returns_True()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .Returns(true);

            List<List<object>> createNormalizedDataForListOfIEnumerableTypeReturn = new List<List<object>>() {new List<object>() {"Return Data"}};

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(createNormalizedDataForListOfIEnumerableTypeReturn);

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedData(new List<object>() { "test" });
            
            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type) args[0] == typeof(object))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedDataForListOfIEnumerableTypeReturn, normalizedData);
        }

        [Fact]
        public void Should_Call_CreateNormalizedDataForListOfComplexType_When_IsSimpleType_Returns_False()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .Returns(false);

            A.CallTo(() => reflectionHelper.IsSimpleType(A<Type>.Ignored))
                .Returns(false);

            List<List<object>> createNormalizedDataForListOfComplexTypeReturn = new List<List<object>>() { new List<object>() { "Return Data" } };

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfComplexType(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(new List<List<object>>() { new List<object>() { "Return Data" } });

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedData(new List<object>() { "test" });

            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == typeof(object))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => reflectionHelper.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == typeof(object))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfComplexType(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedDataForListOfComplexTypeReturn, normalizedData);
        }

        [Fact]
        public void Should_Call_CreateNormalizedDataForListOfSimpleType_When_IsSimpleType_Returns_True()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .Returns(false);

            A.CallTo(() => reflectionHelper.IsSimpleType(A<Type>.Ignored))
                .Returns(true);

            List<List<object>> createNormalizedDataForListOfSimpleTypeReturn = new List<List<object>>() { new List<object>() { "Return Data" } };

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfSimpleType(A<List<object>>.Ignored, A<string>.Ignored))
                .Returns(new List<List<object>>() { new List<object>() { "Return Data" } });

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedData(new List<object>() { "test" });

            A.CallTo(() => reflectionHelper.IsIEnumerableType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == typeof(object))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => reflectionHelper.IsSimpleType(A<Type>.Ignored))
                .WhenArgumentsMatch(args => (Type)args[0] == typeof(object))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfSimpleType(A<List<object>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedDataForListOfSimpleTypeReturn, normalizedData);
        }
    }

    #endregion

    #region CreateNormalizedDataForListOfSimpleType
    
    public class CreateNormalizedDataForListOfSimpleType
    {
        private readonly NormalizedDataManager normalizedDataManager;

        public CreateNormalizedDataForListOfSimpleType()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfSimpleType(A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedList_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedDataManager.CreateNormalizedDataForListOfSimpleType<string>(null, string.Empty));
        }

        [Fact]
        public void Should_Contain_The_DataName()
        {
            string dataName = "soliloquy";
            List<string> denormalizedData = new List<string>();

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfSimpleType(denormalizedData, dataName);

            Assert.Equal(dataName, normalizedData[0][0]);
        }

        [Fact]
        public void Should_Contain_The_Unique_Values_When_The_DenormalizedList_Contains_Duplicates()
        {
            string dataName = "soliloquy";
            List<string> denormalizedData = new List<string>() {"This", "List", "Contains", "Some", "Duplicate", "Items", "Duplicate", "Items"};

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfSimpleType(denormalizedData, dataName);

            Assert.Equal(new List<object>() { "soliloquy", "This", "List", "Contains", "Some", "Duplicate", "Items" }, normalizedData[0]);
        }

        [Fact]
        public void Should_Not_Contain_Values_When_The_DenormalizedList_Does_Not_Contains_Duplicates()
        {
            string dataName = "soliloquy";
            List<string> denormalizedData = new List<string>() { "This", "List", "Does", "Not", "Contain", "Duplicate", "Items" };

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfSimpleType(denormalizedData, dataName);

            Assert.Equal(new List<object>() { "soliloquy" }, normalizedData[0]);
        }
    }

    #endregion

    #region CreateNormalizedDataForListOfComplexType

    public class CreateNormalizedDataForListOfComplexType
    {
        private readonly NormalizedDataManager normalizedDataManager;

        public CreateNormalizedDataForListOfComplexType()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfComplexType(A<List<Animal>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedList_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedDataManager.CreateNormalizedDataForListOfComplexType<Animal>(null, new List<string>(), string.Empty));
        }

        [Fact]
        public void Should_Return_The_Formatted_Data_From_Calling_CallCreateNormalizedDataGenerically_For_Each_Property_In_T()
        {
            List<List<List<object>>> callCreateNormalizedDataGenericallyReturns = new List<List<List<object>>>()
            {
                new List<List<object>>(),
                new List<List<object>>(),
                new List<List<object>>(),
                new List<List<object>>()
            };

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .ReturnsNextFromSequence(callCreateNormalizedDataGenericallyReturns.ToArray());

            List<List<object>> formatNormalizedDataForListOfComplexTypeReturn = new List<List<object>>();

            A.CallTo(() => normalizedDataManager.FormatNormalizedDataForListOfComplexType(A<string>.Ignored, A<List<List<object>>>.Ignored))
                .Returns(formatNormalizedDataForListOfComplexTypeReturn);

            List<Animal> denormalizedData = new List<Animal>()
            {
                new Animal() {Age = 2, AnimalId = 1, Name = "Fluffy", Type = "Bird"}
            };

            List<string> previousDataNames = new List<string>();
            string dataName = "soliloquy";

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfComplexType(denormalizedData, previousDataNames, dataName);

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(4));

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args => (string)args[2] == "AnimalId" && (int)(((List<object>)args[0])[0]) == 1 && args[1] == previousDataNames )
                .MustHaveHappened();

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args => (string)args[2] == "Age" && (int)(((List<object>)args[0])[0]) == 2 && args[1] == previousDataNames)
                .MustHaveHappened();

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args => (string)args[2] == "Name" && (string)(((List<object>)args[0])[0]) == "Fluffy" && args[1] == previousDataNames)
                .MustHaveHappened();

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args => (string)args[2] == "Type" && (string)(((List<object>)args[0])[0]) == "Bird" && args[1] == previousDataNames)
                .MustHaveHappened();

            A.CallTo(() => normalizedDataManager.FormatNormalizedDataForListOfComplexType(A<string>.Ignored, A<List<List<object>>>.Ignored))
                .WhenArgumentsMatch(args => (string) args[0] == dataName)
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(formatNormalizedDataForListOfComplexTypeReturn, normalizedData);
        }

    }

    #endregion

    #region FormatNormalizedDataForListOfComplexType

    public class FormatNormalizedDataForListOfComplexType
    {
        private readonly NormalizedDataManager normalizedDataManager;

        public FormatNormalizedDataForListOfComplexType()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();

            A.CallTo(() => normalizedDataManager.FormatNormalizedDataForListOfComplexType(A<string>.Ignored, A< List<List<object>>>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Return_The_NormalizedDataByProperty_When_DataName_Is_Null()
        {
            string dataName = null;
            List<List<object>> normalizedDataByProperty = new List<List<object>>();

            List<List<object>> normalizedData = normalizedDataManager.FormatNormalizedDataForListOfComplexType(dataName, normalizedDataByProperty);

            Assert.Equal(normalizedDataByProperty, normalizedData);
        }

        [Fact]
        public void Should_Return_The_NormalizedDataByProperty_When_DataName_Is_Empty()
        {
            string dataName = string.Empty;
            List<List<object>> normalizedDataByProperty = new List<List<object>>();

            List<List<object>> normalizedData = normalizedDataManager.FormatNormalizedDataForListOfComplexType(dataName, normalizedDataByProperty);

            Assert.Equal(normalizedDataByProperty, normalizedData);
        }

        [Fact]
        public void Should_Return_The_NormalizedData_With_The_DataName_When_DataName_Is_Not_Null_Or_Empty()
        {
            string dataName = "Desperado";
            List<List<object>> normalizedDataByProperty = new List<List<object>>()
            {
                new List<object>() { "1st", 201 },
                new List<object>() { "2nd", 1502 }
            };

            List<List<object>> normalizedData = normalizedDataManager.FormatNormalizedDataForListOfComplexType(dataName, normalizedDataByProperty);

            Assert.Equal("Desperado.", normalizedData[0][0]);
            Assert.Equal(normalizedDataByProperty[0], normalizedData[0][1]);
            Assert.Equal(normalizedDataByProperty[1], normalizedData[0][2]);
        }

    }

    #endregion

    #region CreateNormalizedDataForListOfIEnumerableType

    public class CreateNormalizedDataForListOfIEnumerableType
    {
        private readonly NormalizedDataManager normalizedDataManager;

        public CreateNormalizedDataForListOfIEnumerableType()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();

            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(A<List<List<Animal>>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedList_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType<List<Animal>>(null, new List<string>(), string.Empty));
        }

        [Fact]
        public void Should_Call_CallCreateNormalizedDataGenerically_With_The_Data_In_The_List()
        {
            List<List<object>> callCreateNormalizedDataGenericallyReturn = new List<List<object>>();

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .Returns(callCreateNormalizedDataGenericallyReturn);

            List<List<Animal>> denormalizedList = new List<List<Animal>>()
            {
                new List<Animal>() { new Animal() { Age=1, Name = "First"} },
                new List<Animal>() { new Animal() { Age=2, Name = "Second"}, new Animal() { Age = 3, Name = "Third" } }
            };
            List<string> previousDataNames = new List<string>();
            string dataName = "Astronomical";

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(denormalizedList, previousDataNames, dataName);

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args =>
                {
                    return  ((List<object>)args[0])[0] == denormalizedList[0][0] &&
                            ((List<object>)args[0])[1] == denormalizedList[1][0] &&
                            ((List<object>)args[0])[2] == denormalizedList[1][1] &&
                            args[1] == previousDataNames &&
                            (string) args[2] == string.Empty &&
                            (Type) args[3] == typeof (Animal);
                })
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_Call_CallCreateNormalizedDataGenerically_With_The_Data_In_The_List_When_The_List_Is_Of_A_Simple_Type()
        {
            A.CallTo(() => normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(A<List<List<int>>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            List<List<object>> callCreateNormalizedDataGenericallyReturn = new List<List<object>>();

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .Returns(callCreateNormalizedDataGenericallyReturn);

            List<List<int>> denormalizedList = new List<List<int>>()
            {
                new List<int>() { 1 },
                new List<int>() { 1, 2 },
            };
            List<string> previousDataNames = new List<string>();
            string dataName = "Astronomical";

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(denormalizedList, previousDataNames, dataName);

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .WhenArgumentsMatch(args =>
                {
                    return (int)((List<object>)args[0])[0] == denormalizedList[0][0] &&
                            (int)((List<object>)args[0])[1] == denormalizedList[1][0] &&
                            (int)((List<object>)args[0])[2] == denormalizedList[1][1] &&
                            args[1] == previousDataNames &&
                            (string) args[2] == string.Empty &&
                            (Type)args[3] == typeof(int);
                })
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Should_Return_The_NormalizedData_Containing_The_Returned_List_From_CallCreateNormalizedDataGenerically()
        {
            List<List<object>> callCreateNormalizedDataGenericallyReturn = new List<List<object>>()
            {
                new List<object>(),
                new List<object>()
            };

            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .Returns(callCreateNormalizedDataGenericallyReturn);

            List<List<Animal>> denormalizedList = new List<List<Animal>>()
            {
                new List<Animal>() { new Animal() { Age=1, Name = "First"} },
                new List<Animal>() { new Animal() { Age=2, Name = "Second"}, new Animal() { Age = 3, Name = "Third" } }
            };
            List<string> previousDataNames = new List<string>();
            string dataName = "Astronomical";

            List<List<object>> normalizedData = normalizedDataManager.CreateNormalizedDataForListOfIEnumerableType(denormalizedList, previousDataNames, dataName);
            
            Assert.Equal(dataName + "~", normalizedData[0][0]);
            Assert.Equal(callCreateNormalizedDataGenericallyReturn[0], normalizedData[0][1]);
            Assert.Equal(callCreateNormalizedDataGenericallyReturn[1], normalizedData[0][2]);
        }
    }
    #endregion

    #region CallCreateNormalizedDataGenerically

    public class CallCreateNormalizedDataGenerically
    {
        private readonly NormalizedDataManager normalizedDataManager;
        private readonly ReflectionHelper reflectionHelper;

        public CallCreateNormalizedDataGenerically()
        {
            reflectionHelper = A.Fake<ReflectionHelper>();
            normalizedDataManager = A.Fake<NormalizedDataManager>(x => x.WithArgumentsForConstructor(() => new NormalizedDataManager(reflectionHelper)));
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_Type_Is_Null()
        {
            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .CallsBaseMethod();

            Assert.Throws<ArgumentNullException>(() => normalizedDataManager.CallCreateNormalizedDataGenerically(new List<object>(), new List<string>(), string.Empty, null));
        }

        /// <summary>
        /// This is needed because exceptions thrown through Reflection get wrapped in an exception...
        /// </summary>
        [Fact]
        public void Should_Throw_The_Exception_When_CreateNormalizedData_Throws_An_Exception()
        {
            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .CallsBaseMethod();

            Exception exception = new Exception("This is the Exception!");

            List<string> convertListReturn = new List<string>();

            A.CallTo(() => reflectionHelper.ConvertList<string>(A<List<object>>.Ignored))
                .Returns(convertListReturn);

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<string>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Throws(ex => exception);

            List<object> denormalizedData = new List<object>() { };
            List<string> previousDataNames = new List<string>();
            string dataName = "soliloquy";
            Type type = typeof(string);

            Exception thrownException = Assert.Throws<Exception>(() => normalizedDataManager.CallCreateNormalizedDataGenerically(denormalizedData, previousDataNames, dataName, type));

            Assert.Equal(exception, thrownException);
        }

        [Fact]
        public void Should_Call_ConvertList_And_CreateNormalizedData_With_The_Correct_Generic_Type__String()
        {
            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .CallsBaseMethod();

            List<string> convertListReturn = new List<string>();

            A.CallTo(() => reflectionHelper.ConvertList<string>(A<List<object>>.Ignored))
                .Returns(convertListReturn);

            List<List<object>> createNormalizedDataReturn = new List<List<object>>();

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<string>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(createNormalizedDataReturn);

            List<object> denormalizedData = new List<object>() {};
            List<string> previousDataNames = new List<string>();
            string dataName = "soliloquy";
            Type type = typeof (string);

            List<List<object>> normalizedData = normalizedDataManager.CallCreateNormalizedDataGenerically(denormalizedData, previousDataNames, dataName, type);

            A.CallTo(() => reflectionHelper.ConvertList<string>(A<List<object>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == denormalizedData)
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<string>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == convertListReturn && args[1] == previousDataNames && (string) args[2] == dataName)
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedDataReturn, normalizedData);
        }

        [Fact]
        public void Should_Call_ConvertList_And_CreateNormalizedData_With_The_Correct_Generic_Type__Animal()
        {
            A.CallTo(() => normalizedDataManager.CallCreateNormalizedDataGenerically(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored, A<Type>.Ignored))
                .CallsBaseMethod();

            List<Animal> convertListReturn = new List<Animal>();

            A.CallTo(() => reflectionHelper.ConvertList<Animal>(A<List<object>>.Ignored))
                .Returns(convertListReturn);

            List<List<object>> createNormalizedDataReturn = new List<List<object>>();

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<Animal>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(createNormalizedDataReturn);

            List<object> denormalizedData = new List<object>() { };
            List<string> previousDataNames = new List<string>();
            string dataName = "soliloquy";
            Type type = typeof(Animal);

            List<List<object>> normalizedData = normalizedDataManager.CallCreateNormalizedDataGenerically(denormalizedData, previousDataNames, dataName, type);

            A.CallTo(() => reflectionHelper.ConvertList<Animal>(A<List<object>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == denormalizedData)
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<Animal>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == convertListReturn && args[1] == previousDataNames && (string)args[2] == dataName)
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedDataReturn, normalizedData);
        }
    }

    #endregion

    #region CheckForCircularReferences

    public class CheckForCircularReferences
    {
        private readonly NormalizedDataManager normalizedDataManager;

        public CheckForCircularReferences()
        {
            normalizedDataManager = new NormalizedDataManager();
        }

        public static IEnumerable<object[]> ValidScenarioArguments = new[]
        {
            new object[] {new List<string> {}, "Test" },
            new object[] {null, "Test" },
            new object[] {new List<string> { "Test" }, null },
            new object[] {new List<string> { "Test" }, string.Empty },
            new object[] {new List<string> { "Test", "Test", "Test", "Test", "Test" }, "Test" },
            new object[] {new List<string>
            {
                "Test1",
                "Test2",
                "Test3",
                "Test4",
                "Test5",
                "Test6",
                "Test7",
                "Test8",
                "Test9",
                "Test10",
                "Test11",
                "Test12",
                "Test13",
                "Test14",
                "Test15",
                "Test16",
                "Test17",
                "Test18",
                "Test19",
                "Test20",
                "Test21",
                "Test22",
                "Test23",
                "Test24",
                "Test25",
                "Test26",
                "Test27",
                "Test28",
                "Test29",
                "Test30"
            }, "Test" },
            new object[] {new List<string>
            {
                "Test",
                "Test",
                "Test",
                "Test",
                "Test"
            }, "Test" }
        };

        [Theory]
        [MemberData("ValidScenarioArguments")]
        public void Should_NOT_Throw_An_Exception_When_There_Is_No_Circular_Reference(List<string> previousDataNames, string dataName )
        {
            normalizedDataManager.CheckForCircularReferences(ref previousDataNames, dataName);

            Assert.Equal(dataName, previousDataNames.Last());
        }

        public static IEnumerable<object[]> InvalidScenarioArguments = new[]
        {
            new object[] {new List<string>
            {
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
                "Test",
            }, "Test" }
        };

        [Theory]
        [MemberData("InvalidScenarioArguments")]
        public void Should_Throw_An_Exception_When_Parameters_Indicate_There_Is_A_Circular_Reference(List<string> previousDataNames, string dataName)
        {
            Exception exception = Assert.Throws<Exception>(() => normalizedDataManager.CheckForCircularReferences(ref previousDataNames, dataName));

            Assert.Equal("Circular Reference Detected in object.", exception.Message);
        }
    }

    #endregion
}
