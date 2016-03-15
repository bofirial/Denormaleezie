using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Normaleezie.Sample.Models
{
    public class ZipCodeDTO
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("loc")]
        public List<float> LatitudeLongitude { get; set; }

        [JsonProperty("pop")]
        public int Population { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("_id")]
        public string ZipCode { get; set; }
    }
}