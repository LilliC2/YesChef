using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float cameraSpeed;

    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        if(vertical >0)
        {
            print("go up");
            //move up
            if(transform.position.z < - 6.71)
            {
                transform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime, Space.World);

            }
        }
        else if(vertical < 0) //move down
        {
            print("go down");

            if (transform.position.z > -26) transform.Translate(Vector3.back * cameraSpeed * Time.deltaTime, Space.World);


        }

    }
}
