using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Normaleezie.Sample.Models;
using WebApi.OutputCache.V2;

namespace Normaleezie.Sample.Controllers
{
    public class NormalizedZipCodeController : ApiController
    {
        [CacheOutput(ServerTimeSpan = 10, ExcludeQueryStringFromCacheKey = true)]
        public List<List<List<object>>> Get()
        {
            string zipCodesJson = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/zipCodes.json"));

            List<ZipCodeDescription> zipCodes = JsonConvert.DeserializeObject<List<ZipCodeDTO>>(zipCodesJson).Select(z => new ZipCodeDescription(z)).ToList();

            List<List<List<object>>> normalZipCodes = new Normalizer().Normalize(zipCodes);

            return normalZipCodes;
        }
    }
}
