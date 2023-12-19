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
    GameObject heldFood;
    FoodData heldFoodData;

    NavMeshAgent agent;

    [SerializeField]
    GameObject currentCustomer;

    [SerializeField]
    GameObject holdfoodspot;

    // Start is called before the first frame update
    void Start()
    {



        holdfoodspot = transform.Find("HoldFoodSpot").gameObject;
        agent = GetComponent<NavMeshAgent>();
        //set speed
        agent.speed = waiterData.speed;
        _GM.event_startOfDay.AddListener(GetAllCustomers);
        _GM.event_foodToBeServed.AddListener(GetFood);
    }

    // Update is called once per frame
    void Update()
    {
        if(placed)
        {
            //hasnt picked up food yet but has a target;
            if (!isHoldingFood && heldFood != null)
            {
                agent.SetDestination(heldFood.transform.position);

                if (Vector3.Distance(transform.position, heldFood.transform.position) < 2f)
                {
                    print("close enought to grab food");
                    _FM.cookedFood.Remove(heldFood);
                    isHoldingFood = true;
                }
                //else print(Vector3.Distance(transform.position, heldFood.transform.position));

            }

            if (isHoldingFood) heldFood.transform.position = holdfoodspot.transform.position;

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
                            if (customerOrderData.name == heldFoodData.foodData.name && !customerData.hasBeenAttened)
                            {
                                print("found customer");
                                currentCustomer = customer;
                                customerData.hasBeenAttened = true;
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
                    isHoldingFood = false;
                    var customerData = currentCustomer.GetComponent<CustomerData>();
                    //give customer food
                    heldFood.GetComponent<FoodMovement>().served = true;
                    heldFood.transform.position = customerData.plateSpot.transform.position;
                    customerData.order = heldFood;
                    customerData.ServedFood();

                    ResetWaiter();

                }
            }
        }


    }

    void ResetWaiter()
    {
        heldFood = null;
        currentCustomer = null;
        agent.SetDestination(homePos);
        //if no food to grab, go home
        if (_FM.cookedFood.Count == 0) agent.destination = homePos;
        else GetFood();
    }
    void GetFood()
    {
        //check if alredy holding food, if they are that means they're in the middle of an order
        foreach (var food in _FM.foodInWave)
        {

            if (heldFood == null)
            {
                //food that is cooked and not already being picked up
                if (food.GetComponent<FoodData>().foodData.isBeingPickedUp == false)
                {
                    heldFood = food;
                    heldFoodData = heldFood.GetComponent<FoodData>();
                    heldFoodData.foodData.isBeingPickedUp = true;

                }
            }
            else return;



        }
    }
    void GetAllCustomers()
    {

    }
}
