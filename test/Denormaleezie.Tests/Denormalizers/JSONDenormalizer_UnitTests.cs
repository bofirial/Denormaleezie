using Denormaleezie.Denormalizers;
using Denormaleezie.Tests.Test_Classes;
using FakeItEasy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy.Configuration;
using System.Reflection;

namespace Denormaleezie.Tests.Denormalizers
{
    #region DenormalizeToJSON
    public class When_Calling_DenormalizeToJSON_With_A_Null_Object
    {
        private string json;
        private JSONDenormalizer denormalizer;

        public When_Calling_DenormalizeToJSON_With_A_Null_Object()
        {

            denormalizer = new JSONDenormalizer();

            json = denormalizer.DenormalizeToJSON<List<object>>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_String()
        {
            Assert.Equal(string.Empty, json);
        }
    }

    public class When_Calling_DenormalizeToJSON_With_A_List
    {
        private string json;
        private JSONDenormalizer denormalizer;
        private List<Animal> animals;
        private IReturnValueArgumentValidationConfiguration<List<List<List<string>>>> aCallToConvertToDenormalizedList;
        private List<List<List<string>>> deserializedObject;

        public When_Calling_DenormalizeToJSON_With_A_List()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

            aCallToConvertToDenormalizedList = A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored));

            deserializedObject = new List<List<List<string>>>()
            {
                new List<List<string>>()
                {
                    new List<string>()
                    {
                        "deserializedObject"
                    }
                }
            };

            aCallToConvertToDenormalizedList.Returns(deserializedObject);

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

