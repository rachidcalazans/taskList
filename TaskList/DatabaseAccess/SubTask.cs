using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TaskList.DatabaseAccess
{
    [Table]
    class SubTask
    {
        [Column(IsDbGenerated=true, IsPrimaryKey=true)]
        public int Id { get; set; }

        [Column()]
        public String Description { get; set; }

        [Column()]
        public int Status { get; set; }

        [Column()]
        public int TaskId { get; set; }

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
