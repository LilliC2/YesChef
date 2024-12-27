using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Ricimi;
using DG.Tweening;
using static ChefData;
using static AudioManager;
using static SceneController;


public class UIManager : Singleton<UIManager>
{
    #region Variables

    [Header("HUD")]
    [SerializeField] Canvas inGame_Canvas, inTitleScreen_Canvas;
    Camera cam;

    [Header("HUD")]
    [SerializeField] Image resturantRating_Image;
    [SerializeField] GameObject openResturantButton_GO;
    [SerializeField] GameObject ordersPanel_GO;
    [SerializeField] TMP_Text day_Text;
    [SerializeField] TMP_Text error_Text;

    [Header("Demo")]
    [SerializeField] GameObject demoPanel_GO;

    [Header("Open/Close Dial")]
    [SerializeField]
    CircularProgressBar openDayDial_CircularProgressBar;

    [Header("Player Profile")]
    [SerializeField] TMP_Text moneyValue_TMPText;
    [SerializeField] TMP_Text levelValue_TMPText;
    [SerializeField] Slider currentEXPValue_Slider;

    [Header("Button Panel")]
    [SerializeField] GameObject buttonPanel_GO;
    [SerializeField] Vector3 openPos_V3;
    [SerializeField] Vector3 closePos_V3;

    [Header("Purchase Produce")]
    [SerializeField] GameObject producePanel_GO;
    [SerializeField] Camera produceCamera_Cam;
    [SerializeField] Image vegButton_Image, fruitButton_Image, proteinButton_Image, dairyButton_Image, grainButton_Image;
    [SerializeField] Color cannotAfford_Colour;
    [SerializeField] Color canAfford_Colour;
    [SerializeField] TMP_Text vegButton_Txt, fruitButton_Txt, proteinButton_Txt, dairyButton_Txt, grainButton_Txt;

    [Header("Orders")]
    [SerializeField] GameObject orderPanel;
    [SerializeField] Transform orderBar_Panel;
    public List<GameObject> ordersGO_List = new List<GameObject>();
    [SerializeField] float quaterWidth, thirdsWidth, halfWidth, singleWidth;

    [Header("Hire Staff")]
    [SerializeField] GameObject chefOrganisePanel_GO, waiterOrganisePanel_GO, staffOrganisePanel_GO;
    [SerializeField] GameObject unlockChoicePanel_GO, unlockScreen_GO, unlockCamera_GO, modelsParent_GO;
    [SerializeField] TMP_Text unlockStaffName_Txt;
    [SerializeField] List<GameObject> unlockStaffModels_ListGO, organiseStaffButtons_GO;

    [Header("Staff Dialog")]
    [SerializeField] GameObject staffDialog_GO;
    [SerializeField] GameObject dialogBox_GO;
    [SerializeField] GameObject dialogBoxNextButton_GO;
    [SerializeField] GameObject dialogBoxAnswers_GO;
    [SerializeField] TMP_Text dialogStaffName_TMPText;
    [SerializeField] TMP_Text dialogBox_TMPText;
    [SerializeField] TMP_Text dialogBoxAnswer1_TMPText, dialogBoxAnswer2_TMPText;
    [SerializeField] List<string> currentDialogString_List = new List<string>();
    int currentDialogIndex = -1;
    [SerializeField] Dialog currentDialog_Dialog;

    [Header("Outside")]
    public TMP_Text resturantSign_TMPText;
    [SerializeField] GameObject renameResturantPanel_GO;

    [Header("Build System")]
    [SerializeField] GameObject buildShopPanel_GO;
    [SerializeField] GameObject buildHoverInfo_GO;
    public ObjectData objectHoveredOver;

    #endregion

    #region Start/House Keeping
    private void Start()
    {

        cam = Camera.main;
        _GM.event_playStateClose.AddListener(ActivateButtonPanel);
        _GM.event_playStateClose.AddListener(ClosedButtonsActive);
        _GM.event_playStateOpen.AddListener(ActivateButtonPanel);
        _GM.event_gameStateTitleScreen.AddListener(ActivateTitleScreenUI);
        _GM.event_gameStateOpenGameScene.AddListener(ActivateGameHUDUI);

        //Set inital UI (either title or in game
        if(_GM.gameState == GameManager.GameState.Title)
        {
            ActivateTitleScreenUI();
        }
        else if(_GM.gameState == GameManager.GameState.Playing)
        {
            ActivateGameHUDUI();

        }
    }

