using Denormaleezie;
using Denormaleezie.Tests.Test_Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dnz.E2E
{
    public class When_Calling_DenormalizeToJSON_With_A_Null_Object
    {
        private string json;

        public When_Calling_DenormalizeToJSON_With_A_Null_Object()
        {
            Denormalizer denormalizer = new Denormalizer();

            json = denormalizer.DenormalizeToJSON<object>(null);
        }

        [Fact]
        public void It_Should_Return_An_Empty_String()
        {
            Assert.Equal(string.Empty, json);
        }
    }
    public class When_Calling_DenormalizeToJSON_With_A_List_Of_Flat_Objects
    {
        private List<Animal> animals;
        private string json;

        public When_Calling_DenormalizeToJSON_With_A_List_Of_Flat_Objects()
        {
            Denormalizer denormalizer = new Denormalizer();

            animals = new List<Animal>()
            {
                new Animal() {AnimalId = 101, Age = 10, Name = "Tony", Type = "Tiger" },
                new Animal() {AnimalId = 102, Age = 11, Name = "Lenny", Type = "Tiger" },
                new Animal() {AnimalId = 103, Age = 2, Name = "John", Type = "Tiger" },
                new Animal() {AnimalId = 104, Age = 15, Name = "Tony", Type = "Giraffe" },
                new Animal() {AnimalId = 105, Age = 10, Name = "Garry", Type = "Giraffe" },
                new Animal() {AnimalId = 106, Age = 10, Name = "Zachary", Type = "Zebra" },
            };

            json = denormalizer.DenormalizeToJSON(animals);
        }
        
        [Fact]
        public void It_Should_Return_JSON()
        {
            JToken.Parse(json);
        }

        [Fact]
        public void It_Should_Reduce_The_Size_Vs_JSON_Serialization()
        {
            Assert.True(json.Length < JsonConvert.SerializeObject(animals).Length);
        }
    }
}
