using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Personality;
using static StaffManager;

[System.Serializable]
public class StaffBehaviour
{
    public PersonalityTypes personalityType;

    public Personality personalityData;
    
    public enum MovementState { NotSet, Idle, Wander, TravelToDestination, Sit}
    public MovementState movementState;

    public enum ActionState { NotSet, Idle, AskPlayerQuestion, TalkToPlayer, TalkToStaff}
    public ActionState actionState;
}
[System.Serializable]
public class Personality
{
    public PersonalityTypes name;

    public enum StateProbability { Low, Medium, High }
    public StateProbability idle;
    public StateProbability wander;
    public StateProbability travelToDestination;
    public StateProbability sit;


    public StateProbability talkToPlayer;
    public StateProbability askPlayerQuestion;
    public StateProbability talkToStaff;



}


public class StaffManager : Singleton<StaffManager>
{
    public enum PersonalityTypes { Chatty, Loner, Cynical, Sassy, AirHead, Hardy, Timid, Chill}
    public Personality[] personalityBehaviours;



    [Header("Staff")]
    public List<GameObject> allWaiterStaff; //all staff in game waiter
    public List<GameObject> allChefStaff; //all staff in game chef

    public int maxActiveStaff;

    //staff unlocked by player
    int unlockedChefs;
    int unlockedWaiters;
    public List<GameObject> totalActiveStaff; //staff on the floor rn
    public List<GameObject> chefActiveStaff; //staff on the floor rn
    public List<GameObject> waiterActiveStaff; //staff on the floor rn
    public List<GameObject> totalHiredStaff; //all staff player has unlocked

    [Header("Work Zones")]
    public Transform staffRoomZone, kitchenZone, FOHZone;

    public float returnToWorkSpeed;
    public float casualSpeed;


    [Header("AI")]
    public Sprite staffQuestionSprite;
    public Sprite staffTalkSprite;


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
            while(totalHiredStaff.Contains(staffHired))
            {
                staffHired = allChefStaff[Random.Range(0, allChefStaff.Count)];

            }

            unlockedChefs++;
        }
        else if(_staffType == "Waiter")
        {
            staffHired = allWaiterStaff[Random.Range(0, allWaiterStaff.Count)];

            //keep randomising until we get one that is not in hired staff
            while (totalHiredStaff.Contains(staffHired))
            {
                staffHired = allWaiterStaff[Random.Range(0, allWaiterStaff.Count)];

            }

            unlockedWaiters++;
        }

        totalHiredStaff.Add(staffHired);

        //activate on purchase
        if (totalActiveStaff.Count != maxActiveStaff)
        {
            //ActivateStaff(staffHired);
            if(_staffType == "Waiter") waiterActiveStaff.Add(staffHired);
            else if (_staffType == "Chef") chefActiveStaff.Add(staffHired);

            _UI.ToggleStaffOn(staffHired,staffHired.GetComponent<StaffData>().staffName);

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
        if(totalHiredStaff.Contains(_staff))
        {
            //add to activestaff
            if (_staff.tag == "Waiter") waiterActiveStaff.Add(_staff);
            else if (_staff.tag == "Chef") chefActiveStaff.Add(_staff);
            totalActiveStaff.Add(_staff);
            _staff.SetActive(true);

        }

    }

    public void DeactivateStaff(GameObject _staff)
    {
        if (totalHiredStaff.Contains(_staff))
        {
            //add to activestaff
            totalActiveStaff.Remove(_staff);
            if (waiterActiveStaff.Contains(_staff)) waiterActiveStaff.Remove(_staff);
            else if (chefActiveStaff.Contains(_staff)) chefActiveStaff.Remove(_staff);
            _staff.SetActive(false);
        }
    }

    public int ActionPercentageProbability(StateProbability actionProbabilty)
    {
        int probability = 0;
        switch (actionProbabilty)
        {
            case StateProbability.Low:
                probability = 20;
                break;
            case StateProbability.Medium:
                probability = 50;
                break;
            case StateProbability.High:
                probability = 70;
                break;
        }

        return probability;
    }
}
