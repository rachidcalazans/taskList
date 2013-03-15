using System;
using System.Collections.Generic;
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

        [Column()]
        public string Description { get; set; }

        [Column()]
        public DateTime StartDate { get; set; }

        [Column()]
        public DateTime FinishDate { get; set; }

        [Column()]
        public int Status { get; set; }
    }
}
