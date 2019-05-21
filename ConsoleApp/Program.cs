using System.Data;
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Database;Integrated Security=SSPI";

        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (SqlCommand sqlCommand = new SqlCommand("dbo.SaveTest", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;

                    var outputParam = sqlCommand.Parameters.Add("@id", SqlDbType.Int);
                    outputParam.SourceColumn = "Id";
                    outputParam.Direction = ParameterDirection.Output;

                    sqlCommand.Parameters.Add("@value", SqlDbType.Int).SourceColumn = "Value";

                    var dataTable = GetDataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        // Send rows in batches of 50
                        adapter.UpdateBatchSize = 50;
                        adapter.InsertCommand = sqlCommand;
                        adapter.Update(dataTable);
                    }
                }
            }
        }

        private static DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Value", typeof(int));

            var row = dataTable.NewRow();
            row.SetField("Value", 1);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row.SetField("Value", 2);
            dataTable.Rows.Add(row);

            return dataTable;
        }
    }
}
