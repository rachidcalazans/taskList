using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TaskList.Database
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

        [Column(Storage="TaskId")]
        public int TaskId { get; set; }

        [Association(Storage="SubTaskTaskFK", ThisKey="TaskId", OtherKey="Id", IsForeignKey=true)]
        public Task Task { get; set; }
    }
}
