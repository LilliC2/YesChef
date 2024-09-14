using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StaffManager : Singleton<StaffManager>
{
    [Header("Staff")]
    public List<GameObject> allWaiterStaff; //all staff in game waiter
    public List<GameObject> allChefStaff; //all staff in game chef

    public int maxActiveStaff;

    //staff unlocked by player
    int unlockedChefs;
    int unlockedWaiters;
    public List<GameObject> activeStaff; //staff on the floor rn
    public List<GameObject> hiredStaff; //all staff player has unlocked

    [Header("Work Zones")]
    public Transform staffRoomZone, kitchenZone, FOHZone;

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

    public bool CanHireMoreStaff(string _staffType)
    {
        if (_staffType == "Chef")
        {
            if(unlockedChefs == allChefStaff.Count)
                return false;
        }
        else if (_staffType == "Waiter")
        {
            if (unlockedWaiters == allWaiterStaff.Count)
                return false;
        }
          
        return true;
    }

    public GameObject HireStaff(string _staffType)
    {
        GameObject staffHired = null;

        if(_staffType == "Chef")
        {
            staffHired = allChefStaff[Random.Range(0, allChefStaff.Count)];

            //keep randomising until we get one that is not in hired staff
            while(hiredStaff.Contains(staffHired))
            {
                staffHired = allChefStaff[Random.Range(0, allChefStaff.Count)];

            }

            unlockedChefs++;
        }
        else if(_staffType == "Waiter")
        {
            staffHired = allWaiterStaff[Random.Range(0, allWaiterStaff.Count)];

            //keep randomising until we get one that is not in hired staff
            while (hiredStaff.Contains(staffHired))
            {
                staffHired = allWaiterStaff[Random.Range(0, allWaiterStaff.Count)];

            }

            unlockedWaiters++;
        }

        hiredStaff.Add(staffHired);

        //activate on purchase
        if(activeStaff.Count != maxActiveStaff)
        {
            //ActivateStaff(staffHired);

            _UI.ToggleStaffOn(staffHired);
            

        }

        return staffHired;

    }

    /// <summary>
    /// Pause NavMesh agent for period of time
    /// </summary>
    public IEnumerator PauseAgent(NavMeshAgent _agent, float _time)
    {
        //print("Pause");
        _agent.isStopped = true;
        yield return new WaitForSeconds(_time);
        _agent.isStopped = false;
        
        yield return _agent.isStopped;

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
