using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodMovement : GameBehaviour
{
    [SerializeField]
    float foodSpeed;
    Transform[] conveyerbeltCorners;
    [SerializeField]
    int conveyerbeltIndex;
    public bool served;
    public bool beingHeld;

    public enum FoodState { OnConveyerbelt, OnPass, BeingHeld}
    public FoodState foodState;

    // Start is called before the first frame update
    void Start()
    {
        conveyerbeltCorners = _GM.conveyerbeltPoints;
        //track what index the food is currently at on the conveyerbelt
        conveyerbeltIndex = _GM.conveyerbeltPoints.Length-1;
    }

    // Update is called once per frame
    void Update()
    {
        /* x = index of food in _FM.foodNeedPreperation_list
         * 
         * food will move down conveyerbelt until converybeltIndex = x
         */

        if (foodState == FoodState.OnConveyerbelt)
        {
            //If food is raw and just placed
            if (conveyerbeltIndex != _FM.foodNeedPreperation_list.IndexOf(gameObject))
            {
                transform.position = Vector3.MoveTowards(transform.position, _GM.conveyerbeltPoints[conveyerbeltIndex].position, Time.deltaTime * _FM.conveyerbeltSpeed);

                if (Vector3.Distance(transform.position, _GM.conveyerbeltPoints[conveyerbeltIndex].position) < 0.2f)
                {
                    conveyerbeltIndex--;

                }
            }
            
        }




    }
}
