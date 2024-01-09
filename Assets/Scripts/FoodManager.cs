using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    public GameObject[] foodArray;

    Vector3 startOfConveyerBelt;

    public Vector3 destroyRawFoodPoint;

    public List<GameObject> foodInWave;

    public List<GameObject> orderedFood;
   
    public List<GameObject> queuedFood;

    // Start is called before the first frame update
    void Start()
    {
        _GM.event_endOfDay.AddListener(ResetLists);
        startOfConveyerBelt = GameObject.Find("FoodInstantiationPoint").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }


    void ResetLists()
    {
        foodInWave.Clear();
        orderedFood.Clear();
        queuedFood.Clear();
    }

    public void InstantiateFood(int _index)
    {

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(orderedFood[_index], startOfConveyerBelt, Quaternion.identity);
        //print("Ordered food: " + food.name);
        queuedFood.Add(food);
        foodInWave.Add(food);

    }
}
