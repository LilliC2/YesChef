using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassManager : Singleton<PassManager> 
{
    [SerializeField] List<FurnitureItemHolder> unoccupiedPassPoints;
    [SerializeField] List<FurnitureItemHolder> occupiedPassPoints;

    private void Awake()
    {
        //add all children to passPoints
       var array = GetComponentsInChildren<FurnitureItemHolder>();
        unoccupiedPassPoints = array.ToList();
    }
    /// <summary>
    /// Set pass point to occupied
    /// </summary>
    /// <param name="passPoint"></param>
    public void OccupiedPassPoint(FurnitureItemHolder passPoint)
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
    public void UnoccupiedPassPoint(FurnitureItemHolder passPoint)
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
    public FurnitureItemHolder FindClosestPassPoint(GameObject chef)
    {
        //default start so they have something to compare to
        float distance = 100000;
        FurnitureItemHolder closestPass = null;

        foreach (FurnitureItemHolder t in unoccupiedPassPoints) 
        {
            if(Vector3.Distance(t.transform.position, chef.transform.position) < distance)
            {
                closestPass = t.GetComponent<FurnitureItemHolder>();
                distance = Vector3.Distance(t.transform.position, chef.transform.position);
            }
        }

        return closestPass;
    }

}
