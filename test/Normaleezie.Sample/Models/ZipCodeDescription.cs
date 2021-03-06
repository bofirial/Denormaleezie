﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Normaleezie.Sample.Models
{
    public class ZipCodeDescription
    {
        public ZipCodeDescription()
        {
            
        }

        public ZipCodeDescription(ZipCodeDTO zipCodeDto)
        {
            City = zipCodeDto.City;
            LatitudeLongitude = zipCodeDto.LatitudeLongitude;
            Population = zipCodeDto.Population;
            State = zipCodeDto.State;
            ZipCode = zipCodeDto.ZipCode;

            LatitudeLongitude[0] = (float)Math.Round(LatitudeLongitude[0], 2);
            LatitudeLongitude[1] = (float)Math.Round(LatitudeLongitude[1], 2);
        }

        public string City { get; set; }
        
        public List<float> LatitudeLongitude { get; set; }
        
        public int Population { get; set; }
        
        public string State { get; set; }
        
        public string ZipCode { get; set; }
    }
}