using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CustomerData : GameBehaviour
{
    public GameObject order;
    public bool hasBeenAttened;
    Image orderDisplay;

    GameObject seat;
    public GameObject plateSpot;

    NavMeshAgent agent;
    public bool leaving;

    public GameObject currentFoodSprite;
    // Start is called before the first frame update
    void Start()
    {

        currentFoodSprite.SetActive(false);

        agent = GetComponent<NavMeshAgent>();
        FindSeat();

        orderDisplay = GetComponentInChildren<Image>();

        _GM.event_startOfDay.AddListener(OrderFood);
    }

    // Update is called once per frame
    void Update()
    {
        currentFoodSprite.transform.LookAt(Camera.main.transform.position);

        if (leaving && Vector3.Distance(transform.position, _CustM.resturantDoor.transform.position) < 2)
        {
            currentFoodSprite.SetActive(false);
            Destroy(gameObject);

        }
    }
    public void ServedFood()
    {
        print("YUM!");

        //begin eating after X seconds
        ExecuteAfterSeconds(order.GetComponent<FoodData>().foodData.eatTime,()=> LeaveResturant());

        //eating aniamtion??


        //destroy food

        //leave resturant



    }

    void OrderFood()
    {
        order = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        _FM.orderedFood.Add(order);
        currentFoodSprite.SetActive(true);
        orderDisplay.sprite = order.GetComponent<FoodData>().foodData.pfp;

    }

    void FindSeat()
    {
        seat = _CustM.emptyChairQueue[0];
        agent.SetDestination(seat.transform.position);
        _CustM.emptyChairQueue.Remove(_CustM.emptyChairQueue[0]);

        plateSpot = seat.transform.GetChild(0).gameObject;
    }

    void LeaveResturant()
    {
        Destroy(order);
        _CustM.customersList.Remove(gameObject);
        _FM.foodInWave.Remove(order);
        _FM.cookedFood.Remove(order);

        //stop eating animation
        _CustM.emptyChairQueue.Add(seat);
        agent.SetDestination(_CustM.resturantDoor.transform.position);
        leaving = true;
    }
}
