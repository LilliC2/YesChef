using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;

public class UIManager : Singleton<UIManager>
{
    [Header("Audio")]
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    [Header("Help Screens")]
    public GameObject failSafeObj;
    public TMP_Text missingTxt;
    public bool continuePlay;


    [Header("Tutorial")]
    public GameObject tutorialMainPanel;
    public GameObject[] tutorialPanels;
    bool inTutotial;
    bool placingTanuki = false;
    bool placingWaiter =false;

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
    Canvas parentCanvas;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public GameObject completePanel;

    #region Resturant Upgrades
    [Header("Resturant Upgrades")]

    [SerializeField]
    GameObject restaurantUpgradePanel;

    int selectedUpgrade;
    public TMP_Text upgradeRestCost;
    public TMP_Text upgradeRestName;
    public TMP_Text upgradeRestDescription;

    public Image[] upgradeRestButtonImages;

    #endregion

    [Header("Waiter UI")]
    public GameObject waiterMenu;
    public GameObject waiterMenuButton;
    #region Waiters

    #region Waiter Pop Up
    [Header("Waiter Popup")]
    private GameObject selectedWaiter;
    public GameObject waiterPopUp;
    public TMP_Text speedSkillWaiterfPopUp;
    public TMP_Text StrengthSkillWaiterfPopUp;
    public Image waiterPopUpPFP;

    #endregion

    [Header("Waiter 0")]
    public TMP_Text nameWaiter0;
    public Image pfpWaiter0;
    public TMP_Text costWaiter0;
    public TMP_Text strengthSkillWaiter0;
    public TMP_Text speedSkillWaiter0;
    public GameObject cannotAffordWaiter0;
    
    [Header("Waiter 1")]
    public TMP_Text nameWaiter1;
    public Image pfpWaiter1;
    public TMP_Text costWaiter1;
    public TMP_Text strengthSkillWaiter1;
    public TMP_Text speedSkillWaiter1;
    public GameObject cannotAffordWaiter1;
    
    [Header("Waiter 2")]
    public TMP_Text nameWaiter2;
    public Image pfpWaiter2;
    public TMP_Text costWaiter2;
    public TMP_Text strengthSkillWaiter2;
    public TMP_Text speedSkillWaiter2;
    public GameObject cannotAffordWaiter2;
    
    [Header("Waiter 3")]
    public TMP_Text nameWaiter3;
    public Image pfpWaiter3;
    public TMP_Text costWaiter3;
    public TMP_Text strengthSkillWaiter3;
    public TMP_Text speedSkillWaiter3;
    public GameObject cannotAffordWaiter3;
    
    [Header("Waiter 4")]
    public TMP_Text nameWaiter4;
    public Image pfpWaiter4;
    public TMP_Text costWaiter4;
    public TMP_Text strengthSkillWaiter4;
    public TMP_Text speedSkillWaiter4;
    public GameObject cannotAffordWaiter4;
    #endregion
    [Header("Chef UI")]
    public GameObject chefMenu;
    public GameObject chefMenuButton;

    #region Chefs
    #region Chefs in Shop
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
    
    [Header("Chef 4")]
    public TMP_Text nameChef4;
    public Image pfpChef4;
    public TMP_Text costChef4;
    public TMP_Text cookingSkillChef4;
    public TMP_Text kneedingSkillChef4;
    public TMP_Text cuttingSkillChef4;
    public TMP_Text mixingSkillChef4;
    public GameObject cannotAffordChef4;
    public Image[] skillImagesChef4;
    
    [Header("Chef 5")]
    public TMP_Text nameChef5;
    public Image pfpChef5;
    public TMP_Text costChef5;
    public TMP_Text cookingSkillChef5;
    public TMP_Text kneedingSkillChef5;
    public TMP_Text cuttingSkillChef5;
    public TMP_Text mixingSkillChef5;
    public GameObject cannotAffordChef5;
    public Image[] skillImagesChef5;    

    [Header("Chef 6")]
    public TMP_Text nameChef6;
    public Image pfpChef6;
    public TMP_Text costChef6;
    public TMP_Text cookingSkillChef6;
    public TMP_Text kneedingSkillChef6;
    public TMP_Text cuttingSkillChef6;
    public TMP_Text mixingSkillChef6;
    public GameObject cannotAffordChef6;
    public Image[] skillImagesChef6;
    
