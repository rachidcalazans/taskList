using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace TaskList.Database 
{
    class Database : DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/TasksDatabase.sdf";
        //Cria o construtor e usa o método da superclasse (pai) para criar a conexão, somente passando a string de conexão
        public Database(string connectionString) : base(connectionString) { }
        //Mapeando a tabela alunos com a classe aluno
        public Table<Task> Task;
    }
}
