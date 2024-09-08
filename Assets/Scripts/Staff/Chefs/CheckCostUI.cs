using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCostUI : GameBehaviour
{
    [SerializeField]
    GameObject cannotAffordOB;
    [SerializeField]
    float price;


    // Update is called once per frame
    void Update()
    {
        while(this.gameObject.activeSelf)
        {
            if (_GM.money < price) cannotAffordOB.SetActive(true);
        }
    }
}
