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

    [SerializeField]
    GameObject cookingImgObj;
    [SerializeField]
    GameObject cuttingImgObj;
    [SerializeField]
    GameObject kneadingImgObj;
    [SerializeField]
    GameObject mixingImgObj;


    FoodData itemFoodData;

    private void Start()
    {
        itemFoodData = GetComponentInParent<FoodData>();

        cookingImgObj.SetActive(itemFoodData.foodData.needsCooking);
        cuttingImgObj.SetActive(itemFoodData.foodData.needsCutting);
        kneadingImgObj.SetActive(itemFoodData.foodData.needsKneading);
        mixingImgObj.SetActive(itemFoodData.foodData.needsMixing);
    }

    // Update is called once per frame
    void Update()
    {
        //look at camera
        transform.LookAt(Camera.main.transform.position);

        //check if has cooking
        //if(cookingImg != null)
        //{
        //    if (itemFoodData.foodData.needsCooking) cookingImg.fillAmount = (itemFoodData.foodData.cookPrepPoints / itemFoodData.foodData.maxCookPrepPoints);
        //    else cookingImg.fillAmount = 0;
        //}

        //if (cuttingImg != null)
        //{
        //    if (itemFoodData.foodData.needsCutting) cuttingImg.fillAmount = (itemFoodData.foodData.cutPrepPoints / itemFoodData.foodData.maxCutPrepPoints);
        //    else cuttingImg.fillAmount = 0;
        //}
        
        //if(kneadingImg != null)
        //{
        //    if (itemFoodData.foodData.needsKneading) kneadingImg.fillAmount = (itemFoodData.foodData.kneedPrepPoints / itemFoodData.foodData.maxKneedPrepPoints);
        //    else kneadingImg.fillAmount = 0;

        //}

        //if (mixingImg != null)
        //{
        //    if (itemFoodData.foodData.needsMixing) mixingImg.fillAmount = (itemFoodData.foodData.mixPrepPoints / itemFoodData.foodData.maxMixPrepPoints);
        //    else mixingImg.fillAmount = 0;
        //}
    }

}
