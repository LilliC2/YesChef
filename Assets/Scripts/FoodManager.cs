using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Order
{
    public GameObject foodPrefab;
    public GameObject customer;
}

public class FoodManager : Singleton<FoodManager>
{

    public List<Order> menu = new List<Order>();

    public List<Order> orderedFood = new List<Order>();

    public List<GameObject> foodNeedPreperation_list = new List<GameObject>();

    public List<GameObject> finishedFood_list = new List<GameObject>();
    
    public float conveyerbeltSpeed;

    public Ease foodSpawnEase;




    public void OrderUp(Order _order)
    {
        if(!orderedFood.Contains(_order)) orderedFood.Add(_order);
        var prefab = _order.foodPrefab;

        //spawn point
        var conveyorPoint = _GM.conveyerbeltPoints[Random.Range(0, _GM.conveyerbeltPoints.Length)].transform.position;

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(prefab, conveyorPoint, Quaternion.identity);
        foodNeedPreperation_list.Add(food);

        //print("Ordered food: " + food.name);

        food.GetComponent<FoodData>().order = _order;


        print("added order");

    }
}
