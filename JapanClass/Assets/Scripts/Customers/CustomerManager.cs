using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public List<GameObject> emptyChairQueue;

    public GameObject resturantDoor;

    public GameObject customer;

    // Start is called before the first frame update
    void Start()
    {
        _GM.event_startOfDay.AddListener(SpawnCustomers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCustomers()
    {
        for (int i = 0; i < _GM.foodPerWave[_GM.dayCount]; i++)
        {
            Instantiate(customer, resturantDoor.transform.position, Quaternion.identity);
        }
    }
}
