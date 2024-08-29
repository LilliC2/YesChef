using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CustomerManager : Singleton<CustomerManager>
{
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

    // Start is called before the first frame update
    void Start()
    {

        //customerQueueWaitingCheck = new bool[customerQueueSpots.Count];

        //testing, spawn customers immedately
        StartCoroutine(SpawnCustomers());
        //_GM.event_endOfDay.AddListener(SpawnCustomersEventListener);

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //void SpawnCustomersEventListener()
    //{
    //    StartCoroutine(SpawnCustomers());
    //}

    IEnumerator SpawnCustomers()
    {
        //spawn amount temp
        for (int i = 0; i < 2; i++)
        {
            var newCustomers = Instantiate(customer, customerSpawnPoint.position, Quaternion.identity);
            customersInQueue.Add(newCustomers);
            //customersList.Add(newCustomers);
            yield return new WaitForSeconds(0.2f);

        }
    }
}
