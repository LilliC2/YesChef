using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    float range;

    // Update is called once per frame
    void Update()
    {
        range = GetComponentInParent<ChefData>().chefData.range *2;

        transform.localScale = new Vector3(range, range, range);
    }
}
