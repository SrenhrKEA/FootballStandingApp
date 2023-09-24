using Csv;
class Program
{
    static void Main()
    {
        // string filePath = "setup.csv";
        
        // if (File.Exists(filePath))
        // {
        //     Console.WriteLine($"File {filePath} exist.");

        //     using (StreamReader reader = new StreamReader(filePath))
        //     {
        //         string? line;
        //         while ((line = reader.ReadLine()) != null)
        //         {
        //             Console.WriteLine(line);
        //         }
        //     }
        // }
        // else
        // {
        //     Console.WriteLine($"File {filePath} does not exist.");
        // }
        ResultsGenerator.GenerateResults(32);
    }
}
