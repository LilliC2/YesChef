using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : Singleton<FoodManager>
{
    public GameObject[] foodArray;

    public float conveyerbeltSpeed;

    [SerializeField]
    Vector3 startOfConveyerBelt;

    public List<GameObject> foodNeedPreperation_list;

    public List<GameObject> finishedFood_list;


    public void InstantiateFood(GameObject orderedFood)
    {

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(orderedFood, _GM.conveyerbeltPoints[_GM.conveyerbeltPoints.Length-1].transform.position, Quaternion.identity);
        //print("Ordered food: " + food.name);
        foodNeedPreperation_list.Add(food);

    }
}
