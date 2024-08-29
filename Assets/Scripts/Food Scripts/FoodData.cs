using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : GameBehaviour
{
    

    public FoodClass foodData;
    bool isComplete;

    GameObject uncookedFood;
    GameObject cookedFood;

    [SerializeField]
    ParticleSystem starburst;
    bool playAnim;

    // Start is called before the first frame update
    void Start()
    {
        starburst = GetComponentInChildren<ParticleSystem>();
        uncookedFood = transform.GetChild(0).gameObject;
        cookedFood = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        foodData.isComplete = CheckIfComplete();
        
        if(foodData.isComplete)
        {
            if(!playAnim)
            {
                starburst.Play();
                playAnim = true;
            }
            uncookedFood.SetActive(false);
            cookedFood.SetActive(true);

            //change layer to complete food
            gameObject.layer = 6;
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
