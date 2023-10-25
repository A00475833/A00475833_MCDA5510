namespace ProgAssign1
{


    public class DirWalker
    {
        public CSVParser file = new CSVParser();

        public void Walk(String path)
        {

            string[] list = Directory.GetDirectories(path);


            if (list == null) return;

            /*foreach (string dirpath in list)
            {
                if (Directory.Exists(dirpath))
                {
                    Walk(dirpath);
                    Console.WriteLine("Dir:" + dirpath);
                }
            }*/
            List<string> fileList = new List<string>(Directory.EnumerateFiles(path, "*.csv", SearchOption.AllDirectories));
            /*Console.WriteLine(Directory.Exists(path));
            Console.WriteLine(fileList.Count);
            Console.WriteLine(path);*/
            foreach (string filepath in fileList)
            {
                //Console.WriteLine("File:" + filepath);
                file.CsvReadWrite(filepath);
            }
        }

        public static void Main(String[] args)
        {
            var startDate = DateTime.Now;
            DirWalker fw = new DirWalker();
            fw.Walk(@"C:\Users\Siddharth Singh\source\repos\ProgAssign1\Sample Data");
            var endDate = DateTime.Now;
            var executionTime = endDate - startDate;
            // var DW = new DirWalker();
            Console.WriteLine($"Valid Rows: {fw.file.validRowsFinal}");
            Console.WriteLine($"Invalid Rows: {fw.file.missingDataFinal}");
            Console.WriteLine($"Execution time: {executionTime}");
            
            fw.file.logWriter.WriteLine($"Total Execution Time: {executionTime}");
            fw.file.CloseLog();


        }

    }
}