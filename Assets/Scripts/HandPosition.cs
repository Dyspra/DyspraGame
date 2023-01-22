using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition
{
    public List<Package> packages = new List<Package>();
    public double date = 0;

    public HandPosition()
    {
    }

    public HandPosition(Package newPackage, double newDate)
    {
        packages.Add(newPackage);
        date = newDate;
    }
}
