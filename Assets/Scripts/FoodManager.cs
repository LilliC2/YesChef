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

    public float conveyerbeltSpeed;

    [SerializeField]
    Vector3 startOfConveyerBelt;

    public List<GameObject> foodNeedPreperation_list;

    public List<GameObject> finishedFood_list;




    public void OrderUp(Order _order)
    {
        if(!orderedFood.Contains(_order)) orderedFood.Add(_order);
        var prefab = _order.foodPrefab;

        orderedFood.Add(_order);

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(prefab, _GM.conveyerbeltPoints[_GM.conveyerbeltPoints.Length-1].transform.position, Quaternion.identity);
        //print("Ordered food: " + food.name);
        foodNeedPreperation_list.Add(food);

        print("added order");

    }
}
