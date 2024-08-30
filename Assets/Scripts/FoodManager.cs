using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#region Classes
[System.Serializable]
// Order class contains any information related to customers or waiters
public class Order
{
    public GameObject foodPrefab;
    public GameObject customer;

    //for waiter
    public bool isComplete;
    public bool isBeingPickedUp;

    //for customers
    public float orderCost;
    public float eatTime;
}


[System.Serializable]
// Food class contains any information related to the player or the chefs
public class FoodClass
{
    //for player
    public string name;
    public string description;
    public Sprite pfp;

    public float unlockCost;
    public float reputationLoss;

    //for chefs
    public bool needsKneading;
    public float kneadWorkTime;
    public bool kneadedWorkComplete;

    public bool needsCutting;
    public float cutWorkTime;
    public bool cutWorkComplete;

    public bool needsCooking;
    public float cookWorkTime;
    public bool cookWorkComplete;

    public bool needsMixing;
    public float mixWorkTime;
    public bool mixWorkComplete;

}
#endregion
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
