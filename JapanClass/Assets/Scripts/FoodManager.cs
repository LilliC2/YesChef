using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    public int dayCount;

    public GameObject rawfoodTemp;
    Vector3 startOfConveyerBelt;

    public List<GameObject> foodInWave;
   
    // Start is called before the first frame update
    void Start()
    {
        startOfConveyerBelt = GameObject.Find("FoodInstantiationPoint").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InstantiateFood()
    {
        print("food coming in");
        var food = Instantiate(rawfoodTemp, startOfConveyerBelt, Quaternion.identity);
        foodInWave.Add(food);
        //food.GetComponent<FoodData>().foodData = foodArray[0];
    }
}
