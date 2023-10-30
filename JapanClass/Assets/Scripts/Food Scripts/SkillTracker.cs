using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTracker : MonoBehaviour
{
    [SerializeField]
    Image cookingImg;
    [SerializeField]
    Image cuttingImg;
    [SerializeField]
    Image kneadingImg;
    [SerializeField]
    Image mixingImg;

    FoodData itemFoodData;

    private void Start()
    {
        itemFoodData = GetComponentInParent<FoodData>();
    }

    // Update is called once per frame
    void Update()
    {
        //look at camera
        transform.LookAt(Camera.main.transform.position);

        //check if has cooking
        if(cookingImg != null)
        {
            cookingImg.fillAmount = (itemFoodData.foodData.cookPrepPoints / itemFoodData.foodData.maxCookPrepPoints);
        }
        
        if(cuttingImg != null)
        {
            cuttingImg.fillAmount = (itemFoodData.foodData.cutPrepPoints / itemFoodData.foodData.maxCutPrepPoints) / 100;
        }
        
        if(kneadingImg != null)
        {
            kneadingImg.fillAmount = (itemFoodData.foodData.kneedPrepPoints / itemFoodData.foodData.maxKneedPrepPoints) / 100;
        }
        
        if(mixingImg != null)
        {
            mixingImg.fillAmount = (itemFoodData.foodData.mixPrepPoints / itemFoodData.foodData.maxMixPrepPoints) / 100;
        }
    }

    void UpdateCooking()
    {
        
    }
}
