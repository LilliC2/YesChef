using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;



public class TanukiChefData : GameBehaviour
{

    public enum TanukiUpgradesLine1 { None_0, Econimist_1, Capitalist_2, TanukiTycoon_3}
    public TanukiUpgradesLine1 tanukiUpgradesLine1;
    public enum TanukiUpgradesLine2 { None_0, Pupil_1, SousChef_2, MasterChef_3}
    public TanukiUpgradesLine2 tanukiUpgradesLine2;

    ChefClass currentChefData;
    ChefData chefDataClass;

    bool foundFood;
    GameObject currentFood;
    float elapsed = 1;
    LayerMask rawFood;

    enum Targeting { First, Last, Strongest }
    [SerializeField]
    Targeting targeting;

    [Header("Base Stats")]
    ChefClass baseChefData; //used for calcuations

    [Header("Audio")]
    [SerializeField]
    AudioSource cuttingAudio;
    AudioSource cookingAudio;
    AudioSource mixingAudio;
    AudioSource kneadingAudio;

    //public GameObject rangeIndicator;

    [SerializeField]
    Animator anim;

    //public bool validPos;

    [SerializeField]
    Collider[] rawFoodInRange;
    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        //default targeting
        targeting = Targeting.First;

        chefDataClass = GetComponent<ChefData>();
        rawFood = chefDataClass.rawFood;

        currentChefData = GetComponent<ChefData>().chefData;

        baseChefData = GetComponent<ChefData>().chefData;


        foundFood = false;

