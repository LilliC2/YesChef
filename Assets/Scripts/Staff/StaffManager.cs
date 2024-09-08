using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : Singleton<StaffManager>
{
    [Header("Staff")]
    public List<GameObject> allWaiterStaff; //all staff in game waiter
    public List<GameObject> allChefStaff; //all staff in game chef

    //staff unlocked by player
    public List<GameObject> activeStaff; //staff on the floor rn
    public List<GameObject> hiredStaff; //all staff player has unlocked

    [Header("Work Zones")]
    public Vector3 staffRoomZone, kitchenZone, FOHZone;

    public float returnToWorkSpeed;
    public float casualSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HireStaff()
    {

    }


    public void ActivateStaff(GameObject _staff)
    {
        if(hiredStaff.Contains(_staff))
        {
            //add to activestaff
            activeStaff.Add(_staff);
            _staff.SetActive(true);
        }
    }

    public void DeactivateStaff(GameObject _staff)
    {
        if (hiredStaff.Contains(_staff))
        {
            //add to activestaff
            activeStaff.Remove(_staff);
            _staff.SetActive(false);
        }
    }
}
