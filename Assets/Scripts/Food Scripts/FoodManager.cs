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

    public FoodClass foodClass;

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
    public int requiredProduce_grain; 
    public int requiredProduce_fruit; 
    public int requiredProduce_veg; 
    public int requiredProduce_dairy; 
    public int requiredProduce_protein; 

}
#endregion
public class FoodManager : Singleton<FoodManager>
{
    [Header("Produce")]
    
    public int grainTotal_produce, dairyTotal_produce, fruitTotal_produce, vegTotal_produce, protienTotal_produce;
    public int grainPrice_produce, dairyPrice_produce, fruitPrice_produce, vegPrice_produce, protienPrice_produce;

    [Header("Cooking")]

    public List<OrderClass> menu = new List<OrderClass>();
    public List<OrderClass> avalibleMenu = new List<OrderClass>();

    public List<GameObject> orderedFood_GO = new List<GameObject>();
    public List<OrderClass> orderedFood_orderClass = new List<OrderClass>();

    public List<GameObject> foodNeedPreperation_list = new List<GameObject>();

    public List<GameObject> finishedFood_list = new List<GameObject>();
    
    public float conveyerbeltSpeed;

    public Ease foodSpawnEase;

    private void Start()
    {
        _GM.event_playStateOpen.AddListener(UpdateMenuBasedOnProduce);
    }

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
        food.GetComponent<FoodData>().foodData = _order.foodClass;

        //remove produce
        dairyTotal_produce -= _order.foodClass.requiredProduce_dairy;
        protienTotal_produce -= _order.foodClass.requiredProduce_protein;
        grainTotal_produce -= _order.foodClass.requiredProduce_grain;
        fruitTotal_produce -= _order.foodClass.requiredProduce_fruit;
        vegTotal_produce -= _order.foodClass.requiredProduce_veg;
        UpdateMenuBasedOnProduce();

        //print("added order");

    }

    /// <summary>
    /// Checks if player has enough produce for menu
    /// </summary>
    public void UpdateMenuBasedOnProduce()
    {
        List < OrderClass > _menu = new();

        foreach (var item in menu)
        {
            bool hasProduceAvalible = true;

            if (dairyTotal_produce < item.foodClass.requiredProduce_dairy)
                hasProduceAvalible = false;
            
            if (grainTotal_produce < item.foodClass.requiredProduce_grain)
                hasProduceAvalible = false;
            
            if (fruitTotal_produce < item.foodClass.requiredProduce_fruit)
                hasProduceAvalible = false;
            
            if (vegTotal_produce < item.foodClass.requiredProduce_veg)
                hasProduceAvalible = false;
            
            if (protienTotal_produce < item.foodClass.requiredProduce_protein)
                hasProduceAvalible = false;

            if(hasProduceAvalible) _menu.Add(item);
        }

        avalibleMenu = _menu;

        //IF no avalible products, shut resturant down earlier
        if(avalibleMenu.Count == 0)
        {
            _GM.playState = GameManager.PlayState.Closed;
            _GM.event_playStateClose.Invoke();
        }

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
