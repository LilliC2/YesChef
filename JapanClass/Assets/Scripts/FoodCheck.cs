using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCheck : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            //Check if food is raw
            if (other.gameObject.GetComponent<FoodData>().foodData.isCooked)
            {
                //Add money
                print("Food is cooked");

                _GM.money += other.gameObject.GetComponent<FoodData>().foodData.orderCost;

            }
            else
            {
                print("Food is uncooked");
                _GM.reputation -= other.gameObject.GetComponent<FoodData>().foodData.reputationLoss;
                //lose reputation points
                

            }

            _FM.foodInWave.Remove(other.gameObject);


            _UI.UpdateReputationSlider();
            _UI.UpdateMoney();

            ExecuteAfterSeconds(1, () => Destroy(other.gameObject));
        }
    }
}
