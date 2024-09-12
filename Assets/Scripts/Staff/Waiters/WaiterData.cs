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



    [Header("Seating Customer")]
    [SerializeField]
    GameObject customer;
    CustomerData customerData;
    bool isCustomerFollowing;
    [SerializeField] float customerBreakingDistance;
    [SerializeField] float tableBreakingDistance;
    GameObject targetTable;

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
                if(_CustM.customersInQueue.Count > 0)
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
                    if (customer == null && !_CustM.customersReadyToOrder[0].GetComponent<CustomerData>().beingAttened)
                    {
                        _CustM.customersReadyToOrder[0].GetComponent<CustomerData>().beingAttened = true;
                        customer = _CustM.customersReadyToOrder[0];
                        customerData = customer.GetComponent<CustomerData>();
                        tasks = Task.TakeCustomerOrder;

                    }
                }


                //Serving Finished Food
                if(_FM.finishedFood_list.Count != 0)
                {
                    //when finished food is grabbed by waiter it will be removed from list
                    if(targetOrder == null)
                    {
                        targetOrder = _FM.finishedFood_list.FirstOrDefault();
                        _FM.finishedFood_list.Remove(targetOrder);
                        customer = targetOrder.GetComponent<FoodData>().order.customer;
                        customerData = customer.GetComponent<CustomerData>();
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
                            isCustomerFollowing = true;
                            //print("close to customre");
                            customerData.StartFollowWaiter(gameObject);
                            //set customer to follow
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

                        //walk to table
                        if (Vector3.Distance(transform.position, targetTable.transform.position) <= tableBreakingDistance)
                        {
                            //seat customer

                            //print("at table");
                            customerData.BeSeated(targetTable); //give them their table
                            customerData.beingAttened = false; //no longer atteneding this customer
                            ResetWaiter();

                            //reset
                        }
                    }
                }
                else tasks = Task.Idle;
                

                break;
            case Task.TakeCustomerOrder:

                //null check
                if (customer != null)
                {
                    agent.SetDestination(customer.transform.position);

                    if (Vector3.Distance(transform.position, customer.transform.position) <= customerBreakingDistance)
                    {
                        //end customer timer
                        customer.GetComponent<CustomerData>().OrderHasBeenTaken();

                        TakeOrder();
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
                    tasks = Task.DeliverFood;
                }

                break;
            case Task.DeliverFood:
                targetOrder.transform.position = holdFoodSpot.position;

                agent.SetDestination(customer.transform.position);

                if (Vector3.Distance(transform.position, customer.transform.position) <= customerBreakingDistance)
                {
                    //place food
                    targetOrder.transform.position = customerData.plateSpot.position;
                    customerData.orderGO = targetOrder;
                    customerData.task = CustomerData.Task.EatFood;
                    ResetWaiter();

                }

                break;
        }
    }

    void TakeOrder()
    {
        if (!isTakingOrder)
        {
            isTakingOrder = true;
            _FM.OrderUp(customerData.order);
            _UI.AddOrder(customerData.order);
            ExecuteAfterSeconds(1,()=> ResetWaiter());

        }
    }

    private void ResetWaiter()
    {
        //seating customer
        customer = null;
        customerData = null;
        targetTable = null;
        isCustomerFollowing = false;
        agent.isStopped = false;

        //taking order
        isTakingOrder = false;

        tasks = Task.Idle;
    }

    void CheckForCustomersToBeSeated()
    {

        if (customer == null && !_CustM.customersInQueue[0].GetComponent<CustomerData>().beingAttened)
        {
            _CustM.customersInQueue[0].GetComponent<CustomerData>().beingAttened = true;
            tasks = Task.SeatCustomer;

        }
        else return;
    }

    
}

