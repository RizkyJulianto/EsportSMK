using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulasiEsport
{
    internal class UserID
    {

        public static int userid = 0;
        public static SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");
    }
}
