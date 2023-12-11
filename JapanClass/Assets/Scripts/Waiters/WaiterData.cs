using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterData :GameBehaviour
{
    bool isHoldingFood;

    List<CustomerData> customers;

    Vector3 homePos;
    [SerializeField]
    GameObject heldFood;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //temp
        homePos = transform.position;

        _GM.event_startOfDay.AddListener(GetAllCustomers);
        _GM.event_foodToBeServed.AddListener(GetFood);
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var customer in _CustM.customersList)
        //{
        //    if (customer != null) customers.Add(customer.GetComponent<CustomerData>());

        //}
        if(isHoldingFood)
        {
            if (Vector3.Distance(transform.position, heldFood.transform.position) < 1)
            {
                print("holding");
            }
        }

    }

    void GetFood()
    {
        //check if alredy holding food, if they are that means they're in the middle of an order
        if (!isHoldingFood)
        {
            var foodFound = false;


            if (!foodFound)
            {
                foreach (var food in _FM.foodInWave)
                {
                    //food that is cooked and not already being picked up
                    if (food.GetComponent<FoodData>().foodData.isCooked)// && food.GetComponent<FoodData>().foodData.isBeingPickedUp == false
                    {
                        heldFood = food;
                        //heldFood.GetComponent<FoodData>().foodData.isBeingPickedUp = true;

                        agent.SetDestination(heldFood.transform.position);
                        foodFound = true;
                    }
                }

            }

        }
        else return;

    }

    void GetAllCustomers()
    {

    }
}
