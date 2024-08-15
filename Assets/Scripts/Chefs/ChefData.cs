using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ChefData : GameBehaviour
{

    public ChefClass chefData; //data used by chef, including percentages
    public ChefClass baseStatsChefData; //holds base stats, used when calculating percentages

    public enum Targeting { First, Last, Strongest }
    public Targeting targeting;

    public enum WorkingOnSkill { Cooking, Kneading, Cutting, Mixing, None}
    public WorkingOnSkill workingOnSkill; //this is the skill the chef is using on current food

    public enum Task { Idle, FindFood, GetFood, GoToStation, WorkOnFood, ReturnFood }
    public Task tasks;

    [Header("AI and Travel")]
    bool isHoldingFood;
    NavMeshAgent agent;
    [SerializeField]
    GameObject holdFoodSpot;
    bool foundFood;
    [SerializeField]
    GameObject currentFood;
    [SerializeField]
    GameObject currentWorkStation;

    [Header("Audio")]
    [SerializeField]
    AudioSource cuttingAudio;
    AudioSource cookingAudio;
    AudioSource mixingAudio;
    AudioSource kneadingAudio;

    [Header("Working")]
    float startTime;
    bool isWorking;

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
            //walk to food
            switch (tasks)
            {
                case Task.Idle:

                    if (_FM.foodNeedPreperation_list.Count > 0) tasks = Task.FindFood;
                    if (workingOnSkill != WorkingOnSkill.None) workingOnSkill = WorkingOnSkill.None; //reset

                break;

                case Task.FindFood: //Find compaitble food

                    if (!foundFood)
                    {
                        anim.SetBool("Cooking", false); //turn off cooking anim
                        SearchForFood();
                    }

                    if (foundFood) tasks = Task.GetFood;


                    break;

                case Task.GetFood: //travel to food

                    agent.SetDestination(currentFood.transform.position);

                    //if in range
                    if (Vector3.Distance(transform.position, currentFood.transform.position) < 2f)
                    {
                        _FM.foodNeedPreperation_list.Remove(currentFood); //remove food from queue
                        currentFood.GetComponent<FoodMovement>().foodState = FoodMovement.FoodState.BeingHeld; //stops food from trying to travel from conveyerbelt
                        isHoldingFood = true;
                        tasks = Task.GoToStation;

                    }
                    break;

                case Task.GoToStation:



                    if (currentWorkStation == null)
                    {
                        //find workstation then travel there
                        currentWorkStation = SearchForWorkstation();
                        agent.SetDestination(currentWorkStation.transform.position);
                    }
                    if (Vector3.Distance(transform.position, currentWorkStation.transform.position) < 2f)
                    {
                        agent.isStopped = true;

                        //place food
                        currentFood.transform.position = currentWorkStation.GetComponent<WorkStation>().holdFoodPos.position;

                        tasks = Task.WorkOnFood;
                    }
                    else
                    {
                        //chef holding food
                        currentFood.transform.position = holdFoodSpot.transform.position;

                    }
                    break;
                case Task.WorkOnFood:

                    if(!isWorking) WorkOnFood();

                    //look at food
                    if (currentFood != null) transform.LookAt(currentFood.transform.position);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);



                    break;

            }
        }
        #region old script
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
        #endregion
    }

    public bool SearchForFood()
    {
        for (int i = 0; i < _FM.foodNeedPreperation_list.Count; i++)
        {
            currentFood = _FM.foodNeedPreperation_list[i].gameObject;
            if (chefData.kneadSkill && currentFood.GetComponent<FoodData>().foodData.needsKneading)
            {
                //print("I can kneed it");
                workingOnSkill = WorkingOnSkill.Kneading;
                foundFood = true;
            }
            else if (chefData.cutSkill && currentFood.GetComponent<FoodData>().foodData.needsCutting)
            {
                //print("I can cut it");
                workingOnSkill = WorkingOnSkill.Cutting;
                foundFood = true;
            }
            else if (chefData.cookSkill && currentFood.GetComponent<FoodData>().foodData.needsCooking)
            {
                //print("I can cook it");
                workingOnSkill |= WorkingOnSkill.Cooking;
                foundFood = true;
            }
            else if (chefData.mixSkill && currentFood.GetComponent<FoodData>().foodData.needsMixing)
            {
                //print("I can mix it");
                workingOnSkill &= WorkingOnSkill.Mixing;
                foundFood = true;
            }
            else
            {
                foundFood = false;
            }
        }

        return foundFood;
    }

    public GameObject SearchForWorkstation()
    {
        //CuttingStation
        GameObject station = _WSM.FindClosestWorkstation(workingOnSkill.ToString(), gameObject);
        _WSM.AddToUnoccupiedList(station);

       return station;

    }

    public void WorkOnFood()
    {
        //execute after x, complete  = true
        isWorking = true;

        var currentFoodfoodata = currentFood.GetComponent<FoodData>().foodData;
        switch(workingOnSkill) 
        {
            case WorkingOnSkill.Cooking:
                ExecuteAfterSeconds(currentFoodfoodata.cookWorkTime, () => currentFoodfoodata.cookWorkComplete = true);
                break;
            case WorkingOnSkill.Mixing:
                ExecuteAfterSeconds(currentFoodfoodata.mixWorkTime, () => currentFoodfoodata.mixWorkComplete = true);
                break;
            case WorkingOnSkill.Cutting:
                print("Cutting food");
                ExecuteAfterSeconds(currentFoodfoodata.cutWorkTime, () => currentFoodfoodata.cutWorkComplete = true);
                break;
            case WorkingOnSkill.Kneading:
                ExecuteAfterSeconds(currentFoodfoodata.kneadWorkTime, () => currentFoodfoodata.kneadedWorkComplete = true);
                break;
        
        }

        CheckFoodStatus();

    }

    void CheckFoodStatus()
    {
        print("check status");
        isWorking = false;
        //if food is complete go to pass


        //if food is incomplete 1. check if chef can work on it, if not 2. return to idle and leave food
    }

    private void OnMouseDown()
    {
        if (placed)
        {
            anim.SetTrigger("Spawn");
            //_UI.OpenChefPopUp(this.gameObject);
        }

    }
}
