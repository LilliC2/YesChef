using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChefData : GameBehaviour
{

    public ChefClass chefData;
    //bool foundFood;
    //GameObject currentFood;
    //float elapsed = 1;
    public LayerMask rawFood;

    public enum Targeting { First, Last, Strongest}
    public Targeting targeting;


    [Header("Audio")]
    [SerializeField]
    AudioSource cuttingAudio;
    AudioSource cookingAudio;
    AudioSource mixingAudio;
    AudioSource kneadingAudio;

    public GameObject rangeIndicator;

    public Animator anim;

    public bool validPos;

    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        //default targeting
        targeting = Targeting.First;

        anim = GetComponentInChildren<Animator>();

        //foundFood = false;
    }

    public void UpdateChefTargetting()
    {
        BroadcastMessage("UpdateTargetting");
    }

    void ChefPlaced()
    {
        rangeIndicator.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        ////check if any raw food are in range
        //rawFoodInRange = Physics.OverlapSphere(transform.position, chefData.range, rawFood);

        ////check if placed
        //if (placed)
        //{
        //    //check if chef has compatible skill
        //    if (!foundFood)
        //    {
        //        anim.SetBool("Cooking", false);

        //        if(rawFoodInRange.Length != 0)
        //        {
        //            switch (targeting)
        //            {
        //                case Targeting.First:
        //                    for (int i = 0; i < rawFoodInRange.Length; i++)
        //                    {
        //                        currentFood = rawFoodInRange[i].gameObject;
        //                        if (chefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
        //                        {
        //                            //print("I can kneed it");

        //                            foundFood = true;
        //                        }
        //                        else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
        //                        {
        //                            //print("I can cut it");

        //                            foundFood = true;
        //                        }
        //                        else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
        //                        {
        //                            //print("I can cook it");
        //                            foundFood = true;
        //                        }
        //                        else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
        //                        {
        //                            //print("I can mix it");

        //                            foundFood = true;
        //                        }
        //                        else
        //                        {
        //                            foundFood = false;
        //                        }
        //                    }
        //                    break;

        //                case Targeting.Last:
        //                    for (int i = rawFoodInRange.Length; i > 0; i--)
        //                    {
        //                        currentFood = rawFoodInRange[i].gameObject;
        //                        if (chefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
        //                        {
        //                            //print("I can kneed it");

        //                            foundFood = true;
        //                        }
        //                        else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
        //                        {
        //                            //print("I can cut it");

        //                            foundFood = true;
        //                        }
        //                        else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
        //                        {
        //                            //print("I can cook it");
        //                            foundFood = true;
        //                        }
        //                        else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
        //                        {
        //                            //print("I can mix it");

        //                            foundFood = true;
        //                        }
        //                        else
        //                        {
        //                            foundFood = false;
        //                        }
        //                    }

        //                    break;

        //                case Targeting.Strongest:

        //                    GameObject strongest = null;
        //                    FoodClass strongestFoodData = null;

        //                    if (rawFoodInRange.Length != 0)
        //                    {
        //                        strongest = rawFoodInRange[0].gameObject;
        //                        strongestFoodData = strongest.GetComponent<FoodData>().foodData;
        //                    }



        //                    for (int i = 0; i < rawFoodInRange.Length; i++)
        //                    {
        //                        currentFood = rawFoodInRange[i].gameObject;
        //                        var currentFoodData = currentFood.GetComponent<FoodData>().foodData;
        //                        if (chefData.kneedSkill && currentFoodData.needsKneading)
        //                        {
        //                            //print("I can kneed it");

        //                            if (strongestFoodData.kneedPrepPoints > currentFoodData.kneedPrepPoints)
        //                            {
        //                                strongest = currentFood;
        //                                strongestFoodData = currentFoodData;
        //                            }

        //                        }
        //                        else if (chefData.cutSkill && currentFoodData.needsCutting)
        //                        {
        //                            if (strongestFoodData.cutPrepPoints > currentFoodData.cutPrepPoints)
        //                            {
        //                                strongest = currentFood;
        //                                strongestFoodData = currentFoodData;
        //                            }
        //                        }
        //                        else if (chefData.cookSkill && currentFoodData.needsCooking)
        //                        {
        //                            if (strongestFoodData.cookPrepPoints > currentFoodData.cookPrepPoints)
        //                            {
        //                                strongest = currentFood;
        //                                strongestFoodData = currentFoodData;
        //                            }
        //                        }
        //                        else if (chefData.mixSkill && currentFoodData.needsMixing)
        //                        {
        //                            if (strongestFoodData.mixPrepPoints > currentFoodData.mixPrepPoints)
        //                            {
        //                                strongest = currentFood;
        //                                strongestFoodData = currentFoodData;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foundFood = false;
        //                        }
        //                    }
        //                    currentFood = strongest;
        //                    foundFood = true;



        //                    break;
        //            }
        //        }
               

                

        //    }
        //    //when food is found
        //    else
        //    {

        //        //look at food
        //        if (currentFood != null) transform.LookAt(currentFood.transform.position);
        //        //print("Found food i can cook");
        //        //every second, add skillPrepPoints to food skillPrepPoints

        //        if (currentFood != null && (rawFoodInRange.Contains(currentFood.gameObject.GetComponent<Collider>()) && currentFood.GetComponent<FoodData>().foodData.isCooked != true))
        //        {
        //            //print("Cooking");
        //            anim.SetBool("Cooking", true);

        //            elapsed += Time.deltaTime;
        //            if (elapsed >= 0.2f)
        //            {

        //                elapsed = elapsed % 0.2f;
        //                //add prep points
        //                //kneeding
        //                if (chefData.kneedSkill)
        //                {
        //                    if (kneadingAudio != null) kneadingAudio.Play();
        //                    currentFood.GetComponent<FoodData>().foodData.kneedPrepPoints += chefData.kneedEffectivness;
        //                }
        //                //cutting
        //                if (chefData.cutSkill)
        //                {
        //                    if (cuttingAudio != null) cuttingAudio.Play();
        //                    currentFood.GetComponent<FoodData>().foodData.cutPrepPoints += chefData.cutEffectivness;
        //                }
        //                //mixing
        //                if (chefData.mixSkill)
        //                {
        //                    if (mixingAudio != null) mixingAudio.Play();
        //                    currentFood.GetComponent<FoodData>().foodData.mixPrepPoints += chefData.mixEffectivness;
        //                }

        //                //cooking
        //                if (chefData.cookSkill)
        //                {
        //                    if (cookingAudio != null) cookingAudio.Play();
        //                    currentFood.GetComponent<FoodData>().foodData.cookPrepPoints += chefData.cookEffectivness;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (cookingAudio != null) cookingAudio.Stop();
        //            if (kneadingAudio != null) kneadingAudio.Stop();
        //            if (mixingAudio != null) mixingAudio.Stop();
        //            if (cuttingAudio != null) cuttingAudio.Stop();

        //            foundFood = false;
        //            currentFood = null;
        //        }
        //    }
        //}

        

    }

    void OnDrawGizmosSelected()
    {
        //// Display the explosion radius when selected
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, chefData.range);
    }

    private void OnMouseDown()
    {
        if(placed) _UI.OpenChefPopUp(this.gameObject);

    }
}
