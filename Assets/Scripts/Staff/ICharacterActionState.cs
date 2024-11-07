using System.Numerics;
using UnityEngine;

public interface ICharacterActionState
{
    bool IsActionActive { get; }
    bool IsHoldingItem { get; }
    bool SetAction { set; }
    GameObject OrderGameObject { get; }
    FoodClass OrderClass { get; }
    FoodData OrderData { get; }
    bool CheckOrderProgress(ChefData.WorkingOnSkill skill);

    void UpdateOrderPosition();
    void PickUpOrder();
    void PlaceOrder(UnityEngine.Vector3 position);

    void TakeCustomerOrder(CustomerData customer);


}