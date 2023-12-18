using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodMovement : GameBehaviour
{
    [SerializeField]
    float foodSpeed;
    Transform[] conveyerbeltCorners;
    int index;

    private float initializationTime;

    // Start is called before the first frame update
    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
        conveyerbeltCorners = _GM.conveyerbeltPoints;

    }

    // Update is called once per frame
    void Update()
    {
        //look towards corner

        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        //print(timeSinceInitialization);


        //move towards corner
        if (index != conveyerbeltCorners.Length-1)
        {
            transform.LookAt(conveyerbeltCorners[index].position);

            transform.position = Vector3.MoveTowards(transform.position, conveyerbeltCorners[index].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());

            if (Vector3.Distance(transform.position, conveyerbeltCorners[index].position) <= 0.05f)
            {
                //if (Vector3.Distance(transform.position, conveyerbeltCorners[conveyerbeltCorners.Length-1].position) <= 0.05f) Destroy(gameObject);
                if(index<conveyerbeltCorners.Length) index++;



            }
        }
        else
        {
            //join finished food queue
            if(_FM.cookedFood.Contains(gameObject)) transform.position = Vector3.MoveTowards(transform.position,_GM.finishedFoodQueue[_FM.cookedFood.IndexOf(gameObject)].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());
        }

        

    }
}
