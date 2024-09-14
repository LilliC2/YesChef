using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassManager : Singleton<PassManager> 
{
    [SerializeField] List<Transform> unoccupiedPassPoints;
    [SerializeField] List<Transform> occupiedPassPoints;

    private void Awake()
    {
        //add all children to passPoints
        foreach (Transform t in transform)
        {
            unoccupiedPassPoints.Add(t);
        }
    }
    /// <summary>
    /// Set pass point to occupied
    /// </summary>
    /// <param name="passPoint"></param>
    public void OccupiedPassPoint(Transform passPoint)
    {
        if(!occupiedPassPoints.Contains(passPoint))
        {
            unoccupiedPassPoints.Remove(passPoint);
            occupiedPassPoints.Add(passPoint);
        }
        
    }
    
    /// <summary>
    /// Set pass point to unoccupied
    /// </summary>
    /// <param name="passPoint"></param>
    public void UnoccupiedPassPoint(Transform passPoint)
    {

        if(!unoccupiedPassPoints.Contains(passPoint))
        {
            unoccupiedPassPoints.Add(passPoint);
            occupiedPassPoints.Remove(passPoint);
        }
        
    }


    /// <summary>
    /// Find closest pass point to gameobject
    /// </summary>
    /// <param name="chef"></param>
    /// <returns></returns>
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
