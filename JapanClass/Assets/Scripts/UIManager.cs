using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("HUD")]
    public TMP_Text dayCount;
    public TMP_Text moneyCount;
    public Slider reputationSlider;

    [Header("Chef UI")]
    public GameObject chefMenu;
    public GameObject chefMenuButton;

    public TMP_Text nameChef0;
    public GameObject cannotAffordChef0;

    [Header("Chef PopUP UI")]
    public GameObject chefPopUp;
    public GameObject selectedChef;
    public TMP_Text chefPopUpName;


    [Header("Receipe UI")]
    public GameObject receipeMenu;
    public GameObject receipeMenuButton;


    private void Start()
    {
        LoadChefData();
    }

    public void UpdateDay()
    {
        dayCount.text = "Day " + _GM.dayCount.ToString();
    }
    
    public void UpdateMoney()
    {
        moneyCount.text = "$" + _GM.money.ToString("F2");
    }

    public void UpdateReputationSlider()
    {
        reputationSlider.value = _GM.reputation;
    }

    public void OpenChefMenu()
    {
        chefMenu.SetActive(true);
        CheckWhatPlayerCanAffordChefs();
        chefMenuButton.SetActive(false);
    }

    public void CloseChefMenu()
    {
        chefMenu.SetActive(false);
        chefMenuButton.SetActive(true);
    }
    
    public void OpenReceipeMenu()
    {
        receipeMenu.SetActive(true);
        receipeMenuButton.SetActive(false);
    }

    public void CloseReceipeMenu()
    {
        receipeMenu.SetActive(false);
        receipeMenuButton.SetActive(true);
    }

    public void OpenChefPopUp(GameObject _chefData)
    {

        selectedChef = _chefData;
        chefPopUp.SetActive(true);

        chefPopUpName.text = _chefData.GetComponent<ChefData>().chefData.name;
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
        _GM.playerReady = true;

        Time.timeScale = 1;
    }

    public void Speed2X()
    {
        Time.timeScale = 2;

    }

    public void BuyChef(int _arrayNum)
    {
        var chefToBuy = _CM.chefArray[_arrayNum];

        if (chefToBuy.GetComponent<ChefData>().chefData.hireCost <= _GM.money)
        {
            _CM.CreateNewChef(chefToBuy);

            CheckWhatPlayerCanAffordChefs();
        }
        else
        {
            print("Not enough money");
        }

        


    }

    public void LoadChefData()
    {
        nameChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.name;
    }

    public void CheckWhatPlayerCanAffordChefs()
    {
        if(_GM.money < _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.hireCost)
        {
            cannotAffordChef0.SetActive(true);
        }
        else cannotAffordChef0.SetActive(false);
    }
}
