using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChefData;
using UnityEngine.AI;

public class StaffData : GameBehaviour
{
    public enum StaffroomInteractions { Wander, Idle }
    public StaffroomInteractions staffroomInteractions;

    NavMeshAgent agent;

    [Header("Wander")]
    [SerializeField] float wanderRadius;
    [SerializeField] Vector3 wanderDestination;
    bool reachedWanderPos;

    [Header("Return to Station")]
    Vector3 stationPos;
    bool inWorkArea = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_GM.playState == GameManager.PlayState.Closed)
        {
            if(agent.speed != _SM.casualSpeed) agent.speed = _SM.casualSpeed;
            if (inWorkArea) inWorkArea = false;
            switch (staffroomInteractions)
            {
                case StaffroomInteractions.Wander:

                    //Get positiont to walk to
                    if (wanderDestination == Vector3.zero)
                    {
                        //print("get new pos");
                        wanderDestination = GetWanderPoint(_SM.staffRoomZone.position, "Staff");
                        reachedWanderPos = false;

                        if(wanderDestination != Vector3.zero)
                        {
                            agent.isStopped = false;
                            agent.SetDestination(wanderDestination);
                        }
                        

                    }

                    //Stop for a bit them find new positon
                    if (!reachedWanderPos && Vector3.Distance(transform.position, wanderDestination) < 1)
                    {
                        reachedWanderPos = true;
                        agent.isStopped = true;
                        ExecuteAfterSeconds(Random.Range(1, 8), () => wanderDestination = Vector3.zero);

                    }

                    break;
            }
        }
        else if (_GM.playState == GameManager.PlayState.Open)
        {
            if (!inWorkArea)
            {
                NavMeshHit navMeshHit;
                agent.SamplePathPosition(NavMesh.AllAreas, 1f, out navMeshHit);
                //walk to appropriate zone
                if (navMeshHit.mask == 1 << NavMesh.GetAreaFromName("Staff") && stationPos == Vector3.zero)
                {
                    agent.isStopped = false;
                    ReturnToWork();
                }
                //check if in waiter or chef zone
                else if ( Vector3.Distance(transform.position, stationPos) < 1)
                {
                    inWorkArea = true;
                }
            }

        }




    }

    void ReturnToWork()
    {
        if(gameObject.tag == "Chef")
        {
            stationPos = GetWanderPoint(_SM.kitchenZone.position,gameObject.tag);
            agent.speed = _SM.returnToWorkSpeed;
        }
        if(gameObject.tag == "Waiter")
        {
            stationPos = GetWanderPoint(_SM.FOHZone.position, gameObject.tag);
            agent.speed = _SM.returnToWorkSpeed;

        }

        agent.SetDestination(stationPos);

    }

    //randomly wander in staffroom
    Vector3 GetWanderPoint(Vector3 center, string area)
    {
        Vector3 result = Vector3.zero;
        Vector3 randomPoint = center + Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1, 1 << NavMesh.GetAreaFromName(area)))
        {
            //print("valid pos");
            result = hit.position;
            return result;


        }
        return result;
    }

}
