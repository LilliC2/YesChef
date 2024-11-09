using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChefData;
using DG.Tweening;

public class CharacterActionState : GameBehaviour, ICharacterActionState
{
    GameObject orderGO;
    Transform holdOrderTransform;
    FoodData foodData;
    FoodClass foodClass;
    bool action;
    bool isHoldingOrder;


    public CharacterActionState(GameObject orderGO,
                                Transform holdOrderTransform,
                                FoodData orderData,
                                FoodClass orderClass)
    {
        this.orderGO = orderGO;
        this.holdOrderTransform = holdOrderTransform;
        this.foodData = orderData;
        this.foodClass = orderClass;
    }

    public bool IsActionActive {  get { return action; } }
    public bool SetAction { set { action = value; } }
    public bool IsHoldingItem { get { return isHoldingOrder; } }

    public GameObject OrderGameObject { get { return orderGO; } }
    public FoodData OrderData { get {  return foodData; } }
    public FoodClass OrderClass { get {  return foodClass; } }
    //check food progress
    public bool CheckOrderProgress(WorkingOnSkill skill)
    {
        bool isCurrentWorkComplete = false;
        //has chef completed current work
        switch (skill)
        {
            case WorkingOnSkill.Cooking:
                isCurrentWorkComplete = foodClass.cookWorkComplete;
                break;
            case WorkingOnSkill.Mixing:
                isCurrentWorkComplete = foodClass.mixWorkComplete;
                break;
            case WorkingOnSkill.Cutting:
                isCurrentWorkComplete = foodClass.cutWorkComplete;
                break;
            case WorkingOnSkill.Kneading:
                isCurrentWorkComplete = foodClass.kneadedWorkComplete;
                break;

        }

        return isCurrentWorkComplete;

    }

    //pick up food
    public void PickUpOrder()
    {
        isHoldingOrder = true;
        orderGO.transform.position = holdOrderTransform.position;
    }
    public void UpdateOrderPosition()
    {
        if(isHoldingOrder)
            orderGO.transform.position = holdOrderTransform.position;
    }

    public void PlaceOrder(Vector3 position)
    {
        isHoldingOrder = false;
        orderGO.transform.position = position;
    }

    public void TakeCustomerOrder(CustomerData customer)
    {
        customer.beingAttened = true;
        _FM.OrderUp(customer.order, customer.gameObject);
        _UI.AddOrder(customer.orderClass);
    }


}
