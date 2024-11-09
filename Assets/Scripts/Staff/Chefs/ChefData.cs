using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ChefData : GameBehaviour
{

    public ChefClass chefData; //data used by chef, including percentages
    StaffData staffData;

    public enum Targeting { First, Last, Strongest }
    public Targeting targeting;

    public enum WorkingOnSkill { Cooking, Kneading, Cutting, Mixing, None}
    public WorkingOnSkill workingOnSkill; //this is the skill the chef is using on current food

    public enum Task { Idle, FindFood, GetFood, GoToStation, WorkOnFood, GoToPass }
    public Task tasks;

    ICharacterActionState characterActionState;

    #region Variables

    [Header("AI and Travel")]
    NavMeshAgent agent;
    [SerializeField]
    GameObject holdFoodSpot;
    bool isPaused;

    [Header("Targets")]

    [SerializeField]
    GameObject targetFood;
    [SerializeField]
    FoodClass targetFoodClass;
    FoodData targetFoodData;
    [SerializeField]
    GameObject targetWorkStation;
    Transform targetWorkStationStandPos;
    FurnitureItemHolder targetPassPoint;

    [Header("Audio")]
    [SerializeField]
    AudioSource audioSource;


    [Header("Working")]
    bool isWorking;


    public Animator anim;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //default targeting
        targeting = Targeting.First;
        agent = GetComponent<NavMeshAgent>();
        staffData = GetComponent<StaffData>();

    }

    // Update is called once per frame
    void Update()
    {
        //walk to food
        switch(_GM.playState)
        {
            #region Open
            case GameManager.PlayState.Open:
                switch (tasks)
                {
                    case Task.Idle:

                        if (targetFood == null)
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
                            //change furniture to unoccupied
                            targetFoodData.FurnitureHolder.SetStatus = FurnitureItemHolder.Status.Unoccupied;
                            targetFoodData.FurnitureHolder = null;

                            characterActionState.PickUpOrder();
                            tasks = Task.GoToStation;


                        }
                        break;

                    case Task.GoToStation:


                        if (targetWorkStation == null)
                        {
                            //find workstation then travel there
                            targetWorkStation = SearchForWorkstation();
                            targetWorkStationStandPos = _WSM.GetWorkstationStandPos(targetWorkStation);

                            agent.SetDestination(targetWorkStationStandPos.transform.position);
                        }
                        if (Vector3.Distance(transform.position, targetWorkStationStandPos.transform.position) < 0.5f)
                        {
                            agent.isStopped = true;

                            //place food
                            characterActionState.PlaceOrder(targetWorkStation.GetComponent<FurnitureItemHolder>().ReturnHoldSpotV3);
                            _WSM.ChangeToOccupied(targetWorkStation);
                            targetFoodData.FurnitureHolder = targetWorkStation.GetComponent<FurnitureItemHolder>();

                            //pause for a little
                            if (!StartPauseAgent(1f))
                            {
                                tasks = Task.WorkOnFood;

                            }                           
                        }
                       
                        break;

                    case Task.WorkOnFood:

                        if (!isWorking) WorkOnFood();

                        if (CheckFoodStatus())
                        {
                            //if food is complete go to pass
                            if (targetFood.GetComponent<FoodData>().CheckIfComplete())
                            {
                                targetFoodData.foodState = FoodData.FoodState.Finished;

                                //pick food back up
                                characterActionState.PickUpOrder();
                                targetFoodData.FurnitureHolder = null;
                                _WSM.ChangeToUnoccupied(targetWorkStation);


                                //pause for a little
                                if (!StartPauseAgent(1f))
                                {
                                    tasks = Task.GoToPass;
                                }                            
                            }
                        }


                        //look at food
                        if (targetFood != null) transform.LookAt(targetFood.transform.position);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);



                        break;

                    case Task.GoToPass:

                        
                        //Get position to place food on pass
                        if (targetPassPoint == null)
                        {
                            if (agent.isStopped) agent.isStopped = false;

                            //print("go to pass");
                            //add workstation back to unoccupied list

                            targetPassPoint = FindPassPoint();
                            _PM.OccupiedPassPoint(targetPassPoint);
                            targetFoodData.SetPassPoint(targetPassPoint.ReturnHoldSpotTransform);
                        }
                        else
                        {
                            agent.SetDestination(targetPassPoint.ReturnHoldSpotV3);

                        }

                        if (Vector3.Distance(transform.position, targetPassPoint.ReturnHoldSpotV3) < 2f)
                        {
                            //remove from need prep and add to finished
                            if (_FM.foodNeedPreperation_list.Contains(targetFood)) _FM.foodNeedPreperation_list.Remove(targetFood);
                            _FM.finishedFood_list.Add(targetFood);


                            targetFoodData.foodMovement = FoodData.FoodMovement.OnPass; //stops food from trying to travel from conveyerbelt
                            characterActionState.PlaceOrder(targetPassPoint.ReturnHoldSpotV3);
                            targetFoodData.FurnitureHolder = targetPassPoint;

                            //get rid of order ticket UI


                            if (!StartPauseAgent(0.5f))
                            {
                                _UI.RemoveOrder(_FM.orderedFood_GO.IndexOf(targetFood));
                                _FM.RemoveFood(targetFood, false);
                                ResetChef();

                            }


                        }
                        

                        break;

                }

                if (characterActionState != null)
                    characterActionState.UpdateOrderPosition();

                break;
            #endregion   
        }
    }


    /// <summary>
    /// Search kitchen for food which can be worked on by chef
    /// </summary>
    /// <returns></returns>
    public bool SearchForFood()
    {
        bool isFoodFound = false;

        //Iterate throughh 
        for (int i = 0; i < _FM.foodNeedPreperation_list.Count; i++)
        {
             targetFood = _FM.foodNeedPreperation_list[i].gameObject;
             targetFoodClass = targetFood.GetComponent<FoodData>().order.foodClass;
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

        //set character action state
        characterActionState = new CharacterActionState(targetFood,holdFoodSpot.transform,targetFoodData,targetFoodClass);

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

        //set action to true
        characterActionState.SetAction = isWorking;

        //start lerp of skill in OrderProgress
        var skillProgressBarScript = targetFood.GetComponentInChildren<OrderProgressTracker>();
        skillProgressBarScript.StartFoodProgress(workingOnSkill.ToString(), characterActionState);


        if (audioSource.clip == null)
        {
            //Start working audio
            audioSource.clip = _AM.ReturnChefWorkingAudioClip(workingOnSkill);
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    /// <summary>
    /// Check if skill progress being work on is complete for target food
    /// </summary>
    bool CheckFoodStatus()
    {
        //check is food is finished
        bool isCurrentWorkComplete = characterActionState.CheckOrderProgress(workingOnSkill);

        if(isCurrentWorkComplete)
        {
            //Stop working audio
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.Stop();

            //stop action
            isWorking = false;
            characterActionState.SetAction = isWorking;

        }

        return isCurrentWorkComplete;
    }
    
    /// <summary>
    /// Return unoccupied point on the pass
    /// </summary>
    /// <returns></returns>
    FurnitureItemHolder FindPassPoint()
    {
        return _PM.FindClosestPassPoint(gameObject);

    }

    bool StartPauseAgent(float _time)
    {
        //pause for a little
        if (!isPaused)
        {
            isPaused = true;
            StartCoroutine(_SM.PauseAgent(agent, _time));
        }
        else
        {
            //check if pause is over bc they can move again
            if (agent.isStopped)
            {
                isPaused = false;
            }
        }

        return isPaused;
    }

    /// <summary>
    /// Set target variables to null
    /// </summary>
    void ResetChef()
    {
        characterActionState = null;
        //print("Reset chef");
        targetFood = null;
        targetFoodClass = null;
        targetFoodData = null;
        isPaused = false;
        targetWorkStation = null;
        targetPassPoint = null;
        tasks = Task.Idle;
    }

}
