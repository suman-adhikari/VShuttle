﻿using System.Collections.Generic;
using VShuttle.Models;

namespace VShuttle.Model.ViewModel
{
    public class LocationRoute
    {
        public Locations  Location  { get; set; }
        public List<Routes> Route { get; set; }
    }
}
