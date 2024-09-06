using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#region Classes
[System.Serializable]
// Order class contains any information related to customers or waiters
public class OrderClass
{
    public GameObject foodPrefab;
    public GameObject customer;
    public Sprite pfp;

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

    //produce
    public float requiredProduce_grain; 
    public float requiredProduce_fruit; 
    public float requiredProduce_veg; 
    public float requiredProduce_dairy; 
    public float requiredProduce_protein; 

}
#endregion
public class FoodManager : Singleton<FoodManager>
{
    [Header("Produce")]
    public int grainTotal_produce, diaryTotal_produce, fruitTotal_produce, vegTotal_produce, protienTotal_produce;
    public int grainPrice_produce, diaryPrice_produce, fruitPrice_produce, vegPrice_produce, protienPrice_produce;

    [Header("Cooking")]

    public List<OrderClass> menu = new List<OrderClass>();

    public List<GameObject> orderedFood_GO = new List<GameObject>();
    public List<OrderClass> orderedFood_orderClass = new List<OrderClass>();

    public List<GameObject> foodNeedPreperation_list = new List<GameObject>();

    public List<GameObject> finishedFood_list = new List<GameObject>();
    
    public float conveyerbeltSpeed;

    public Ease foodSpawnEase;

    public void OrderUp(OrderClass _order)
    {
        if(!orderedFood_orderClass.Contains(_order)) orderedFood_orderClass.Add(_order);
        var prefab = _order.foodPrefab;
        orderedFood_GO.Add(prefab);
        //spawn point
        var conveyorPoint = _GM.conveyerbeltPoints[Random.Range(0, _GM.conveyerbeltPoints.Length)].transform.position;

        //var randomFood = _GM.receipesUnlocked[Random.Range(0, _GM.receipesUnlocked.Count)];
        var food = Instantiate(prefab, conveyorPoint, Quaternion.identity);
        foodNeedPreperation_list.Add(food);

        //print("Ordered food: " + food.name);

        food.GetComponent<FoodData>().order = _order;


        //print("added order");

    }

    public void RemoveFood(GameObject _food)
    {
        if(finishedFood_list.Contains(_food)) finishedFood_list.Remove(_food);
        if(orderedFood_GO.Contains(_food)) orderedFood_GO.Remove(_food);
        Destroy(_food);
    }


    public OrderClass GetOrderClassFromFoodGameObject(GameObject _food)
    {
        int index = _FM.orderedFood_GO.IndexOf(_food) + 1;

        return orderedFood_orderClass[index];
    }
}
