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

                _GM.money += other.gameObject.GetComponent<FoodData>().foodData.value;

            }
            else
            {
                print("Food is uncooked");
                _GM.reputation -= other.gameObject.GetComponent<FoodData>().foodData.reputationLoss;
                //lose reputation points

            }

            ExecuteAfterSeconds(1, () => Destroy(other.gameObject));
        }
    }
}
