using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{

    public enum FoodState { Unprepared, Finished, Dirty}
    public FoodState foodState;
    public enum FoodMovement { OnConveyerbelt, OnPass, BeingHeld }
    public FoodMovement foodMovement;

    public FoodClass foodData;
    bool isComplete;

    public OrderClass order;

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

    public bool CheckIfComplete()
    {
        isComplete = true;

        //check if all the skills it required are complete
        if(foodData.needsKneading && !foodData.kneadedWorkComplete) isComplete = false;

        if (foodData.needsCooking && !foodData.cookWorkComplete) isComplete = false;

        if(foodData.needsMixing && !foodData.mixWorkComplete) isComplete = false;
        
        if(foodData.needsCutting && !foodData.cutWorkComplete) isComplete = false ;


        return isComplete;
    }
}
