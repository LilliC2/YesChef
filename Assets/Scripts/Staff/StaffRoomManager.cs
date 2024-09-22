using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaffRoomManager : Singleton<StaffRoomManager>
{
    [Header("Interactable Objects")]
    List<GameObject> occupiedChairs = new List<GameObject>();
    public List<GameObject> unoccupiedChairs = new List<GameObject>();

    [SerializeField] GameObject staffRoomRoof;

    // Start is called before the first frame update
    void Start()
    {
        //get all objects with StaffChair tag
        //var chairs = (GameObject.FindGameObjectsWithTag("StaffChair")).ToList();
        //unoccupiedChairs = (chairs).ToList();

        _GM.event_playStateClose.AddListener(CloseOrOpenRoof);
        _GM.event_playStateOpen.AddListener(CloseOrOpenRoof);
    }

    void CloseOrOpenRoof()
    {
        staffRoomRoof.SetActive(!staffRoomRoof.activeSelf);
    }

    public void ChangeObjectToOccupied(GameObject obj)
    {
        switch(obj.tag)
        {
            case "StaffChair":

                occupiedChairs.Add(obj);
                unoccupiedChairs.Remove(obj);

                break;
        }
    }
    
    public void ChangeObjectToUnoccupied(GameObject obj)
    {
        switch(obj.tag)
        {
            case "StaffChair":

                occupiedChairs.Remove(obj);
                unoccupiedChairs.Add(obj);

                break;
        }
    }

    public GameObject FindClosestChair(GameObject _staff)
    {
        float distance = 10000;
        GameObject closestChair = null;

        foreach (var chair in unoccupiedChairs)
        {
            if (Vector3.Distance(_staff.transform.position, chair.transform.position) < distance)
            {
                distance = Vector3.Distance(_staff.transform.position, chair.transform.position);
                closestChair = chair;
            }
        }

        return closestChair;
    }
}
