using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    public GameObject[] foodArray;

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
        var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];

        print("food coming in");
        var food = Instantiate(randomFood, startOfConveyerBelt, Quaternion.identity);
        foodInWave.Add(food);
        //food.GetComponent<FoodData>().foodData = foodArray[0];
    }
}
