using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{

    public FoodClass foodData;
    bool isComplete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodData.isCooked = CheckIfComplete();
    }

    bool CheckIfComplete()
    {
        isComplete = true;

        //find all needed skills
        if(foodData.needsKneeding)
        {
            //check if all skills are complete
            if (foodData.kneedPrepPoints < foodData.maxKneedPrepPoints) isComplete = false;
        }

        return isComplete;
    }
}