            json = denormalizer.DenormalizeToJSON(animals);
        }

        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Call_ConvertToStringList()
        {
            aCallToConvertToDenormalizedList.WhenArgumentsMatch(args => args[0] == animals).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Have_Denormalized_The_Object()
        {
            Assert.Equal(JsonConvert.SerializeObject(deserializedObject), json);
        }
    }

    public class When_Calling_DenormalizeToJSON_With_A_List_And_The_Denormalized_Object_Is_Larger_Than_The_JSON_Object
    {
        private string json;
        private JSONDenormalizer denormalizer;
        List<Animal> animals;
        private IReturnValueArgumentValidationConfiguration<List<List<List<string>>>> aCallToConvertToDenormalizedList;

        public When_Calling_DenormalizeToJSON_With_A_List_And_The_Denormalized_Object_Is_Larger_Than_The_JSON_Object()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

            aCallToConvertToDenormalizedList = A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored));

            aCallToConvertToDenormalizedList.Returns(new List<List<List<string>>>()
            {
                new List<List<string>>()
                {
                    new List<string>()
                    {
                "When Serialized This List will be larger than the JSON for Animal.",
                "When Serialized This List will be larger than the JSON for Animal.",
                "When Serialized This List will be larger than the JSON for Animal.",
                "When Serialized This List will be larger than the JSON for Animal.",
                "When Serialized This List will be larger than the JSON for Animal."
                    }
                }
            });

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

            json = denormalizer.DenormalizeToJSON(animals);
        }

        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Call_ConvertToStringList()
        {
            aCallToConvertToDenormalizedList.WhenArgumentsMatch(args => args[0] == animals).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Not_Have_Denormalized_The_Object()
        {
            Assert.Equal(JsonConvert.SerializeObject(animals), json);
        }
    }
    #endregion

    #region ConvertToDenormalizedLists
    public class When_Calling_ConvertToDenormalizedLists_With_Null
    {
        private JSONDenormalizer denormalizer;

        public When_Calling_ConvertToDenormalizedLists_With_Null()
        {
            denormalizer = new JSONDenormalizer();            
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => denormalizer.ConvertToDenormalizedLists<object>(null));
        }
    }

    public class When_Calling_ConvertToDenormalizedLists
    {
        private JSONDenormalizer denormalizer;
        private List<Animal> animals;
        private List<List<List<string>>> denormalizedLists;
        private List<List<string>> denormalizedDataLists;
        private List<List<string>> dataStructureLists;

        public When_Calling_ConvertToDenormalizedLists()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored)).CallsBaseMethod();

            denormalizedDataLists = new List<List<string>>()
            {
                new List<string>() {"Id" },
                new List<string>() {"Name", "Tony", "Tania", "Talia" },
                new List<string>() {"Age", "21", "1", "3" },
                new List<string>() {"Type", "Tiger" }
            };

            dataStructureLists = new List<List<string>>()
            {
                new List<string>() {"1", "1", "1", "1" },
                new List<string>() {"2", "2", "2", "1" },
                new List<string>() {"3", "3", "3", "1" }
            };

            A.CallTo(() => denormalizer.CreateDenormalizedDataLists(A<List<Animal>>.Ignored)).Returns(denormalizedDataLists);

            A.CallTo(() => denormalizer.CreateDataStructureLists(A<List<Animal>>.Ignored, A<List<List<string>>>.Ignored)).Returns(dataStructureLists);

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

            denormalizedLists = denormalizer.ConvertToDenormalizedLists(animals);
        }

        [Fact]
        public void It_Should_Return_The_DenormalizedLists()
        {
            Assert.Equal(denormalizedDataLists, denormalizedLists[0]);
            Assert.Equal(dataStructureLists, denormalizedLists[1]);
        }

        [Fact]
        public void It_Should_Call_CreateDenormalizedDataLists()
        {
            A.CallTo(() => denormalizer.CreateDenormalizedDataLists(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Call_CreateDataStructureLists()
        {
            A.CallTo(() => denormalizer.CreateDataStructureLists(A<List<Animal>>.Ignored, A<List<List<string>>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals && args[1] == denormalizedDataLists)
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
    #endregion

    #region CreateDenormalizedDataLists
    public class When_Calling_CreateDenormalizedDataLists_With_Null
    {
        private JSONDenormalizer denormalizer;

        public When_Calling_CreateDenormalizedDataLists_With_Null()
        {
            denormalizer = new JSONDenormalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => denormalizer.CreateDenormalizedDataLists<object>(null));
        }
    }

    public class When_Calling_CreateDenormalizedDataLists
    {
        private JSONDenormalizer denormalizer;
        private List<Animal> animals;
        private List<List<string>> denormalizedDataLists;
        private List<List<string>> createDenormalizedDataListForPropertyReturnValues;

        public When_Calling_CreateDenormalizedDataLists()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.CreateDenormalizedDataLists(A<List<Animal>>.Ignored)).CallsBaseMethod();

            createDenormalizedDataListForPropertyReturnValues = new List<List<string>>()
            {
                new List<string>() {"1", "2", "3" },
                new List<string>() {"21", "12" },
                new List<string>() {"Tony", "Tania", "Tori" },
                new List<string>() {"Tiger" }
            };

            A.CallTo(() => denormalizer.CreateDenormalizedDataListForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .ReturnsNextFromSequence(createDenormalizedDataListForPropertyReturnValues.ToArray());

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal(),
                new Animal()
            };

            denormalizedDataLists = denormalizer.CreateDenormalizedDataLists(animals);
        }

        [Fact]
        public void It_Should_Return_A_List_For_Every_Property()
        {
            Assert.Equal(typeof(Animal).GetProperties().Count(), denormalizedDataLists.Count);
        }

        [Fact]
        public void The_First_String_In_Each_List_Should_Be_The_Property_Name()
        {
            Assert.Equal("AnimalId", denormalizedDataLists[0][0]);
            Assert.Equal("Age", denormalizedDataLists[1][0]);
            Assert.Equal("Name", denormalizedDataLists[2][0]);
            Assert.Equal("Type", denormalizedDataLists[3][0]);
        }

        [Fact]
        public void Property_Lists_As_Long_As_The_Normalized_List_Should_Not_Be_Included()
        {
            Assert.Equal(1, denormalizedDataLists[0].Count());
            Assert.Equal(1, denormalizedDataLists[2].Count());
        }

        [Fact]
        public void Property_Lists_Should_Be_Included_After_The_Property_Name()
        {
            for (int i = 0; i < createDenormalizedDataListForPropertyReturnValues[1].Count; i++)
            {
                Assert.Equal(createDenormalizedDataListForPropertyReturnValues[1][i], denormalizedDataLists[1][i+1]);
            }

            for (int i = 0; i < createDenormalizedDataListForPropertyReturnValues[3].Count; i++)
            {
                Assert.Equal(createDenormalizedDataListForPropertyReturnValues[3][i], denormalizedDataLists[3][i+1]);
            }
        }

        [Fact]
        public void It_Should_Call_CreateDenormalizedDataListForProperty()
        {
            A.CallTo(() => denormalizer.CreateDenormalizedDataListForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Times(typeof(Animal).GetProperties().Count()));
        }
    }
    #endregion
}
