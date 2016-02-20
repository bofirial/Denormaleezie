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

namespace Denormaleezie.Tests.Denormalizers
{
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
        private IReturnValueArgumentValidationConfiguration<List<string>> aCallToConvertToStringList;
        private List<string> deserializedObject;

        public When_Calling_DenormalizeToJSON_With_A_List()
        {
            using (Fake.CreateScope())
            {
                denormalizer = A.Fake<JSONDenormalizer>();

                A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

                aCallToConvertToStringList = A.CallTo(() => denormalizer.ConvertToStringList(A<List<Animal>>.Ignored));

                deserializedObject = new List<string>()
                {
                    "deserializedObject"
                };

                aCallToConvertToStringList.Returns(deserializedObject);

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
        }

        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Call_ConvertToStringList()
        {
            aCallToConvertToStringList.WhenArgumentsMatch(args => args[0] == animals).MustHaveHappened(Repeated.Exactly.Once);
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
        private IReturnValueArgumentValidationConfiguration<List<string>> aCallToConvertToStringList;

        public When_Calling_DenormalizeToJSON_With_A_List_And_The_Denormalized_Object_Is_Larger_Than_The_JSON_Object()
        {
            using (Fake.CreateScope())
            {
                denormalizer = A.Fake<JSONDenormalizer>();

                A.CallTo(() => denormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored)).CallsBaseMethod();

                aCallToConvertToStringList = A.CallTo(() => denormalizer.ConvertToStringList(A<List<Animal>>.Ignored));

                aCallToConvertToStringList.Returns(new List<string>()
                {
                    "When Serialized This List will be larger than the JSON for Animal.",
                    "When Serialized This List will be larger than the JSON for Animal.",
                    "When Serialized This List will be larger than the JSON for Animal.",
                    "When Serialized This List will be larger than the JSON for Animal.",
                    "When Serialized This List will be larger than the JSON for Animal."
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
        }

        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Call_ConvertToStringList()
        {
            aCallToConvertToStringList.WhenArgumentsMatch(args => args[0] == animals).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void It_Should_Not_Have_Denormalized_The_Object()
        {
            Assert.Equal(JsonConvert.SerializeObject(animals), json);
        }
    }
}
