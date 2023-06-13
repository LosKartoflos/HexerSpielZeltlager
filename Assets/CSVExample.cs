using System.Collections.Generic;
using UnityEngine;

public class CSVExample : MonoBehaviour
{
    private void Start()
    {
        string filePath = Application.streamingAssetsPath + "/example.csv";

        // Write data to CSV
        List<List<string>> dataToWrite = new List<List<string>>();
        dataToWrite.Add(new List<string> { "1", "John", "Doe" });
        dataToWrite.Add(new List<string> { "2", "Jane", "Smith" });
        CSVWriter.WriteCSV(filePath, dataToWrite);
        Debug.Log("CSV file written!");

        // Read data from CSV
        List<List<string>> dataRead = CSVReader.ReadCSV(filePath);
        foreach (List<string> row in dataRead)
        {
            string line = string.Join(", ", row.ToArray());
            Debug.Log(line);
        }
    }
}
