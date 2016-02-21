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

namespace Dnz.Unit.JSONDnz
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
        private IEnumerable<IEnumerable<IEnumerable<object>>> deserializedObject;

        public When_Calling_DenormalizeToJSON_With_A_List()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

            deserializedObject = new List<IEnumerable<IEnumerable<object>>>()
            {
                new List<IEnumerable<object>>()
                {
                    new List<object>()
                    {
                        "denormaleezie"
                    }
                }
            };

            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored)).Returns(deserializedObject);

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
            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
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

        public When_Calling_DenormalizeToJSON_With_A_List_And_The_Denormalized_Object_Is_Larger_Than_The_JSON_Object()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored)).Returns(
                new List<IEnumerable<IEnumerable<object>>>()
            {
                new List<IEnumerable<object>>()
                {
                    new List<object>()
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
            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored))
                .WhenArgumentsMatch(args => args[0] == animals)
                .MustHaveHappened(Repeated.Exactly.Once);
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
        private IEnumerable<IEnumerable<IEnumerable<object>>> denormalizedLists;
        private IEnumerable<IEnumerable<object>> denormalizedDataLists;
        private IEnumerable<IEnumerable<object>> dataStructureLists;

        public When_Calling_ConvertToDenormalizedLists()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.ConvertToDenormalizedLists(A<List<Animal>>.Ignored)).CallsBaseMethod();

            denormalizedDataLists = new List<IEnumerable<object>>()
            {
                new List<object>() { "Id"},
                new List<object>() { "Name", "Tony", "Tania", "Talia"},
                new List<object>() { "Age", 21, 1, 3},
                new List<object>() { "Type", "Tiger" }
            };

            dataStructureLists = new List<IEnumerable<object>>()
            {
                new List<object>() {1, 1, 1, 1 },
                new List<object>() {2, 2, 2, 1 },
                new List<object>() {3, 3, 3, 1 }
            };

            A.CallTo(() => denormalizer.CreateDenormalizedDataLists(A<List<Animal>>.Ignored)).Returns(denormalizedDataLists);

            A.CallTo(() => denormalizer.CreateDataStructureLists(A<List<Animal>>.Ignored, A<IEnumerable<IEnumerable<object>>>.Ignored))
                .Returns(dataStructureLists);

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
            Assert.Equal(denormalizedDataLists, denormalizedLists.ElementAt(0));
            Assert.Equal(dataStructureLists, denormalizedLists.ElementAt(1));
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
            A.CallTo(() => denormalizer.CreateDataStructureLists(A<List<Animal>>.Ignored, A<IEnumerable<IEnumerable<object>>>.Ignored))
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
        private IEnumerable<IEnumerable<object>> denormalizedDataLists;
        private IEnumerable<IEnumerable<object>> createDenormalizedDataListForPropertyReturnValues;

        public When_Calling_CreateDenormalizedDataLists()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.CreateDenormalizedDataLists(A<List<Animal>>.Ignored)).CallsBaseMethod();

            createDenormalizedDataListForPropertyReturnValues = new List<IEnumerable<object>>()
            {
                new List<object>() {1, 2, 3 },
                new List<object>() {21, 12 },
                new List<object>() {"Tony", "Tania", "Tori" },
                new List<object>() {"Tiger" }
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
            Assert.Equal(typeof(Animal).GetProperties().Count(), denormalizedDataLists.Count());
        }

        [Fact]
        public void The_First_String_In_Each_List_Should_Be_The_Property_Name()
        {
            var propNames = denormalizedDataLists.Select(i => i.First());

            Assert.Contains(propNames, p => p.ToString() == "AnimalId");
            Assert.Contains(propNames, p => p.ToString() == "Age");
            Assert.Contains(propNames, p => p.ToString() == "Name");
            Assert.Contains(propNames, p => p.ToString() == "Type");
        }

        [Fact]
        public void Property_Lists_As_Long_As_The_Normalized_List_Should_Not_Be_Included()
        {
            Assert.Equal(1, denormalizedDataLists.ElementAt(0).Count());
            Assert.Equal(1, denormalizedDataLists.ElementAt(2).Count());
        }

        [Fact]
        public void Property_Lists_Should_Be_Included_After_The_Property_Name()
        {
            for (int i = 0; i < createDenormalizedDataListForPropertyReturnValues.ElementAt(1).Count(); i++)
            {
                Assert.Equal(createDenormalizedDataListForPropertyReturnValues.ElementAt(1).ElementAt(i), denormalizedDataLists.ElementAt(1).ElementAt(i + 1));
            }

            for (int i = 0; i < createDenormalizedDataListForPropertyReturnValues.ElementAt(3).Count(); i++)
            {
                Assert.Equal(createDenormalizedDataListForPropertyReturnValues.ElementAt(3).ElementAt(i), denormalizedDataLists.ElementAt(3).ElementAt(i + 1));
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

    #region CreateDenormalizedDataListForProperty
    public class When_Calling_CreateDenormalizedDataListForProperty_With_A_Null_ObjectToDenormalize
    {
        private JSONDenormalizer denormalizer;

        public When_Calling_CreateDenormalizedDataListForProperty_With_A_Null_ObjectToDenormalize()
        {
            denormalizer = new JSONDenormalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => denormalizer.CreateDenormalizedDataListForProperty<object>(null, typeof(Animal).GetProperty("AnimalId")));
        }
    }
    public class When_Calling_CreateDenormalizedDataListForProperty_With_A_Null_PropertyInfo
    {
        private JSONDenormalizer denormalizer;

        public When_Calling_CreateDenormalizedDataListForProperty_With_A_Null_PropertyInfo()
        {
            denormalizer = new JSONDenormalizer();
        }

        [Fact]
        public void It_Should_Throw_An_Exception()
        {
            Assert.Throws(typeof(ArgumentException), () => denormalizer.CreateDenormalizedDataListForProperty(new List<Animal>(), null));
        }
    }

    public class When_Calling_CreateDenormalizedDataListForProperty_For_A_Property_With_No_Duplicates
    {
        private JSONDenormalizer denormalizer;
        private List<Animal> animals;
        private PropertyInfo propInfo;
        private IEnumerable<object> denormalizedDataList;

        public When_Calling_CreateDenormalizedDataListForProperty_For_A_Property_With_No_Duplicates()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.CreateDenormalizedDataListForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal()
                {
                    AnimalId = 2,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tania"
                },
                new Animal()
                {
                    AnimalId = 3,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tali"
                }
            };

            propInfo = typeof(Animal).GetProperty("AnimalId");

            denormalizedDataList = denormalizer.CreateDenormalizedDataListForProperty(animals, propInfo);
        }

        [Fact]
        public void It_Should_Return_A_String_For_Every_ListItem()
        {
            Assert.Equal(animals.Count(), denormalizedDataList.Count());
        }

        [Fact]
        public void The_List_Should_Contain_The_Property_Values()
        {
            Assert.Equal(1, (int)denormalizedDataList.ElementAt(0));
            Assert.Equal(2, (int)denormalizedDataList.ElementAt(1));
            Assert.Equal(3, (int)denormalizedDataList.ElementAt(2));
        }
    }
    public class When_Calling_CreateDenormalizedDataListForProperty_For_A_Property_With_Duplicates
    {
        private JSONDenormalizer denormalizer;
        private List<Animal> animals;
        private PropertyInfo propInfo;
        private IEnumerable<object> denormalizedDataList;

        public When_Calling_CreateDenormalizedDataListForProperty_For_A_Property_With_Duplicates()
        {
            denormalizer = A.Fake<JSONDenormalizer>();

            A.CallTo(() => denormalizer.CreateDenormalizedDataListForProperty(A<List<Animal>>.Ignored, A<PropertyInfo>.Ignored)).CallsBaseMethod();

            animals = new List<Animal>()
            {
                new Animal()
                {
                    AnimalId = 1,
                    Type = "Tiger",
                    Age = 21,
                    Name = "Tony"
                },
                new Animal()
                {
                    AnimalId = 2,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tania"
                },
                new Animal()
                {
                    AnimalId = 3,
                    Type = "Tiger",
                    Age = 12,
                    Name = "Tali"
                }
            };

            propInfo = typeof(Animal).GetProperty("Type");

            denormalizedDataList = denormalizer.CreateDenormalizedDataListForProperty(animals, propInfo);
        }

        [Fact]
        public void It_Should_Return_A_String_For_Every_Unique_ListItem()
        {
            Assert.Equal(1, denormalizedDataList.Count());
        }

        [Fact]
        public void The_List_Should_Contain_Only_The_Unique_Property_Values()
        {
            Assert.Equal("Tiger", (string)denormalizedDataList.ElementAt(0));
        }
    }
    #endregion
}
