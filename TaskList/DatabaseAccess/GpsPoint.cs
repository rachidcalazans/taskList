﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using TaskList.DatabaseAccess;

namespace TaskList.DatabaseAccess
{
    [Table]
    class GpsPoint
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column()]
        public string Longitude { get; set; }
        [Column()]
        public string Latitude { get; set; }
    }
}
