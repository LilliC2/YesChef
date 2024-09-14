using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WaiterData : GameBehaviour
{
    public WaiterClass waiterData;

    public enum Task { Idle, SeatCustomer ,TakeCustomerOrder, GetFood, DeliverFood }
    public Task tasks;


    public bool placed;

    public Animator anim;

    NavMeshAgent agent;

    [SerializeField]
    bool isPaused;

    [Header("Seating Customer")]
    [SerializeField]
    GameObject customer;
    CustomerData customerData;
    bool isCustomerFollowing;
    [SerializeField] float customerBreakingDistance;
    [SerializeField] float tableBreakingDistance;
    [SerializeField] GameObject targetTable;

    [Header("Take Customer Order")]
    bool isTakingOrder;

    [Header("Deliver Finished Order")]
    GameObject targetOrder;
    [SerializeField]
    bool isHoldingFood;
    [SerializeField]
    Transform holdFoodSpot;


    // Start is called before the first frame update
    void Start()
    {

        holdFoodSpot = transform.Find("HoldFoodSpot").gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        //set speed
        agent.speed = waiterData.speed;


        //_GM.event_foodToBeServed.AddListener(GetFood);
    }

    private void Update()
    {
        //set customer data
        if(customer != null && customerData == null) { customerData = customer.GetComponent<CustomerData>(); }

        switch(tasks)
        {
            case Task.Idle:

                //task check

                //Seating Customer
                if(_CustM.customersInQueue.Count > 0 && _CustM.customerIsWaiting)
                {
                    //check if customer is being attended AND there is a table avalible
                    if (customer == null && !_CustM.customersInQueue[0].GetComponent<CustomerData>().beingAttened && _FOHM.unoccupiedTables.Count != 0)
                    {
                        _CustM.customersInQueue[0].GetComponent<CustomerData>().beingAttened = true;
                        customer = _CustM.customersInQueue[0];
                        customerData = customer.GetComponent<CustomerData>();
                        tasks = Task.SeatCustomer;

                    }
                }
                
                //Taking Order
                if(_CustM.customersReadyToOrder.Count > 0)
                {
                    foreach (var _customer in _CustM.customersReadyToOrder)
                    {
                        if (customer == null && !_customer.GetComponent<CustomerData>().beingAttened && _customer.GetComponent<CustomerData>().task == CustomerData.Task.ReadyToOrder)
                        {
                            _customer.GetComponent<CustomerData>().beingAttened = true;
                            customer = _customer;
                            customerData = customer.GetComponent<CustomerData>();
                            targetTable = customerData.GetCustomerTable();

                            tasks = Task.TakeCustomerOrder;
                            break;

                        }
                    }
                    
                }


                //Serving Finished Food
                if(_FM.finishedFood_list.Count != 0)
                {
                    //when finished food is grabbed by waiter it will be removed from list
                    if(targetOrder == null)
                    {
                        //get order
                        targetOrder = _FM.finishedFood_list.FirstOrDefault();
                        _FM.finishedFood_list.Remove(targetOrder);

                        //get customer who ordered it
                        customer = targetOrder.GetComponent<FoodData>().order.customer;
                        print("Serve to customer " + customer.name);
                        customerData = customer.GetComponent<CustomerData>();
                        targetTable = customerData.GetCustomerTable();
                        customerData.beingAttened = true;
                        tasks = Task.GetFood;
                    }
                }

                break;
            case Task.SeatCustomer:

                //null check
                if (customer != null)
                {
                    if (!isCustomerFollowing)
                    {
                        //walk to customer
                        agent.SetDestination(customer.transform.position);

                        if (Vector3.Distance(transform.position, customer.transform.position) <= customerBreakingDistance)
                        {
                            //print("close to customre");
                            customerData.StartFollowWaiter(gameObject);
                            
                            if(!StartPauseAgent(1f))
                            {
                                isCustomerFollowing = true;
                            }

                            
                        }

                    }
                    else
                    {
                        //customer is now following

                        //find avalible table
                        if (targetTable == null)
                        {
                            targetTable = _FOHM.FindClosestTable(gameObject);
                            _FOHM.ChangeToOccupied(targetTable);

                            agent.SetDestination(targetTable.transform.position);
                        }
                        else if(targetTable != null)
                        {

                            if (TravelToTable(targetTable))
                            {
                                //if customer close enough to table
                                customerData.BeSeated(targetTable); //give them their table
                                customerData.beingAttened = false; //no longer atteneding this customer

                                //pause for a little
                                if (!isPaused)
                                {
                                    isPaused = true;
                                    //bool time = _SM.PauseAgent(agent, 0.5f);
                                    if (!agent.isStopped)
                                    {
                                        ResetWaiter();
                                    }

                                }
                                
                                

                            }
                        }

                        
                    }
                }
                else tasks = Task.Idle;
                

                break;
            case Task.TakeCustomerOrder:

                //null check
                if (customer != null)
                {
                    if (TravelToTable(targetTable))
                    {
                        //end customer timer
                        customer.GetComponent<CustomerData>().OrderHasBeenTaken();

                        if (!isTakingOrder)
                        {
                            isTakingOrder = true;

                            _FM.OrderUp(customerData.order, customer);
                            _UI.AddOrder(customerData.orderClass);

                            

                        }
                        else
                        {
                            if (!StartPauseAgent(1f))
                            {
                                ResetWaiter();

                            }
                        }
                    }
                   

                }
                else tasks = Task.Idle;

                break;
            case Task.GetFood:

                //while not holding food, go get target food
                agent.SetDestination(targetOrder.transform.position);

                if (Vector3.Distance(transform.position, targetOrder.transform.position)<=2f)
                {

                    //change food status
                    targetOrder.GetComponent<FoodData>().foodMovement = FoodData.FoodMovement.BeingHeld; 

                    targetOrder.transform.position = holdFoodSpot.position;

                    //set pass spot to unoccupied
                    _PM.UnoccupiedPassPoint(targetOrder.GetComponent<FoodData>().ReturnPassPoint());

                    if (!StartPauseAgent(0.5f))
                    {
                        print("go deliver");
                        tasks = Task.DeliverFood;

                    }

                }

                break;
            case Task.DeliverFood:
                targetOrder.transform.position = holdFoodSpot.position;

                if(TravelToTable(targetTable))
                {
                    //place food
                    targetOrder.transform.position = customerData.plateSpot.position;
                    customerData.order = targetOrder; //change their order to the intantiated object and not the root prefab asset
                    customerData.task = CustomerData.Task.EatFood;
                    //pause for a little
                    if (!StartPauseAgent(1f))
                    {
                        ResetWaiter() ;

                    }
                }

                break;
        }
    }

    /// <summary>
    /// Set destination table, return True when destination is reached
    /// </summary>
    /// <param name="_table"></param>
    /// <returns></returns>
    bool TravelToTable(GameObject _table)
    {
        agent.SetDestination(_table.GetComponent<Table>().waiterAttendPosition.position);

        if (Vector3.Distance(transform.position, agent.destination) <= customerBreakingDistance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Reset values to prepare for new customer
    /// </summary>
    private void ResetWaiter()
    {
        //seating customer
        print("Reset Waiter");
        customer = null;
        customerData = null;
        targetTable = null;
        isCustomerFollowing = false;
        agent.isStopped = false;

        isPaused = false;

        //taking order
        isTakingOrder = false;

        tasks = Task.Idle;
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
            if (!agent.isStopped)
            {
                isPaused = false;
            }
        }

        return isPaused;
    }
    
}

