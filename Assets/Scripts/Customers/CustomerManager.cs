using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CustomerManager : Singleton<CustomerManager>
{
    public List<GameObject> emptyChairQueue;

    public List<GameObject> customersList;
    public List<GameObject> customerQueueSpots;
    public List<GameObject> customerQueueWaiting;
    public bool[] customerQueueWaitingCheck;

    public Sprite happyCustomer;
    public Sprite sadCustomer;

    public GameObject resturantDoor;

    public GameObject customer;

    public UnityEvent event_newSeatAvalible;

    // Start is called before the first frame update
    //void Start()
    //{

    //    customerQueueWaitingCheck = new bool[customerQueueSpots.Count];
    //    StartCoroutine(SpawnCustomers());
    //    _GM.event_endOfDay.AddListener(SpawnCustomersEventListener);
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //void SpawnCustomersEventListener()
    //{
    //    StartCoroutine(SpawnCustomers());
    //}

    //IEnumerator SpawnCustomers()
    //{
    //    for (int i = 0; i < _GM.foodPerWave[_GM.dayCount]; i++)
    //    {
    //        var newCustomers = Instantiate(customer, resturantDoor.transform.position, Quaternion.identity);
    //        customersList.Add(newCustomers);
    //        yield return new WaitForSeconds(0.2f);

    //    }
    //}
}
