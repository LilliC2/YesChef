using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefData : GameBehaviour
{

    public ChefClass chefData;
    bool foundFood;
    GameObject currentFood;
    float elapsed = 1;
    [SerializeField]
    LayerMask rawFood;

    [SerializeField]
    Collider[] rawFoodInRange;

    // Start is called before the first frame update
    void Start()
    {
        foundFood = false;

    }

    // Update is called once per frame
    void Update()
    {

        //look at food
        if (currentFood != null) transform.LookAt(currentFood.transform.position);

        //check if any raw food are in range
        rawFoodInRange = Physics.OverlapSphere(transform.position, chefData.range, rawFood);

        //check if chef has compatible skill
        if (!foundFood)
        {
            for (int i = 0; i < rawFoodInRange.Length; i++)
            {
                currentFood = rawFoodInRange[i].gameObject;
                if (chefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneeding)
                {
                    print("I can kneed it");

                    foundFood = true;
                }
                else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
                {
                    print("I can cut it");

                    foundFood = true;
                }
                else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
                {
                    print("I can cook it");
                    foundFood = true;
                }
                else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
                {
                    print("I can mix it");

                    foundFood = true;
                }
                else
                {
                    foundFood = false;
                }

            }
        }
        //when food is found
        else
        {
            print("Found food i can cook");
            //every second, add skillPrepPoints to food skillPrepPoints

            if (Vector3.Distance(transform.position, currentFood.transform.position) < chefData.range 
                && currentFood != null && currentFood.GetComponent<FoodData>().foodData.isCooked != true)
            {
                print("Cooking");

                elapsed += Time.deltaTime;
                if (elapsed >= 1f)
                {
                    elapsed = elapsed % 1f;
                    //add prep points
                    //kneeding
                    if (chefData.kneedSkill) currentFood.GetComponent<FoodData>().foodData.kneedPrepPoints += chefData.kneedEffectivness;
                    //cutting
                    if (chefData.cutSkill) currentFood.GetComponent<FoodData>().foodData.cutPrepPoints += chefData.cutEffectivness;
                    //mixing
                    if (chefData.mixSkill) currentFood.GetComponent<FoodData>().foodData.mixPrepPoints += chefData.mixEffectivness;
                    //cooking
                    if (chefData.cookSkill) currentFood.GetComponent<FoodData>().foodData.cookPrepPoints += chefData.cookEffectivness;
                }
            }
            else
            {
                foundFood = false;
                currentFood = null;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chefData.range);
    }

    private void OnMouseDown()
    {
        _UI.OpenChefPopUp(this.gameObject);
    }
}
