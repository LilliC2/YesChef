using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationManager : Singleton<WorkStationManager>
{
    public List<GameObject> unoccupiedWorkStations_Cutting;
    public List<GameObject> unoccupiedWorkStations_Kneading;
    public List<GameObject> unoccupiedWorkStations_Mixing;
    public List<GameObject> unoccupiedWorkStations_Cooking;

    public GameObject FindClosestWorkstation(string skill, GameObject chef)
    {
        //default start so they have something to compare to
        float distance = 100000;
        GameObject closestsStation = null;

        switch (skill)
        {
            case "Cooking":

                foreach(GameObject workstation in unoccupiedWorkStations_Cooking)
                {
                    if(Vector3.Distance(workstation.transform.position,chef.transform.position)< distance)
                    {
                        distance = Vector3.Distance(workstation.transform.position, chef.transform.position);
                        closestsStation = workstation;
                    }
                }
                break;
            case "Cutting":

                foreach(GameObject workstation in unoccupiedWorkStations_Cutting)
                {
                    if(Vector3.Distance(workstation.transform.position,chef.transform.position)< distance)
                    {
                        distance = Vector3.Distance(workstation.transform.position, chef.transform.position);
                        closestsStation = workstation;
                    }
                }
                break;
            case "Mixing":

                foreach(GameObject workstation in unoccupiedWorkStations_Mixing)
                {
                    if(Vector3.Distance(workstation.transform.position,chef.transform.position)< distance)
                    {
                        distance = Vector3.Distance(workstation.transform.position, chef.transform.position);
                        closestsStation = workstation;
                    }
                }
                break;
            case "Kneading":

                foreach(GameObject workstation in unoccupiedWorkStations_Kneading)
                {
                    if(Vector3.Distance(workstation.transform.position,chef.transform.position)< distance)
                    {
                        distance = Vector3.Distance(workstation.transform.position, chef.transform.position);
                        closestsStation = workstation;
                    }
                }
                break;
        }

        return closestsStation;
    }

    public void AddToUnoccupiedList(GameObject workstation)
    {
        workstation.GetComponent<WorkStation>().status = WorkStation.Status.Unoccupied;

        switch (workstation.tag) 
        {
            case "CuttingStation":
                unoccupiedWorkStations_Cutting.Add(workstation);
                break;
            case "CookingStation":
                unoccupiedWorkStations_Cooking.Add(workstation);
                break;
            case "MixingStation":
                unoccupiedWorkStations_Mixing.Add(workstation);
                break;
            case "KneadingStation":
                unoccupiedWorkStations_Kneading.Add(workstation);
                break;
        }
    }

    public void RemoveFromUnoccupiedList(GameObject workstation)
    {
        workstation.GetComponent<WorkStation>().status = WorkStation.Status.Occupied;

        switch (workstation.tag)
        {
            case "CuttingStation":
                unoccupiedWorkStations_Cutting.Remove(workstation);
                break;
            case "CookingStation":
                unoccupiedWorkStations_Cooking.Remove(workstation);
                break;
            case "MixingStation":
                unoccupiedWorkStations_Mixing.Remove(workstation);
                break;
            case "KneadingStation":
                unoccupiedWorkStations_Kneading.Remove(workstation);
                break;
        }
    }
}
