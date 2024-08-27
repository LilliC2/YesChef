using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkStationManager : Singleton<WorkStationManager>
{
    public List<GameObject> unoccupiedWorkStations_Cutting;
    public List<GameObject> unoccupiedWorkStations_Kneading;
    public List<GameObject> unoccupiedWorkStations_Mixing;
    public List<GameObject> unoccupiedWorkStations_Cooking;

    private void Awake()
    {
        //add workstations to lists
        unoccupiedWorkStations_Cutting = (GameObject.FindGameObjectsWithTag("CuttingStation")).ToList();
        unoccupiedWorkStations_Kneading = (GameObject.FindGameObjectsWithTag("KneadingStation")).ToList();
        unoccupiedWorkStations_Mixing = (GameObject.FindGameObjectsWithTag("MixingStation")).ToList();
        unoccupiedWorkStations_Cooking = (GameObject.FindGameObjectsWithTag("CookingStation")).ToList();

    }

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

    public void ChangeToUnoccupied(GameObject workstation)
    {
        workstation.GetComponent<WorkStation>().status = WorkStation.Status.Unoccupied;

        switch (workstation.tag) 
        {
            case "CuttingStation":
                if (!unoccupiedWorkStations_Cutting.Contains(workstation)) unoccupiedWorkStations_Cutting.Add(workstation);
                break;
            case "CookingStation":
                if (unoccupiedWorkStations_Cooking.Contains(workstation)) unoccupiedWorkStations_Cooking.Add(workstation);
                break;
            case "MixingStation":
                if (unoccupiedWorkStations_Mixing.Contains(workstation)) unoccupiedWorkStations_Mixing.Add(workstation);
                break;
            case "KneadingStation":
                if (unoccupiedWorkStations_Kneading.Contains(workstation)) unoccupiedWorkStations_Kneading.Add(workstation);
                break;
        }
    }

    public void ChangeToOccupied(GameObject workstation)
    {
        workstation.GetComponent<WorkStation>().status = WorkStation.Status.Occupied;

        switch (workstation.tag)
        {
            case "CuttingStation":
                if(unoccupiedWorkStations_Cutting.Contains(workstation)) unoccupiedWorkStations_Cutting.Remove(workstation);

                break;
            case "CookingStation":
                if (unoccupiedWorkStations_Cooking.Contains(workstation)) unoccupiedWorkStations_Cooking.Remove(workstation);
                break;
            case "MixingStation":
                if (unoccupiedWorkStations_Mixing.Contains(workstation)) unoccupiedWorkStations_Mixing.Remove(workstation);
                break;
            case "KneadingStation":
                if (unoccupiedWorkStations_Kneading.Contains(workstation)) unoccupiedWorkStations_Kneading.Remove(workstation);
                break;
        }
    }
}
