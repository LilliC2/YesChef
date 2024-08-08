using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : GameBehaviour
{
    ChefClass senderChef;
    Vector3 targetPosition;
    [SerializeField] float projSpeed;
    [SerializeField] float destroysDistance;

    public void SetUp(Vector3 targetPos, ChefClass chefData)
    {
        senderChef = chefData;
        targetPosition = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        //basic movement towards target
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        transform.position += moveDir * projSpeed * Time.deltaTime;

        //exits destroy distance
        //if(Vector3.Distance(transform.position, targetPosition) < destroysDistance) 
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        print("eoe");
        if (other.gameObject.layer == _GM.rawFood)
        {
            //kneading
            if (senderChef.kneadSkill)
            {
                other.gameObject.GetComponent<FoodData>().foodData.kneedPrepPoints += senderChef.kneadEffectivness;
            }
            //cutting
            if (senderChef.cutSkill)
            {
                other.gameObject.GetComponent<FoodData>().foodData.cutPrepPoints += senderChef.cutEffectivness;
            }
            //mixing
            if (senderChef.mixSkill)
            {
                other.gameObject.GetComponent<FoodData>().foodData.mixPrepPoints += senderChef.mixEffectivness;
            }

            //cooking
            if (senderChef.cookSkill)
            {
                other.gameObject.GetComponent<FoodData>().foodData.cookPrepPoints += senderChef.cookEffectivness;
            }

            Destroy(gameObject);
        }
    }
}