    [Header("Chef 7")]
    public TMP_Text nameChef7;
    public Image pfpChef7;
    public TMP_Text costChef7;
    public TMP_Text cookingSkillChef7;
    public TMP_Text kneedingSkillChef7;
    public TMP_Text cuttingSkillChef7;
    public TMP_Text mixingSkillChef7;
    public GameObject cannotAffordChef7;
    public Image[] skillImagesChef7;
    
    [Header("Chef 8")]
    public TMP_Text nameChef8;
    public Image pfpChef8;
    public TMP_Text costChef8;
    public TMP_Text cookingSkillChef8;
    public TMP_Text kneedingSkillChef8;
    public TMP_Text cuttingSkillChef8;
    public TMP_Text mixingSkillChef8;
    public GameObject cannotAffordChef8;
    public Image[] skillImagesChef8;
    
    [Header("Chef 9")]
    public TMP_Text nameChef9;
    public Image pfpChef9;
    public TMP_Text costChef9;
    public TMP_Text cookingSkillChef9;
    public TMP_Text kneedingSkillChef9;
    public TMP_Text cuttingSkillChef9;
    public TMP_Text mixingSkillChef9;
    public GameObject cannotAffordChef9;
    public Image[] skillImagesChef9;
    #endregion

    [Header("Chef PopUP UI")]
    public GameObject chefPopUp;
    public GameObject selectedChef;
    public ChefClass selectedChef_chefData;
    public Image chefPopUpPFP;
    public TMP_Text chefPopUpName;
    public TMP_Text cookingSkillChefchefPopUp;
    public TMP_Text kneedingSkillChefchefPopUp;
    public TMP_Text cuttingSkillChefPopUp;
    public TMP_Text mixingSkillChefchefPopUp;
    public Image rangeSlider1;
    public Image rangeSlider2;
    public TMP_Text targettingButtonTxt;

    public TMP_Text upgradeButtonKneadTxt;
    public TMP_Text upgradeButtonCutTxt;
    public TMP_Text upgradeButtonCookTxt;
    public TMP_Text upgradeButtonMixTxt;


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
        inTutotial = true;

        tutorialMainPanel.SetActive(true);
        tutorialPanels[0].SetActive(true);
        LoadChefData();
        LoadWaiterData();
        LoadReceipeData();

        parentCanvas = GetComponent<Canvas>();

