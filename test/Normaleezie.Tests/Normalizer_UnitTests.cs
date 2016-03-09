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

namespace Unit_Normalizer
{
    #region Normalize

    public class Normalize
    {
        private readonly Normalizer normalizer;

        public Normalize()
        {
            normalizer = A.Fake<Normalizer>();
        }

        [Fact]
        public void Should_Return_An_Empty_List_When_The_DenormalizedList_Is_Null()
        {
            A.CallTo(() => normalizer.Normalize<List<object>>(null)).CallsBaseMethod();

            List<List<List<object>>> normalizedForm = normalizer.Normalize<List<object>>(null);

            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);
            
            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<object>>.Ignored))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Should_Return_An_Empty_List_When_The_DenormalizedList_Is_Empty()
        {
            A.CallTo(() => normalizer.Normalize(A<List<object>>.Ignored)).CallsBaseMethod();

            List<List<List<object>>> normalizedForm = normalizer.Normalize(new List<object>());

            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);
            Assert.Empty(normalizedForm);

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<object>>.Ignored))
                .MustNotHaveHappened();
        }
        
        [Fact]
        public void Should_Return_The_Value_From_ConvertToNormalizedForm()
        {
            A.CallTo(() => normalizer.Normalize(A<List<Animal>>.Ignored)).CallsBaseMethod();

            List<List<List<object>>> convertToNormalizedFormReturnValue = new List<List<List<object>>>()
            {
                new List<List<object>>()
                {
                    new List<object>()
                    {
                        "normaleezie"
                    }
                }
            };

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored)).Returns(convertToNormalizedFormReturnValue);

            List<Animal> animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1, Type = "Tiger", Age = 21, Name = "Tony"
                }
            };

            List<List<List<object>>> normalizedForm = normalizer.Normalize(animals);

            Assert.IsType(typeof(List<List<List<object>>>), normalizedForm);

            Assert.NotEmpty(normalizedForm);

            Assert.Equal(convertToNormalizedFormReturnValue, normalizedForm);

            A.CallTo(() => normalizer.ConvertToNormalizedForm(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    #endregion

    #region ConvertToNormalizedForm

    public class ConvertToNormalizedForm
    {
        private readonly Normalizer normalizer;
        private readonly NormalizedDataManager normalizedDataManager;
        private readonly NormalizedStructureManager normalizedStructureManager;

        public ConvertToNormalizedForm()
        {
            normalizedDataManager = A.Fake<NormalizedDataManager>();
            normalizedStructureManager = A.Fake<NormalizedStructureManager>();

            normalizer = new Normalizer(normalizedDataManager, normalizedStructureManager);

        }

        [Fact]
        public void Should_Return_The_NormalizedForm()
        {
            List<List<object>> normalizedDataList = new List<List<object>>()
            {
                new List<object>() {"Id"}, new List<object>() {"Name", "Tony", "Tania", "Talia"},
                new List<object>() {"Age", 21, 1, 3}, new List<object>() {"Type", "Tiger"}
            };

            List<List<object>> normalizedStructureList = new List<List<object>>()
            {
                new List<object>() {1, 1, 1, 1}, new List<object>() {2, 2, 2, 1}, new List<object>() {3, 3, 3, 1}
            };

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .Returns(normalizedDataList);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureList(A<List<object>>.Ignored, A<List<List<object>>>.Ignored))
                .Returns(normalizedStructureList);

            List<object> animals = new List<object>()
            {
                new Animal()
                {
                    AnimalId = 1, Type = "Tiger", Age = 21, Name = "Tony"
                }
            };

            List<List<List<object>>> normalizedForm = normalizer.ConvertToNormalizedForm(animals);

            Assert.Equal(normalizedDataList, normalizedForm[0]);
            Assert.Equal(normalizedStructureList, normalizedForm[1]);

            A.CallTo(() => normalizedDataManager.CreateNormalizedData(A<List<object>>.Ignored, A<List<string>>.Ignored, A<string>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == null && (string)args[2] == string.Empty)
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => normalizedStructureManager.CreateNormalizedStructureList(A<List<object>>.Ignored, A<List<List<object>>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == normalizedDataList)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    #endregion
}
