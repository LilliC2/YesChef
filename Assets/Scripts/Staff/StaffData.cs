using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChefData;
using UnityEngine.AI;
using System.Reflection.Emit;
using UnityEngine.UI;

public class StaffData : GameBehaviour
{

    public StaffBehaviour staffBehaviour;

    //generate action
    [SerializeField]
    List<StaffBehaviour.ActionState> actionStatePercentage =new List<StaffBehaviour.ActionState>();
    [SerializeField]
    List<StaffBehaviour.MovementState> movementStatePercentage =new List<StaffBehaviour.MovementState>();

    NavMeshAgent agent;



    [Header("Alert Player")]
    bool alertPlayer; //does the staff member want to talk to player
    bool talkingToPlayer;
    [SerializeField]
    Image alertPlayerImage;

    [Header("Wander")]
    [SerializeField] float wanderRadius;
    [SerializeField] Vector3 wanderDestination;
    bool reachedWanderPos;

    [Header("Return to Station")]
    Vector3 stationPos;
    bool inWorkArea = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetPersonalityBehaviour();
        GenerateActionState();

    }
    // Update is called once per frame
    void Update()
    {
        if (_GM.playState == GameManager.PlayState.Closed)
        {
            if(agent.speed != _SM.casualSpeed) agent.speed = _SM.casualSpeed;
            if (inWorkArea) inWorkArea = false;

            #region Movement
            switch (staffBehaviour.movementState)
            {
                case StaffBehaviour.MovementState.Wander:

                    //Get positiont to walk to
                    if (wanderDestination == Vector3.zero)
                    {
                        //find wanderdestination within staffroom
                        wanderDestination = GetWanderPoint(_SM.staffRoomZone.position, "Staff");
                        reachedWanderPos = false;

                        //set agent to travel
                        if(wanderDestination != Vector3.zero)
                        {
                            agent.isStopped = false;
                            agent.SetDestination(wanderDestination);
                        }
                        

                    }

                    //Stop for a bit them find new positon
                    if (!reachedWanderPos && Vector3.Distance(transform.position, wanderDestination) < 1)
                    {
                        reachedWanderPos = true;
                        agent.isStopped = true;
                        ExecuteAfterSeconds(Random.Range(1, 8), () => wanderDestination = Vector3.zero);

                    }

                    break;
                case StaffBehaviour.MovementState.Idle:
                    print("idle");
                    break;
                case StaffBehaviour.MovementState.Sit:
                    print("sit");
                    break;
            }
            #endregion
        }
        else if (_GM.playState == GameManager.PlayState.Open)
        {
            if (!inWorkArea)
            {
                NavMeshHit navMeshHit;
                agent.SamplePathPosition(NavMesh.AllAreas, 1f, out navMeshHit);
                //walk to appropriate zone
                if (navMeshHit.mask == 1 << NavMesh.GetAreaFromName("Staff") && stationPos == Vector3.zero)
                {
                    agent.isStopped = false;
                    ReturnToWork();
                }
                //check if in waiter or chef zone
                else if ( Vector3.Distance(transform.position, stationPos) < 1)
                {
                    inWorkArea = true;
                }
            }

        }




    }

    #region Talk/Question Player
    void AskPlayerQuestionAction()
    {
        alertPlayer = true;

        //set image
        alertPlayerImage.gameObject.SetActive(true);
        alertPlayerImage.sprite = _SM.staffQuestionSprite;
    }
    
    void TalkTooPlayerQuestionAction()
    {
        alertPlayer = true;

        //set image
        alertPlayerImage.gameObject.SetActive(true);
        alertPlayerImage.sprite = _SM.staffTalkSprite;
    }

    void BeginDialog()
    {
        alertPlayerImage.gameObject.SetActive(false);

        agent.isStopped = true; //so player cant move
        talkingToPlayer = true; //so new action cannot be started mid convo

        _UI.OpenDialogBox();

        //_UI.LoadDialog();

        print("being dialog");
    }

    private void OnMouseDown()
    {
        //If staff wants to talk to player
        if(alertPlayer)
        {
            //move camera to focus on gameobject
            Camera.main.GetComponent<CameraController>().CameraFocusStaff(gameObject);
            BeginDialog();
        }
    }

    #endregion
    #region Generating Action and Movement States
    void GenerateActionState()
    {
        if (!talkingToPlayer)
        {
            float actionLength = Random.Range(7, 15); //how long after the begin the action until they generate a new one

            //should they generate a new state if they want to talk/question to player??


            staffBehaviour.actionState = actionStatePercentage[Random.Range(0, 100)];

            switch (staffBehaviour.actionState)
            {
                case StaffBehaviour.ActionState.Idle:

                    GenerateMovementState();

                    break;

                case StaffBehaviour.ActionState.AskPlayerQuestion:

                    AskPlayerQuestionAction();
                    GenerateMovementState();

                    break;

                case StaffBehaviour.ActionState.TalkToPlayer:

                    TalkTooPlayerQuestionAction();
                    GenerateMovementState();

                    break;

                case StaffBehaviour.ActionState.TalkToStaff:

                    //dont generate movement state, will direction set travel to destination here

                    break;
            }

            //set movement state based on action state
            //bc some action states require specific movement e.g go talk to a specifc staff member


            print(staffBehaviour.actionState);

            StartCoroutine(StopActionAfterActionLength(actionLength));
        }

       

    }
    void GenerateMovementState()
    {
        if (!talkingToPlayer)
        {
            float movementLength = Random.Range(7, 15); //how long after the begin the action until they generate a new one

            staffBehaviour.movementState = movementStatePercentage[Random.Range(0, 100)];
            print(staffBehaviour.movementState);

            StartCoroutine(StopActionAfterActionLength(movementLength));
        }
         

    }

    /// <summary>
    /// Generate new action after current action's action length is over
    /// </summary>
    /// <param name="_actionLength"></param>
    /// <returns></returns>
    IEnumerator StopActionAfterActionLength(float _actionLength)
    {
        yield return new WaitForSeconds(_actionLength);

        GenerateActionState();
    }
    
    IEnumerator StopMovementAfterMovementLength(float _actionLength)
    {
        yield return new WaitForSeconds(_actionLength);

        GenerateMovementState();
    }
    #endregion

    /// <summary>
    /// Return to work zone NavMesh, either Kitchen or Front Of House
    /// </summary>
    void ReturnToWork()
    {
        if(gameObject.tag == "Chef")
        {
            stationPos = GetWanderPoint(_SM.kitchenZone.position,gameObject.tag);
            agent.speed = _SM.returnToWorkSpeed;
        }
        if(gameObject.tag == "Waiter")
        {
            stationPos = GetWanderPoint(_SM.FOHZone.position, gameObject.tag);
            agent.speed = _SM.returnToWorkSpeed;

        }

        agent.SetDestination(stationPos);

    }

   /// <summary>
   /// Get random position in given area within radius of center
   /// </summary>
   /// <param name="center"></param>
   /// <param name="area"></param>
   /// <returns></returns>
    Vector3 GetWanderPoint(Vector3 center, string area)
    {
        Vector3 result = Vector3.zero;
        Vector3 randomPoint = center + Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1, 1 << NavMesh.GetAreaFromName(area)))
        {
            //print("valid pos");
            result = hit.position;
            return result;


        }
        return result;
    }
   
    #region Personality

    /// <summary>
    /// Set Personality class data based on SM
    /// </summary>
    void SetPersonalityBehaviour()
    {
        switch (staffBehaviour.personalityType)
        {
            case StaffManager.PersonalityTypes.Chatty:
                staffBehaviour.personalityData = _SM.personalityBehaviours[0];
                break;
            case StaffManager.PersonalityTypes.Loner:
                staffBehaviour.personalityData = _SM.personalityBehaviours[1];
                break;
            case StaffManager.PersonalityTypes.Cynical:
                staffBehaviour.personalityData = _SM.personalityBehaviours[2];
                break;
            case StaffManager.PersonalityTypes.Sassy:
                staffBehaviour.personalityData = _SM.personalityBehaviours[3];
                break;
            case StaffManager.PersonalityTypes.AirHead:
                staffBehaviour.personalityData = _SM.personalityBehaviours[4];
                break;
            case StaffManager.PersonalityTypes.Hardy:
                staffBehaviour.personalityData = _SM.personalityBehaviours[5];
                break;
            case StaffManager.PersonalityTypes.Timid:
                staffBehaviour.personalityData = _SM.personalityBehaviours[6];
                break;
            case StaffManager.PersonalityTypes.Chill:
                staffBehaviour.personalityData = _SM.personalityBehaviours[7];
                break;
        }

        //set actionstatelist to generate action state from
        actionStatePercentage = SetActionStateList();
        movementStatePercentage = SetMovementStateList();

    }

    /// <summary>
    /// Initalise list which will be used to generate new action state
    /// </summary>
    List<StaffBehaviour.ActionState> SetActionStateList()
    { 
        //all probabilities in a list (so their Low Medium and High values)
        List<float> allActionStateProbabilities = new List<float>();

        float totalProbability = 0;

        //convert allActionStateProbabilities into a percentage of totalProbability
        List<float> actionStatePercentage = new List<float>();

        //idle
        float idle = _SM.ActionPercentageProbability(staffBehaviour.personalityData.idle);
        allActionStateProbabilities.Add(idle);
        totalProbability += idle;

        //question
        float question = _SM.ActionPercentageProbability(staffBehaviour.personalityData.askPlayerQuestion);
        allActionStateProbabilities.Add(question);
        totalProbability += question;

        //talk to player
        float talkPlayer = _SM.ActionPercentageProbability(staffBehaviour.personalityData.talkToPlayer);
        allActionStateProbabilities.Add(talkPlayer);
        totalProbability += talkPlayer;

        //talk to staff
        float talkStaff = _SM.ActionPercentageProbability(staffBehaviour.personalityData.talkToStaff);
        allActionStateProbabilities.Add(talkStaff);
        totalProbability += talkStaff;


        //convert allActionStateProbabilities into a percentage of totalProbability
        foreach (int actionProbability in allActionStateProbabilities)
        {
            float result = (actionProbability / totalProbability)*100;
            actionStatePercentage.Add(Mathf.RoundToInt(result));
        }

        //make a list which will have each action in it per actionStatePercentage.
        //then we generate a number out of 100 which will get that index. that is the returned action
        List<StaffBehaviour.ActionState> generateActionList = new List<StaffBehaviour.ActionState>();

        for (int i = 0; i < actionStatePercentage.Count; i++)
        {
            for (int j = 0; j < actionStatePercentage[i]; j++)
            {
                //depending on i add the correct action
                switch(i)
                {
                    case 0:
                        generateActionList.Add(StaffBehaviour.ActionState.Idle);
                        break;
                    case 1:
                        generateActionList.Add(StaffBehaviour.ActionState.AskPlayerQuestion);
                        break;
                    case 2:
                        generateActionList.Add(StaffBehaviour.ActionState.TalkToPlayer);
                        break;
                    case 3:
                        generateActionList.Add(StaffBehaviour.ActionState.TalkToStaff);
                        break;
                }                
            }

        }

        return generateActionList;
    }

    List<StaffBehaviour.MovementState> SetMovementStateList()
    {
        //all probabilities in a list (so their Low Medium and High values)
        List<float> allMovementStateProbabilities = new List<float>();

        float totalProbability = 0;

        //convert allActionStateProbabilities into a percentage of totalProbability
        List<float> movementStatePercentage = new List<float>();

        //idle
        float idle = _SM.ActionPercentageProbability(staffBehaviour.personalityData.idle);
        allMovementStateProbabilities.Add(idle);
        //print("idle" + idle);
        totalProbability += idle;

        //question
        float question = _SM.ActionPercentageProbability(staffBehaviour.personalityData.wander);
        allMovementStateProbabilities.Add(question);
        totalProbability += question;
        //talk to staff
        float talkStaff = _SM.ActionPercentageProbability(staffBehaviour.personalityData.sit);
        allMovementStateProbabilities.Add(talkStaff);
        totalProbability += talkStaff;


        //convert allActionStateProbabilities into a percentage of totalProbability
        foreach (int actionProbability in allMovementStateProbabilities)
        {
            //print(actionProbability + "/" + totalProbability);
            float result = (actionProbability / totalProbability) * 100;
            movementStatePercentage.Add(Mathf.RoundToInt(result));
        }

        //make a list which will have each action in it per actionStatePercentage.
        //then we generate a number out of 100 which will get that index. that is the returned action
        List<StaffBehaviour.MovementState> generateMovementList = new List<StaffBehaviour.MovementState>();

        for (int i = 0; i < movementStatePercentage.Count; i++)
        {
            for (int j = 0; j < movementStatePercentage[i]; j++)
            {
                //depending on i add the correct action
                switch (i)
                {
                    case 0:
                        generateMovementList.Add(StaffBehaviour.MovementState.Idle);
                        break;
                    case 1:
                        generateMovementList.Add(StaffBehaviour.MovementState.Wander);
                        break;
                    case 2:
                        generateMovementList.Add(StaffBehaviour.MovementState.Sit);
                        break;

                }
            }

        }

        return generateMovementList;
    }
    #endregion
}
