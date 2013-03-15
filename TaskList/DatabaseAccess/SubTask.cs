using System;
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
    }
}
