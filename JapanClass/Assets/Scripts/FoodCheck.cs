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
            }
            else
            {
                print("Food is uncooked");
                //lose reputation points

            }
        }
    }
}
