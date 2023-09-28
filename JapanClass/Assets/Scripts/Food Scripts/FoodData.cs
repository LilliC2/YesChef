using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{

    public FoodClass foodData;
    bool isComplete;
    bool updateGameObject;

    // Start is called before the first frame update
    void Start()
    {
        foodData.cookedFood = Resources.Load<GameObject>("Prefabs/"+foodData.name + "_CookedFood");
    }

    // Update is called once per frame
    void Update()
    {
        foodData.isCooked = CheckIfComplete();
        
        if(foodData.isCooked)
        {
            var angles = transform.rotation;
            //create new gameobject
            var cookedFoodOB = Instantiate(foodData.cookedFood, transform.position, angles,transform);

            //delete this gameobject
            Destroy(transform.GetChild(0).gameObject);
        }

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