        _GM.event_updateMoney.AddListener(TanukiTycoonPrepPointBonus);
    }

    // Update is called once per frame
    void Update()
    {
        //check if any raw food are in range
        rawFoodInRange = Physics.OverlapSphere(transform.position, currentChefData.range, rawFood);

        //check if placed
        if (placed)
        {
            //check if chef has compatible skill
            if (!foundFood)
            {
                anim.SetBool("Cooking", false);

                if (rawFoodInRange.Length != 0)
                {
                    switch (targeting)
                    {
                        #region Targeting First
                        case Targeting.First:
                            for (int i = 0; i < rawFoodInRange.Length; i++)
                            {
                                currentFood = rawFoodInRange[i].gameObject;
                                if (currentChefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
                                {
                                    //print("I can kneed it");

                                    foundFood = true;
                                }
                                else if (currentChefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
                                {
                                    //print("I can cut it");

                                    foundFood = true;
                                }
                                else if (currentChefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
                                {
                                    //print("I can cook it");
                                    foundFood = true;
                                }
                                else if (currentChefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
                                {
                                    //print("I can mix it");

                                    foundFood = true;
                                }
                                else
                                {
                                    foundFood = false;
                                }
                            }
                            break;
                        #endregion

                        #region Targeting Last
                        case Targeting.Last:
                            for (int i = rawFoodInRange.Length; i > 0; i--)
                            {
                                currentFood = rawFoodInRange[i].gameObject;
                                if (currentChefData.kneedSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
                                {
                                    //print("I can kneed it");

                                    foundFood = true;
                                }
                                else if (currentChefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
                                {
                                    //print("I can cut it");

                                    foundFood = true;
                                }
                                else if (currentChefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
                                {
                                    //print("I can cook it");
                                    foundFood = true;
                                }
                                else if (currentChefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
                                {
                                    //print("I can mix it");

                                    foundFood = true;
                                }
                                else
                                {
                                    foundFood = false;
                                }
                            }

                            break;
                        #endregion

                        #region Targetting Strongest
                        case Targeting.Strongest:

                            GameObject strongest = null;
                            FoodClass strongestFoodData = null;

                            if (rawFoodInRange.Length != 0)
                            {
                                strongest = rawFoodInRange[0].gameObject;
                                strongestFoodData = strongest.GetComponent<FoodData>().foodData;
                            }



                            for (int i = 0; i < rawFoodInRange.Length; i++)
                            {
                                currentFood = rawFoodInRange[i].gameObject;
                                var currentFoodData = currentFood.GetComponent<FoodData>().foodData;
                                if (currentChefData.kneedSkill && currentFoodData.needsKneading)
                                {
                                    //print("I can kneed it");

                                    if (strongestFoodData.kneedPrepPoints > currentFoodData.kneedPrepPoints)
                                    {
                                        strongest = currentFood;
                                        strongestFoodData = currentFoodData;
                                    }

                                }
                                else if (currentChefData.cutSkill && currentFoodData.needsCutting)
                                {
                                    if (strongestFoodData.cutPrepPoints > currentFoodData.cutPrepPoints)
                                    {
                                        strongest = currentFood;
                                        strongestFoodData = currentFoodData;
                                    }
                                }
                                else if (currentChefData.cookSkill && currentFoodData.needsCooking)
                                {
                                    if (strongestFoodData.cookPrepPoints > currentFoodData.cookPrepPoints)
                                    {
                                        strongest = currentFood;
                                        strongestFoodData = currentFoodData;
                                    }
                                }
                                else if (currentChefData.mixSkill && currentFoodData.needsMixing)
                                {
                                    if (strongestFoodData.mixPrepPoints > currentFoodData.mixPrepPoints)
                                    {
                                        strongest = currentFood;
                                        strongestFoodData = currentFoodData;
                                    }
                                }
                                else
                                {
                                    foundFood = false;
                                }
                            }
                            currentFood = strongest;
                            foundFood = true;



                            break;
                            #endregion
                    }
                }




            }
            //when food is found
            else
            {

                //look at food
                if (currentFood != null) transform.LookAt(currentFood.transform.position);
                //print("Found food i can cook");
                //every second, add skillPrepPoints to food skillPrepPoints

                if (currentFood != null && (rawFoodInRange.Contains(currentFood.gameObject.GetComponent<Collider>()) && currentFood.GetComponent<FoodData>().foodData.isCooked != true))
                {
                    print("Cooking");
                    anim.SetBool("Cooking", true);
                    var currentFoodData = currentFood.GetComponent<FoodData>().foodData;
                    elapsed += Time.deltaTime;
                    if (elapsed >= 0.2f)
                    {

                        elapsed = elapsed % 0.2f;
                        //add prep points
                        //kneeding
                        if (currentChefData.kneedSkill)
                        {
                            if (kneadingAudio != null) kneadingAudio.Play();
                            currentFoodData.kneedPrepPoints += currentChefData.kneedEffectivness;
                        }
                        //cutting
                        if (currentChefData.cutSkill)
                        {
                            if (cuttingAudio != null) cuttingAudio.Play();
                            currentFoodData.cutPrepPoints += currentChefData.cutEffectivness;
                        }
                        //mixing
                        if (currentChefData.mixSkill)
                        {
                            if (mixingAudio != null) mixingAudio.Play();
                            currentFoodData.mixPrepPoints += currentChefData.mixEffectivness;
                        }

                        //cooking
                        if (currentChefData.cookSkill)
                        {
                            if (cookingAudio != null) cookingAudio.Play();
                            currentFoodData.cookPrepPoints += currentChefData.cookEffectivness;
                        }
                    }
                }
                else
                {
                    #region Cooked Food Bonuses from Upgrades

                    //upgrade higher than 0 (none)
                    if(tanukiUpgradesLine1 == TanukiUpgradesLine1.Econimist_1)
                    {
                        var orderCost = currentFood.GetComponent<FoodData>().foodData.orderCost;
                        currentFood.GetComponent<FoodData>().foodData.orderCost = orderCost + orderCost * 0.2f;
                        print("Update order cost, was " +orderCost + ", now " + currentFood.GetComponent<FoodData>().foodData.orderCost);

                    }
                    else if(tanukiUpgradesLine1 == TanukiUpgradesLine1.Capitalist_2 || tanukiUpgradesLine1 == TanukiUpgradesLine1.TanukiTycoon_3)
                    {
                        var orderCost = currentFood.GetComponent<FoodData>().foodData.orderCost;
                        currentFood.GetComponent<FoodData>().foodData.orderCost = orderCost + orderCost * 0.5f;
                        print("Update order cost, was " +orderCost + ", now " + currentFood.GetComponent<FoodData>().foodData.orderCost);

                    }

                    #endregion  

                    if (cookingAudio != null) cookingAudio.Stop();
                    if (kneadingAudio != null) kneadingAudio.Stop();
                    if (mixingAudio != null) mixingAudio.Stop();
                    if (cuttingAudio != null) cuttingAudio.Stop();

                    foundFood = false;
                    currentFood = null;
                }
            }
        }



    }

    void UpdateTargetting()
    {
        if (chefDataClass.targeting == ChefData.Targeting.First) targeting = Targeting.First;
        if (chefDataClass.targeting == ChefData.Targeting.Last) targeting = Targeting.Last;
        if (chefDataClass.targeting == ChefData.Targeting.Strongest) targeting = Targeting.Strongest;
    }

    void TanukiTycoonPrepPointBonus()
    {
        if(tanukiUpgradesLine1 == TanukiUpgradesLine1.TanukiTycoon_3)
        {
            print("Adding " + (_GM.money / 1000));
            currentChefData.cutEffectivness = baseChefData.cutEffectivness + Mathf.RoundToInt(0.1f * (_GM.money / 1000));
        }
    }

    void ChefPlaced()
    {
        anim.SetTrigger("Spawn");

        placed = true;

    }
    public void TanukiUpgradeLine2_AddSkills()
    {
        print("times called?");
        if (tanukiUpgradesLine2 == TanukiUpgradesLine2.Pupil_1)
        {
            //Unlock Mixing Skill
            baseChefData.mixEffectivness = 5;
            currentChefData.mixEffectivness = 5;
            currentChefData.mixSkill = true;

            //Upgrade current skills
            currentChefData.cutEffectivness += 5;
        }

        if(tanukiUpgradesLine2 == TanukiUpgradesLine2.SousChef_2)
        {
            //Unlock Kneading Skill
            baseChefData.kneedEffectivness = 5;
            currentChefData.kneedEffectivness = 5;
            currentChefData.kneedSkill = true;

            //Upgrade current skills
            currentChefData.cutEffectivness += 5;
            currentChefData.mixEffectivness += 5;
        }
        
        if(tanukiUpgradesLine2 == TanukiUpgradesLine2.MasterChef_3)
        {
            //Unlock Kneading Skill
            baseChefData.cookEffectivness = 5;
            currentChefData.cookEffectivness = 5;
            currentChefData.cookSkill = true;

            //Upgrade current skills
            currentChefData.cutEffectivness += 5;
            currentChefData.mixEffectivness += 5;
            currentChefData.kneedEffectivness += 5;

            //increase range
            currentChefData.range += 4;

            //THIS MAY CAUSE FUTURE PROBLEMSSS!!!!
            baseChefData.range = currentChefData.range;
            
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, currentChefData.range);
    }

    private void OnMouseDown()
    {
        if (placed) _UI.OpenChefPopUp(this.gameObject);

    }
}
