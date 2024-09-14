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


    FoodClass itemFoodData;

    float timeElapsed;
    float lerpDuration = 3;

    float startValue = 0;
    float endValue = 10;
    float valueToLerp;

    string workingSkill;

    Image currentImage;

    private void Start()
    {
        itemFoodData = GetComponentInParent<FoodData>().order.foodClass;

        cookingImgObj.SetActive(itemFoodData.needsCooking);
        cuttingImgObj.SetActive(itemFoodData.needsCutting);
        kneadingImgObj.SetActive(itemFoodData.needsKneading);
        mixingImgObj.SetActive(itemFoodData.needsMixing);
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
        print(transform.parent.gameObject);
        print(_FM.orderedFood_GO.IndexOf(transform.parent.gameObject));
        workingSkill = skill;
        switch(skill)
        {
            case "Cooking":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject)].GetComponent<OrderTicketUI>().progressSkillCooking_Image;
                break;
            case "Cutting":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject)].GetComponent<OrderTicketUI>().progressSkillCutting_Image;
                break;
            case "Kneading":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject)].GetComponent<OrderTicketUI>().progressSkillKneading_Image;
                break;
            case "Mixing":
                currentImage = _UI.ordersGO_List[_FM.orderedFood_GO.IndexOf(transform.parent.gameObject)].GetComponent<OrderTicketUI>().progressSkillMixing_Image;
                break;
        }

        currentImage.gameObject.SetActive(true);

    }

    public void EndFoodProgress(string skill)
    {

        switch (skill)
        {
            case "Cooking":
                itemFoodData.cookWorkComplete = true;
                break;
            case "Cutting":
                itemFoodData.cutWorkComplete = true;
                break;
            case "Kneading":
                itemFoodData.kneadedWorkComplete = true;
                break;
            case "Mixing":
                itemFoodData.mixWorkComplete = true;
                break;
        }


        //_UI.UpdateOrderProgressBar(transform.parent.gameObject);

    }


}
