using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows.Media;

namespace TaskList.DatabaseAccess
{
    [Table]
    class SubTask
    {
        [Column(IsDbGenerated=true, IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column(DbType = "NVarChar(29) NOT NULL")]
        public String Description { get; set; }

        [Column()]
        public int Alert { get; set; }

        [Column()]
        public int Status { get; set; }

        [Column(CanBeNull=false)]
        public int TaskId { get; set; }

        public SolidColorBrush Color { get; set; }

        private EntityRef<Task> taskRef;

        [Association(Storage = "taskRef", ThisKey = "TaskId", OtherKey = "Id", IsForeignKey = true)]
        public Task Task
        {
            get { return this.taskRef.Entity; }
            set
            {
                taskRef.Entity = value;
            }
        }
    }
}
