using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace DB
{
    public class ResultsToDB
    {
        //---------Class's properties and event's declarations-----------
        public int SearchID = 0;
        public string Value;
        public string FileDirectory;
        public int TotalResults = 0;
        public List<string> FilePath;
        public event Action<string> DataBaseError;
        static string DBLocation;

        public ResultsToDB() { }
        public ResultsToDB(int searchID, string value, string directory, int totalResults, List<string> filePath)
        {
            SearchID = searchID;
            Value = value;
            FileDirectory = directory;
            TotalResults = totalResults;
            FilePath = filePath;
        }
        //------------------------------------------

        SqlConnection ConnectDB = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={DBLocation};Integrated Security=True");
        public void WriteToDB()
        {
            ConnectDB.Open();
            SqlTransaction Transaction1 = ConnectDB.BeginTransaction();
            try
            {
                //Save data to Properties table
                new SqlCommand($"Insert into SearchProperties (SearchID,value,directory,totalResults) " +
                $"values ('{SearchID}','{Value}','{FileDirectory}','{TotalResults}')", ConnectDB, Transaction1).ExecuteNonQuery();

                //Save data to Results table
                foreach (string path in FilePath)
                {
                    new SqlCommand($"Insert into searchResults (SearchID,value,filePath)" +
                    $"values('{SearchID}','{Value}','{path}')", ConnectDB, Transaction1).ExecuteNonQuery();
                }
                Transaction1.Commit();
            }
            catch (Exception e)
            {
                DataBaseError?.Invoke(e.Message);
                Transaction1.Rollback();
            }
            ConnectDB.Close();
        }
        public void SetDBLocation()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\DB");
            var newDBLocation = Directory.GetCurrentDirectory();
            foreach (var file in Directory.GetFiles(newDBLocation, "*.mdf", SearchOption.TopDirectoryOnly))
            {
                DBLocation=file;
            }
        }
    }
}
