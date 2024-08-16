using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassManager : Singleton<PassManager> 
{
    public List<Transform> unoccupiedPassPoints;

    private void Awake()
    {
        //add all children to passPoints
        foreach (Transform t in transform)
        {
            unoccupiedPassPoints.Add(t);
        }
    }
}