        _GM.event_endOfDay.AddListener(UpdateDay);
        _GM.event_updateMoney.AddListener(UpdateMoney);
    }

    private void Update()
    {
        //temp\
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (Camera.main.transform.position == kitchenCamPos) LookAtResturant();
        //    else if(Camera.main.transform.position == resturantCamPos) LookAtKitchen();

        //}

        if (inTutotial)
        {
            //check for if chef is placed
            if (!tutorialMainPanel.activeSelf && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (placingTanuki)
                {
                    placingTanuki = false;
                    NextTutorialPanel(5);
                    CloseChefMenu();
                    ExecuteAfterSeconds(2f, () => tutorialMainPanel.SetActive(true));
                }
                else if (placingWaiter)
                {
                    placingWaiter = false;
                    NextTutorialPanel(14);
                    CloseWaiterMenu();
                    ExecuteAfterSeconds(2f, () => tutorialMainPanel.SetActive(true));
                }

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

    #region Restaurant Upgrades
    
    public void OpenAndCloseRestauranttUpgradePanel()
    {
        if(restaurantUpgradePanel.activeSelf == true)
        {
            restaurantUpgradePanel.SetActive(false);
        }
        else restaurantUpgradePanel.SetActive(true);
    }

    public void PurachseRestaurantUpgrade()
    {

        var upgrade = _UM.resturantUpgradeInformation[selectedUpgrade];
        print(selectedUpgrade);

        if (!upgrade.active && upgrade.costToUnlock < _GM.money)
        {
            //check if previous upgrade has been purchased
            if (_UM.resturantUpgradeInformation[selectedUpgrade - 1].active || selectedUpgrade == 0)
            {
                //remove money
                _GM.money = _GM.money - upgrade.costToUnlock;

                //make button yellow
                upgradeRestButtonImages[selectedUpgrade].color = highlightedColour;

                _UM.PurchaseUpgrade(selectedUpgrade);
            }
            else print("Needs prerequisite");
        }
        else print("Cannot purchase");
    }

    public void UpdateRestaurantUpgradeInfo(int _index)
    {
        selectedUpgrade = _index;
        var upgrade = _UM.resturantUpgradeInformation[_index];

        upgradeRestCost.text = upgrade.costToUnlock.ToString();
        upgradeRestDescription.text = upgrade.description;
        upgradeRestName.text = upgrade.name;
    }

    #endregion

    #region Audio

    void AudioLowPass()
    {
        if(Time.timeScale ==0)
        {
            //transition to paused audio snap
            paused.TransitionTo(0.1f);
        }
        else
        {
            unpaused.TransitionTo(0.1f);

        }
    }




    #endregion

    #region Help

    public void CloseFailSafe()
    {
        failSafeObj.SetActive(false);

    }
    public void CloseFailSafeContinue()
    {
        if (_GM.receipesUnlocked.Count != 0)
        {
            failSafeObj.SetActive(false);

            continuePlay = true;
        }
        else missingTxt.text = "You cannot continue without at least one receipe";

    }

    bool CheckSafeToPlay()
    {
        string youAreMissingTxt = "You are missing: ";
        bool safeToPlay = true;
        //have they bought a receipe?
        if (_GM.receipesUnlocked.Count == 0)
        {
            safeToPlay = false;
            youAreMissingTxt = youAreMissingTxt + "\n\tat least one receipe";
        }

        //have they bought a chef
        if (_ChefM.currentChefs.Count == 0)
        {
            safeToPlay = false;
            youAreMissingTxt = youAreMissingTxt + "\n\tat least one chef";

        }

        //have they bought a waiter
        if (_WM.currentWaiters.Count == 0)
        {
            youAreMissingTxt = youAreMissingTxt + "\n\tat least one waiter";
            safeToPlay = false;
        }

        youAreMissingTxt = youAreMissingTxt + "\n You must have at least one receipe";

        missingTxt.text = youAreMissingTxt;

        return safeToPlay;
    }

    #endregion

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


    public void MoveCamera()
    {
        Camera.main.transform.DOMove(new Vector3(-11.21f, 10.4499998f, -26.0514069f), 2);
    }

    public void BuyWaiterTut()
    {
        placingWaiter = true;

        tutorialMainPanel.SetActive(false);
        BuyWaiter(0);
    }
    
    public void BuyTanuki()
    {
        placingTanuki = true;
        tutorialMainPanel.SetActive(false);
        BuyChef(0);
    }
    #endregion

    #region Menus
    public void Pause()
    {
        pause = !pause;
        if(pause)
        {
            Time.timeScale = 0;
            AudioLowPass();

            _GM.gameState = GameManager.GameState.Pause;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = _GM.currentTimeScale;
            AudioLowPass();

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

    #region Upgrade Buttons

    public void UpgradeSkill(int _skill)
    {
        //check if can afford
        bool canBuy = false;

        switch(_skill)
        {
            case 0://mixing

                //check if can buy
                if (_GM.money >= selectedChef_chefData.mixUpgradeCost)
                {
                    print("buy");
                    canBuy = true;
                    print("mix " + selectedChef_chefData.mixUpgradeCost);
                    _GM.money = _GM.money - selectedChef_chefData.mixUpgradeCost;
                    selectedChef_chefData.mixEffectivness++;

                    //increase cost by 10%
                    selectedChef_chefData.mixUpgradeCost = selectedChef_chefData.mixUpgradeCost + (selectedChef_chefData.mixUpgradeCost * _UM.chefUpgradeCostMultiplier);
                    //update UI
                    UpdateChefPopUp();
                }

                break;
            case 1://kneading

                //check if can buy
                if (_GM.money >= selectedChef_chefData.kneadUpgradeCost)
                {
                    canBuy = true;
                    _GM.money = _GM.money - selectedChef_chefData.kneadUpgradeCost;
                    selectedChef_chefData.kneadEffectivness++;

                    //increase cost by 10%
                    selectedChef_chefData.kneadUpgradeCost = selectedChef_chefData.kneadUpgradeCost + (selectedChef_chefData.kneadUpgradeCost * _UM.chefUpgradeCostMultiplier);
                    //update UI
                    UpdateChefPopUp();

                }

                break;
            case 2://cutting

                //check if can buy
                if (_GM.money >= selectedChef_chefData.cutUpgradeCost)
                {
                    canBuy = true;
                    _GM.money = _GM.money - selectedChef_chefData.cutUpgradeCost;
                    selectedChef_chefData.cutEffectivness++;

                    //increase cost by 10%
                    selectedChef_chefData.cutUpgradeCost = selectedChef_chefData.cutUpgradeCost + (selectedChef_chefData.cutUpgradeCost * _UM.chefUpgradeCostMultiplier);
                    //update UI
                    UpdateChefPopUp();

                }
                break;
            case 3://cooking

                //check if can buy
                if (_GM.money >= selectedChef_chefData.cookUpgradeCost)
                {
                  
                    canBuy = true;
                    _GM.money = _GM.money - selectedChef_chefData.cookUpgradeCost;
                    selectedChef_chefData.cookEffectivness++;

                    //increase cost by 10%
                    selectedChef_chefData.cookUpgradeCost = selectedChef_chefData.cookUpgradeCost + (selectedChef_chefData.cookUpgradeCost * _UM.chefUpgradeCostMultiplier);
                    //update UI
                    UpdateChefPopUp();

                }

                break;
        }

        if (canBuy)
        {
            UpdateMoney();
            selectedChef_chefData.level++;
        }
        else print("Cannot afford");
    }


    #endregion

    public void OpenChefPopUp(GameObject _chefData)
    {
        //check if waiter pop up open
        if(waiterPopUp.activeSelf) { CloseWaiterPopUp(); }


        if(_chefData != selectedChef && selectedChef != null) selectedChef.GetComponent<ChefData>().rangeIndicator.SetActive(false);


        selectedChef = _chefData;
        selectedChef_chefData = selectedChef.GetComponent<ChefData>().chefData;
        chefPopUp.SetActive(true);

        UpdateChefPopUp();
        selectedChef.GetComponent<ChefData>().rangeIndicator.SetActive(true);
        targettingButtonTxt.text = selectedChef.GetComponent<ChefData>().targeting.ToString();

        //chefPopUpName.text = chefData.name;
       
    }

    public void UpdateChefPopUp()
    {

        chefPopUpPFP.sprite = selectedChef_chefData.pfp;
        cookingSkillChefchefPopUp.text = selectedChef_chefData.cookEffectivness.ToString();
        cuttingSkillChefPopUp.text = selectedChef_chefData.cutEffectivness.ToString();
        mixingSkillChefchefPopUp.text = selectedChef_chefData.mixEffectivness.ToString();
        kneedingSkillChefchefPopUp.text = selectedChef_chefData.kneadEffectivness.ToString();

        upgradeButtonCookTxt.text = selectedChef_chefData.cookUpgradeCost.ToString();
        upgradeButtonCutTxt.text = selectedChef_chefData.cutUpgradeCost.ToString();
        upgradeButtonMixTxt.text = selectedChef_chefData.mixUpgradeCost.ToString();
        upgradeButtonKneadTxt.text = selectedChef_chefData.kneadUpgradeCost.ToString();

        rangeSlider1.fillAmount = selectedChef_chefData.range / 10;
        rangeSlider2.fillAmount = selectedChef_chefData.range / 10;
    }

    public void CloseChefPopUp()
    {
        selectedChef.GetComponent<ChefData>().rangeIndicator.SetActive(false);
        selectedChef = null;

        chefPopUp.SetActive(false);
    }


    public void ChangeChefTargeting()
    {
        var chefData = selectedChef.GetComponent<ChefData>();

        print("Clicked button");
        if( chefData.targeting == ChefData.Targeting.First)
        {
            targettingButtonTxt.text = "Last";
            chefData.targeting = ChefData.Targeting.Last;
        }
        else if(chefData.targeting == ChefData.Targeting.Last)
        {
            targettingButtonTxt.text = "Strongest";
            chefData.targeting = ChefData.Targeting.Strongest;
        }
        else if(chefData.targeting == ChefData.Targeting.Strongest)
        {
            targettingButtonTxt.text = "First";
            chefData.targeting = ChefData.Targeting.First;
        }
        else
        {
            print("not entering the ifs");
        }


    }
    public void FireChef()
    {
        //give money back to player
        _GM.money += selectedChef.GetComponent<ChefData>().chefData.hireCost/2;

        _GM.event_updateMoney.Invoke();

        print("Destroy him");
        //destroy chef
        Destroy(selectedChef);

        CloseChefPopUp();
    }

    #endregion

    #region Waiter Pop Up
    public void OpenWaiterPopUp(GameObject _watierData)
    {
        if(chefPopUp.activeSelf) CloseChefPopUp();
        selectedWaiter = _watierData;
        waiterPopUp.SetActive(true);

        var waiterData = _watierData.GetComponent<WaiterData>().waiterData;

        //chefPopUpName.text = chefData.name;
        waiterPopUpPFP.sprite = waiterData.pfp;
        speedSkillWaiterfPopUp.text = waiterData.speed.ToString();
        StrengthSkillWaiterfPopUp.text = waiterData.strength.ToString();

    }

    public void CloseWaiterPopUp()
    {
        waiterPopUp.SetActive(false);

    }

    public void FireWaiter()
    {
        //give money back to player
        _GM.money += selectedWaiter.GetComponent<WaiterData>().waiterData.hireCost / 2;

        _GM.event_updateMoney.Invoke();

        selectedChef.GetComponent<WaiterData>().FireChef();

        //destroy chef
        //Destroy(selectedWaiter);

        CloseWaiterPopUp();
    }

    #endregion

    #region Abilities


    #endregion

    #region Buttons


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
        if(!CheckSafeToPlay() && !continuePlay)
        {
            failSafeObj.SetActive(true);
        }
        else
        {
            _GM.playerReady = true;

            _GM.UpdateTimeScale(1);
            _AM.slowDown.Play();
        }


    }

    public void SpeedUp()
    {
        if (!CheckSafeToPlay() && !continuePlay)
        {
            failSafeObj.SetActive(true);
        }
        else
        {
            print("Speed up");
            _GM.UpdateTimeScale(2);
            _AM.speedUp.Play();
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

        var chefToBuy = _ChefM.chefArray[_arrayNum];

        if (chefToBuy.GetComponent<ChefData>().chefData.hireCost <= _GM.money)
        {
            _AM.successfulPurchase.Play();

            _ChefM.CreateNewChef(chefToBuy);

            CheckWhatPlayerCanAffordChefs();
        }
        else print("Cannot afford");

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

                    _GM.event_updateMoney.Invoke();

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
        var chef4 = _ChefM.chefArray[4].gameObject.GetComponent<ChefData>().chefData;
        var chef5 = _ChefM.chefArray[5].gameObject.GetComponent<ChefData>().chefData;
        var chef6 = _ChefM.chefArray[6].gameObject.GetComponent<ChefData>().chefData;
        var chef7 = _ChefM.chefArray[7].gameObject.GetComponent<ChefData>().chefData;
        var chef8 = _ChefM.chefArray[8].gameObject.GetComponent<ChefData>().chefData;
        //var chef9 = _ChefM.chefArray[9].gameObject.GetComponent<ChefData>().chefData;

        #region chef 0
        nameChef0.text = chef0.name;
        pfpChef0.sprite = chef0.pfp;
        cookingSkillChef0.text = chef0.cookEffectivness.ToString();
        kneedingSkillChef0.text = chef0.kneadEffectivness.ToString();
        mixingSkillChef0.text = chef0.mixEffectivness.ToString();
        cuttingSkillChef0.text = chef0.cutEffectivness.ToString();
        costChef0.text = "¥" + chef0.hireCost.ToString();

        if (chef0.kneadSkill)
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
        #endregion

        #region chef 1
        nameChef1.text = chef1.name;
        pfpChef1.sprite = chef1.pfp;
        cookingSkillChef1.text = chef1.cookEffectivness.ToString();
        kneedingSkillChef1.text = chef1.kneadEffectivness.ToString();
        mixingSkillChef1.text = chef1.mixEffectivness.ToString();
        cuttingSkillChef1.text = chef1.cutEffectivness.ToString();
        costChef1.text = "¥" + chef1.hireCost.ToString();

        if (chef1.kneadSkill)
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
        #endregion

        #region chef 2
        nameChef2.text = chef2.name;
        pfpChef2.sprite = chef2.pfp;
        cookingSkillChef2.text = chef2.cookEffectivness.ToString();
        kneedingSkillChef2.text = chef2.kneadEffectivness.ToString();
        mixingSkillChef2.text = chef2.mixEffectivness.ToString();
        cuttingSkillChef2.text = chef2.cutEffectivness.ToString();
        costChef2.text = "¥" + chef2.hireCost.ToString();

        if (chef2.kneadSkill)
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
        #endregion

        #region chef 3
        nameChef3.text = chef3.name;
        pfpChef3.sprite = chef3.pfp;
        cookingSkillChef3.text = chef3.cookEffectivness.ToString();
        kneedingSkillChef3.text = chef3.kneadEffectivness.ToString();
        mixingSkillChef3.text = chef3.mixEffectivness.ToString();
        cuttingSkillChef3.text = chef3.cutEffectivness.ToString();
        costChef3.text = "¥" + chef3.ToString();


        if (chef3.kneadSkill)
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
        #endregion

        #region chef 4
        nameChef4.text = chef4.name;
        pfpChef4.sprite = chef4.pfp;
        cookingSkillChef4.text = chef4.cookEffectivness.ToString();
        kneedingSkillChef4.text = chef4.kneadEffectivness.ToString();
        mixingSkillChef4.text = chef4.mixEffectivness.ToString();
        cuttingSkillChef4.text = chef4.cutEffectivness.ToString();
        costChef4.text = "¥" + chef4.hireCost.ToString();


        if (chef4.kneadSkill)
        {
            kneedingSkillChef4.color = highlightedColour;
            skillImagesChef4[0].color = highlightedColour;
        }
        if (chef4.cookSkill)
        {
            cookingSkillChef4.color = highlightedColour;
            skillImagesChef4[1].color = highlightedColour;
        }
        if (chef4.cutSkill)
        {
            cuttingSkillChef4.color = highlightedColour;
            skillImagesChef4[2].color = highlightedColour;
        }
        if (chef4.mixSkill)
        {
            mixingSkillChef4.color = highlightedColour;
            skillImagesChef4[3].color = highlightedColour;
        }
        #endregion

        #region chef 5
        nameChef5.text = chef5.name;
        pfpChef5.sprite = chef5.pfp;
        cookingSkillChef5.text = chef5.cookEffectivness.ToString();
        kneedingSkillChef5.text = chef5.kneadEffectivness.ToString();
        mixingSkillChef5.text = chef5.mixEffectivness.ToString();
        cuttingSkillChef5.text = chef5.cutEffectivness.ToString();
        costChef5.text = "¥" + chef5.hireCost.ToString();


        if (chef5.kneadSkill)
        {
            kneedingSkillChef5.color = highlightedColour;
            skillImagesChef5[0].color = highlightedColour;
        }
        if (chef5.cookSkill)
        {
            cookingSkillChef5.color = highlightedColour;
            skillImagesChef5[1].color = highlightedColour;
        }
        if (chef5.cutSkill)
        {
            cuttingSkillChef5.color = highlightedColour;
            skillImagesChef5[2].color = highlightedColour;
        }
        if (chef5.mixSkill)
        {
            mixingSkillChef5.color = highlightedColour;
            skillImagesChef5[3].color = highlightedColour;
        }
        #endregion

        #region chef 6
        nameChef6.text = chef6.name;
        pfpChef6.sprite = chef6.pfp;
        cookingSkillChef6.text = chef6.cookEffectivness.ToString();
        kneedingSkillChef6.text = chef6.kneadEffectivness.ToString();
        mixingSkillChef6.text = chef6.mixEffectivness.ToString();
        cuttingSkillChef6.text = chef6.cutEffectivness.ToString();
        costChef6.text = "¥" + chef6.hireCost.ToString();


        if (chef6.kneadSkill)
        {
            kneedingSkillChef6.color = highlightedColour;
            skillImagesChef6[0].color = highlightedColour;
        }
        if (chef6.cookSkill)
        {
            cookingSkillChef6.color = highlightedColour;
            skillImagesChef6[1].color = highlightedColour;
        }
        if (chef6.cutSkill)
        {
            cuttingSkillChef6.color = highlightedColour;
            skillImagesChef6[2].color = highlightedColour;
        }
        if (chef6.mixSkill)
        {
            mixingSkillChef6.color = highlightedColour;
            skillImagesChef6[3].color = highlightedColour;
        }
        #endregion

        #region chef 7
        nameChef7.text = chef7.name;
        pfpChef7.sprite = chef7.pfp;
        cookingSkillChef7.text = chef7.cookEffectivness.ToString();
        kneedingSkillChef7.text = chef7.kneadEffectivness.ToString();
        mixingSkillChef7.text = chef7.mixEffectivness.ToString();
        cuttingSkillChef7.text = chef7.cutEffectivness.ToString();
        costChef7.text = "¥" + chef7.hireCost.ToString();


        if (chef7.kneadSkill)
        {
            kneedingSkillChef7.color = highlightedColour;
            skillImagesChef7[0].color = highlightedColour;
        }
        if (chef7.cookSkill)
        {
            cookingSkillChef7.color = highlightedColour;
            skillImagesChef7[1].color = highlightedColour;
        }
        if (chef7.cutSkill)
        {
            cuttingSkillChef7.color = highlightedColour;
            skillImagesChef7[2].color = highlightedColour;
        }
        if (chef7.mixSkill)
        {
            mixingSkillChef7.color = highlightedColour;
            skillImagesChef7[3].color = highlightedColour;
        }
        #endregion

        #region chef 8
        nameChef8.text = chef8.name;
        pfpChef8.sprite = chef8.pfp;
        cookingSkillChef8.text = chef8.cookEffectivness.ToString();
        kneedingSkillChef8.text = chef8.kneadEffectivness.ToString();
        mixingSkillChef8.text = chef8.mixEffectivness.ToString();
        cuttingSkillChef8.text = chef8.cutEffectivness.ToString();
        costChef8.text = "¥" + chef8.hireCost.ToString();


        if (chef8.kneadSkill)
        {
            kneedingSkillChef8.color = highlightedColour;
            skillImagesChef8[0].color = highlightedColour;
        }
        if (chef8.cookSkill)
        {
            cookingSkillChef8.color = highlightedColour;
            skillImagesChef8[1].color = highlightedColour;
        }
        if (chef8.cutSkill)
        {
            cuttingSkillChef8.color = highlightedColour;
            skillImagesChef8[2].color = highlightedColour;
        }
        if (chef8.mixSkill)
        {
            mixingSkillChef8.color = highlightedColour;
            skillImagesChef8[3].color = highlightedColour;
        }
        #endregion

        //#region chef 9
        //nameChef9.text = chef9.name;
        //pfpChef9.sprite = chef9.pfp;
        //cookingSkillChef9.text = chef9.cookEffectivness.ToString();
        //kneedingSkillChef9.text = chef9.kneadEffectivness.ToString();
        //mixingSkillChef9.text = chef9.mixEffectivness.ToString();
        //cuttingSkillChef9.text = chef9.cutEffectivness.ToString();
        //costChef9.text = "¥" + chef9.hireCost.ToString();


        //if (chef9.kneadSkill)
        //{
        //    kneedingSkillChef9.color = highlightedColour;
        //    skillImagesChef9[0].color = highlightedColour;
        //}
        //if (chef9.cookSkill)
        //{
        //    cookingSkillChef9.color = highlightedColour;
        //    skillImagesChef9[1].color = highlightedColour;
        //}
        //if (chef9.cutSkill)
        //{
        //    cuttingSkillChef9.color = highlightedColour;
        //    skillImagesChef9[2].color = highlightedColour;
        //}
        //if (chef9.mixSkill)
        //{
        //    mixingSkillChef9.color = highlightedColour;
        //    skillImagesChef9[3].color = highlightedColour;
        //}
        //#endregion
    }

    public void LoadWaiterData()
    {
        var waiter0 = _WM.waiterArray[0].gameObject.GetComponent<WaiterData>().waiterData;

        nameWaiter0.text = waiter0.name;
        pfpWaiter0.sprite = waiter0.pfp;
        strengthSkillWaiter0.text = waiter0.strength.ToString();
        speedSkillWaiter0.text = waiter0.speed.ToString();
        costWaiter0.text = "¥" + waiter0.hireCost.ToString();
        
        var waiter1 = _WM.waiterArray[1].gameObject.GetComponent<WaiterData>().waiterData;

        nameWaiter1.text = waiter1.name;
        pfpWaiter1.sprite = waiter1.pfp;
        strengthSkillWaiter1.text = waiter1.strength.ToString();
        speedSkillWaiter1.text = waiter1.speed.ToString();
        costWaiter1.text = "¥" + waiter1.hireCost.ToString();
        
        var waiter2 = _WM.waiterArray[2].gameObject.GetComponent<WaiterData>().waiterData;

        nameWaiter2.text = waiter2.name;
        pfpWaiter2.sprite = waiter2.pfp;
        strengthSkillWaiter2.text = waiter2.strength.ToString();
        speedSkillWaiter2.text = waiter2.speed.ToString();
        costWaiter2.text = "¥" + waiter2.hireCost.ToString();
        
        var waiter3 = _WM.waiterArray[3].gameObject.GetComponent<WaiterData>().waiterData;

        nameWaiter3.text = waiter3.name;
        pfpWaiter3.sprite = waiter3.pfp;
        strengthSkillWaiter3.text = waiter3.strength.ToString();
        speedSkillWaiter3.text = waiter3.speed.ToString();
        costWaiter3.text = "¥" + waiter3.hireCost.ToString();

        var waiter4 = _WM.waiterArray[4].gameObject.GetComponent<WaiterData>().waiterData;

        nameWaiter4.text = waiter4.name;
        pfpWaiter4.sprite = waiter4.pfp;
        strengthSkillWaiter4.text = waiter4.strength.ToString();
        speedSkillWaiter4.text = waiter4.speed.ToString();
        costWaiter4.text = "¥" + waiter4.hireCost.ToString();
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
        
        if (_GM.money < _WM.waiterArray[1].gameObject.GetComponent<WaiterData>().waiterData.hireCost)
        {
            cannotAffordWaiter1.SetActive(true);
        }
        else cannotAffordWaiter1.SetActive(false);
        
        if (_GM.money < _WM.waiterArray[2].gameObject.GetComponent<WaiterData>().waiterData.hireCost)
        {
            cannotAffordWaiter2.SetActive(true);
        }
        else cannotAffordWaiter2.SetActive(false);
        
        if (_GM.money < _WM.waiterArray[3].gameObject.GetComponent<WaiterData>().waiterData.hireCost)
        {
            cannotAffordWaiter3.SetActive(true);
        }
        else cannotAffordWaiter3.SetActive(false);

        if (_GM.money < _WM.waiterArray[4].gameObject.GetComponent<WaiterData>().waiterData.hireCost)
        {
            cannotAffordWaiter4.SetActive(true);
        }
        else cannotAffordWaiter4.SetActive(false);
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
        
        if(_GM.money < _ChefM.chefArray[4].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef4.SetActive(true);
        }
        else cannotAffordChef4.SetActive(false);

        if (_GM.money < _ChefM.chefArray[5].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef5.SetActive(true);
        }
        else cannotAffordChef5.SetActive(false);


        if (_GM.money < _ChefM.chefArray[6].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef6.SetActive(true);
        }
        else cannotAffordChef6.SetActive(false);


        if (_GM.money < _ChefM.chefArray[7].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef7.SetActive(true);
        }
        else cannotAffordChef7.SetActive(false);


        if (_GM.money < _ChefM.chefArray[8].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef8.SetActive(true);
        }
        else cannotAffordChef8.SetActive(false);

        //if (_GM.money < _ChefM.chefArray[9].gameObject.GetComponent<ChefData>().chefData.hireCost)
        //{
        //    cannotAffordChef9.SetActive(true);
        //}
        //else cannotAffordChef9.SetActive(false);
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

    }
    #endregion

}
