using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterData : GameBehaviour
{
    public WaiterClass waiterData;


    public enum Task { Idle, FindCustomer, GetFood, DeliverFood }
    public Task tasks;

    [SerializeField]
    bool isHoldingFood;
    public bool placed;
    [SerializeField]
    bool isFoodEmpty;

    public Vector3 homePos;
    [SerializeField]
    GameObject heldFood_1;
    FoodData heldFoodData_1;
    [SerializeField]
    GameObject heldFood_2;
    FoodData heldFoodData_2;

    public Animator anim;

    NavMeshAgent agent;

    [SerializeField]
    GameObject currentCustomer_1;
    [SerializeField]
    GameObject currentCustomer_2;

    [SerializeField]
    GameObject holdfoodspot_1;
    [SerializeField]
    GameObject holdfoodspot_2;

    // Start is called before the first frame update
    void Start()
    {



        //holdfoodspot = transform.Find("HoldFoodSpot").gameObject;
        agent = GetComponent<NavMeshAgent>();
        //set speed
        agent.speed = waiterData.speed;
        _GM.event_foodToBeServed.AddListener(GetFood);
    }

    // Update is called once per frame
    void Update()
    {
        if (placed)
        {

            //Animations
            if (agent.velocity.magnitude < 0.2f)
            {
                anim.SetBool("Walking", false);
                anim.SetBool("HoldWalking", false);
            }
            else
            {
                if (isHoldingFood)
                {
                    anim.SetBool("Walking", false);

                    if (heldFood_1 != null)
                    {
                        anim.SetBool("HoldWalking2", false);
                        anim.SetBool("HoldWalking", true);
                    }
                    else
                    {
                        anim.SetBool("HoldWalking", false);
                        anim.SetBool("HoldWalking2", true);
                    }
                }
                if (!isHoldingFood)
                {
                    anim.SetBool("HoldWalking", false);
                    anim.SetBool("HoldWalking2", false);
                    anim.SetBool("Walking", true);
                }


            }
            //Put food in hands
            if (isHoldingFood)
            {
                if (heldFood_1 != null) heldFood_1.transform.position = holdfoodspot_1.transform.position;
                if (heldFood_2 != null) heldFood_2.transform.position = holdfoodspot_2.transform.position;
            }

            switch (tasks)
            {
                case Task.Idle:

                    //GO TO HOME SPOT
                    agent.SetDestination(homePos);

                    break;

                #region Find Customer
                case Task.FindCustomer:

                    switch (waiterData.strength)
                    {
                        case 2:

                            foreach (var customer in _CustM.customersList)
                            {
                                var customerData = customer.GetComponent<CustomerData>();
                                var customerOrderData = customerData.order.GetComponent<FoodData>();
                                if (currentCustomer_1 == null)
                                {
                                    if (!customer.GetComponent<CustomerData>().leaving)
                                    {
                                        if (customerOrderData.name == heldFoodData_1.foodData.name && !customerData.hasBeenAttened)
                                        {
                                            currentCustomer_1 = customer;
                                            customerData.hasBeenAttened = true;
                                            if (currentCustomer_1 != null && heldFood_2 == null) tasks = Task.DeliverFood;

                                        }

                                    }
                                    else return;
                                }
                                else if (currentCustomer_2 == null && heldFood_2 != null)
                                {
                                    if (!customer.GetComponent<CustomerData>().leaving)
                                    {
                                        if (customerOrderData.name == heldFoodData_2.foodData.name && !customerData.hasBeenAttened)
                                        {
                                            {
                                                currentCustomer_2 = customer;
                                                customerData.hasBeenAttened = true;
                                                if (currentCustomer_1 != null && currentCustomer_2 != null) tasks = Task.DeliverFood;

                                            }
                                        }
                                    }
                                    else return;

                                }

                            }

                            break;
                        default:

                            foreach (var customer in _CustM.customersList)
                            {
                                if (currentCustomer_1 == null)
                                {
                                    if (!customer.GetComponent<CustomerData>().leaving)
                                    {
                                        var customerData = customer.GetComponent<CustomerData>();
                                        var customerOrderData = customerData.order.GetComponent<FoodData>();
                                        if (customerOrderData.name == heldFoodData_1.foodData.name && !customerData.hasBeenAttened)
                                        {
                                            currentCustomer_1 = customer;
                                            customerData.hasBeenAttened = true;
                                            tasks = Task.DeliverFood;
                                        }

                                    }
                                    else return;
                                }
                            }
                            break;

                    }


                    break;
                #endregion

                #region Get Food
                case Task.GetFood:

                    switch (waiterData.strength)
                    {
                        case 2:




                            if (_FM.queuedFood.Contains(heldFood_1))
                            {
                                agent.SetDestination(heldFood_1.transform.position);

                                if (Vector3.Distance(transform.position, heldFood_1.transform.position) < 2f)
                                {

                                    print("I have food 1");
                                    _FM.queuedFood.Remove(heldFood_1);
                                    if (heldFood_2 == null)
                                    {
                                        tasks = Task.FindCustomer;
                                        isHoldingFood = true;

                                    }
                                }

                            }
                            else if (_FM.queuedFood.Contains(heldFood_2) && heldFood_2 != null)
                            {
                                print("going to get food 2");

                                agent.SetDestination(heldFood_2.transform.position);

                                print(Vector3.Distance(transform.position, heldFood_2.transform.position));

                                if (Vector3.Distance(transform.position, heldFood_2.transform.position) < 3f)
                                {
                                    print("I have 2 food");
                                    _FM.queuedFood.Remove(heldFood_2);
                                    isHoldingFood = true;
                                    tasks = Task.FindCustomer;
                                }

                            }
                            else tasks = Task.FindCustomer;



                            break;
                        default:

                            agent.SetDestination(heldFood_1.transform.position);

                            if (Vector3.Distance(transform.position, heldFood_1.transform.position) < 2f)
                            {

                                print("close enought to grab food");
                                _FM.queuedFood.Remove(heldFood_1);
                                isHoldingFood = true;

                                tasks = Task.FindCustomer;


                            }

                            break;
                    }

                    break;
                #endregion

                #region Deliever Food
                case Task.DeliverFood:

                    //if(currentCustomer_1 == null) tasks = Task.FindCustomer;


                    switch (waiterData.strength)
                    {
                        case 2:

                            if (currentCustomer_1 != null)
                            {
                                agent.SetDestination(currentCustomer_1.transform.position);
                                if (Vector3.Distance(transform.position, currentCustomer_1.transform.position) < 2f)
                                {

                                    if (currentCustomer_2 == null) isHoldingFood = false;

                                    var customerData = currentCustomer_1.GetComponent<CustomerData>();
                                    var customerOrderData = customerData.order.GetComponent<FoodData>();

                                    //give customer food

                                    if (customerOrderData.name == heldFoodData_1.foodData.name)
                                    {
                                        heldFood_1.GetComponent<FoodMovement>().served = true;
                                        heldFood_1.transform.position = customerData.plateSpot.transform.position;
                                        customerData.order = heldFood_1;
                                        heldFood_1 = null;
                                        customerData.ServedFood();
                                        currentCustomer_1 = null;
                                    }

                                    if (heldFood_2 == null)
                                    {
                                        ResetWaiter();
                                    }


                                }
                            }
                            else if (currentCustomer_2 != null)
                            {
                                agent.SetDestination(currentCustomer_2.transform.position);
                                if (Vector3.Distance(transform.position, currentCustomer_2.transform.position) < 2f)
                                {
                                    isHoldingFood = false;


                                    var customerData = currentCustomer_2.GetComponent<CustomerData>();
                                    var customerOrderData = customerData.order.GetComponent<FoodData>();

                                    //give customer food

                                    if (customerOrderData.name == heldFoodData_2.foodData.name)
                                    {
                                        heldFood_2.GetComponent<FoodMovement>().served = true;
                                        heldFood_2.transform.position = customerData.plateSpot.transform.position;
                                        customerData.order = heldFood_2;
                                        heldFood_2 = null;
                                        customerData.ServedFood();
                                        ResetWaiter();
                                    }


                                }
                            }

                            break;
                        default:

                            if (currentCustomer_1 != null)
                            {
                                agent.SetDestination(currentCustomer_1.transform.position);
                                if (Vector3.Distance(transform.position, currentCustomer_1.transform.position) < 2f)
                                {
                                    isHoldingFood = false;


                                    var customerData = currentCustomer_1.GetComponent<CustomerData>();
                                    var customerOrderData = customerData.order.GetComponent<FoodData>();

                                    //give customer food

                                    if (customerOrderData.name == heldFoodData_1.foodData.name)
                                    {
                                        heldFood_1.GetComponent<FoodMovement>().served = true;
                                        heldFood_1.transform.position = customerData.plateSpot.transform.position;
                                        customerData.order = heldFood_1;
                                        customerData.ServedFood();
                                        ResetWaiter();
                                    }


                                }

                            }
                            else tasks = Task.Idle;


                            break;
                    }

                    break;
                    #endregion
            }
        }



    }

    void ResetWaiter()
    {
        print("reset waiter");
        heldFood_1 = null;
        heldFood_2 = null;
        currentCustomer_1 = null;
        currentCustomer_2 = null;
        //if no food to grab, go home
        if (_FM.queuedFood.Count == 0)
        {
            agent.destination = homePos;
            tasks = Task.Idle;

        }
        else
        {
            GetFood();

        }
    }
    void GetFood()
    {
        //check if alredy holding food, if they are that means they're in the middle of an order
        if (isHoldingFood) return;
        else
        {
            foreach (var food in _FM.foodInWave)
            {
                switch (waiterData.strength)

                {
                    case 2:

                        if (heldFood_1 == null)
                        {
                            //food that is cooked and not already being picked up
                            if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                            {
                                print("grabbed food for 1");
                                heldFood_1 = food;
                                heldFoodData_1 = heldFood_1.GetComponent<FoodData>();
                                heldFoodData_1.foodData.isBeingPickedUp = true;

                            }
                        }
                        else if (heldFood_2 == null && _FM.foodInWave.Count != 1)
                        {
                            //food that is cooked and not already being picked up
                            if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                            {
                                print("grabbed food for 2");

                                heldFood_2 = food;
                                heldFoodData_2 = heldFood_2.GetComponent<FoodData>();
                                heldFoodData_2.foodData.isBeingPickedUp = true;

                            }
                        }
                        else return;

                        if (heldFood_1 != null)
                        {
                            print("go get food");
                            tasks = Task.GetFood;

                        }

                        break;
                    default:

                        if (heldFood_1 == null)
                        {
                            //food that is cooked and not already being picked up
                            if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                            {
                                print("grabbed food for 1");
                                heldFood_1 = food;
                                heldFoodData_1 = heldFood_1.GetComponent<FoodData>();
                                heldFoodData_1.foodData.isBeingPickedUp = true;

                                tasks = Task.GetFood;
                            }
                        }
                        else return;

                        break;
                }
            }



        }
    }

    public void FireChef()
    {
        //dont fire if holding food
        if (isHoldingFood)
        {
            print("Cant fire chef while holding food");
        }
        else
        {
            if (heldFoodData_1 != null) heldFoodData_1.foodData.isBeingPickedUp = false;
            if (heldFoodData_2 != null) heldFoodData_2.foodData.isBeingPickedUp = false;
            if (currentCustomer_1 != null) currentCustomer_1.GetComponent<CustomerData>().hasBeenAttened = false;
            if (currentCustomer_2 != null) currentCustomer_2.GetComponent<CustomerData>().hasBeenAttened = false;
            Destroy(gameObject);
        }


    }

    private void OnMouseDown()
    {
        if (placed)
        {
            _UI.OpenWaiterPopUp(this.gameObject);
            anim.SetTrigger("Spawn");


        }
    }
}

