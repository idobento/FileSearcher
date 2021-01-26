using System;
using System.IO;
using System.Collections.Generic;
using DB;

namespace SearchFile
{
    public class SearchClass
    {
        //----------Class's Properties+event declaration---------------
        #region
        public int SearchID = 0;
        public List<string> SearchResults = new List<string>();//contains the paths to to search results
        public int NumOfResults = 0;
        public string SearchString { get; set; }
        public event Action<string> printEvent;
        #endregion
        //----------Search method--------------
        public void search(string ParentDirectory)
        {
            NumOfResults = 0; //reset the value from previous searches
            string DirectoryToSearchIn = ParentDirectory;
            StartSearch(DirectoryToSearchIn);
            void StartSearch(string DirectoryToSearchIn)
            {
                //searching all files and put them in SearchResult's list:
                foreach (string file in Directory.GetFiles(DirectoryToSearchIn, "*" + this.SearchString + "*.*", SearchOption.TopDirectoryOnly))
                {
                    SearchResults.Add(file);//add the file path to a list
                    NumOfResults++;
                    printEvent?.Invoke(file);//activiate printResults function 
                }
                //recursive method to keep searching in inner folders:
                string[] innerFolders = Directory.GetDirectories(DirectoryToSearchIn, "*", SearchOption.TopDirectoryOnly);
                foreach (string folder in innerFolders)
                {
                    StartSearch(folder);
                }
            }
            SearchID++;
            ResultsToDB writeinDB = new ResultsToDB(SearchID, SearchString, ParentDirectory, NumOfResults, SearchResults);
            writeinDB.WriteToDB();
        }
    }
}
