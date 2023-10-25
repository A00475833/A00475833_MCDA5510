using CsvHelper;
using CsvHelper.Configuration;
using ProgAssign1;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.IO;

namespace ProgAssign1
{
    public class CSVParser
    {
        public int missingData = 0;
        public int validRows = 0;
        public int missingDataFinal = 0;
        public int validRowsFinal = 0;
        bool flag = true;
        public StreamWriter logWriter;
        /*String outputPath = "C:\\Users\\Siddharth Singh\\source\\repos\\ProgAssign1\\Output\\output.csv";*/
        String outputPath = "~\\..\\..\\..\\..\\..\\Output\\output.csv";
        
        public CSVParser()
        {
            // Initialize the StreamWriter for logging
            logWriter = new StreamWriter("~\\..\\..\\..\\..\\..\\logs\\log.txt", true);
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(email, pattern);
        }
        public void CsvReadWrite(String path)
        {
            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            using var writer = new StreamWriter(outputPath, true);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            try
            {

                if (flag)
                {
                    csv.WriteHeader<POJO>();
                    csv.WriteField("Date");
                    csv.NextRecord();
                    flag = false;
                }

                var records = csvReader.GetRecords<POJO>().ToList();
                checkData(records, csv, path);

                logWriter.WriteLine($"File: {path}");
                logWriter.WriteLine($"Valid Rows: {validRows}");
                logWriter.WriteLine($"Missing Rows: {missingData}");
                logWriter.WriteLine("================================================================================================");
                validRows = 0;
                missingData = 0;


            }
            catch (Exception ex) 
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                using var reader = new CsvReader(new StreamReader(path), config);
                var validRecords = new List<POJO>();
                while (reader.Read())
                {
                    try
                    {
                        POJO record = reader.GetRecord<POJO>();
                        
                            validRecords.Add(record);
                    }
                    catch (CsvHelper.MissingFieldException exx)
                    {
                        logWriter.WriteLine("There was an error that occured!!");
                        missingData += 1;
                        missingDataFinal += 1;


                    } catch (CsvHelper.TypeConversion.TypeConverterException)
                    {
                        logWriter.WriteLine("Typeconversion err");
                        missingData += 1;
                        missingDataFinal += 1;
                    }


                }
                checkData(validRecords, csv, path);

                logWriter.WriteLine($"File: {path}");
                logWriter.WriteLine($"Valid Rows: {validRows}");
                logWriter.WriteLine($"Missing Rows: {missingData}");
                logWriter.WriteLine("================================================================================================");
                validRows = 0;
                missingData = 0;

            }

        }

        public void checkData(List<POJO> data, CsvWriter csv, String path)
        {

            foreach (var POJO in data)
            {
                if (string.IsNullOrEmpty(POJO.firstname) || string.IsNullOrEmpty(POJO.lastname)
                    || string.IsNullOrEmpty(POJO.street) || string.IsNullOrEmpty(POJO.city)
                    || string.IsNullOrEmpty(POJO.province) || string.IsNullOrEmpty(POJO.postalCode)
                    || string.IsNullOrEmpty(POJO.country) || string.IsNullOrEmpty(POJO.email)
                    || (POJO.streetNumber < 1) || (POJO.phoneNumber < 1) || !IsValidEmail(POJO.email))
                {
                    missingData += 1;
                    missingDataFinal += 1;
                    continue;
                }

                else

                {
                    validRows += 1;
                    validRowsFinal += 1;    
                    
                    string[] pathParts = path.Split(Path.DirectorySeparatorChar);
                    int level1 = int.Parse(pathParts[pathParts.Length - 4]);
                    int level2 = int.Parse(pathParts[pathParts.Length - 3]);
                    int level3 = int.Parse(pathParts[pathParts.Length - 2]);
                    DateTime specificDate = new DateTime(level1, level2, level3);
                    string dateAsString = specificDate.ToString("yyyy/MM/dd");
                    //Console.WriteLine("{0:MM/dd/yyyy}", specificDate.Date);
                    csv.WriteRecord(POJO);
                    csv.WriteField(dateAsString);

                    csv.NextRecord();

                }
                csv.Flush();
            }


        }
        public void CloseLog()
        {
            logWriter.WriteLine($"Final Valid Rows: {validRowsFinal}");
            logWriter.WriteLine($"Final Missing Data: {missingDataFinal}");
            

            logWriter.Close();
        }
    }
}