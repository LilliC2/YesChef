using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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






    public void OrderUp(Order _order)
    {
        if(!orderedFood.Contains(_order)) orderedFood.Add(_order);
        var prefab = _order.foodPrefab;

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(prefab, _GM.conveyerbeltPoints[_GM.conveyerbeltPoints.Length-1].transform.position, Quaternion.identity);
        //print("Ordered food: " + food.name);
        foodNeedPreperation_list.Add(food);

        print("added order");

    }
}
