using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodMovement : GameBehaviour
{
    [SerializeField]
    float foodSpeed;
    Transform[] conveyerbeltCorners;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        conveyerbeltCorners = _GM.conveyerbeltPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(index < conveyerbeltCorners.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, conveyerbeltCorners[index].position, 0.01f);

            if (Vector3.Distance(transform.position, conveyerbeltCorners[index].position) <= 0.05f)
            {
                index++;
            }
        }
        

    }
}
