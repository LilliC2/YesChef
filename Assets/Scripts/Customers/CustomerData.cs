using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CustomerData : GameBehaviour
{
    public enum Task { Queue, WaitToBeSeated,FollowWaiter ,SelectFromMenu, WaitForFood, EatFood, PayAndLeave}
    public Task task;

    [Header("Movement")]
    NavMeshAgent agent;

    [Header("Queue")]
    [SerializeField]
    int queueIndex;

    [Header("Follow Waiter")]
    [SerializeField] float waiterFollowDistance; //follow x distance behind waiter
    GameObject waiterFollow;

    //public GameObject order;
    //public bool hasBeenAttened;
    //[SerializeField]
    //Image orderDisplay;

    //[SerializeField]
    //GameObject moneyEarned;

    //GameObject seat;
    //public GameObject plateSpot;

    //NavMeshAgent agent;
    //public bool leaving;

    //public bool isOrderCooked;

    //public GameObject currentFoodSprite;
    // Start is called before the first frame update
    //void Start()
    //{

    //    currentFoodSprite.SetActive(false);

    //    agent = GetComponent<NavMeshAgent>();
    //    FindSeat();

    //    _GM.event_startOfDay.AddListener(OrderFood);
    //    _CustM.event_newSeatAvalible.AddListener(FindSeat);
    //}

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //track index when customer is spawned
        queueIndex = _CustM.customerOutsideQueueSpots.Count - 1;
    }

    private void Update()
    {
        switch(task)
        {
            //In any position other than 0
            case Task.Queue:

                //if not at correct queue position
                if(queueIndex != _CustM.customersInQueue.IndexOf(gameObject))
                {
                    agent.SetDestination(_CustM.customerOutsideQueueSpots[queueIndex].position);

                    //check if at position
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {

                        //move down queue
                        queueIndex--;
                        agent.SetDestination(_CustM.customerOutsideQueueSpots[queueIndex].position);

                        //check if at front of queue
                        if(queueIndex == 0)
                        {
                            _EM.event_customerReadyToBeSeated.Invoke();
                            task = Task.WaitToBeSeated;
                        }
                    }
                }


                break;
            case Task.WaitToBeSeated:
            
                
                break;
            case Task.FollowWaiter:

                if(waiterFollow != null)
                {
                    if (Vector3.Distance(transform.position, waiterFollow.transform.position) > waiterFollowDistance)
                    {
                        agent.SetDestination(waiterFollow.transform.position);
                    }
                }
                

                break;

            case Task.SelectFromMenu:

                //check they have sat down
                if (agent.remainingDistance <= agent.stoppingDistance) agent.isStopped = true;


                    break;
        }
    }

    public void StartFollowWaiter(GameObject _waiter)
    {
        print("Follow waiter");
        waiterFollow = _waiter;
        task = Task.FollowWaiter;
    }

    /// <summary>
    /// Find chair avalible chair at given table
    /// </summary>
    public void BeSeated(GameObject _table)
    {
        print("seated");
        var tableData = _table.GetComponent<Table>();
        var targetChair = tableData.unoccupiedSeats.FirstOrDefault();

        tableData.ChangeToOccupied(targetChair);

        agent.SetDestination(targetChair.position);
        task = Task.SelectFromMenu;

    }

    //// Update is called once per frame
    //void Update()
    //{
    //    currentFoodSprite.transform.LookAt(Camera.main.transform.position);

    //    if (leaving && Vector3.Distance(transform.position, _CustM.resturantDoor.transform.position) < 2)
    //    {
    //        currentFoodSprite.SetActive(false);
    //        Destroy(gameObject);

    //    }
    //}
    //public void ServedFood()
    //{

    //    currentFoodSprite.SetActive(false);

    //    print("YUM!");

    //    isOrderCooked = order.GetComponent<FoodData>().foodData.isCooked;
    //    print("Order is cooked is " + isOrderCooked);
    //    //begin eating after X seconds
    //    ExecuteAfterSeconds(order.GetComponent<FoodData>().foodData.eatTime,()=> LeaveResturant());

    //    //eating aniamtion??


    //    //destroy food

    //    //leave resturant



    //}

    //void OrderFood()
    //{
    //    print("order food");
    //    if(order == null)
    //    {
    //        order = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
    //        _FM.orderedFood.Add(order);
    //        currentFoodSprite.SetActive(true);
    //        orderDisplay.sprite = order.GetComponent<FoodData>().foodData.pfp;
    //    }


    //}

    //void FindSeat()
    //{
    //    if(seat == null && !leaving)
    //    {
    //        //queue outside
    //        if (_CustM.emptyChairQueue.Count == 0)
    //        {
    //            _CustM.customerQueueWaiting.Add(gameObject);
    //            bool foundSpot = false;

    //            for (int i = 0; i < _CustM.customerQueueSpots.Count; i++)
    //            {
    //                if (!foundSpot)
    //                {
    //                    if(_CustM.customerQueueWaitingCheck[i] == false)
    //                    {
    //                        foundSpot = true;
    //                        _CustM.customerQueueWaitingCheck[i] = true;
    //                        agent.SetDestination(_CustM.customerQueueSpots[i].transform.position);
    //                    }
    //                }
    //            }

    //           // agent.SetDestination(_CustM.customerQueueSpots[_CustM.customerQueueWaiting.IndexOf(gameObject)].transform.position);
    //        }
    //        else
    //        {
    //            _CustM.customerQueueWaiting.Remove(gameObject);
    //            seat = _CustM.emptyChairQueue[0];
    //            agent.SetDestination(seat.transform.position);
    //            _CustM.emptyChairQueue.Remove(_CustM.emptyChairQueue[0]);

    //            plateSpot = seat.transform.GetChild(0).gameObject;
    //        }
    //    }


    //}

    //void LeaveResturant()
    //{
    //    leaving = true;
    //    currentFoodSprite.SetActive(true);
    //    //pay
    //    if (isOrderCooked)
    //    {

    //        _GM.money += order.GetComponent<FoodData>().foodData.orderCost;
    //        orderDisplay.sprite = _CustM.happyCustomer;

    //    }
    //    else
    //    {
    //        orderDisplay.sprite = _CustM.sadCustomer;

    //        _GM.money += order.GetComponent<FoodData>().foodData.orderCost / 2;
    //        _GM.reputation -= order.GetComponent<FoodData>().foodData.reputationLoss;
    //    }

    //    ExecuteAfterSeconds(1, () => currentFoodSprite.SetActive(false));

    //    moneyEarned.SetActive(true);

    //    _GM.event_updateMoney.Invoke();
    //    _UI.UpdateReputationSlider();
    //        Destroy(order);
    //        _CustM.customersList.Remove(gameObject);
    //        _FM.foodInWave.Remove(order);
    //        _FM.queuedFood.Remove(order);

    //        //stop eating animation
    //        _CustM.emptyChairQueue.Add(seat);
    //    _CustM.event_newSeatAvalible.Invoke();

    //        agent.SetDestination(_CustM.resturantDoor.transform.position);
    //}
}
