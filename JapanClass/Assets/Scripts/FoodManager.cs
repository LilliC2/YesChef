using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : GameBehaviour
{
    public int dayCount;

    public GameObject rawfoodTemp;
    Vector3 startOfConveyerBelt;

   
    // Start is called before the first frame update
    void Start()
    {
        startOfConveyerBelt = GameObject.Find("FoodInstantiationPoint").transform.position;
        InstantiateFood();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateFood()
    {
        var food = Instantiate(rawfoodTemp, startOfConveyerBelt, Quaternion.identity);
        //food.GetComponent<FoodData>().foodData = foodArray[0];
    }
}
