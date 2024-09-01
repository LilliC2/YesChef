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
                if(_CustM.customersInQueue.Count != 0)
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
                if(_CustM.customersReadyToOrder.Count != 0)
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
                            print("close to customre");
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

                            print("at table");
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

                if (agent.remainingDistance <= agent.stoppingDistance)
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

    //// Update is called once per frame
    //void Update()
    //{
    //    if (placed)
    //    {

    //        //Animations
    //        if (agent.velocity.magnitude < 0.2f)
    //        {
    //            anim.SetBool("Walking", false);
    //            anim.SetBool("HoldWalking", false);
    //        }
    //        else
    //        {
    //            if (isHoldingFood)
    //            {
    //                anim.SetBool("Walking", false);

    //                if (heldFood_1 != null)
    //                {
    //                    anim.SetBool("HoldWalking2", false);
    //                    anim.SetBool("HoldWalking", true);
    //                }
    //                else
    //                {
    //                    anim.SetBool("HoldWalking", false);
    //                    anim.SetBool("HoldWalking2", true);
    //                }
    //            }
    //            if (!isHoldingFood)
    //            {
    //                anim.SetBool("HoldWalking", false);
    //                anim.SetBool("HoldWalking2", false);
    //                anim.SetBool("Walking", true);
    //            }


    //        }
    //        //Put food in hands
    //        if (isHoldingFood)
    //        {
    //            if (heldFood_1 != null) heldFood_1.transform.position = holdfoodspot_1.transform.position;
    //            if (heldFood_2 != null) heldFood_2.transform.position = holdfoodspot_2.transform.position;
    //        }

    //        switch (tasks)
    //        {
    //            case Task.Idle:

    //                //GO TO HOME SPOT
    //                agent.SetDestination(homePos);

    //                break;

    //            #region Find Customer
    //            case Task.FindCustomer:

    //                switch (waiterData.strength)
    //                {
    //                    case 2:

    //                        foreach (var customer in _CustM.customersList)
    //                        {
    //                            var customerData = customer.GetComponent<CustomerData>();
    //                            var customerOrderData = customerData.order.GetComponent<FoodData>();
    //                            if (currentCustomer_1 == null)
    //                            {
    //                                if (!customer.GetComponent<CustomerData>().leaving)
    //                                {
    //                                    if (customerOrderData.name == heldFoodData_1.foodData.name && !customerData.hasBeenAttened)
    //                                    {
    //                                        currentCustomer_1 = customer;
    //                                        customerData.hasBeenAttened = true;
    //                                        if (currentCustomer_1 != null && heldFood_2 == null) tasks = Task.DeliverFood;

    //                                    }

    //                                }
    //                                else return;
    //                            }
    //                            else if (currentCustomer_2 == null && heldFood_2 != null)
    //                            {
    //                                if (!customer.GetComponent<CustomerData>().leaving)
    //                                {
    //                                    if (customerOrderData.name == heldFoodData_2.foodData.name && !customerData.hasBeenAttened)
    //                                    {
    //                                        {
    //                                            currentCustomer_2 = customer;
    //                                            customerData.hasBeenAttened = true;
    //                                            if (currentCustomer_1 != null && currentCustomer_2 != null) tasks = Task.DeliverFood;

    //                                        }
    //                                    }
    //                                }
    //                                else return;

    //                            }

    //                        }

    //                        break;
    //                    default:

    //                        foreach (var customer in _CustM.customersList)
    //                        {
    //                            if (currentCustomer_1 == null)
    //                            {
    //                                if (!customer.GetComponent<CustomerData>().leaving)
    //                                {
    //                                    var customerData = customer.GetComponent<CustomerData>();
    //                                    var customerOrderData = customerData.order.GetComponent<FoodData>();
    //                                    if (customerOrderData.name == heldFoodData_1.foodData.name && !customerData.hasBeenAttened)
    //                                    {
    //                                        currentCustomer_1 = customer;
    //                                        customerData.hasBeenAttened = true;
    //                                        tasks = Task.DeliverFood;
    //                                    }

    //                                }
    //                                else return;
    //                            }
    //                        }
    //                        break;

    //                }


    //                break;
    //            #endregion

    //            #region Get Food
    //            case Task.GetFood:

    //                switch (waiterData.strength)
    //                {
    //                    case 2:




    //                        if (_FM.queuedFood.Contains(heldFood_1))
    //                        {
    //                            agent.SetDestination(heldFood_1.transform.position);

    //                            if (Vector3.Distance(transform.position, heldFood_1.transform.position) < 2f)
    //                            {

    //                                print("I have food 1");
    //                                _FM.queuedFood.Remove(heldFood_1);
    //                                if (heldFood_2 == null)
    //                                {
    //                                    tasks = Task.FindCustomer;
    //                                    isHoldingFood = true;

    //                                }
    //                            }

    //                        }
    //                        else if (_FM.queuedFood.Contains(heldFood_2) && heldFood_2 != null)
    //                        {
    //                            print("going to get food 2");

    //                            agent.SetDestination(heldFood_2.transform.position);

    //                            print(Vector3.Distance(transform.position, heldFood_2.transform.position));

    //                            if (Vector3.Distance(transform.position, heldFood_2.transform.position) < 3f)
    //                            {
    //                                print("I have 2 food");
    //                                _FM.queuedFood.Remove(heldFood_2);
    //                                isHoldingFood = true;
    //                                tasks = Task.FindCustomer;
    //                            }

    //                        }
    //                        else tasks = Task.FindCustomer;



    //                        break;
    //                    default:

    //                        agent.SetDestination(heldFood_1.transform.position);

    //                        if (Vector3.Distance(transform.position, heldFood_1.transform.position) < 2f)
    //                        {

    //                            print("close enought to grab food");
    //                            _FM.queuedFood.Remove(heldFood_1);
    //                            isHoldingFood = true;

    //                            tasks = Task.FindCustomer;


    //                        }

    //                        break;
    //                }

    //                break;
    //            #endregion

    //            #region Deliever Food
    //            case Task.DeliverFood:

    //                //if(currentCustomer_1 == null) tasks = Task.FindCustomer;


    //                switch (waiterData.strength)
    //                {
    //                    case 2:

    //                        if (currentCustomer_1 != null)
    //                        {
    //                            agent.SetDestination(currentCustomer_1.transform.position);
    //                            if (Vector3.Distance(transform.position, currentCustomer_1.transform.position) < 2f)
    //                            {

    //                                if (currentCustomer_2 == null) isHoldingFood = false;

    //                                var customerData = currentCustomer_1.GetComponent<CustomerData>();
    //                                var customerOrderData = customerData.order.GetComponent<FoodData>();

    //                                //give customer food

    //                                if (customerOrderData.name == heldFoodData_1.foodData.name)
    //                                {
    //                                    heldFood_1.GetComponent<FoodMovement>().served = true;
    //                                    heldFood_1.transform.position = customerData.plateSpot.transform.position;
    //                                    customerData.order = heldFood_1;
    //                                    heldFood_1 = null;
    //                                    customerData.ServedFood();
    //                                    currentCustomer_1 = null;
    //                                }

    //                                if (heldFood_2 == null)
    //                                {
    //                                    ResetWaiter();
    //                                }


    //                            }
    //                        }
    //                        else if (currentCustomer_2 != null)
    //                        {
    //                            agent.SetDestination(currentCustomer_2.transform.position);
    //                            if (Vector3.Distance(transform.position, currentCustomer_2.transform.position) < 2f)
    //                            {
    //                                isHoldingFood = false;


    //                                var customerData = currentCustomer_2.GetComponent<CustomerData>();
    //                                var customerOrderData = customerData.order.GetComponent<FoodData>();

    //                                //give customer food

    //                                if (customerOrderData.name == heldFoodData_2.foodData.name)
    //                                {
    //                                    heldFood_2.GetComponent<FoodMovement>().served = true;
    //                                    heldFood_2.transform.position = customerData.plateSpot.transform.position;
    //                                    customerData.order = heldFood_2;
    //                                    heldFood_2 = null;
    //                                    customerData.ServedFood();
    //                                    ResetWaiter();
    //                                }


    //                            }
    //                        }

    //                        break;
    //                    default:

    //                        if (currentCustomer_1 != null)
    //                        {
    //                            agent.SetDestination(currentCustomer_1.transform.position);
    //                            if (Vector3.Distance(transform.position, currentCustomer_1.transform.position) < 2f)
    //                            {
    //                                isHoldingFood = false;


    //                                var customerData = currentCustomer_1.GetComponent<CustomerData>();
    //                                var customerOrderData = customerData.order.GetComponent<FoodData>();

    //                                //give customer food

    //                                if (customerOrderData.name == heldFoodData_1.foodData.name)
    //                                {
    //                                    heldFood_1.GetComponent<FoodMovement>().served = true;
    //                                    heldFood_1.transform.position = customerData.plateSpot.transform.position;
    //                                    customerData.order = heldFood_1;
    //                                    customerData.ServedFood();
    //                                    ResetWaiter();
    //                                }


    //                            }

    //                        }
    //                        else tasks = Task.Idle;


    //                        break;
    //                }

    //                break;
    //                #endregion
    //        }
    //    }



    //}

    //void ResetWaiter()
    //{
    //    print("reset waiter");
    //    heldFood_1 = null;
    //    heldFood_2 = null;
    //    currentCustomer_1 = null;
    //    currentCustomer_2 = null;
    //    //if no food to grab, go home
    //    if (_FM.queuedFood.Count == 0)
    //    {
    //        agent.destination = homePos;
    //        tasks = Task.Idle;

    //    }
    //    else
    //    {
    //        GetFood();

    //    }
    //}
    //void GetFood()
    //{
    //    //check if alredy holding food, if they are that means they're in the middle of an order
    //    if (isHoldingFood) return;
    //    else
    //    {
    //        foreach (var food in _FM.foodInWave)
    //        {
    //            switch (waiterData.strength)

    //            {
    //                case 2:

    //                    if (heldFood_1 == null)
    //                    {
    //                        //food that is cooked and not already being picked up
    //                        if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
    //                        {
    //                            print("grabbed food for 1");
    //                            heldFood_1 = food;
    //                            heldFoodData_1 = heldFood_1.GetComponent<FoodData>();
    //                            heldFoodData_1.foodData.isBeingPickedUp = true;

    //                        }
    //                    }
    //                    else if (heldFood_2 == null && _FM.foodInWave.Count != 1)
    //                    {
    //                        //food that is cooked and not already being picked up
    //                        if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
    //                        {
    //                            print("grabbed food for 2");

    //                            heldFood_2 = food;
    //                            heldFoodData_2 = heldFood_2.GetComponent<FoodData>();
    //                            heldFoodData_2.foodData.isBeingPickedUp = true;

    //                        }
    //                    }
    //                    else return;

    //                    if (heldFood_1 != null)
    //                    {
    //                        print("go get food");
    //                        tasks = Task.GetFood;

    //                    }

    //                    break;
    //                default:

    //                    if (heldFood_1 == null)
    //                    {
    //                        //food that is cooked and not already being picked up
    //                        if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
    //                        {
    //                            print("grabbed food for 1");
    //                            heldFood_1 = food;
    //                            heldFoodData_1 = heldFood_1.GetComponent<FoodData>();
    //                            heldFoodData_1.foodData.isBeingPickedUp = true;

    //                            tasks = Task.GetFood;
    //                        }
    //                    }
    //                    else return;

    //                    break;
    //            }
    //        }



    //    }
    //}

    //public void FireChef()
    //{
    //    //dont fire if holding food
    //    if (isHoldingFood)
    //    {
    //        print("Cant fire chef while holding food");
    //    }
    //    else
    //    {
    //        if (heldFoodData_1 != null) heldFoodData_1.foodData.isBeingPickedUp = false;
    //        if (heldFoodData_2 != null) heldFoodData_2.foodData.isBeingPickedUp = false;
    //        if (currentCustomer_1 != null) currentCustomer_1.GetComponent<CustomerData>().hasBeenAttened = false;
    //        if (currentCustomer_2 != null) currentCustomer_2.GetComponent<CustomerData>().hasBeenAttened = false;
    //        Destroy(gameObject);
    //    }


    //}

    //private void OnMouseDown()
    //{
    //    if (placed)
    //    {
    //        _UI.OpenWaiterPopUp(this.gameObject);
    //        anim.SetTrigger("Spawn");


    //    }
    //}
}