    public void ActivateTitleScreenUI()
    {
        inTitleScreen_Canvas.gameObject.SetActive(true);
        inGame_Canvas.gameObject.SetActive(false);
    }

    public void ActivateGameHUDUI()
    {
        inTitleScreen_Canvas.gameObject.SetActive(false);
        inGame_Canvas.gameObject.SetActive(true);
        if (_GM.demoMode) ActivateDemo();


        LoadDataIntoOrganiseStaffButtons();

        //set stats to start
        UpdatePlayerEXP();
        UpdatePlayerMoney();
        UpdatePlayerLevel(10, 1); //temp

        UpdatePurchaseButtons_Produce();
    }

    public void OpenResturant()
    {
        if(_GM.CheckIfSafeToOpen())
        {
            CloseAllPanels(null);
            _GM.playState = GameManager.PlayState.Open;
            _GM.event_playStateOpen.Invoke();

            openResturantButton_GO.SetActive(false);
        }
        
    }

    public void LoadGameScene(int state)
    {
        switch(state)
        {
            case 0:
                StartCoroutine(_SC.LoadGameScene(GameLoadState.NewGame));
                
                break;
            case 1:
                StartCoroutine(_SC.LoadGameScene(GameLoadState.LoadSaveFile));
                break;


        }
    }

    public void ErrorText(string text)
    {
        error_Text.gameObject.SetActive(true);
        error_Text.text = "ERROR: " + text;
        ExecuteAfterSeconds(3, () => error_Text.gameObject.SetActive(false));
    }

    #endregion

    #region Demo

    public void ActivateDemo()
    {
        demoPanel_GO.SetActive(!demoPanel_GO.activeSelf);

    }



    #endregion

    #region Scenes

    public void OpenGameScene()
    {
        _SC.LoadAsyncScene("Yes Chef v2");
        _SC.UnLoadScene("TitleScreen");
        _SC.SetActiveScene("Yes Chef v2");
    }

    public void OpenTitleScreenScene()
    {
        _SC.LoadAsyncScene("TitleScreen");
        _SC.UnLoadScene("Yes Chef v2");
        _SC.SetActiveScene("TitleScreen");
    }    

    #endregion

    #region Update Functions
    public void UpdateOpenDayDial(float _currentTime, float _maxTime)
    {
        openDayDial_CircularProgressBar.Percentage = (_currentTime / _maxTime) * 100;
    }

    public void UpdatePlayerLevel(int _newMaxEXPCap, int _playerlevel)
    {
        //update level
        levelValue_TMPText.text = _playerlevel.ToString();

        //update slider max value
        currentEXPValue_Slider.maxValue = _newMaxEXPCap;
    }

    public void UpdatePlayerEXP()
    {
        currentEXPValue_Slider.value = _GM.currentPlayerEXP;
    }

    public void UpdatePlayerMoney()
    {
        //add roll effect for text

        moneyValue_TMPText.text = _GM.money.ToString();
    }

    public void UpdateResturantRating()
    {
        resturantRating_Image.fillAmount = (_GM.resturantRating / 100);
    }

    public void UpdateDay()
    {
        day_Text.text = "Day " + _GM.dayCount.ToString();
    }

    public void UpdateResturantSign(string _sign)
    {
        _GM.resturantName = _sign;
        resturantSign_TMPText.text = _sign;
    }


    #endregion

    #region Open Functions
    /// <summary>
    /// Open during game phase Closed
    /// </summary>
    public void ActivateButtonPanel()
    {

        switch (_GM.playState)
        {
            //resturant open
            case GameManager.PlayState.Open:
                ordersPanel_GO.SetActive(true);

                buttonPanel_GO.GetComponent<RectTransform>().DOAnchorPos(closePos_V3, 1);
                ExecuteAfterSeconds(1, () => buttonPanel_GO.SetActive(false));
                break;
            case GameManager.PlayState.Closed:
                buttonPanel_GO.SetActive(true);
                ordersPanel_GO.SetActive(false);

                buttonPanel_GO.GetComponent<RectTransform>().DOAnchorPos(openPos_V3, 1);
                break;
        }

    }

