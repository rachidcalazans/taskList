using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TaskList.Database
{
    [Table]
    class Task
    {
        [Column(IsDbGenerated=true, IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column()]
        public string Description { get; set; }

        [Column()]
        public DateTime StartDate { get; set; }

        [Column()]
        public DateTime FinishDate { get; set; }

        [Column()]
        public DateTime StartHour { get; set; }

        [Column()]
        public DateTime FinishHour { get; set; }

        [Column()]
        public int Status { get; set; }

        [Association(Name="SubTaskTaskFK", Storage="SubTasks", ThisKey="Id", OtherKey="TaskId")]
        public List<SubTask> SubTasks { get; set; }

        [Association(Name = "GpsPointTaskFK", Storage = "GpsPoint", ThisKey = "Id", OtherKey = "TaskId")]
        public GpsPoint GpsPoint { get; set; }
    }
}
