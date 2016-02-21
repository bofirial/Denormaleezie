using Denormaleezie.Denormalizers;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy.Configuration;
using Denormaleezie.Tests.Test_Classes;

namespace Denormaleezie.Tests
{
    public class When_Calling_DenormalizeToJSON_With_A_Null_Object
    {
        private string json;
        private Denormalizer denormalizer;
        private IJSONDenormalizer fakeJsonDenormalizer;
        private IReturnValueArgumentValidationConfiguration<string> aCalltoDenormalize;
        private const string returnValue = "";

        public When_Calling_DenormalizeToJSON_With_A_Null_Object()
        {
            fakeJsonDenormalizer = A.Fake<IJSONDenormalizer>();

            aCalltoDenormalize = A.CallTo(() => fakeJsonDenormalizer.DenormalizeToJSON(A<IEnumerable<object>>.Ignored));

            aCalltoDenormalize.Returns(returnValue);

            denormalizer = new Denormalizer(fakeJsonDenormalizer);

            json = denormalizer.DenormalizeToJSON<object>(null);
        }

        [Fact]
        public void It_Should_Return_The_JSONDenormalizer_Result()
        {
            Assert.Equal(returnValue, json);
        }

        [Fact]
        public void It_Should_Call_The_JSONDenormalizer_With_The_Animal()
        {
            aCalltoDenormalize.WhenArgumentsMatch(args => {
                return args[0] == null;
                }).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_Calling_DenormalizeToJSON_With_A_List
    {
        private string json;
        private Denormalizer denormalizer;
        private IJSONDenormalizer fakeJsonDenormalizer;
        private IReturnValueArgumentValidationConfiguration<string> aCalltoDenormalize;
        private const string returnValue = "{\"thisIs\": \"a dummy return value.\"}";
        private List<Animal> animals;

        public When_Calling_DenormalizeToJSON_With_A_List()
        {
            fakeJsonDenormalizer = A.Fake<IJSONDenormalizer>();

            aCalltoDenormalize = A.CallTo(() => fakeJsonDenormalizer.DenormalizeToJSON(A<List<Animal>>.Ignored));

            aCalltoDenormalize.Returns(returnValue);

            denormalizer = new Denormalizer(fakeJsonDenormalizer);

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
        public void It_Should_Return_The_JSONDenormalizer_Result()
        {
            Assert.Equal(returnValue, json);
        }

        [Fact]
        public void It_Should_Call_The_JSONDenormalizer_With_The_List()
        {
            aCalltoDenormalize.WhenArgumentsMatch(args => args[0] == animals).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
