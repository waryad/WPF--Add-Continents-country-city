using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2DeepinderKaurWarya
{
    internal class DataAccess
    {
        private static string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=WorldDB;Integrated Security=True";

        public static string ConnectionString { get => connStr; }
    }
}

