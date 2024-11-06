using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChefData;
using DG.Tweening;

public class CharacterActionState : GameBehaviour, ICharacterActionState
{
    GameObject orderGO;
    Transform holdOrderTransform;
    FoodData orderData;
    FoodClass orderClass;
    bool action;
    bool isHoldingOrder;


    public CharacterActionState(GameObject orderGO,
                                Transform holdOrderTransform,
                                FoodData orderData,
                                FoodClass orderClass)
    {
        this.orderGO = orderGO;
        this.holdOrderTransform = holdOrderTransform;
        this.orderData = orderData;
        this.orderClass = orderClass;
    }

    public bool IsActionActive {  get { return action; } }
    public bool SetAction { set { action = value; } }

    public GameObject OrderGameObject { get { return orderGO; } }
    public FoodData OrderData { get {  return orderData; } }
    public FoodClass OrderClass { get {  return orderClass; } }
    //check food progress
    public bool CheckOrderProgress(WorkingOnSkill skill)
    {
        bool isCurrentWorkComplete = false;
        //has chef completed current work
        switch (skill)
        {
            case WorkingOnSkill.Cooking:
                isCurrentWorkComplete = orderClass.cookWorkComplete;
                break;
            case WorkingOnSkill.Mixing:
                isCurrentWorkComplete = orderClass.mixWorkComplete;
                break;
            case WorkingOnSkill.Cutting:
                isCurrentWorkComplete = orderClass.cutWorkComplete;
                break;
            case WorkingOnSkill.Kneading:
                isCurrentWorkComplete = orderClass.kneadedWorkComplete;
                break;

        }

        return isCurrentWorkComplete;

    }

    //pick up food
    public void PickUpOrder()
    {
        isHoldingOrder = true;
        orderGO.transform.DOMove(holdOrderTransform.position,1);
    }
    public void UpdateOrderPosition()
    {
        if(isHoldingOrder)
            orderGO.transform.position = holdOrderTransform.position;
    }

    public void PlaceOrder(Vector3 position)
    {
        isHoldingOrder = false;
        orderGO.transform.DOMove(position, 1);
    }

    public void TakeCustomerOrder(CustomerData customer)
    {
        customer.beingAttened = true;
        _FM.OrderUp(customer.order, customer.gameObject);
        _UI.AddOrder(customer.orderClass);
    }


}
