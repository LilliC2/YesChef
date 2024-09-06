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
     

    // Start is called before the first frame update
    void Start()
    {

        //customerQueueWaitingCheck = new bool[customerQueueSpots.Count];

        //testing, spawn customers immedately
        StartCoroutine(SpawnCustomers());
        //_GM.event_endOfDay.AddListener(SpawnCustomersEventListener);

    }

    IEnumerator SpawnCustomers()
    {
        //spawn amount temp
        for (int i = 0; i < 1; i++)
        {
            var newCustomers = Instantiate(customer, customerSpawnPoint.position, Quaternion.identity);
            customersInQueue.Add(newCustomers);
            customersInResturant.Add(newCustomers);
            //customersList.Add(newCustomers);
            yield return new WaitForSeconds(0.2f);

        }
    }

    //int CalculateCurrentCustomerIntake()
    //{

    //}

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
