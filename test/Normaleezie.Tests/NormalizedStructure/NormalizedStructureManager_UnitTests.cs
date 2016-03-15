using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Normaleezie.Helpers;
using Normaleezie.NormalizedStructure;
using Normaleezie.Tests.Test_Classes;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Unit_NormalizedStructureManager
{
    #region CreateNormalizedData

    public class CreateNormalizedStructure
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public CreateNormalizedStructure()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructure(A<List<Animal>>.Ignored, A<List<List<object>>>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedList_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.CreateNormalizedStructure<Animal>(null, new List<List<object>>()));
        }

        [Fact]
        public void Should_Return_An_Empty_List_When_The_DenormalizedList_Is_Empty()
        {
            List<Animal> denormalizedList = new List<Animal>();
            List<List<object>> normalizedData = new List<List<object>>();

            List<List<object>> normalizedStructure = normalizedStructureManager.CreateNormalizedStructure(denormalizedList, normalizedData);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .MustNotHaveHappened();

            Assert.Equal(new List<List<object>>(), normalizedStructure);
        }

        [Fact]
        public void Should_Call_CreateNormalizedStructureItem_For_Each_Item_In_The_DenormalizedList()
        {
            List<List<object>> createNormalizedStructureItemReturn = new List<List<object>>()
            {
                new List<object>(),
                new List<object>()
            };

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .ReturnsNextFromSequence(createNormalizedStructureItemReturn.ToArray());

            List<Animal> denormalizedList = new List<Animal>()
            {
                new Animal() {AnimalId = 1},
                new Animal() {AnimalId = 2}
            };
            List<List<object>> normalizedData = new List<List<object>>();

            List<List<object>> normalizedStructure = normalizedStructureManager.CreateNormalizedStructure(denormalizedList, normalizedData);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(denormalizedList.Count));

            foreach (var denormalizedItem in denormalizedList)
            {
                A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                    .WhenArgumentsMatch(args => args[0] == denormalizedItem && args[1] == normalizedData)
                    .MustHaveHappened(Repeated.Exactly.Once);
            }

            Assert.Equal(createNormalizedStructureItemReturn[0], normalizedStructure[0]);
            Assert.Equal(createNormalizedStructureItemReturn[1], normalizedStructure[1]);
        }
    }

    #endregion

    #region CreateNormalizedStructureItem

    public class CreateNormalizedStructureItem
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public CreateNormalizedStructureItem()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<Animal>.Ignored, A<List<List<object>>>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedData_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.CreateNormalizedStructureItem(new Animal(), null));
        }

        [Fact]
        public void Should_Return_An_Empty_List_When_The_NormalizedData_Is_Empty()
        {
            Animal denormalizedItem = new Animal();
            List<List<object>> normalizedData = new List<List<object>>();

            List<object> normalizedStructureItem = normalizedStructureManager.CreateNormalizedStructureItem(denormalizedItem, normalizedData);

            A.CallTo(() => normalizedStructureManager.GetNormalizedField(A<object>.Ignored, A<List<object>>.Ignored))
                .MustNotHaveHappened();

            Assert.Equal(new List<object>(), normalizedStructureItem);
        }

        [Fact]
        public void Should_Call_GetNormalizedField_For_Each_Item_In_The_NormalizedData()
        {
            List<object> getNormalizedFieldReturn = new List<object>()
            {
                "Test",
                42
            };

            A.CallTo(() => normalizedStructureManager.GetNormalizedField(A<object>.Ignored, A<List<object>>.Ignored))
                .ReturnsNextFromSequence(getNormalizedFieldReturn.ToArray());

            Animal denormalizedItem = new Animal() { AnimalId = 1 };

            List<List<object>> normalizedData = new List<List<object>>()
            {
                new List<object>() {"Name" },
                new List<object>() {"Age" }
            };

            List<object> normalizedStructureItem = normalizedStructureManager.CreateNormalizedStructureItem(denormalizedItem, normalizedData);

            A.CallTo(() => normalizedStructureManager.GetNormalizedField(A<object>.Ignored, A<List<object>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(normalizedData.Count));

            foreach (var normalizedDataItem in normalizedData)
            {
                A.CallTo(() => normalizedStructureManager.GetNormalizedField(A<object>.Ignored, A<List<object>>.Ignored))
                    .WhenArgumentsMatch(args => args[0] == denormalizedItem && args[1] == normalizedDataItem)
                    .MustHaveHappened(Repeated.Exactly.Once);
            }

            Assert.Equal(getNormalizedFieldReturn, normalizedStructureItem);
        }
    }

    #endregion

    #region GetNormalizedField

    public class GetNormalizedField
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public GetNormalizedField()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.GetNormalizedField(A<Animal>.Ignored, A<List<object>>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedDataItem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedField(new Animal(), null));
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedDataItem_Is_Empty()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedField(new Animal(), new List<object>()));
        }

        [Fact]
        public void Should_Return_Data_From_GetNormalizedFieldForComplexType_When_The_DataName_Ends_With_A_Period()
        {
            object getNormalizedFieldForComplexTypeReturn = "ReturnObject";

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForComplexType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .Returns(getNormalizedFieldForComplexTypeReturn);

            Animal denormalizedItem = new Animal() { AnimalId = 1 };

            List<object> normalizedDataItem = new List<object>() {"Name."};

            object normalizedField = normalizedStructureManager.GetNormalizedField(denormalizedItem, normalizedDataItem);

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForComplexType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            Assert.Equal(getNormalizedFieldForComplexTypeReturn, normalizedField);
        }

        [Fact]
        public void Should_Return_Data_From_GetNormalizedFieldForIEnumerableType_When_The_DataName_Ends_With_A_Tilda()
        {
            object getNormalizedFieldForIEnumerableTypeReturn = "ReturnObject";

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .Returns(getNormalizedFieldForIEnumerableTypeReturn);

            Animal denormalizedItem = new Animal() { AnimalId = 1 };

            List<object> normalizedDataItem = new List<object>() { "Name~" };

            object normalizedField = normalizedStructureManager.GetNormalizedField(denormalizedItem, normalizedDataItem);

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForComplexType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            Assert.Equal(getNormalizedFieldForIEnumerableTypeReturn, normalizedField);
        }

        [Fact]
        public void Should_Return_Data_From_GetNormalizedFieldForSimpleType_When_The_DataName_Does_Not_End_With_A_Tilda_Or_Period()
        {
            object getNormalizedFieldForSimpleTypeReturn = "ReturnObject";

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .Returns(getNormalizedFieldForSimpleTypeReturn);

            Animal denormalizedItem = new Animal() { AnimalId = 1 };

            List<object> normalizedDataItem = new List<object>() { "Name" };

            object normalizedField = normalizedStructureManager.GetNormalizedField(denormalizedItem, normalizedDataItem);

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForComplexType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustNotHaveHappened();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<object>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(getNormalizedFieldForSimpleTypeReturn, normalizedField);
        }
    }

    #endregion

    #region GetNormalizedFieldForSimpleType

    public class GetNormalizedFieldForSimpleType
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public GetNormalizedFieldForSimpleType()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedItem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(null, new List<object>(), string.Empty));
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedDataItem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(new Animal(), null, string.Empty));
        }

        [Fact]
        public void Should_Return_The_Property_Value_When_The_NormalizedDataItem_Only_Has_One_Item()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Name" };

            object normalizedField = normalizedStructureManager.GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, "Name");

            Assert.Equal(denormalizedItem.Name, normalizedField);
        }

        [Fact]
        public void Should_Return_The_NormalizedDataItem_Index_When_The_NormalizedDataItem_Contains_The_DenormalizedItem_Value()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Name", "Beth", "Fluffy", "Tony" };

            object normalizedField = normalizedStructureManager.GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, "Name");

            Assert.Equal(2, normalizedField);
        }

        [Fact]
        public void Should_Use_The_DenormalizedItem_When_DataName_Is_Empty()
        {
            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(A<int>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();

            int denormalizedItem = 100;

            List<object> normalizedDataItem = new List<object>() { "Name", 1, 10, 100 };

            object normalizedField = normalizedStructureManager.GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, string.Empty);

            Assert.Equal(3, normalizedField);
        }

        [Fact]
        public void Should_Throw_An_Exception_When_The_NormalizedDataItem_Does_Not_Contain_The_DenormalizedItem_Value()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Name", "Beth", "Tony" };

            Assert.Throws<InvalidOperationException>(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, "Name"));
        }

        [Fact]
        public void Should_Throw_An_Exception_When_The_DataName_Is_Not_A_Property_Of_The_DenormalizedItem()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Guerilla", "Beth", "Tony" };

            Assert.ThrowsAny<Exception>(() => normalizedStructureManager.GetNormalizedFieldForSimpleType(denormalizedItem, normalizedDataItem, "Guerilla"));
        }
    }

    #endregion

    #region GetNormalizedFieldForIEnumerableType

    public class GetNormalizedFieldForIEnumerableType
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public GetNormalizedFieldForIEnumerableType()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedDataItem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedFieldForIEnumerableType(new Animal(), null, "John"));
        }

        [Fact]
        public void Should_Call_CreateNormalizedStructureItem_For_Each_Item_Returned_From_Calling_GetTargetDenormalizedItemByDataName()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Name", new List<object>() {"DataName"}, new List<object>() { "DataNameNumberTwo", "SomeData" } };

            string dataName = "Name";

            List<object> getTargetDenormalizedItemByDataNameReturn = new List<object>()
            {
                "First",
                "Second"
            };

            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .Returns(getTargetDenormalizedItemByDataNameReturn);

            List<List<object>> createNormalizedStructureItemReturn = new List<List<object>>()
            {
                new List<object>(),
                new List<object>()
            };

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .ReturnsNextFromSequence(createNormalizedStructureItemReturn.ToArray());

            object normalizedField = normalizedStructureManager.GetNormalizedFieldForIEnumerableType(denormalizedItem, normalizedDataItem, dataName);

            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .WhenArgumentsMatch(args => args[0] == denormalizedItem && (string)args[1] == dataName && (char)args[2] == '~')
                .MustHaveHappened(Repeated.Exactly.Once);
            
            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Times(getTargetDenormalizedItemByDataNameReturn.Count));

            foreach (var denormalizedItemFromList in getTargetDenormalizedItemByDataNameReturn)
            {
                A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                    .WhenArgumentsMatch(args =>
                    {
                        return args[0] == denormalizedItemFromList &&
                               (string) ((List<List<object>>) args[1])[0][0] == "DataName" &&
                               (string)((List<List<object>>)args[1])[1][0] == "DataNameNumberTwo" &&
                               (string)((List<List<object>>)args[1])[1][1] == "SomeData";
                    })
                    .MustHaveHappened(Repeated.Exactly.Once);
            }

            Assert.Equal(createNormalizedStructureItemReturn, normalizedField);
        }
    }

    #endregion

    #region GetNormalizedFieldForComplexType

    public class GetNormalizedFieldForComplexType
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public GetNormalizedFieldForComplexType()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            A.CallTo(() => normalizedStructureManager.GetNormalizedFieldForComplexType(A<Animal>.Ignored, A<List<object>>.Ignored, A<string>.Ignored))
                .CallsBaseMethod();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_NormalizedDataItem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetNormalizedFieldForComplexType(new Animal(), null, "John"));
        }

        [Fact]
        public void Should_Call_CreateNormalizedStructureItem_For_Each_Item_Returned_From_Calling_GetDenormalizedListForIEnumerableType()
        {
            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            List<object> normalizedDataItem = new List<object>() { "Name", new List<object>() { "DataName" }, new List<object>() { "DataNameNumberTwo", "SomeData" } };

            string dataName = "Name";

            object getTargetDenormalizedItemByDataNameReturn = "Abominable";

            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .Returns(getTargetDenormalizedItemByDataNameReturn);

            List<object> createNormalizedStructureItemReturn = new List<object>();

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                .Returns(createNormalizedStructureItemReturn);

            object normalizedField = normalizedStructureManager.GetNormalizedFieldForComplexType(denormalizedItem, normalizedDataItem, dataName);

            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .WhenArgumentsMatch(args => args[0] == denormalizedItem && (string)args[1] == dataName && (char)args[2] == '.')
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureItem(A<object>.Ignored, A<List<List<object>>>.Ignored))
                    .WhenArgumentsMatch(args =>
                    {
                        return args[0] == getTargetDenormalizedItemByDataNameReturn &&
                               (string)((List<List<object>>)args[1])[0][0] == "DataName" &&
                               (string)((List<List<object>>)args[1])[1][0] == "DataNameNumberTwo" &&
                               (string)((List<List<object>>)args[1])[1][1] == "SomeData";
                    })
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.Equal(createNormalizedStructureItemReturn, normalizedField);
        }
    }

    #endregion

    #region GetTargetDenormalizedItemByDataName

    public class GetTargetDenormalizedItemByDataName
    {
        private readonly NormalizedStructureManager normalizedStructureManager;

        public GetTargetDenormalizedItemByDataName()
        {
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DenormalizedItem_Is_Null()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(null, "John", '~'));
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DataName_Is_Null()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(new Animal(), null, '~'));
        }

        [Fact]
        public void Should_Throw_An_ArgumentNullException_When_The_DataName_Is_Empty()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            Assert.Throws<ArgumentNullException>(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(new Animal(), string.Empty, '~'));
        }

        [Fact]
        public void Should_Throw_An_Exception_When_The_DataName_Is_Not_A_Property_Of_The_DenormalizedItem()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Animal>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            Animal denormalizedItem = new Animal() { Name = "Fluffy" };

            Assert.ThrowsAny<Exception>(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(denormalizedItem, "Guerilla~", '~'));
        }

        [Fact]
        public void Should_Return_The_DenormalizedItem_When_DataName_Is_Only_The_SuffixSymbol()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<List<Animal>>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            List<Animal> denormalizedItem = new List<Animal>() { new Animal() { Name = "Fluffy" } };

            object denormalizedList = normalizedStructureManager.GetTargetDenormalizedItemByDataName(denormalizedItem, "~", '~');

            Assert.Equal(denormalizedItem, denormalizedList);
        }

        [Fact]
        public void Should_Return_The_Property_In_The_DenormalizedItem_When_DataName_Is_Not_Only_The_SuffixSymbol()
        {
            A.CallTo(() => normalizedStructureManager.GetTargetDenormalizedItemByDataName(A<Zoo>.Ignored, A<string>.Ignored, A<char>.Ignored))
                .CallsBaseMethod();

            Zoo denormalizedItem = new Zoo() { Animals = new List<Animal>() { new Animal() { Name = "Fluffy" } } };

            object denormalizedList = normalizedStructureManager.GetTargetDenormalizedItemByDataName(denormalizedItem, "Animals~", '~');

            Assert.Equal(denormalizedItem.Animals, denormalizedList);
        }
    }

    #endregion
}
