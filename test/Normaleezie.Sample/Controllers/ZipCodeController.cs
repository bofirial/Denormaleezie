using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Normaleezie.Sample.Models;
using WebApi.OutputCache.V2;

namespace Normaleezie.Sample.Controllers
{
    public class ZipCodeController : ApiController
    {
        [CacheOutput(ServerTimeSpan = 10, ExcludeQueryStringFromCacheKey = true)]
        public List<ZipCodeDescription> Get()
        {
            string zipCodesJson = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/zipCodes.json"));

            List<ZipCodeDescription> zipCodes = JsonConvert.DeserializeObject<List<ZipCodeDTO>>(zipCodesJson).Select(z => new ZipCodeDescription(z)).ToList();

            return zipCodes;
        }
    }
}