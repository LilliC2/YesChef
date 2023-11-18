using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Tutorial UI")]
    public GameObject recipeCheckOB;
    bool recipeBought;

    [Header("Pause")]
    public GameObject pausePanel;
    bool pause;
    public TMP_Text autoplayButtonTxt;

    [Header("HUD")]
    public TMP_Text dayCount;
    public TMP_Text moneyCount;
    public Slider reputationSlider;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("Chef UI")]
    public GameObject chefMenu;
    public GameObject chefMenuButton;

    [Header("Chef 0")]
    public TMP_Text nameChef0;
    public Image pfpChef0;
    public TMP_Text costChef0;
    public TMP_Text cookingSkillChef0;
    public TMP_Text kneedingSkillChef0;
    public TMP_Text cuttingSkillChef0;
    public TMP_Text mixingSkillChef0;
    public GameObject cannotAffordChef0;
    
    [Header("Chef 1")]
    public TMP_Text nameChef1;
    public Image pfpChef1;
    public TMP_Text costChef1;
    public TMP_Text cookingSkillChef1;
    public TMP_Text kneedingSkillChef1;
    public TMP_Text cuttingSkillChef1;
    public TMP_Text mixingSkillChef1;
    public GameObject cannotAffordChef1;
    
    [Header("Chef 2")]
    public TMP_Text nameChef2;
    public Image pfpChef2;
    public TMP_Text costChef2;
    public TMP_Text cookingSkillChef2;
    public TMP_Text kneedingSkillChef2;
    public TMP_Text cuttingSkillChef2;
    public TMP_Text mixingSkillChef2;
    public GameObject cannotAffordChef2;
    
    [Header("Chef 3")]
    public TMP_Text nameChef3;
    public Image pfpChef3;
    public TMP_Text costChef3;
    public TMP_Text cookingSkillChef3;
    public TMP_Text kneedingSkillChef3;
    public TMP_Text cuttingSkillChef3;
    public TMP_Text mixingSkillChef3;
    public GameObject cannotAffordChef3;

    [Header("Chef PopUP UI")]
    public GameObject chefPopUp;
    public GameObject selectedChef;
    public Image chefPopUpPFP;
    public TMP_Text chefPopUpName;
    public TMP_Text cookingSkillChefchefPopUp;
    public TMP_Text kneedingSkillChefchefPopUp;
    public TMP_Text cuttingSkillChefchefPopUp;
    public TMP_Text mixingSkillChefchefPopUp;
    public Image rangeSlider1;
    public Image rangeSlider2;

    [Header("Receipe UI")]
    public GameObject receipeMenu;
    public GameObject receipeMenuButton;

    [Header("Receipe 0")]
    public TMP_Text nameReceipe0;
    public Image pfpReceipe0;
    public TMP_Text orderCostReceipe0;
    public TMP_Text unlockCostReceipe0;
    public TMP_Text repLossReceipe0;
    public TMP_Text cookingPointsReceipe0;
    public TMP_Text kneedingPointsReceipe0;
    public TMP_Text cuttingPointsReceipe0;
    public TMP_Text mixingPointsReceipe0;

    public GameObject cannotAffordReceipe0;
    public GameObject soldReceipe0;

    [Header("Receipe 1")]
    public TMP_Text nameReceipe1;
    public Image pfpReceipe1;
    public TMP_Text orderCostReceipe1;
    public TMP_Text unlockCostReceipe1;
    public TMP_Text repLossReceipe1;
    public TMP_Text cookingPointsReceipe1;
    public TMP_Text kneedingPointsReceipe1;
    public TMP_Text cuttingPointsReceipe1;
    public TMP_Text mixingPointsReceipe1;

    public GameObject cannotAffordReceipe1;
    public GameObject soldReceipe1;

    [Header("Receipe 2")]
    public TMP_Text nameReceipe2;
    public Image pfpReceipe2;
    public TMP_Text orderCostReceipe2;
    public TMP_Text unlockCostReceipe2;
    public TMP_Text repLossReceipe2;
    public TMP_Text cookingPointsReceipe2;
    public TMP_Text kneedingPointsReceipe2;
    public TMP_Text cuttingPointsReceipe2;
    public TMP_Text mixingPointsReceipe2;

    public GameObject cannotAffordReceipe2;
    public GameObject soldReceipe2;
    
    [Header("Receipe 3")]
    public TMP_Text nameReceipe3;
    public Image pfpReceipe3;
    public TMP_Text orderCostReceipe3;
    public TMP_Text unlockCostReceipe3;
    public TMP_Text repLossReceipe3;
    public TMP_Text cookingPointsReceipe3;
    public TMP_Text kneedingPointsReceipe3;
    public TMP_Text cuttingPointsReceipe3;
    public TMP_Text mixingPointsReceipe3;

    public GameObject cannotAffordReceipe3;
    public GameObject soldReceipe3;


    private void Start()
    {
        LoadChefData();

        LoadReceipeData();
    }

    public void UpdateDay()
    {
        dayCount.text = "Day " + (_GM.dayCount +1).ToString();
    }
    
    public void UpdateMoney()
    {
        moneyCount.text = "$" + _GM.money.ToString("F2");
    }

    public void UpdateReputationSlider()
    {
        reputationSlider.value = _GM.reputation;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


    public void Pause()
    {
        pause = !pause;
        print("clicking dat but");
        if(pause)
        {
            Time.timeScale = 0;
            _GM.gameState = GameManager.GameState.Pause;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _GM.gameState = GameManager.GameState.Playing;
            pausePanel.SetActive(false);
        }
    }

    public void OpenChefMenu()
    {

        chefMenu.SetActive(true);
        CheckWhatPlayerCanAffordChefs();
        receipeMenu.SetActive(false);

    }

    public void CloseChefMenu()
    {
        chefMenu.SetActive(false);
    }
    
    public void OpenReceipeMenu()
    {
        receipeMenu.SetActive(true);
        chefMenu.SetActive(false);
        CheckWhatPlayerCanAffordReceipes();
    }

    public void CloseReceipeMenu()
    {
        receipeMenu.SetActive(false);
    }

    public void OpenChefPopUp(GameObject _chefData)
    {

        selectedChef = _chefData;
        chefPopUp.SetActive(true);

        var chefData = _chefData.GetComponent<ChefData>().chefData;

        //chefPopUpName.text = chefData.name;
        chefPopUpPFP.sprite = chefData.pfp;
        cookingSkillChefchefPopUp.text = chefData.cookEffectivness.ToString();
        cuttingSkillChefchefPopUp.text = chefData.cutEffectivness.ToString();
        mixingSkillChefchefPopUp.text = chefData.mixEffectivness.ToString();
        kneedingSkillChefchefPopUp.text = chefData.kneedEffectivness.ToString();

        rangeSlider1.fillAmount = chefData.range / 100;
        rangeSlider2.fillAmount = chefData.range / 100;
    }

    public void CloseChefPopUp()
    {
        selectedChef = null;
        chefPopUp.SetActive(false);
    }

    public void FireChef()
    {
        //give money back to player
        _GM.money += selectedChef.GetComponent<ChefData>().chefData.hireCost/2;

        UpdateMoney();

        print("Destroy him");
        //destroy chef
        Destroy(selectedChef);

        CloseChefPopUp();
    }

    public void PlayerReady()
    {
        if (!recipeBought) CheckForRecipes();
        else
        {
            _GM.playerReady = true;

            Time.timeScale = 1;
        }


    }

    public void SpeedUp()
    {
        if (!recipeBought) CheckForRecipes();
        else
        {
            _GM.playerReady = true;
            Time.timeScale = 5;
        }
            

    }

    public void AutoPlayButton()
    {
        _GM.autoPlayEnabled = !_GM.autoPlayEnabled;

        if (_GM.autoPlayEnabled) autoplayButtonTxt.text = "Auto Play Enabled";
        if (!_GM.autoPlayEnabled) autoplayButtonTxt.text = "Auto Play Disabled";
    }

    public void BuyChef(int _arrayNum)
    {
        chefMenu.SetActive(false);

        var chefToBuy = _CM.chefArray[_arrayNum];

        if (chefToBuy.GetComponent<ChefData>().chefData.hireCost <= _GM.money)
        {
            _CM.CreateNewChef(chefToBuy);

            CheckWhatPlayerCanAffordChefs();
        }

    }


    public void BuyReceipe(int _arrayNum)
    {
        if (!_GM.activeWave)
        {
            //check if receipe is bought

            var receipeToBuy = _FM.foodArray[_arrayNum];
            if (!_GM.receipesUnlocked.Contains(receipeToBuy))
            { 
                if (receipeToBuy.GetComponent<FoodData>().foodData.unlockCost <= _GM.money)
                {
                    _GM.receipesUnlocked.Add(receipeToBuy);

                    _GM.money -= receipeToBuy.GetComponent<FoodData>().foodData.unlockCost;

                    UpdateMoney();

                    CheckWhatPlayerCanAffordReceipes();
                }
            }
        }
    }

    public void LoadChefData()
    {
        nameChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.name;
        pfpChef0.sprite = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.pfp;
        cookingSkillChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.cookEffectivness.ToString();
        kneedingSkillChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.kneedEffectivness.ToString();
        mixingSkillChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.mixEffectivness.ToString();
        cuttingSkillChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.cutEffectivness.ToString();
        costChef0.text = "$" + _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.hireCost.ToString();

        nameChef1.text = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.name;
        pfpChef1.sprite = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.pfp;
        cookingSkillChef1.text = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.cookEffectivness.ToString();
        kneedingSkillChef1.text = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.kneedEffectivness.ToString();
        mixingSkillChef1.text = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.mixEffectivness.ToString();
        cuttingSkillChef1.text = _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.cutEffectivness.ToString();
        costChef1.text = "$" + _CM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.hireCost.ToString();

        nameChef2.text = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.name;
        pfpChef2.sprite = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.pfp;
        cookingSkillChef2.text = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.cookEffectivness.ToString();
        kneedingSkillChef2.text = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.kneedEffectivness.ToString();
        mixingSkillChef2.text = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.mixEffectivness.ToString();
        cuttingSkillChef2.text = _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.cutEffectivness.ToString();
        costChef2.text = "$" + _CM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.hireCost.ToString();

        nameChef3.text = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.name;
        pfpChef3.sprite = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.pfp;
        cookingSkillChef3.text = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.cookEffectivness.ToString();
        kneedingSkillChef3.text = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.kneedEffectivness.ToString();
        mixingSkillChef3.text = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.mixEffectivness.ToString();
        cuttingSkillChef3.text = _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.cutEffectivness.ToString();
        costChef3.text = "$" + _CM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.hireCost.ToString();
    }

    public void LoadReceipeData()
    {
        nameReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.name;
        orderCostReceipe0.text = "Cost: $"+_FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.orderCost.ToString("F2");
        unlockCostReceipe0.text = "Unlock: $"+_FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.unlockCost.ToString("F2");
        repLossReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.reputationLoss.ToString();
        cookingPointsReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.maxCookPrepPoints.ToString();
        mixingPointsReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.maxMixPrepPoints.ToString();
        kneedingPointsReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.maxCutPrepPoints.ToString();
        pfpReceipe0.sprite = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.pfp;



        nameReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.name;
        orderCostReceipe1.text = "Cost: $" + _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.orderCost.ToString("F2");
        unlockCostReceipe1.text = "Unlock: $" + _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.unlockCost.ToString("F2");
        repLossReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.reputationLoss.ToString();
        cookingPointsReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.maxCookPrepPoints.ToString();
        mixingPointsReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.maxMixPrepPoints.ToString();
        kneedingPointsReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe1.text = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.maxCutPrepPoints.ToString();
        pfpReceipe1.sprite = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.pfp;

        nameReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.name;
        orderCostReceipe2.text = "Cost: $" + _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.orderCost.ToString("F2");
        unlockCostReceipe2.text = "Unlock: $" + _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.unlockCost.ToString("F2");
        repLossReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.reputationLoss.ToString();
        cookingPointsReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.maxCookPrepPoints.ToString();
        mixingPointsReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.maxMixPrepPoints.ToString();
        kneedingPointsReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe2.text = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.maxCutPrepPoints.ToString();
        pfpReceipe2.sprite = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.pfp;

        nameReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.name;
        orderCostReceipe3.text = "Cost: $" + _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.orderCost.ToString("F2");
        unlockCostReceipe3.text = "Unlock: $" + _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.unlockCost.ToString("F2");
        repLossReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.reputationLoss.ToString();
        cookingPointsReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.maxCookPrepPoints.ToString();
        mixingPointsReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.maxMixPrepPoints.ToString();
        kneedingPointsReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe3.text = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.maxCutPrepPoints.ToString();
        pfpReceipe3.sprite = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.pfp;

    }

    public void CheckWhatPlayerCanAffordChefs()
    {
        if(_GM.money < _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef0.SetActive(true);
        }
        else cannotAffordChef0.SetActive(false);
    }

    public void CheckWhatPlayerCanAffordReceipes()
    {
        if(_GM.receipesUnlocked.Contains(_FM.foodArray[0]))
        {
            soldReceipe0.SetActive(true);
        }
        else
        {
            if (_GM.money < _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.unlockCost)
            {
                cannotAffordReceipe0.SetActive(true);
            }
            else cannotAffordReceipe0.SetActive(false);
        }
        
        if(_GM.receipesUnlocked.Contains(_FM.foodArray[1]))
        {
            soldReceipe1.SetActive(true);
        }
        else
        {
            if (_GM.money < _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData.unlockCost)
            {
                cannotAffordReceipe1.SetActive(true);
            }
            else cannotAffordReceipe1.SetActive(false);
        }
        
        if(_GM.receipesUnlocked.Contains(_FM.foodArray[2]))
        {
            soldReceipe1.SetActive(true);
        }
        else
        {
            if (_GM.money < _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData.unlockCost)
            {
                cannotAffordReceipe2.SetActive(true);
            }
            else cannotAffordReceipe2.SetActive(false);
        }
        
        if(_GM.receipesUnlocked.Contains(_FM.foodArray[3]))
        {
            soldReceipe1.SetActive(true);
        }
        else
        {
            if (_GM.money < _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.unlockCost)
            {
                cannotAffordReceipe3.SetActive(true);
            }
            else cannotAffordReceipe3.SetActive(false);
        }

        
    }

    public void CheckForRecipes()
    {
        if (_GM.receipesUnlocked.Count == 0)
        {
            print("no recipes bought");
            recipeCheckOB.SetActive(true);

            ExecuteAfterSeconds(3, () => recipeCheckOB.SetActive(false));
        }
        else recipeBought = true;
    }
}
