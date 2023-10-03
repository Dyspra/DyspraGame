using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;
using BarGraph.VittorCloud;

public class BarGraphExample : MonoBehaviour
{
    public List<BarGraphDataSet> exampleDataSet;
    BarGraphGenerator barGraphGenerator; 

    void Start()
    {
        barGraphGenerator = GetComponent<BarGraphGenerator>();

        if (exampleDataSet.Count == 0)
        {
            Debug.LogError("ExampleDataSet is Empty!");
            return;
        }
        barGraphGenerator.GeneratBarGraph(exampleDataSet);

    }

    public void StartUpdatingGraph()
    {
        StartCoroutine(CreateDataSet());
    }



    IEnumerator CreateDataSet()
    {
        while (true)
        {
            GenerateRandomData();
            yield return new WaitForSeconds(2.0f);

        }

    }

    void GenerateRandomData()
    {
        int dataSetIndex = UnityEngine.Random.Range(0, exampleDataSet.Count);
        int xyValueIndex = UnityEngine.Random.Range(0, exampleDataSet[dataSetIndex].ListOfBars.Count);
        exampleDataSet[dataSetIndex].ListOfBars[xyValueIndex].YValue = UnityEngine.Random.Range(barGraphGenerator.yMinValue, barGraphGenerator.yMaxValue);
        barGraphGenerator.AddNewDataSet(dataSetIndex, xyValueIndex, exampleDataSet[dataSetIndex].ListOfBars[xyValueIndex].YValue);
    }
}



