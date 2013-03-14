using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TaskList.Database
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

        [Column(Storage = "TaskId")]
        public int TaskId { get; set; }

        [Association(Storage = "TaskId", ThisKey = "TaskId", OtherKey = "Id")]
        public Task Task { get; set; }
    }
}
