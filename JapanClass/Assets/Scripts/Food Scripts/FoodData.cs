using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{

    public FoodClass foodData;
    bool isComplete;

    GameObject uncookedFood;
    GameObject cookedFood;

    // Start is called before the first frame update
    void Start()
    {
        uncookedFood = transform.GetChild(0).gameObject;
        cookedFood = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        foodData.isCooked = CheckIfComplete();
        
        if(foodData.isCooked)
        {
            uncookedFood.SetActive(false);
            cookedFood.SetActive(true);

            //change layer to complete food
            gameObject.layer = 6;
        }
    }

    bool CheckIfComplete()
    {
        isComplete = true;

        //find all needed skills
        if(foodData.needsKneading)
        {
            //check if all skills are complete
            if (foodData.kneedPrepPoints < foodData.maxKneedPrepPoints) isComplete = false;
        }
        
        if(foodData.needsCooking)
        {
            //check if all skills are complete
            if (foodData.cookPrepPoints < foodData.maxCookPrepPoints) isComplete = false;
        }
        
        if(foodData.needsMixing)
        {
            //check if all skills are complete
            if (foodData.mixPrepPoints < foodData.maxMixPrepPoints) isComplete = false;
        }
        
        if(foodData.needsCutting)
        {
            //check if all skills are complete
            if (foodData.cutPrepPoints < foodData.maxCutPrepPoints) isComplete = false;
        }

        return isComplete;
    }
}
