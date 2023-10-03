using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("Tutorial UI")]
    public GameObject recipeCheckOB;
    bool recipeBought;


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

    [Header("Receipe 0")]
    public TMP_Text nameReceipe0;
    public TMP_Text orderCostReceipe0;
    public GameObject cannotAffordReceipe0;
    public GameObject soldReceipe0;

    [Header("Receipe 1")]
    public TMP_Text nameReceipe1;
    public TMP_Text orderCostReceipe1;
    public GameObject cannotAffordReceipe1;
    public GameObject soldReceipe1;


    private void Start()
    {
        LoadChefData();

        LoadReceipeData();
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
        CheckWhatPlayerCanAffordReceipes();
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
        if (!recipeBought) CheckForRecipes();
        else
        {
            _GM.playerReady = true;

            Time.timeScale = 1;
        }


    }

    public void Speed2X()
    {
        Time.timeScale = 2;

    }

    public void BuyChef(int _arrayNum)
    {
        if(!_GM.activeWave)
        {
            var chefToBuy = _CM.chefArray[_arrayNum];

            if (chefToBuy.GetComponent<ChefData>().chefData.hireCost <= _GM.money)
            {
                _CM.CreateNewChef(chefToBuy);

                CheckWhatPlayerCanAffordChefs();
            }

        }    

    }


    public void BuyReceipe(int _arrayNum)
    {
        if(!_GM.activeWave)
        {
            var receipeToBuy = _FM.foodArray[_arrayNum];

            if (receipeToBuy.GetComponent<FoodData>().foodData.unlockCost <= _GM.money)
            {
                _GM.receipesUnlocked.Add(receipeToBuy);

                _GM.money -= receipeToBuy.GetComponent<FoodData>().foodData.unlockCost;

                UpdateMoney();

                CheckWhatPlayerCanAffordReceipes();
            }
        }
    }

    public void LoadChefData()
    {
        nameChef0.text = _CM.chefArray[0].gameObject.GetComponent<ChefData>().chefData.name;
    }

    public void LoadReceipeData()
    {
        nameReceipe0.text = _FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.name;
        orderCostReceipe0.text = "$"+_FM.foodArray[0].gameObject.GetComponent<FoodData>().foodData.orderCost.ToString("F2");
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

        
    }

    public void CheckForRecipes()
    {
        if (_GM.receipesUnlocked.Count == 0)
        {
            recipeCheckOB.SetActive(true);

            ExecuteAfterSeconds(3, () => recipeCheckOB.SetActive(false));
        }
        else recipeBought = true;
    }
}
