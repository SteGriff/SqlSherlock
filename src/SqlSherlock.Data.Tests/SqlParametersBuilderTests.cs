using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class SqlParametersBuilderTests
    {
        [TestMethod]
        public void SqlParamsBuilder_SetsIntValue()
        {
            var target = new SqlParametersBuilder();

            var query = new Query()
            {
                SqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("UserId", System.Data.SqlDbType.Int)
                },
                Inputs = new List<QueryInput>()
                {
                    new QueryInput()
                    {
                        
                    }
                }
            };

            //target.PopulateSqlParameters()
        }
    }
}
