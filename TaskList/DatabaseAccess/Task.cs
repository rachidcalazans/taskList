using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TaskList.DatabaseAccess
{
    [Table]
    class Task
    {
        [Column(IsDbGenerated=true, IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column(CanBeNull = false,DbType = "NVarChar(23) NOT NULL")]
        public string Description { get; set; }

        [Column()]
        public int Status { get; set; }

        private readonly EntitySet<GpsPoint> _gps = new EntitySet<GpsPoint>();

        [Association(Storage = "_gps", ThisKey = "Id", OtherKey = "TaskId")]
        public EntitySet<GpsPoint> GpsPoint
        {
            get { return this._gps; }
        }

        private readonly EntitySet<SubTask> _subTasks = new EntitySet<SubTask>();

        [Association(Storage = "_subTasks", ThisKey = "Id", OtherKey = "TaskId")]
        public EntitySet<SubTask> SubTasks
        {
            get { return this._subTasks; }
        }
    }
}
