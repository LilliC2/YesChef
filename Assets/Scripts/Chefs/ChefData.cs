using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ChefData : GameBehaviour
{

    public ChefClass chefData; //data used by chef, including percentages
    public ChefClass baseStatsChefData; //holds base stats, used when calculating percentages
    bool foundFood;
    GameObject currentFood;
    float elapsed = 1;
    [SerializeField]
    LayerMask rawFood;

    public enum Targeting { First, Last, Strongest }
    public Targeting targeting;

    public enum Task { Idle, FindFood, GetFood, CookFood, ReturnFood }
    public Task tasks;

    [SerializeField]
    bool isHoldingFood;
    public Vector3 homePos;
    NavMeshAgent agent;
    [SerializeField]
    GameObject holdfoodspot;

    [Header("Audio")]
    [SerializeField]
    AudioSource cuttingAudio;
    AudioSource cookingAudio;
    AudioSource mixingAudio;
    AudioSource kneadingAudio;

    public GameObject rangeIndicator;

    public Animator anim;

    public bool validPos;

    [SerializeField]
    Collider[] rawFoodInRange;
    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        //default targeting
        targeting = Targeting.First;
        agent = GetComponent<NavMeshAgent>();
        foundFood = false;
    }

    // Update is called once per frame
    void Update()
    {


        //check if placed
        if (placed)
        {
            }
            else
            {
                


                //take food off conveyerbelt

                //go to cooking station

                //work on food

                //when food finished return it to roughly its old position on conveyerbelt
            }

            //walk to food
            switch (tasks)
            {
                case Task.Idle://check if chef has compatible skill

                    if (!foundFood)
                    {
                        anim.SetBool("Cooking", false); //turn off cooking anim

                        switch (targeting)
                        {
                            #region First
                            case Targeting.First:
                                
                                break;
                            #endregion
                           
                    }

                    if (foundFood) tasks = Task.FindFood;
                    


                    }
                

                break;

                case Task.FindFood: //walk to food

                    agent.SetDestination(currentFood.transform.position);

                    //if in range
                    if(Vector3.Distance(transform.position, currentFood.transform.position) < 2f)
                    {
                        _FM.foodNeedPreperation_list.Remove(currentFood); //remove food from queue, this is the old system and will probably be updated
                        isHoldingFood = true;
                        tasks = Task.CookFood;

                    }




                break;






            }

            ////when food is found
            //else
            //{

            //    //look at food
            //    if (currentFood != null) transform.LookAt(currentFood.transform.position);
            //    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //    //print("Found food i can cook");
            //    //every second, add skillPrepPoints to food skillPrepPoints

            //    if (currentFood != null && (rawFoodInRange.Contains(currentFood.gameObject.GetComponent<Collider>()) && currentFood.GetComponent<FoodData>().foodData.isCooked != true))
            //    {
            //        //print("Cooking");
            //        anim.SetBool("Cooking", true);

            //        elapsed += Time.deltaTime;
            //        if (elapsed >= 0.2f)
            //        {

            //            elapsed = elapsed % 0.2f;
            //            //add prep points
            //            //kneeding
            //            if (chefData.kneadSkill)
            //            {
            //                if (kneadingAudio != null) kneadingAudio.Play();
            //                currentFood.GetComponent<FoodData>().foodData.kneedPrepPoints += chefData.kneadEffectivness;
            //            }
            //            //cutting
            //            if (chefData.cutSkill)
            //            {
            //                if (cuttingAudio != null) cuttingAudio.Play();
            //                currentFood.GetComponent<FoodData>().foodData.cutPrepPoints += chefData.cutEffectivness;
            //            }
            //            //mixing
            //            if (chefData.mixSkill)
            //            {
            //                if (mixingAudio != null) mixingAudio.Play();
            //                currentFood.GetComponent<FoodData>().foodData.mixPrepPoints += chefData.mixEffectivness;
            //            }

            //            //cooking
            //            if (chefData.cookSkill)
            //            {
            //                if (cookingAudio != null) cookingAudio.Play();
            //                currentFood.GetComponent<FoodData>().foodData.cookPrepPoints += chefData.cookEffectivness;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (cookingAudio != null) cookingAudio.Stop();
            //        if (kneadingAudio != null) kneadingAudio.Stop();
            //        if (mixingAudio != null) mixingAudio.Stop();
            //        if (cuttingAudio != null) cuttingAudio.Stop();

            //        foundFood = false;
            //        currentFood = null;
            //    }
        
    }

    public bool SearchForFood()
    {
        for (int i = 0; i < _FM.foodNeedPreperation_list.Count; i++)
        {
            currentFood = _FM.foodNeedPreperation_list[i].gameObject;
            if (chefData.kneadSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
            {
                //print("I can kneed it");

                foundFood = true;
            }
            else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
            {
                //print("I can cut it");

                foundFood = true;
            }
            else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
            {
                //print("I can cook it");
                foundFood = true;
            }
            else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
            {
                //print("I can mix it");

                foundFood = true;
            }
            else
            {
                foundFood = false;
            }
        }

        return foundFood;
    }

    public void SearchForWorkstation()
    {
        //CuttingStation
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chefData.range);
    }

    private void OnMouseDown()
    {
        if (placed)
        {
            print("spawn ");
            anim.SetTrigger("Spawn");
            //_UI.OpenChefPopUp(this.gameObject);
        }

    }
}
