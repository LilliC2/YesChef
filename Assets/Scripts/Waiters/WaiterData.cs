using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterData :GameBehaviour
{
    public WaiterClass waiterData;

    [SerializeField]
    bool isHoldingFood;
    public bool placed;

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
    GameObject currentCustomer;

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
        if(placed)
        {
            //idle animation
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
                    anim.SetBool("HoldWalking", true);
                }
                if (!isHoldingFood)
                {
                    anim.SetBool("HoldWalking", false);
                    anim.SetBool("Walking", true);
                }


            }



            //hasnt picked up food yet but has a target;
            if (!isHoldingFood && heldFood_1 != null)
            {
                agent.SetDestination(heldFood_1.transform.position);

                if (Vector3.Distance(transform.position, heldFood_1.transform.position) < 2f)
                {

                    print("close enought to grab food");
                    _FM.queuedFood.Remove(heldFood_1);
                    isHoldingFood = true;

                }

                if(heldFood_2 == null && waiterData.strength == 2)
                {
                    agent.SetDestination(heldFood_2.transform.position);

                    if (Vector3.Distance(transform.position, heldFood_2.transform.position) < 2f)
                    {
                        _FM.queuedFood.Remove(heldFood_2);
                        isHoldingFood = true;

                    }
                }
                //else print(Vector3.Distance(transform.position, heldFood.transform.position));

            }

            if (isHoldingFood)
            {
                heldFood_1.transform.position = holdfoodspot_1.transform.position;
                if(heldFood_2 != null) heldFood_2.transform.position = holdfoodspot_2.transform.position;
            }

            if (isHoldingFood && currentCustomer == null)
            {

                //find customer who is waiting
                foreach (var customer in _CustM.customersList)
                {
                    if (currentCustomer == null)
                    {
                        if (!customer.GetComponent<CustomerData>().leaving)
                        {
                            var customerData = customer.GetComponent<CustomerData>();
                            var customerOrderData = customerData.order.GetComponent<FoodData>();
                            if (customerOrderData.name == heldFoodData_1.foodData.name && !customerData.hasBeenAttened)
                            {
                                print("found customer");
                                currentCustomer = customer;
                                customerData.hasBeenAttened = true;
                            }
                            else if(customerOrderData.name == heldFoodData_2.foodData.name && !customerData.hasBeenAttened)
                            {
                                {
                                    print("found customer");
                                    currentCustomer = customer;
                                    customerData.hasBeenAttened = true;
                                }
                            }
                        }
                        else return;
                    }

                }
            }

            if (isHoldingFood && currentCustomer != null)
            {
                //bring food to them
                agent.SetDestination(currentCustomer.transform.position);
                if (Vector3.Distance(transform.position, currentCustomer.transform.position) < 2f)
                {
                    if(waiterData.strength == 2)
                    {
                        if(heldFood_1 == null && heldFood_2 == null) isHoldingFood = false;

                    }
                    else isHoldingFood = false;

                    var customerData = currentCustomer.GetComponent<CustomerData>();
                    var customerOrderData = customerData.order.GetComponent<FoodData>();

                    //give customer food

                    if (customerOrderData.name == heldFoodData_1.foodData.name)
                    {
                        heldFood_1.GetComponent<FoodMovement>().served = true;
                        heldFood_1.transform.position = customerData.plateSpot.transform.position;
                        customerData.order = heldFood_1;
                        customerData.ServedFood();
                    }
                    else if (customerOrderData.name == heldFoodData_2.foodData.name)
                    {
                        heldFood_2.GetComponent<FoodMovement>().served = true;
                        heldFood_2.transform.position = customerData.plateSpot.transform.position;
                        customerData.order = heldFood_2;
                        customerData.ServedFood();
                    }

                    if(!isHoldingFood) ResetWaiter();


                }
            }
        }


    }

    void ResetWaiter()
    {
        heldFood_1 = null;
        currentCustomer = null;
        agent.SetDestination(homePos);
        //if no food to grab, go home
        if (_FM.queuedFood.Count == 0) agent.destination = homePos;
        else GetFood();
    }
    void GetFood()
    {
        //check if alredy holding food, if they are that means they're in the middle of an order
        foreach (var food in _FM.foodInWave)
        {

            if (heldFood_1 == null)
            {
                //food that is cooked and not already being picked up
                if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                {
                    heldFood_1 = food;
                    heldFoodData_1 = heldFood_1.GetComponent<FoodData>();
                    heldFoodData_1.foodData.isBeingPickedUp = true;

                }
            }
            else if(waiterData.strength == 2 && heldFood_2 == null)
            {
                //food that is cooked and not already being picked up
                if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                {
                    heldFood_2 = food;
                    heldFoodData_2 = heldFood_2.GetComponent<FoodData>();
                    heldFoodData_2.foodData.isBeingPickedUp = true;

                }
            }
            else return;



        }
    }
    private void OnMouseDown()
    {
        if (placed) _UI.OpenWaiterPopUp(this.gameObject);

    }
}
