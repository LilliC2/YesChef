using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header("Tutorial")]
    public GameObject tutorialMainPanel;
    public GameObject[] tutorialPanels;
    bool inTutotial;

    [Header("Pause")]
    public GameObject pausePanel;
    bool pause;
    public TMP_Text autoplayButtonTxt;

    [Header("HUD")]
    public TMP_Text dayCount;
    public TMP_Text moneyCount;
    public Slider reputationSlider;
    [SerializeField]
    Sprite[] tanukiHandleSlider;

    [SerializeField]
    Color highlightedColour;

    [Header("Camera Controls")]
    [SerializeField]
    Vector3 kitchenCamPos;
    [SerializeField]
    Vector3 kitchenCamRot;
    [SerializeField]
    Vector3 resturantCamPos;
    [SerializeField]
    Vector3 resturantCamRot;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public GameObject completePanel;

    [Header("Waiter UI")]
    public GameObject waiterMenu;
    public GameObject waiterMenuButton;

    [Header("Waiter 0")]
    public TMP_Text nameWaiter0;
    public Image pfpWaiter0;
    public TMP_Text costWaiter0;
    public TMP_Text strengthSkillWaiter0;
    public TMP_Text speedSkillWaiter0;
    public GameObject cannotAffordWaiter0;

    [Header("Chef UI")]
    public GameObject chefMenu;
    public GameObject chefMenuButton;
    #region Chefs
    [Header("Chef 0")]
    public TMP_Text nameChef0;
    public Image pfpChef0;
    public TMP_Text costChef0;
    public TMP_Text cookingSkillChef0;
    public TMP_Text kneedingSkillChef0;
    public TMP_Text cuttingSkillChef0;
    public TMP_Text mixingSkillChef0;
    public GameObject cannotAffordChef0;
    public Image[] skillImagesChef0;


    [Header("Chef 1")]
    public TMP_Text nameChef1;
    public Image pfpChef1;
    public TMP_Text costChef1;
    public TMP_Text cookingSkillChef1;
    public TMP_Text kneedingSkillChef1;
    public TMP_Text cuttingSkillChef1;
    public TMP_Text mixingSkillChef1;
    public GameObject cannotAffordChef1;
    public Image[] skillImagesChef1;


    [Header("Chef 2")]
    public TMP_Text nameChef2;
    public Image pfpChef2;
    public TMP_Text costChef2;
    public TMP_Text cookingSkillChef2;
    public TMP_Text kneedingSkillChef2;
    public TMP_Text cuttingSkillChef2;
    public TMP_Text mixingSkillChef2;
    public GameObject cannotAffordChef2;
    public Image[] skillImagesChef2;


    [Header("Chef 3")]
    public TMP_Text nameChef3;
    public Image pfpChef3;
    public TMP_Text costChef3;
    public TMP_Text cookingSkillChef3;
    public TMP_Text kneedingSkillChef3;
    public TMP_Text cuttingSkillChef3;
    public TMP_Text mixingSkillChef3;
    public GameObject cannotAffordChef3;
    public Image[] skillImagesChef3;


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
    #endregion
    [Header("Receipe UI")]
    public GameObject receipeMenu;
    public GameObject receipeMenuButton;
    #region Receipes
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
    public Image[] skillImagesReceipe0;


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
    public Image[] skillImagesReceipe1;


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
    public Image[] skillImagesReceipe2;


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
    public Image[] skillImagesReceipe3;

    public GameObject cannotAffordReceipe3;
    public GameObject soldReceipe3;

    #endregion
    private void Start()
    {
        //ensure it starts with tutorial
        tutorialMainPanel.SetActive(true);
        tutorialPanels[0].SetActive(true);
        inTutotial = true;
        LoadChefData();
        LoadWaiterData();
        LoadReceipeData();

        _GM.event_endOfDay.AddListener(UpdateDay);

    }

    private void Update()
    {
        //temp\
        if (Input.GetKeyDown(KeyCode.Space)) LookAtKitchen();
        if (Input.GetKeyDown(KeyCode.K)) LookAtResturant();

        if(inTutotial)
        {
            //check for if chef is placed
            if (!tutorialMainPanel.activeSelf && Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextTutorialPanel(5);
                CloseChefMenu();
                ExecuteAfterSeconds(0.3f,() => tutorialMainPanel.SetActive(true));

            }
        }
    }

    public void UpdateDay()
    {
        dayCount.text = "Day " + (_GM.dayCount +1).ToString();
    }
    
    public void UpdateMoney()
    {
        moneyCount.text = "¥" + _GM.money.ToString("F2");
        CheckWhatPlayerCanAffordChefs();
        CheckWhatPlayerCanAffordReceipes();
    }

    public void UpdateReputationSlider()
    {
        reputationSlider.value = _GM.reputation;

        switch(_GM.reputation)
        {
            case > 80:
                reputationSlider.fillRect.GetComponent<Image>().color = Color.green;
                reputationSlider.handleRect.GetComponent<Image>().sprite = tanukiHandleSlider[0];
                break;
            case > 60:
                reputationSlider.fillRect.GetComponent<Image>().color = Color.white;
                reputationSlider.handleRect.GetComponent<Image>().sprite = tanukiHandleSlider[1];
                break;
            case > 40:
                reputationSlider.handleRect.GetComponent<Image>().sprite = tanukiHandleSlider[2];
                reputationSlider.fillRect.GetComponent<Image>().color = Color.yellow;

                break;
            case > 20:
                reputationSlider.handleRect.GetComponent<Image>().sprite = tanukiHandleSlider[3];

                break;
            case < 20:
                reputationSlider.handleRect.GetComponent<Image>().sprite = tanukiHandleSlider[4];   
                reputationSlider.fillRect.GetComponent<Image>().color = Color.red;

                break;
               
        }
    }

    #region Tutorial

    public void CloseTutorial()
    {
        inTutotial = false;

        tutorialMainPanel.SetActive(false);
    }

    public void NextTutorialPanel(int panel)
    {
        tutorialPanels[panel].SetActive(false);
        tutorialPanels[panel + 1].SetActive(true);

    }
    public void PreviousTutorialPanel(int panel)
    {
        tutorialPanels[panel].SetActive(false);
        tutorialPanels[panel - 1].SetActive(true);

    }


    public void BuyTanuki()
    {
        tutorialMainPanel.SetActive(false);
        BuyChef(1);
    }
    #endregion


    #region Menus
    public void Pause()
    {
        pause = !pause;
        if(pause)
        {
            Time.timeScale = 0;
            _GM.gameState = GameManager.GameState.Pause;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = _GM.currentTimeScale;
            _GM.gameState = GameManager.GameState.Playing;
            pausePanel.SetActive(false);
        }
    }

    public void OpenChefMenu()
    {

        chefMenu.SetActive(true);
        CheckWhatPlayerCanAffordChefs();
        receipeMenu.SetActive(false);
        waiterMenu.SetActive(false);

    }

    public void CloseChefMenu()
    {
        chefMenu.SetActive(false);
    }

    public void OpenWaiterMenu()
    {
        waiterMenu.SetActive(true);
        CheckWhatPlayerCanAffordWaiters();
        receipeMenu.SetActive(false);
        chefMenu.SetActive(false);
    }

    public void CloseWaiterMenu()
    {
        waiterMenu.SetActive(false);

    }

    public void OpenReceipeMenu()
    {
        receipeMenu.SetActive(true);
        chefMenu.SetActive(false);
        waiterMenu.SetActive(false);
        CheckWhatPlayerCanAffordReceipes();
    }

    public void CloseReceipeMenu()
    {
        receipeMenu.SetActive(false);
    }

    #endregion

    #region Chef Pop Up
    public void OpenChefPopUp(GameObject _chefData)
    {

        selectedChef = _chefData;
        chefPopUp.SetActive(true);

        var chefData = _chefData.GetComponent<ChefData>().chefData;

        selectedChef.GetComponent<ChefData>().rangeIndicator.SetActive(true);

        //chefPopUpName.text = chefData.name;
        chefPopUpPFP.sprite = chefData.pfp;
        cookingSkillChefchefPopUp.text = chefData.cookEffectivness.ToString();
        cuttingSkillChefchefPopUp.text = chefData.cutEffectivness.ToString();
        mixingSkillChefchefPopUp.text = chefData.mixEffectivness.ToString();
        kneedingSkillChefchefPopUp.text = chefData.kneedEffectivness.ToString();

        rangeSlider1.fillAmount = chefData.range / 10;
        rangeSlider2.fillAmount = chefData.range / 10;
    }

    public void CloseChefPopUp()
    {
        selectedChef = null;
        selectedChef.GetComponent<ChefData>().rangeIndicator.SetActive(false);

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

    #endregion

    #region Buttons


    public void LookAtKitchen()
    {
        Camera.main.transform.DOMove(kitchenCamPos, 1);
        Camera.main.transform.DORotate(kitchenCamRot, 1);
    }

    public void LookAtResturant()
    {
        Camera.main.transform.DOMove(resturantCamPos, 1);
        Camera.main.transform.DORotate(resturantCamRot, 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayerReady()
    {
        _GM.playerReady = true;

        if (_GM.currentTimeScale != 1)
        {
            _GM.currentTimeScale = 1;
            _AM.slowDown.Play();

        }
        Time.timeScale = _GM.currentTimeScale;

    }

    public void SpeedUp()
    {
        _GM.playerReady = true;
        if(_GM.currentTimeScale != 2)
        {
            _GM.currentTimeScale = 2;
            _AM.speedUp.Play();

        }
        Time.timeScale = _GM.currentTimeScale;

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

        var chefToBuy = _ChefM.chefArray[_arrayNum];

        if (chefToBuy.GetComponent<ChefData>().chefData.hireCost <= _GM.money)
        {
            _AM.successfulPurchase.Play();

            _ChefM.CreateNewChef(chefToBuy);

            CheckWhatPlayerCanAffordChefs();
        }

    }

    public void BuyWaiter(int _arrayNum)
    {
        waiterMenu.SetActive(false);

        var waiterToBuy = _WM.waiterArray[_arrayNum];

        if (waiterToBuy.GetComponent<WaiterData>().waiterData.hireCost <= _GM.money)
        {
            _AM.successfulPurchase.Play();

            _WM.CreateNewWaiter(waiterToBuy);

            CheckWhatPlayerCanAffordWaiters();
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
                    _AM.successfulPurchase.Play();


                    _GM.receipesUnlocked.Add(receipeToBuy);

                    _GM.money -= receipeToBuy.GetComponent<FoodData>().foodData.unlockCost;

                    _UM.recipePosters[_arrayNum].SetActive(true);

                    UpdateMoney();

                    CheckWhatPlayerCanAffordReceipes();
                }
                else
                {
                    _AM.errorPurchase.Play();
                }
            }
        }
    }

    #endregion

    #region Load Data

    public void LoadChefData()
    {

        var chef0 = _ChefM.chefArray[0].gameObject.GetComponent<ChefData>().chefData;
        var chef1 = _ChefM.chefArray[1].gameObject.GetComponent<ChefData>().chefData;
        var chef2 = _ChefM.chefArray[2].gameObject.GetComponent<ChefData>().chefData;
        var chef3 = _ChefM.chefArray[3].gameObject.GetComponent<ChefData>().chefData;

        nameChef0.text = chef0.name;
        pfpChef0.sprite = chef0.pfp;
        cookingSkillChef0.text = chef0.cookEffectivness.ToString();
        kneedingSkillChef0.text = chef0.kneedEffectivness.ToString();
        mixingSkillChef0.text = chef0.mixEffectivness.ToString();
        cuttingSkillChef0.text = chef0.cutEffectivness.ToString();
        costChef0.text = "¥" + chef0.hireCost.ToString();

        if (chef0.kneedSkill)
        {
            kneedingSkillChef0.color = highlightedColour;
            skillImagesChef0[0].color = highlightedColour;
        }
        if (chef0.cookSkill)
{
            cookingSkillChef0.color = highlightedColour;
            skillImagesChef0[1].color = highlightedColour;
        }
        if (chef0.cutSkill) 
        {
            cuttingSkillChef0.color = highlightedColour;
            skillImagesChef0[2].color = highlightedColour;
        }
        if (chef0.mixSkill)
        {
            mixingSkillChef0.color = highlightedColour;
            skillImagesChef0[3].color = highlightedColour;
        }

        nameChef1.text = chef1.name;
        pfpChef1.sprite = chef1.pfp;
        cookingSkillChef1.text = chef1.cookEffectivness.ToString();
        kneedingSkillChef1.text = chef1.kneedEffectivness.ToString();
        mixingSkillChef1.text = chef1.mixEffectivness.ToString();
        cuttingSkillChef1.text = chef1.cutEffectivness.ToString();
        costChef1.text = "¥" + chef1.hireCost.ToString();

        if (chef1.kneedSkill)
        {
            kneedingSkillChef1.color = highlightedColour;
            skillImagesChef1[0].color = highlightedColour;
        }
        if (chef1.cookSkill)
        {
            cookingSkillChef1.color = highlightedColour;
            skillImagesChef1[1].color = highlightedColour;
        }
        if (chef1.cutSkill)
        {
            cuttingSkillChef1.color = highlightedColour;
            skillImagesChef1[2].color = highlightedColour;
        }
        if (chef1.mixSkill)
        {
            mixingSkillChef1.color = highlightedColour;
            skillImagesChef1[3].color = highlightedColour;
        }

        nameChef2.text = chef2.name;
        pfpChef2.sprite = chef2.pfp;
        cookingSkillChef2.text = chef2.cookEffectivness.ToString();
        kneedingSkillChef2.text = chef2.kneedEffectivness.ToString();
        mixingSkillChef2.text = chef2.mixEffectivness.ToString();
        cuttingSkillChef2.text = chef2.cutEffectivness.ToString();
        costChef2.text = "¥" + chef2.hireCost.ToString();

        if (chef2.kneedSkill)
        {
            kneedingSkillChef2.color = highlightedColour;
            skillImagesChef2[0].color = highlightedColour;
        }
        if (chef2.cookSkill)
        {
            cookingSkillChef2.color = highlightedColour;
            skillImagesChef2[1].color = highlightedColour;
        }
        if (chef2.cutSkill)
        {
            cuttingSkillChef2.color = highlightedColour;
            skillImagesChef2[2].color = highlightedColour;
        }
        if (chef2.mixSkill)
        {
            mixingSkillChef2.color = highlightedColour;
            skillImagesChef2[3].color = highlightedColour;
        }

        nameChef3.text = chef3.name;
        pfpChef3.sprite = chef3.pfp;
        cookingSkillChef3.text = chef3.cookEffectivness.ToString();
        kneedingSkillChef3.text = chef3.kneedEffectivness.ToString();
        mixingSkillChef3.text = chef3.mixEffectivness.ToString();
        cuttingSkillChef3.text = chef3.cutEffectivness.ToString();
        costChef3.text = "¥" + chef3.ToString();


        if (chef3.kneedSkill)
        {
            kneedingSkillChef3.color = highlightedColour;
            skillImagesChef3[0].color = highlightedColour;
        }
        if (chef3.cookSkill)
        {
            cookingSkillChef3.color = highlightedColour;
            skillImagesChef3[1].color = highlightedColour;
        }
        if (chef3.cutSkill)
        {
            cuttingSkillChef3.color = highlightedColour;
            skillImagesChef3[2].color = highlightedColour;
        }
        if (chef3.mixSkill)
        {
            mixingSkillChef3.color = highlightedColour;
            skillImagesChef3[3].color = highlightedColour;
        }


    }

    public void LoadWaiterData()
    {
        var waiter0 = _WM.waiterArray[0].gameObject.GetComponent<WaiterData>().waiterData;


        nameWaiter0.text = waiter0.name;
        pfpWaiter0.sprite = waiter0.pfp;
        strengthSkillWaiter0.text = waiter0.strength.ToString();
        speedSkillWaiter0.text = waiter0.speed.ToString();
        costWaiter0.text = "¥" + waiter0.hireCost.ToString();
    }
    public void LoadReceipeData()
    {

        var food0 = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData;
        var food1 = _FM.foodArray[1].gameObject.GetComponent<FoodData>().foodData;
        var food2 = _FM.foodArray[2].gameObject.GetComponent<FoodData>().foodData;
        var food3 = _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData;

        nameReceipe0.text = food0.name;
        orderCostReceipe0.text = "Cost: ¥" + food0.orderCost.ToString("F2");
        unlockCostReceipe0.text = "Unlock: ¥" + food0.unlockCost.ToString("F2");
        repLossReceipe0.text = food0.reputationLoss.ToString();
        cookingPointsReceipe0.text = food0.maxCookPrepPoints.ToString();
        mixingPointsReceipe0.text = food0.maxMixPrepPoints.ToString();
        kneedingPointsReceipe0.text = food0.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe0.text = food0.maxCutPrepPoints.ToString();
        pfpReceipe0.sprite = food0.pfp;

        if (food0.needsKneading)
        {
            kneedingPointsReceipe0.color = highlightedColour;
            skillImagesReceipe0[0].color = highlightedColour;

        }
        if (food0.needsCooking)
        {
            cookingPointsReceipe0.color = highlightedColour;
            skillImagesReceipe0[1].color = highlightedColour;

        }
        if (food0.needsCutting)
        {
            cuttingPointsReceipe0.color = highlightedColour;
            skillImagesReceipe0[2].color = highlightedColour;

        }
        if (food0.needsMixing)
        {
            mixingPointsReceipe0.color = highlightedColour;
            skillImagesReceipe0[3].color = highlightedColour;

        }

        nameReceipe1.text = food1.name;
        orderCostReceipe1.text = food1.orderCost.ToString("F2");
        unlockCostReceipe1.text = food1.unlockCost.ToString("F2");
        repLossReceipe1.text = food1.reputationLoss.ToString();
        cookingPointsReceipe1.text = food1.maxCookPrepPoints.ToString();
        mixingPointsReceipe1.text = food1.maxMixPrepPoints.ToString();
        kneedingPointsReceipe1.text = food1.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe1.text = food1.maxCutPrepPoints.ToString();
        pfpReceipe1.sprite = food1.pfp;

        if (food1.needsKneading)
        {
            kneedingPointsReceipe1.color = highlightedColour;
            skillImagesReceipe1[0].color = highlightedColour;

        }
        if (food1.needsCooking)
        {
            cookingPointsReceipe1.color = highlightedColour;
            skillImagesReceipe1[1].color = highlightedColour;

        }
        if (food1.needsCutting)
        {
            cuttingPointsReceipe1.color = highlightedColour;
            skillImagesReceipe1[2].color = highlightedColour;

        }
        if (food1.needsMixing)
        {
            mixingPointsReceipe1.color = highlightedColour;
            skillImagesReceipe1[3].color = highlightedColour;

        }

        nameReceipe2.text = food2.name;
        orderCostReceipe2.text = "Cost: ¥" + food2.orderCost.ToString("F2");
        unlockCostReceipe2.text = "Unlock: ¥" + food2.unlockCost.ToString("F2");
        repLossReceipe2.text = food2.reputationLoss.ToString();
        cookingPointsReceipe2.text = food2.maxCookPrepPoints.ToString();
        mixingPointsReceipe2.text = food2.maxMixPrepPoints.ToString();
        kneedingPointsReceipe2.text = food2.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe2.text = food2.maxCutPrepPoints.ToString();
        pfpReceipe2.sprite = food2.pfp;

        if (food2.needsKneading)
        {
            kneedingPointsReceipe2.color = highlightedColour;
            skillImagesReceipe2[0].color = highlightedColour;

        }
        if (food2.needsCooking)
        {
            cookingPointsReceipe2.color = highlightedColour;
            skillImagesReceipe2[1].color = highlightedColour;

        }
        if (food2.needsCutting)
        {
            cuttingPointsReceipe2.color = highlightedColour;
            skillImagesReceipe2[2].color = highlightedColour;

        }
        if (food2.needsMixing)
        {
            mixingPointsReceipe2.color = highlightedColour;
            skillImagesReceipe2[3].color = highlightedColour;

        }

        nameReceipe3.text = food3.name;
        orderCostReceipe3.text = "Cost: ¥" + food3.orderCost.ToString("F2");
        unlockCostReceipe3.text = "Unlock: ¥" + food3.unlockCost.ToString("F2");
        repLossReceipe3.text = food3.reputationLoss.ToString();
        cookingPointsReceipe3.text = food3.maxCookPrepPoints.ToString();
        mixingPointsReceipe3.text = food3.maxMixPrepPoints.ToString();
        kneedingPointsReceipe3.text = food3.maxKneedPrepPoints.ToString();
        cuttingPointsReceipe3.text = food3.maxCutPrepPoints.ToString();
        pfpReceipe3.sprite = food3.pfp;

        if (food3.needsKneading)
        {
            kneedingPointsReceipe3.color = highlightedColour;
            skillImagesReceipe3[0].color = highlightedColour;

        }
        if (food3.needsCooking)
        {
            cookingPointsReceipe3.color = highlightedColour;
            skillImagesReceipe3[1].color = highlightedColour;

        }
        if (food3.needsCutting)
        {
            cuttingPointsReceipe3.color = highlightedColour;
            skillImagesReceipe3[2].color = highlightedColour;

        }
        if (food3.needsMixing)
        {
            mixingPointsReceipe3.color = highlightedColour;
            skillImagesReceipe3[3].color = highlightedColour;

        }

    }

    public void CheckWhatPlayerCanAffordWaiters()
    {
        if (_GM.money < _WM.waiterArray[0].gameObject.GetComponent<WaiterData>().waiterData.hireCost)
        {
            cannotAffordWaiter0.SetActive(true);
        }
        else cannotAffordWaiter0.SetActive(false);
    }

    public void CheckWhatPlayerCanAffordChefs()
    {
        if(_GM.money < _ChefM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef0.SetActive(true);
        }
        else cannotAffordChef0.SetActive(false);
        
        if(_GM.money < _ChefM.chefArray[1].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef1.SetActive(true);
        }
        else cannotAffordChef1.SetActive(false);
        
        if(_GM.money < _ChefM.chefArray[2].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef2.SetActive(true);
        }
        else cannotAffordChef2.SetActive(false);
        
        if(_GM.money < _ChefM.chefArray[3].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef3.SetActive(true);
        }
        else cannotAffordChef3.SetActive(false);
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
            soldReceipe2.SetActive(true);
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
            soldReceipe3.SetActive(true);
        }
        else
        {
            if (_GM.money < _FM.foodArray[3].gameObject.GetComponent<FoodData>().foodData.unlockCost)
            {
                cannotAffordReceipe3.SetActive(true);
            }
            else cannotAffordReceipe3.SetActive(false);
        }
        #endregion

    }

}
