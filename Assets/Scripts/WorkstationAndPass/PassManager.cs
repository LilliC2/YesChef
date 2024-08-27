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


    public Transform FindClosestPassPoint(GameObject chef)
    {
        //default start so they have something to compare to
        float distance = 100000;
        Transform closestPass = null;

        foreach (Transform t in unoccupiedPassPoints) 
        {
            if(Vector3.Distance(t.transform.position, chef.transform.position) < distance)
            {
                closestPass = t;
                distance = Vector3.Distance(t.transform.position, chef.transform.position);
            }
        }

        return closestPass;
    }

}
