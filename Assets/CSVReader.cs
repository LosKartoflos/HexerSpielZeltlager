using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CSVReader
{
    public static List<List<string>> ReadCSV(string filePath)
    {
        List<List<string>> data = new List<List<string>>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                List<string> row = new List<string>(values);
                data.Add(row);
            }
        }

        return data;
    }
}