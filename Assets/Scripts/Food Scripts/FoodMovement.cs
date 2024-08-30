using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class FoodMovement : GameBehaviour
{
    int conveyerbeltIndex;


    // Start is called before the first frame update
    void Start()
    {
        //track what index the food is currently at on the conveyerbelt
        conveyerbeltIndex = Random.Range(0, _GM.conveyerbeltPoints.Length);

        //transform.DOMove(new Vector3(-0.25f, transform.position.y, transform.position.z), 1).SetEase(_FM.foodSpawnEase);
        
    }

    
}
