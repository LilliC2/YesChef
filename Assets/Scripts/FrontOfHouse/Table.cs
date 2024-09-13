using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : GameBehaviour
{
    public enum Status { Unoccupied, Occupied, Dirty }
    public Status status;

    public Transform waiterAttendPosition;

    //for groups of multiple customers
    [SerializeField] int numOfSeats;
    public List<Transform> unoccupiedSeats = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //get seats
        foreach (Transform t in gameObject.transform)
        {
            if(t.name.Contains("chair")) unoccupiedSeats.Add(t);
            if (t.name.Contains("WaiterAttendPosition")) waiterAttendPosition = t;
        }

        numOfSeats = unoccupiedSeats.Count;
        
    }

    public void ChangeToOccupied(Transform _seat)
    {
        if(unoccupiedSeats.Contains(_seat)) unoccupiedSeats.Remove(_seat);
    }
    
    public void ChangeToUnoccupied(Transform _seat)
    {
        if (unoccupiedSeats.Contains(_seat)) unoccupiedSeats.Add(_seat);

    }


}
