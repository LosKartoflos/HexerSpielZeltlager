using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CSVWriter
{
    public static void WriteCSV(string filePath, List<List<string>> data)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (List<string> row in data)
            {
                string line = string.Join(",", row.ToArray());
                writer.WriteLine(line);
            }
        }
    }
}
