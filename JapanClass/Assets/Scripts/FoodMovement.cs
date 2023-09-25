using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodMovement : GameBehaviour
{

    NavMeshAgent agent;
    Vector3 endOfConveyerBelt;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        endOfConveyerBelt = GameObject.Find("FoodDestroyPoint").transform.position;
        agent.destination = endOfConveyerBelt;
    }

    // Update is called once per frame
    void Update()
    {
        


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                //Reached end of Conveyer Belt

                //Call function here to check food status etc.
            }
        }
    }
}
