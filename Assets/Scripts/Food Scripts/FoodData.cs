using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{

    public enum FoodState { Unprepared, Finished, Dirty}
    public FoodState foodState;
    public enum FoodMovement { OnConveyerbelt, OnPass, BeingHeld }
    public FoodMovement foodMovement;

    bool isComplete;

    public OrderClass order;

    Transform passPoint; //where it is placed on the pass

    GameObject uncookedFood;
    GameObject cookedFood;
    GameObject dirtyFood;

    [SerializeField]
    ParticleSystem starburst;
    bool playAnim;

    // Start is called before the first frame update
    void Start()
    {
        foodState = FoodState.Unprepared;

        starburst = GetComponentInChildren<ParticleSystem>();
        uncookedFood = transform.GetChild(0).gameObject;
        cookedFood = transform.GetChild(1).gameObject;
        dirtyFood = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        isComplete = CheckIfComplete();

        //switch look
        switch (foodState)
        {
            case FoodState.Unprepared:
                if(!uncookedFood.activeSelf)
                {
                    uncookedFood.SetActive(true);
                    cookedFood.SetActive(false);
                    dirtyFood.SetActive(false);
                }
                break;
            case FoodState.Finished:
                if (!cookedFood.activeSelf)
                {
                    starburst.Play();

                    cookedFood.SetActive(true);
                    uncookedFood.SetActive(false);
                    dirtyFood.SetActive(false);
                    gameObject.layer = 6;

                }
                break;
            case FoodState.Dirty:
                if (!dirtyFood.activeSelf)
                {
                    dirtyFood.SetActive(true);
                    uncookedFood.SetActive(false);
                    cookedFood.SetActive(false);

                }
                break;
        }


    }

    public void SetPassPoint(Transform _passPoint)
    {
        passPoint = _passPoint;
    }

    public Transform ReturnPassPoint()
    {
        return passPoint;
    }

    public bool CheckIfComplete()
    {
        isComplete = true;

        //check if all the skills it required are complete
        if(order.foodClass.needsKneading && !order.foodClass.kneadedWorkComplete) isComplete = false;

        if (order.foodClass.needsCooking && !order.foodClass.cookWorkComplete) isComplete = false;

        if(order.foodClass.needsMixing && !order.foodClass.mixWorkComplete) isComplete = false;
        
        if(order.foodClass.needsCutting && !order.foodClass.cutWorkComplete) isComplete = false ;


        return isComplete;
    }
}
