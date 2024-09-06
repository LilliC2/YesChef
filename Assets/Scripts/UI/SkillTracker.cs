using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTracker : GameBehaviour
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

    float timeElapsed;
    float lerpDuration = 3;

    float startValue = 0;
    float endValue = 10;
    float valueToLerp;

    string workingSkill;

    Image currentImage;

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

        if(currentImage != null)
        {
            currentImage.fillAmount = valueToLerp;

            if (timeElapsed < lerpDuration)
            {
                valueToLerp = Mathf.Lerp(0, 1, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;

            }
            else
            {
                valueToLerp = 1;
                EndFoodProgress(workingSkill);
                currentImage = null;
                workingSkill = null;
            }

        }
      
    }

    public void StartFoodProgress(string skill, float duration)
    {
        workingSkill = skill;
        switch(skill)
        {
            case "Cooking":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject)+1].GetComponent<OrderTicketUI>().progressSkillCooking_Image;
                break;
            case "Cutting":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject) + 1].GetComponent<OrderTicketUI>().progressSkillCutting_Image;
                break;
            case "Kneading":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject) + 1].GetComponent<OrderTicketUI>().progressSkillKneading_Image;
                break;
            case "Mixing":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject) + 1].GetComponent<OrderTicketUI>().progressSkillMixing_Image;
                break;
        }

        currentImage.gameObject.SetActive(true);

    }

    public void EndFoodProgress(string skill)
    {

        var foodData = itemFoodData.foodData;
        switch (skill)
        {
            case "Cooking":
                foodData.cookWorkComplete = true;
                break;
            case "Cutting":
                foodData.cutWorkComplete = true;
                break;
            case "Kneading":
                foodData.kneadedWorkComplete = true;
                break;
            case "Mixing":
                foodData.mixWorkComplete = true;
                break;
        }


        //_UI.UpdateOrderProgressBar(transform.parent.gameObject);

    }


}