    public void OpenChefPurchasePanel()
    {
        chefOrganisePanel_GO.SetActive(true);
        waiterOrganisePanel_GO.SetActive(false);
    }
    
    public void OpenWaiterPurchasePanel()
    {
        waiterOrganisePanel_GO.SetActive(true);
        chefOrganisePanel_GO.SetActive(false);
    }

    void ClosedButtonsActive()
    {
        openResturantButton_GO.SetActive(true);

    }

    public void ActivateStaffPurchasePanel()
    {
        CloseAllPanels(staffOrganisePanel_GO);
        staffOrganisePanel_GO.SetActive(!staffOrganisePanel_GO.activeSelf);

    }
    public void ActivatePurchaseProducePanel()
    {
        CloseAllPanels(producePanel_GO);
        //will turn on or off depending on current state
        producePanel_GO.SetActive(!producePanel_GO.activeSelf);
        produceCamera_Cam.gameObject.SetActive(!produceCamera_Cam.gameObject.activeSelf);
    }


    public void ActivateUnlockStaffPanel()
    {
        CloseAllPanels(unlockChoicePanel_GO);

        unlockChoicePanel_GO.SetActive(!unlockChoicePanel_GO.activeSelf);
    }

    public void ActivateResturantSignRenamePanel()
    {
        CloseAllPanels(renameResturantPanel_GO);
        renameResturantPanel_GO.SetActive(!renameResturantPanel_GO.activeSelf);

        //so player doesnt move cam while typing
        if(renameResturantPanel_GO.activeSelf)
            Camera.main.GetComponent<CameraController>().state = CameraController.CameraState.DisablePlayerControl;
        else Camera.main.GetComponent<CameraController>().state = CameraController.CameraState.PlayerControlled;

    }

    /// <summary>
    /// Close all panels
    /// </summary>
    /// <param name="keepOpen">Will not close this panel</param>
    public void CloseAllPanels(GameObject keepOpen)
    {
        List<GameObject> list = new List<GameObject>() { producePanel_GO, unlockChoicePanel_GO, staffOrganisePanel_GO,
                                                         produceCamera_Cam.gameObject,unlockCamera_GO, dialogBox_GO,renameResturantPanel_GO};
        if (keepOpen != null)
        {
            foreach (GameObject go in list)
            {
                if (keepOpen != go) go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in list)
            {
               go.SetActive(false);
            }
        }

        // for cameras
        produceCamera_Cam.gameObject.SetActive(producePanel_GO.activeSelf);
    }

    #endregion

    #region Orders

    public void AddOrder(OrderClass _order)
    {
        var order = Instantiate(orderPanel, orderBar_Panel);
        print("add order");
        var script = order.GetComponent<OrderTicketUI>();

        //set up visuals

        script.foodImage.sprite = _order.pfp;

        var foodData = _order.foodClass;

        //turn on produce
        script.fruit_Image.SetActive(foodData.requiredProduce_fruit > 0);
        script.veg_Image.SetActive(foodData.requiredProduce_veg > 0);
        script.dairy_Image.SetActive(foodData.requiredProduce_dairy > 0);
        script.protien_Image.SetActive(foodData.requiredProduce_protein > 0);
        script.grain_Image.SetActive(foodData.requiredProduce_grain > 0);

        //set up progress bar
        List<GameObject> progressBarSegments = new(); 

        script.progressSkillKneading_GO.SetActive(foodData.needsKneading);
        script.progressSkillCooking_GO.SetActive(foodData.needsCooking);
        script.progressSkillCutting_GO.SetActive(foodData.needsCutting);
        script.progressSkillMixing_GO.SetActive(foodData.needsMixing);
        if (foodData.needsKneading)
        {   progressBarSegments.Add(script.progressSkillKneading_GO); 
            progressBarSegments.Add(script.progressSkillKneading_Image.gameObject); 
        }
        if (foodData.needsCooking)
        {
            progressBarSegments.Add(script.progressSkillCooking_GO);
            progressBarSegments.Add(script.progressSkillCooking_Image.gameObject);
        }
        if (foodData.needsCutting)
        {
            progressBarSegments.Add(script.progressSkillCutting_GO);
            progressBarSegments.Add(script.progressSkillCutting_Image.gameObject);
        }
        if (foodData.needsMixing)
        {
            progressBarSegments.Add(script.progressSkillMixing_GO);
            progressBarSegments.Add(script.progressSkillMixing_Image.gameObject);
        }

        //update segments

        switch(progressBarSegments.Count)
        {
            case 2:

                foreach (GameObject go in progressBarSegments)
                {
                    var rt = go.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(singleWidth, rt.sizeDelta.y);
                }
                

                break;
            case 4:

                foreach (GameObject go in progressBarSegments)
                {
                    var rt2 = go.GetComponent<RectTransform>();
                    rt2.sizeDelta = new Vector2(halfWidth, rt2.sizeDelta.y);
                }

                break;
            case 6:

                foreach (GameObject go in progressBarSegments)
                {
                    var rt3 = go.GetComponent<RectTransform>();
                    rt3.sizeDelta = new Vector2(thirdsWidth, rt3.sizeDelta.y);
                }
                break;

            case 8:
                foreach (GameObject go in progressBarSegments)
                {
                    var rt4 = go.GetComponent<RectTransform>();
                    rt4.sizeDelta = new Vector2(quaterWidth, rt4.sizeDelta.y);
                }
                break;
        }

        ordersGO_List.Add(order);
    }

