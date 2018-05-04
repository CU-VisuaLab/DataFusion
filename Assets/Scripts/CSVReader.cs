/**********************************************************************
 * Alex Thompson
 * 8/11/2016
 * Class file for reading a CSV file and parsing/storing the data within it
 * so that other classes can access the data in the CSV file
 * This class is re-used in all visualization types.
 *********************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSVReader : MonoBehaviour {

    public string fileName;
    public List<string[]> rowData;
    public List<string> dataSets;

    void Start()
    {
    }

    public void ParseData()
    {
        rowData = new List<string[]>();
        TextAsset CSVfile = (TextAsset)Resources.Load(fileName);
        string textContents = CSVfile.text;
        string currentLine;
        StringReader sr = new StringReader(textContents);
        while ((currentLine = sr.ReadLine()) != null)
        {
            string[] lineData = currentLine.Split(',');
            rowData.Add(lineData);
        }
    }

    public void ParseFirst1000()
    {
        rowData = new List<string[]>();
        TextAsset CSVfile = (TextAsset)Resources.Load(fileName);
        string textContents = CSVfile.text;
        string currentLine;
        StringReader sr = new StringReader(textContents);
        int count = 0;
        while ((currentLine = sr.ReadLine()) != null && count < 1000)
        {
            string[] lineData = currentLine.Split(',');
            rowData.Add(lineData);
            count++;
        }
    }
    public void ParseFirst25()
    {
        rowData = new List<string[]>();
        TextAsset CSVfile = (TextAsset)Resources.Load(fileName);
        string textContents = CSVfile.text;
        string currentLine;
        StringReader sr = new StringReader(textContents);
        int count = 0;
        while ((currentLine = sr.ReadLine()) != null && count < 25)
        {
            string[] lineData = currentLine.Split(',');
            rowData.Add(lineData);
            count++;
        }
    }

    public int NumberOfData()
    {
        return rowData.Count - 1;
    }

    public List<string[]> GetRowData()
    {
        return rowData;
    }

    public string[] GetColumnHeads()
    {
        return rowData[0];
    }

    public void PrintContents()
    {
        for (int i=0; i<rowData.Count; i++)
        {
            for (int j=0; j < rowData[i].Length; j++)
            {
                print(rowData[i][j]);
            }
        }
    }

    public double MinXValue()
    {
        double minValue = double.MaxValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][1]) < minValue)
            {
                minValue = Convert.ToDouble(rowData[i][1]);
            }
        }
        return minValue;
    }

    public double MaxXValue()
    {
        double maxValue = double.MinValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][1]) > maxValue)
            {
                maxValue = Convert.ToDouble(rowData[i][1]);
            }
        }
        return maxValue;
    }

    public double MinYValue()
    {
        double minValue = double.MaxValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][2]) < minValue)
            {
                minValue = Convert.ToDouble(rowData[i][2]);
            }
        }
        return minValue;
    }

    public double MaxYValue()
    {
        double maxValue = double.MinValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][2]) > maxValue)
            {
                maxValue = Convert.ToDouble(rowData[i][2]);
            }
        }
        return maxValue;
    }
    public double MinZValue()
    {
        double minValue = double.MaxValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][3]) < minValue)
            {
                minValue = Convert.ToDouble(rowData[i][3]);
            }
        }
        return minValue;
    }
    public double MaxZValue()
    {
        double maxValue = double.MinValue;
        for (int i = 1; i < rowData.Count; i++)
        {
            if (Convert.ToDouble(rowData[i][3]) > maxValue)
            {
                maxValue = Convert.ToDouble(rowData[i][3]);
            }
        }
        return maxValue;
    }

    public void SetDataSets()
    {
        for (int i=1; i< NumberOfData(); i++)
        {
            if (!dataSets.Contains(rowData[i][0]))
            {
                dataSets.Add(rowData[i][0]);
            }
        }
    }
} 
