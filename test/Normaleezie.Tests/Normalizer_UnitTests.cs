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

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
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
            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == null && (string) args[2] == string.Empty)
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
}
