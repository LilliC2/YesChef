using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    public GameObject[] foodArray;

    Vector3 startOfConveyerBelt;

    public List<GameObject> foodInWave;

    public List<GameObject> orderedFood;
   
    // Start is called before the first frame update
    void Start()
    {
        startOfConveyerBelt = GameObject.Find("FoodInstantiationPoint").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InstantiateFood(int _index)
    {
        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        GameObject food = null;
        ExecuteAfterFrames(1,() => food = Instantiate(orderedFood[_index], startOfConveyerBelt, Quaternion.identity));
        //print("Ordered food: " + food.name);
        ExecuteAfterFrames(1, () =>  foodInWave.Add(food));
    }
}
