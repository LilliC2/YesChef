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
    public bool served;

    // Start is called before the first frame update
    void Start()
    {
        conveyerbeltCorners = _GM.conveyerbeltPoints;
        index = _GM.finishedFoodQueue.Length - _FM.foodInWave.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //look towards corner

        if(!served)
        {
            if (index != _FM.queuedFood.IndexOf(gameObject))
            {
                transform.position = Vector3.MoveTowards(transform.position, _GM.finishedFoodQueue[index].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());

                if (Vector3.Distance(transform.position, _GM.finishedFoodQueue[index].position) < 0.2f)
                {
                    index--;

                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _GM.finishedFoodQueue[_FM.queuedFood.IndexOf(gameObject)].position) < 0.2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _GM.finishedFoodQueue[_FM.queuedFood.IndexOf(gameObject)].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());

                }

            }
        }
       

        //if(!served)
        //{
        //    //move towards corner
        //    if (index != conveyerbeltCorners.Length - 1)
        //    {
        //        transform.LookAt(conveyerbeltCorners[index].position);

        //        transform.position = Vector3.MoveTowards(transform.position, conveyerbeltCorners[index].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());

        //        if (Vector3.Distance(transform.position, conveyerbeltCorners[index].position) <= 0.05f)
        //        {
        //            //if (Vector3.Distance(transform.position, conveyerbeltCorners[conveyerbeltCorners.Length-1].position) <= 0.05f) Destroy(gameObject);
        //            if (index < conveyerbeltCorners.Length) index++;



        //        }
        //    }
        //    else
        //    {
        //        //join finished food queue

        //        //not working
        //        //print("FM index = " + _FM.cookedFood.IndexOf(gameObject));
        //        //    print("GM index = " + _GM.finishedFoodQueue[_FM.cookedFood.IndexOf(gameObject)]);
        //        if (_FM.cookedFood.Contains(gameObject)) transform.position = Vector3.MoveTowards(transform.position, _GM.finishedFoodQueue[_FM.cookedFood.IndexOf(gameObject)].position, Time.deltaTime * _GM.CalculateConveyerbeltSpeed());
        //        //  if(_FM.cookedFood[0] == gameObject && !gameObject.GetComponent<FoodData>().foodData.isCooked) transform.position = Vector3.MoveTowards(transform.position, _FM.destroyRawFoodPoint, Time.deltaTime * _GM.CalculateConveyerbeltSpeed()));
        //    }
        //}





    }
}