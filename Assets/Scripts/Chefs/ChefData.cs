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

    public enum Task { Idle, FindFood, GetFood, GoToStation, WorkOnFood, GoToPass }
    public Task tasks;

    [Header("AI and Travel")]
    bool isHoldingFood;
    NavMeshAgent agent;
    [SerializeField]
    GameObject holdFoodSpot;
    bool foundFood;

    [Header("Targets")]

    [SerializeField]
    GameObject targetFood;
    [SerializeField]
    FoodClass targetFoodData;
    [SerializeField]
    GameObject targetWorkStation;
    Transform targetPassPoint;

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
        //walk to food
        switch (tasks)
        {
            case Task.Idle:

                if (_FM.foodNeedPreperation_list.Count > 0) tasks = Task.FindFood;
                if (workingOnSkill != WorkingOnSkill.None) workingOnSkill = WorkingOnSkill.None; //reset

                break;

            case Task.FindFood: //Find compaitble food


                if (SearchForFood())
                {
                    tasks = Task.GetFood;
                }

                break;

            case Task.GetFood: //travel to food

                agent.SetDestination(targetFood.transform.position);

                //if in range
                if (Vector3.Distance(transform.position, targetFood.transform.position) < 2f)
                {
                    PickUpFood();
                    tasks = Task.GoToStation;

                }
                break;

            case Task.GoToStation:



                if (targetWorkStation == null)
                {
                    //find workstation then travel there
                    targetWorkStation = SearchForWorkstation();
                    _WSM.RemoveFromUnoccupiedList(targetWorkStation);

                    agent.SetDestination(targetWorkStation.transform.position);
                }
                if (Vector3.Distance(transform.position, targetWorkStation.transform.position) < 2f)
                {
                    agent.isStopped = true;

                    //place food
                    targetFood.transform.position = targetWorkStation.GetComponent<WorkStation>().holdFoodPos.position;



                    tasks = Task.WorkOnFood;
                }
                else
                {
                    //chef holding food
                    targetFood.transform.position = holdFoodSpot.transform.position;

                }
                break;

            case Task.WorkOnFood:

                if (!isWorking) WorkOnFood();

                CheckFoodStatus();


                //look at food
                if (targetFood != null) transform.LookAt(targetFood.transform.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);



                break;

            case Task.GoToPass:

                //add workstation back to unoccupied list
                _WSM.AddToUnoccupiedList(targetWorkStation);

                if (agent.isStopped) agent.isStopped = false;

                if (targetPassPoint == null)
                {
                    targetPassPoint = FindPassPoint();
                    _PM.unoccupiedPassPoints.Remove(targetPassPoint);
                }

                agent.SetDestination(targetPassPoint.position);

                if (Vector3.Distance(transform.position, targetPassPoint.position) < 2f)
                {
                    targetFood.GetComponent<FoodMovement>().foodState = FoodMovement.FoodState.OnPass; //stops food from trying to travel from conveyerbelt
                    targetFood.transform.position = targetPassPoint.position;
                    isHoldingFood = false;
                    ResetChef();
                }
                else
                {
                    //hold food
                    targetFood.transform.position = holdFoodSpot.transform.position;
                }




                break;

        }

    }

    /// <summary>
    /// Pick up food from conveyorbelt
    /// </summary>
    void PickUpFood()
    {
        if(_FM.foodNeedPreperation_list.Contains(targetFood)) _FM.foodNeedPreperation_list.Remove(targetFood); //remove food from queue
        targetFood.GetComponent<FoodMovement>().foodState = FoodMovement.FoodState.BeingHeld; //stops food from trying to travel from conveyerbelt
        isHoldingFood = true;
    }

    /// <summary>
    /// Search kitchen for food which can be worked on by chef
    /// </summary>
    /// <returns></returns>
    public bool SearchForFood()
    {
        bool isFoodFound = false;

        for (int i = 0; i < _FM.foodNeedPreperation_list.Count; i++)
        {
            targetFood = _FM.foodNeedPreperation_list[i].gameObject;
            targetFoodData = targetFood.GetComponent<FoodData>().foodData;

            if (chefData.kneadSkill && targetFoodData.needsKneading)
            {
                //print("I can kneed it");
                workingOnSkill = WorkingOnSkill.Kneading;
                isFoodFound = true;
            }
            else if (chefData.cutSkill && targetFoodData.needsCutting)
            {
                //print("I can cut it");
                workingOnSkill = WorkingOnSkill.Cutting;
                isFoodFound = true;
            }
            else if (chefData.cookSkill && targetFoodData.needsCooking)
            {
                //print("I can cook it");
                workingOnSkill |= WorkingOnSkill.Cooking;
                isFoodFound = true;
            }
            else if (chefData.mixSkill && targetFoodData.needsMixing)
            {
                //print("I can mix it");
                workingOnSkill &= WorkingOnSkill.Mixing;
                isFoodFound = true;
            }
        }

        return isFoodFound;
    }

    /// <summary>
    /// Sarch for unoccupied workstation that can be used
    /// </summary>
    /// <returns></returns>
    public GameObject SearchForWorkstation()
    {
        //CuttingStation
        GameObject station = _WSM.FindClosestWorkstation(workingOnSkill.ToString(), gameObject);
        _WSM.AddToUnoccupiedList(station);

       return station;

    }

    /// <summary>
    /// Begin food progress based on skill being worked on
    /// </summary>
    public void WorkOnFood()
    {
        //execute after x, complete  = true
        isWorking = true;
        isHoldingFood = false;

        var skillProgressBarScript = targetFood.GetComponentInChildren<SkillTracker>();
       

        switch(workingOnSkill) 
        {
            case WorkingOnSkill.Cooking:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodData.cookWorkTime);
                break;
            case WorkingOnSkill.Mixing:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodData.mixWorkTime);
                break;
            case WorkingOnSkill.Cutting:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodData.cutWorkTime);
                break;
            case WorkingOnSkill.Kneading:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodData.kneadWorkTime);
                break;
        
        }



    }

    /// <summary>
    /// Check if skill progress being work on is complete for target food
    /// </summary>
    void CheckFoodStatus()
    {
        print("check status");
        isWorking = false;

        bool isCurrentWorkComplete = false;
        //has chef completed current work
        switch (workingOnSkill)
        {
            case WorkingOnSkill.Cooking:
                isCurrentWorkComplete = targetFoodData.cookWorkComplete;
                break;
            case WorkingOnSkill.Mixing:
                isCurrentWorkComplete = targetFoodData.mixWorkComplete;
                break;
            case WorkingOnSkill.Cutting:
                isCurrentWorkComplete = targetFoodData.cutWorkComplete;
                break;
            case WorkingOnSkill.Kneading:
                isCurrentWorkComplete = targetFoodData.kneadedWorkComplete;
                break;

        }

        if(isCurrentWorkComplete)
        {

            //if food is complete go to pass
            if (targetFoodData.isComplete)
            {
                //pick food back up
                PickUpFood();

                //go to pass
                tasks = Task.GoToPass;
            }
            else
            {
                //if food is incomplete 1. check if chef can work on it, if not 2. return to idle and leave food

                //leave food and add food to needs prep list
                _FM.foodNeedPreperation_list.Add(targetFood);

                ResetChef();

            }
        }

    }
    
    /// <summary>
    /// Return unoccupied point on the pass
    /// </summary>
    /// <returns></returns>
    Transform FindPassPoint()
    {
        return _PM.unoccupiedPassPoints.FirstOrDefault();

    }

    /// <summary>
    /// Set target variables to null
    /// </summary>
    void ResetChef()
    {
        print("Reset chef");
        targetFood = null;
        targetFoodData = null;
        isHoldingFood = false;
        targetWorkStation = null;
        targetPassPoint = null;
        tasks = Task.Idle;
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
