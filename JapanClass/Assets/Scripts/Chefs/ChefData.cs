using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefData : MonoBehaviour
{

    public ChefClass chefData;
    bool foundFood;
    GameObject currentFood;
    float elapsed = 1;
    [SerializeField]
    LayerMask rawFood;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if any raw food are in range
        var rawFoodInRange = Physics.OverlapSphere(transform.position, chefData.range, rawFood);

        //check if chef has compatible skill
        if (!foundFood)
        {
            for (int i = 0; i < rawFoodInRange.Length; i++)
            {
                //if chef can kneed
                if (chefData.kneedSkill)
                {
                    //check if food can be kneeded
                    if (rawFoodInRange[i].gameObject.GetComponent<FoodData>().foodData.needsKneeding)
                    {
                        currentFood = rawFoodInRange[i].gameObject;
                        foundFood = true;
                    }
                }

            }
        }
        //when food is found
        else
        {
            print("Found food i can cook");
            //every second, add skillPrepPoints to food skillPrepPoints

            if (Vector3.Distance(transform.position, currentFood.transform.position) < chefData.range)
            {
                elapsed += Time.deltaTime;
                if (elapsed >= 1f)
                {
                    elapsed = elapsed % 1f;
                    print("Adding kneed");
                    //add prep points
                    //kneeding
                    if (chefData.kneedSkill) currentFood.GetComponent<FoodData>().foodData.kneedPrepPoints += chefData.kneedPrepPoints;
                }
            }
            else
            {
                foundFood = false;
                currentFood = new();
            }
        }

    }
}