    public void RemoveOrder(int index)
    {
        print(index);
        //-1 means false/could not find
        if(index != -1)
        {
            var go = ordersGO_List[index];
            ordersGO_List.Remove(go);
            Destroy(go);
            UpdateOrderVisibility();

        }


    }

    void UpdateOrderVisibility()
    {
        foreach (GameObject orderTicket in ordersGO_List)
        {
            if (ordersGO_List.IndexOf(orderTicket) >= 8) orderTicket.SetActive(false);
            else orderTicket.SetActive(true);
        }
    }



    #endregion

    #region Shops

    #region Produce

    public void BuyGrainProduce()
    {
        if(_GM.money >= _FM.grainPrice_produce)
        {
            _AM.PlayUIAudio(UIAudioClips.SuccessfulPurchase);

            _FM.grainTotal_produce += 5;
            _GM.money -= _FM.grainPrice_produce;
            UpdatePlayerMoney();

            UpdatePurchaseButtons_Produce();

        }
        else
        {
            //Play audio
            _AM.PlayUIAudio(UIAudioClips.ErrorPurchase);
        }

    }
    public void BuyDairyProduce()
    {
        if(_GM.money >= _FM.dairyPrice_produce)
        {
            _AM.PlayUIAudio(UIAudioClips.SuccessfulPurchase);

            _FM.dairyTotal_produce += 5;
            _GM.money -= _FM.dairyPrice_produce;

            UpdatePlayerMoney();
            UpdatePurchaseButtons_Produce();

        }
        else
        {
            //Play audio
            _AM.PlayUIAudio(UIAudioClips.ErrorPurchase);
        }

    }
    
    public void BuyFruitProduce()
    {
        if(_GM.money >= _FM.fruitPrice_produce)
        {
            _AM.PlayUIAudio(UIAudioClips.SuccessfulPurchase);

            _FM.fruitTotal_produce += 5;
            _GM.money -= _FM.fruitPrice_produce;
            UpdatePlayerMoney();

            UpdatePurchaseButtons_Produce();

        }
        else
        {
            //Play audio
            _AM.PlayUIAudio(UIAudioClips.ErrorPurchase);
        }

    }
    
    public void BuyVegProduce()
    {
        if(_GM.money >= _FM.vegPrice_produce)
        {
            _AM.PlayUIAudio(UIAudioClips.SuccessfulPurchase);

            _FM.vegTotal_produce += 5;
            _GM.money -= _FM.vegPrice_produce;
            UpdatePlayerMoney();

            UpdatePurchaseButtons_Produce();

        }
        else
        {
            //Play audio
            _AM.PlayUIAudio(UIAudioClips.ErrorPurchase);
        }

    }
    
    public void BuyProteinProduce()
    {
        if(_GM.money >= _FM.protienPrice_produce)
        {
            _AM.PlayUIAudio(UIAudioClips.SuccessfulPurchase);
            _FM.protienTotal_produce += 5;
            _GM.money -= _FM.protienPrice_produce;
            UpdatePlayerMoney();

            UpdatePurchaseButtons_Produce();

        }
        else
        {
            //Play audio
            _AM.PlayUIAudio(UIAudioClips.ErrorPurchase);
        }
    }

