using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace TaskList.DatabaseAccess
{
    class MyLocalDatabase: DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/Tasks.sdf";

        public MyLocalDatabase(string connectionString) : base(connectionString){}

        public Table<Task> Tasks;
        public Table<SubTask> SubTasks;
        public Table<GpsPoint> GpsPoints;
    }
}
