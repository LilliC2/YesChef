using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CustomerManager : Singleton<CustomerManager>
{
    public List<GameObject> customersInResturant = new List<GameObject>();

    [Header("Queue")]
    public List<Transform> customerOutsideQueueSpots = new List<Transform>();
    public List<GameObject> customersInQueue = new List<GameObject>();
    [SerializeField]
    Transform customerSpawnPoint;

    [Header("Ready to Order")]
    public List<GameObject> customersReadyToOrder = new List<GameObject>();

    public Sprite happyCustomer;
    public Sprite sadCustomer;

    public GameObject customer;

    public UnityEvent event_newSeatAvalible;

    [Header("Leave")]

    public Transform[] leavePoints;

    [Header("Resturant Ratings")]
    [SerializeField] float minQueueWaitTime, maxQueueWaitTime;
    [SerializeField] float minOrderTakeWaitTime, maxOrderTakeWaitTime;
    [SerializeField] float minOrderArrivalTime, maxOrderArrivalTime;

    int resturantRatingTotal_currentDay;

    [Header("Resturant Ratings")]
    int currentDayCustomerIntake = 0;

    [Header("Customer Spawn Rates")]
    [SerializeField] float customerSpawnRate;

    public enum Deviations { D1, D2, D3, D4, D5, D6}
    public Deviations deviations;
    float sixthOfDayLength = 50;// 5 min days = 300secondsd 
    float d1,d2,d3,d4,d5,d6;
    [SerializeField] float customerSpawnChance;
    int customersSpawned;

    // Start is called before the first frame update
    void Start()
    {
        sixthOfDayLength = _GM.openDayLength / 6;
        SetDeviationInts();
        _GM.event_playStateClose.AddListener(AddResturantRating);
        _GM.event_playStateOpen.AddListener(StartSpawning);
        _GM.event_playStateClose.AddListener(EndSpawning);
        //customerQueueWaitingCheck = new bool[customerQueueSpots.Count];

        //testing, spawn customers immedately
        //StartCoroutine(SpawnCustomers());
        //_GM.event_endOfDay.AddListener(SpawnCustomersEventListener);

    }

    private void Update()
    {
        if(_GM.playState == GameManager.PlayState.Open)
        {
            CalculateCurrentDeviations();
            SetCustomerSpawnChance();
        }
    }

    /*
     * 1. take day length and divide into six for each deviation
     * 2. set deviations using day length
     * 3. increase/decrease spawn rate in each deviation (X% of customer intake)
     * 4. spawn customer
     */

    void SetDeviationInts()
    {
        d1 = sixthOfDayLength * 1;
        d2 = sixthOfDayLength * 2;
        d3 = sixthOfDayLength * 3;
        d4 = sixthOfDayLength * 4;
        d5 = sixthOfDayLength * 5;
        d6 = sixthOfDayLength * 6;
    }

    void CalculateCurrentDeviations()
    {
        if (_GM.currentTime_OpenDay < d1)
            deviations = Deviations.D1;
        else if(_GM.currentTime_OpenDay > d1 && _GM.currentTime_OpenDay < d2)
            deviations = Deviations.D2;
        else if (_GM.currentTime_OpenDay > d2 && _GM.currentTime_OpenDay < d3)
            deviations = Deviations.D3;
        else if (_GM.currentTime_OpenDay > d3 && _GM.currentTime_OpenDay < d4)
            deviations = Deviations.D4;
        else if (_GM.currentTime_OpenDay > d4 && _GM.currentTime_OpenDay < d5)
            deviations = Deviations.D5;
        else if (_GM.currentTime_OpenDay > d5 && _GM.currentTime_OpenDay < d6)
            deviations = Deviations.D6;

    }

    void SetCustomerSpawnChance()
    {
        if(deviations == Deviations.D1 || deviations == Deviations.D6)
            customerSpawnChance = 2.3f;
        else if (deviations == Deviations.D2 || deviations == Deviations.D5)
            customerSpawnChance = 13.6f;
        else if (deviations == Deviations.D3 || deviations == Deviations.D4)
            customerSpawnChance = 34.1f;

    }

    void StartSpawning()
    {
        InvokeRepeating("CalculateCustomerSpawnnChance", 0, customerSpawnRate);
    }

    void EndSpawning()
    {
        print(customersSpawned);
        CancelInvoke("CalculateCustomerSpawnnChance");
    }

    void CalculateCustomerSpawnnChance()
    {
        var r = Random.Range(0, 101);

        if(r < customerSpawnChance)
        {
            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {
        var newCustomers = Instantiate(customer, customerSpawnPoint.position, Quaternion.identity);
        customersInQueue.Add(newCustomers);
        customersInResturant.Add(newCustomers);
        customersSpawned++;
    }


    /// <summary>
    /// When a customer leaves the play area
    /// </summary>
    public void RemoveCustomer(GameObject _customer)
    {
        if (customersInQueue.Contains(_customer)) customersInQueue.Remove(_customer);
        if (customersReadyToOrder.Contains(_customer)) customersReadyToOrder.Remove(_customer);

        //check they are not on any lists
        customersInResturant.Remove(_customer);

        Destroy(_customer);
    }

    void AddResturantRating()
    {
        _GM.resturantRating += resturantRatingTotal_currentDay;
        if(_GM.resturantRating > 100) _GM.resturantRating = 100;
        if (_GM.resturantRating < 0) _GM.resturantRating = 0;
        _UI.UpdateResturantRating();

        resturantRatingTotal_currentDay = 0;
    }

    public void CalculateResturantRating(double _queueWaitTime, double _orderaTakeWaitTime, double _orderArrivalWaitTime)
    {
        //3 levels
        int customersRating = 0;

        //if served less than min time
        #region Positive Rating
        if (_queueWaitTime < minQueueWaitTime)
        {
            customersRating += 1;
        }
        if (_orderaTakeWaitTime < minOrderTakeWaitTime)
        {
            customersRating += 1;
        }
        if (_orderArrivalWaitTime < minOrderArrivalTime)
        {
            customersRating += 1;
        }

        #endregion
        //if betwneen min and max time (currently 0 but leaving here in case I want to change it
        #region Neutral Rating
        if (_queueWaitTime > minQueueWaitTime && _queueWaitTime < maxQueueWaitTime)
        {
            customersRating += 0;
        }
        if (_orderaTakeWaitTime > minOrderTakeWaitTime && _orderaTakeWaitTime < maxOrderTakeWaitTime)
        {
            customersRating += 0;
        }
        if (_orderArrivalWaitTime > minOrderArrivalTime && _orderArrivalWaitTime < maxOrderArrivalTime)
        {
            customersRating += 0;
        }

        #endregion

        //if over max time
        #region Negative Rating
        if (_queueWaitTime > maxQueueWaitTime)
        {
            customersRating -= 1;
        }
        if (_orderaTakeWaitTime > maxOrderTakeWaitTime)
        {
            customersRating -= 1;
        }
        if (_orderArrivalWaitTime > maxOrderArrivalTime)
        {
            customersRating -= 1;
        }
        #endregion

        //print("Queue WT: " + _queueWaitTime + " TakeOrder WT: " + _orderaTakeWaitTime + " OrderArrival WT: " + _orderArrivalWaitTime);
        //print("Rating of " + customersRating);

        resturantRatingTotal_currentDay += customersRating;
    }
}