    /// <summary>
    /// Update UI to reflect if player has enough money to buy the produce
    /// </summary>
    void UpdatePurchaseButtons_Produce()
    {
        grainButton_Txt.text = _FM.grainTotal_produce.ToString();

        dairyButton_Txt.text = _FM.dairyTotal_produce.ToString();

        fruitButton_Txt.text = _FM.fruitTotal_produce.ToString();

        vegButton_Txt.text = _FM.vegTotal_produce.ToString();

        proteinButton_Txt.text = _FM.protienTotal_produce.ToString();

        if (_GM.money < _FM.grainPrice_produce) grainButton_Image.color = cannotAfford_Colour;
        else grainButton_Image.color = canAfford_Colour;
        
        if (_GM.money < _FM.dairyPrice_produce) dairyButton_Image.color = cannotAfford_Colour;
        else dairyButton_Image.color = canAfford_Colour;
        
        if (_GM.money < _FM.protienPrice_produce) proteinButton_Image.color = cannotAfford_Colour;
        else proteinButton_Image.color = canAfford_Colour;
        
        if (_GM.money < _FM.fruitPrice_produce) fruitButton_Image.color = cannotAfford_Colour;
        else fruitButton_Image.color = canAfford_Colour;
        
        if (_GM.money < _FM.vegPrice_produce) vegButton_Image.color = cannotAfford_Colour;
        else vegButton_Image.color = canAfford_Colour;
    }

    #endregion

    #region Staff

    /// <summary>
    /// Activate staff GO
    /// </summary>
    /// <param name="_staff"></param>
    public void ActivateStaff(GameObject _staff)
    {
        if(_staff.activeSelf)
        {
            _SM.DeactivateStaff(_staff);

        }
        else
        {
            _SM.ActivateStaffDuringPlay(_staff);

        }
    }

    public void UnlockStaffScreen(string _staffType)
    {
        //check if any more staff can be unlocked
        if(_SM.CanHireMoreStaff(_staffType))
        {
            //so player doesnt move cam while typing
            Camera.main.GetComponent<CameraController>().state = CameraController.CameraState.DisablePlayerControl;

            unlockChoicePanel_GO.SetActive(false);

            unlockCamera_GO.SetActive(true);
            unlockScreen_GO.SetActive(true);

            var GO = _SM.HireStaff(_staffType);

            //set name
            unlockStaffName_Txt.text = GO.GetComponent<StaffData>().staffName;

            //turn on model
            foreach (var item in unlockStaffModels_ListGO)
            {
                if (item.name.Contains(unlockStaffName_Txt.text))
                {
                    item.SetActive(true);
                    break;
                }
            }

            ActivateOrganiseStaffButton(unlockStaffName_Txt.text);

            //after x seconds, close screen
            ExecuteAfterSeconds(5, () => CloseUnlockStaffScreen());

        }



    }

    void CloseUnlockStaffScreen()
    {

        unlockCamera_GO.SetActive(false);
        unlockScreen_GO.SetActive(false);
        Camera.main.GetComponent<CameraController>().ZoomCameraOut();

        foreach (var item in unlockStaffModels_ListGO)
        {
            item.SetActive(false);
        }
    }

    void ActivateOrganiseStaffButton(string name)
    {
        foreach (var item in organiseStaffButtons_GO)
        {
            if (item.name.Contains(name)) item.SetActive(true);
        }
    }

    /// <summary>
    /// Match UI if staff is toggled when bought
    /// </summary>
    public void ToggleStaffOn(GameObject staff, string staffName)
    {

        if(!_SM.totalActiveStaff.Contains(staff))
        {
            //find button
            GameObject obj = null;
            //print(staffName);
            foreach (var item in organiseStaffButtons_GO)
            {
                if (item.name.Contains(staffName)) obj = item;

            }

            print(obj.name);

            var toggle = obj.GetComponentInChildren<Toggle>();
            toggle.isOn = true;

        }

    }

    void LoadDataIntoOrganiseStaffButtons()
    {
        foreach (var button in organiseStaffButtons_GO)
        {
            var splitString = button.name.Split("-");
            string staffName = splitString[0];

            StaffData staffData = new();

            //find  staff data
            foreach (var item in _SM.allStaffData)
            {
                
                if (item.staffName == staffName)
                {
                    staffData = item;
                    break;
                }
            }

            button.GetComponent<ModularPopupOpener>().StaffData = staffData;

            var toggle = button.transform.Find("Toggle").GetComponent<Toggle>();

            //print(staffData.gameObject);

            toggle.onValueChanged.AddListener(delegate 
            {
                ActivateStaff(staffData.gameObject); 
            });

        }
    }
    #endregion


