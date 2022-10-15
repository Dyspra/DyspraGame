using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package
{
    public Vector3 position;
    public int landmark;

    public Package(Vector3 new_pos, int new_landmark)
    {
        position = new_pos;
        landmark = new_landmark;
    }
}
