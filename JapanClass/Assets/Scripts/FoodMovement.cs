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
        //look towards corner
        transform.LookAt(conveyerbeltCorners[index].position);

        //move towards corner
        if(index < conveyerbeltCorners.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, conveyerbeltCorners[index].position, Time.deltaTime * _GM.conveyrbeltSpeedPerWave[_GM.dayCount]);

            if (Vector3.Distance(transform.position, conveyerbeltCorners[index].position) <= 0.05f)
            {
                index++;
            }
        }
        

    }
}