    #endregion

    #region Build Mode


    public void DisplayPreview(int ID)
    {
        //show preview above player head
    }

    public void EnterBuildMode()
    {
        //When player is placing object
    }

    public void EnableOnOverInformation(int ID)
    {
        buildHoverInfo_GO.SetActive(true);
        buildHoverInfo_GO.GetComponent<BuildOnHoverUI>().UpdateText(ID);

        buildHoverInfo_GO.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void TurnOffOnHoverInformation()
    {
        buildHoverInfo_GO.SetActive(false);
        objectHoveredOver = null;
    }

    #endregion

    #region Dialog

    //temporary
    //add param for question later
    public void LoadDialog(Dialog _dialog, string staffName, GameObject _staff)
    {
        staffDialog_GO = _staff;
        dialogStaffName_TMPText.text = staffName;

        currentDialogString_List = _dialog.list;
        dialogBox_TMPText.text = currentDialogString_List[0].ToString();

        currentDialog_Dialog = _dialog;
        currentDialogIndex = 1;
        dialogBoxAnswers_GO.SetActive(false);
        _UI.OpenDialogBox();

    }

    void LoadResponseDialog(List<string> responseList)
    {
        print("LoadResponseDialog");
        currentDialogString_List = responseList;

        print(currentDialogString_List[0].ToString());
        dialogBox_TMPText.text = currentDialogString_List[0].ToString();

        currentDialogIndex = 0;

    }

    public void UpdateDialogText()
    {
        if(currentDialogIndex != -1)
        {
            print("currentDialogIndex" + currentDialogIndex);
            //if dialoglist set
            if (currentDialogIndex < currentDialogString_List.Count)
            {
                dialogBox_TMPText.text = currentDialogString_List[currentDialogIndex].ToString();
                dialogBoxAnswers_GO.SetActive(false);

                //determine if this is a question
                if (currentDialog_Dialog.isQuestion && currentDialogIndex == currentDialog_Dialog.questionIndex)
                {
                    //turn on buttons
                    dialogBoxAnswers_GO.SetActive(true);

                    //turn off normal next question button
                    dialogBoxNextButton_GO.SetActive(false);

                    dialogBoxAnswer1_TMPText.text = currentDialog_Dialog.playerResponse1;
                    dialogBoxAnswer2_TMPText.text = currentDialog_Dialog.playerResponse2;
                }
                currentDialogIndex++;

            }
            else
            {
                CloseDialogBox();

            }
        }
        else
        {
            CloseDialogBox();

        }

    }

    public void AnswerDialogQuestion(int _response)
    {
        print("AnswerDialogQuestion " + _response);

        if(_response == 1)
            LoadResponseDialog(currentDialog_Dialog.response1);
        else if (_response == 2)
            LoadResponseDialog(currentDialog_Dialog.response2);

        //turn off buttons
        dialogBoxAnswers_GO.SetActive(false);

        //turn on normal next question button
        dialogBoxNextButton_GO.SetActive(true);

        UpdateDialogText();

    }

    public void OpenDialogBox()
    {
        buttonPanel_GO.GetComponent<RectTransform>().DOAnchorPos(closePos_V3, 1);
        ExecuteAfterSeconds(1, () => buttonPanel_GO.SetActive(false));

        openResturantButton_GO.SetActive(false);
        print("OpenDialogBox");
        dialogBox_GO.SetActive(true);

    }

    public void CloseDialogBox()
    {
        staffDialog_GO.GetComponent<StaffData>().EndDialog();
        openResturantButton_GO.SetActive(true);
        buttonPanel_GO.SetActive(true);
        buttonPanel_GO.GetComponent<RectTransform>().DOAnchorPos(openPos_V3, 1);

        Camera.main.GetComponent<CameraController>().ZoomCameraOut();

        currentDialogIndex = -1;

        dialogBox_GO.SetActive(false);
        dialogBoxAnswers_GO.SetActive(false );
    }
    #endregion

    

}
