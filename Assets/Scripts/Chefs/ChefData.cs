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
    FoodClass targetFoodClass;
    FoodData targetFoodData;
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

                if(targetFood == null)
                {
                    if (_FM.foodNeedPreperation_list.Count > 0) tasks = Task.FindFood;
                    if (workingOnSkill != WorkingOnSkill.None) workingOnSkill = WorkingOnSkill.None; //reset
                }
                

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

                if(CheckFoodStatus())
                {
                    //if food is complete go to pass
                    if (targetFood.GetComponent<FoodData>().CheckIfComplete())
                    {
                        targetFoodData.foodState = FoodData.FoodState.Finished;

                        //pick food back up
                        PickUpFood();

                        //go to pass
                        tasks = Task.GoToPass;
                        //print("go to pass called");
                    }

                }


                //look at food
                if (targetFood != null) transform.LookAt(targetFood.transform.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);



                break;

            case Task.GoToPass:


                if (agent.isStopped) agent.isStopped = false;

                if (targetPassPoint == null)
                {
                    //print("go to pass");
                    //add workstation back to unoccupied list
                    _WSM.ChangeToUnoccupied(targetWorkStation);

                    targetPassPoint = FindPassPoint();
                    _PM.unoccupiedPassPoints.Remove(targetPassPoint);
                }

                agent.SetDestination(targetPassPoint.position);

                if (Vector3.Distance(transform.position, targetPassPoint.position) < 2f)
                {
                    targetFoodData.foodMovement = FoodData.FoodMovement.OnPass; //stops food from trying to travel from conveyerbelt
                    targetFood.transform.position = targetPassPoint.position;

                    //get rid of order ticket UI
                    _UI.RemoveOrder(_FM.orderedFood_GO.IndexOf(targetFood) + 1);


                    //remove from need prep and add to finished
                    if (_FM.foodNeedPreperation_list.Contains(targetFood)) _FM.foodNeedPreperation_list.Remove(targetFood);
                    _FM.finishedFood_list.Add(targetFood);
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
        targetFoodData.foodMovement = FoodData.FoodMovement.BeingHeld; //stops food from trying to travel from conveyerbelt
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
            targetFoodClass = targetFood.GetComponent<FoodData>().foodData;
            targetFoodData = targetFood.GetComponent<FoodData>();

            if (chefData.kneadSkill && targetFoodClass.needsKneading)
            {
                //print("I can kneed it");
                workingOnSkill = WorkingOnSkill.Kneading;
                isFoodFound = true;
            }
            else if (chefData.cutSkill && targetFoodClass.needsCutting)
            {
                //print("I can cut it");
                workingOnSkill = WorkingOnSkill.Cutting;
                isFoodFound = true;
            }
            else if (chefData.cookSkill && targetFoodClass.needsCooking)
            {
                //print("I can cook it");
                workingOnSkill |= WorkingOnSkill.Cooking;
                isFoodFound = true;
            }
            else if (chefData.mixSkill && targetFoodClass.needsMixing)
            {
                //print("I can mix it");
                workingOnSkill &= WorkingOnSkill.Mixing;
                isFoodFound = true;
            }
        }

        //make sure no other chef can regiester this has their target food
        _FM.foodNeedPreperation_list.Remove(targetFood);

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
        _WSM.ChangeToOccupied(station);

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
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodClass.cookWorkTime);
                break;
            case WorkingOnSkill.Mixing:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodClass.mixWorkTime);
                break;
            case WorkingOnSkill.Cutting:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodClass.cutWorkTime);
                break;
            case WorkingOnSkill.Kneading:
                skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), targetFoodClass.kneadWorkTime);
                break;
        
        }



    }

    /// <summary>
    /// Check if skill progress being work on is complete for target food
    /// </summary>
    bool CheckFoodStatus()
    {
        //print("check status");
        isWorking = false;

        bool isCurrentWorkComplete = false;
        //has chef completed current work
        switch (workingOnSkill)
        {
            case WorkingOnSkill.Cooking:
                isCurrentWorkComplete = targetFoodClass.cookWorkComplete;
                break;
            case WorkingOnSkill.Mixing:
                isCurrentWorkComplete = targetFoodClass.mixWorkComplete;
                break;
            case WorkingOnSkill.Cutting:
                isCurrentWorkComplete = targetFoodClass.cutWorkComplete;
                break;
            case WorkingOnSkill.Kneading:
                isCurrentWorkComplete = targetFoodClass.kneadedWorkComplete;
                break;

        }

        return isCurrentWorkComplete;

    }
    
    /// <summary>
    /// Return unoccupied point on the pass
    /// </summary>
    /// <returns></returns>
    Transform FindPassPoint()
    {
        return _PM.FindClosestPassPoint(gameObject);

    }

    /// <summary>
    /// Set target variables to null
    /// </summary>
    void ResetChef()
    {
        //print("Reset chef");
        targetFood = null;
        targetFoodClass = null;
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
