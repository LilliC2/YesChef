using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCheck : GameBehaviour
{
    ParticleSystem cookedFoodPS;
    private void Start()
    {
        //cookedFoodPS = GetComponentInChildren<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            print("should start event");
            _GM.event_foodToBeServed.Invoke();

            if(!_FM.cookedFood.Contains(other.gameObject)) _FM.cookedFood.Add(other.gameObject);

            ////Check if food is raw
            //if (other.gameObject.GetComponent<FoodData>().foodData.isCooked)
            //{
            //    //Add money
            //    //print("Day: " + _GM.dayCount+1 + " Cooked Food!");

            //    //cookedFoodPS.Play();
            //    _GM.money += other.gameObject.GetComponent<FoodData>().foodData.orderCost;

            //}
            //else
            //{
            //    //print("Day: " + _GM.dayCount+1 + " Raw Food!");
            //    _GM.reputation -= other.gameObject.GetComponent<FoodData>().foodData.reputationLoss;
            //    //lose reputation points
                

            //}

            //_FM.foodInWave.Remove(other.gameObject);



            //_UI.UpdateReputationSlider();
            //_UI.UpdateMoney();

            //ExecuteAfterSeconds(1, () => Destroy(other.gameObject));
        }
    }
}
