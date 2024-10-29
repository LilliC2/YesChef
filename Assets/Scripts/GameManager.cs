using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [Header("Debug")]
    public bool demoMode;
    public string gameSceneName;

    [Header("Player Progress")]
    public int dayCount;
    public float money;
    public string resturantName;

    public int playerLevel;
    public int currentPlayerEXP; //how much exp the player currently has
    int nextLvlEXPCap; //how much EXP the player needs to level up

    public float resturantRating;

    //[Header("Game States")]
    public enum PlayState { Open, Closed };
    public PlayState playState; //while game is actively being played
    public enum GameState { Playing, Paused, Title}
    public GameState gameState;

    public float openDayLength = 300; //300 = 5 minutes
    public float currentTime_OpenDay; 
    float currentTime_lerp;

    //called to change states
    public UnityEvent event_playStateOpen;
    public UnityEvent event_playStateClose;
    public UnityEvent event_gameStatePlaying;
    public UnityEvent event_gameStatePause;
    public UnityEvent event_gameStateTitleScreen;
    public UnityEvent event_gameStateOpenGameScene;
    public UnityEvent event_playerLevelUp;



    private void Start()
    {
        

        event_playerLevelUp.AddListener(PlayerLevelUp);
        event_playStateClose.AddListener(ClosedResturant);
        _UI.UpdateResturantRating();

        _GM.playState = PlayState.Closed;
        event_playStateClose.Invoke();
    }

    private void Update()
    {

        #region Debug

        if(Input.GetKeyUp(KeyCode.F) && !_GM.demoMode)
        {
            playState = PlayState.Closed;
            event_playStateClose.Invoke();
        }

        #endregion

        switch (gameState)
        {
            case GameState.Playing:

                switch (playState)
                {
                    case PlayState.Open:

                        if (currentTime_lerp <= openDayLength)
                        {
                            currentTime_lerp += Time.deltaTime;
                            currentTime_OpenDay = Mathf.Lerp(0, openDayLength, currentTime_lerp / openDayLength);

                            _UI.UpdateOpenDayDial(currentTime_OpenDay, openDayLength);

                        }
                        else
                        {
                            if(_CustM.customersInResturant.Count == 0)
                            {
                                playState = PlayState.Closed;
                                event_playStateClose.Invoke();
                            }
                            
                        }


                        break;
                }

                break;
        }
       
    }

    public void PlayerMoneyIncrease(float _amount)
    {
        money += _amount;
        _UI.UpdatePlayerMoney();
    }

    void ClosedResturant()
    {
        dayCount++;
        _UI.UpdateDay();
    }

    void PlayerLevelUp()
    {
        //reset current exp to 0
        currentPlayerEXP = 0;
        _UI.UpdatePlayerEXP();

        _UI.UpdatePlayerLevel(nextLvlEXPCap, playerLevel);
    }

    public void PlayerGainEXP(int _gainAmount)
    {
        currentPlayerEXP += _gainAmount;

        //if leveled up
        if(currentPlayerEXP >= nextLvlEXPCap)
        {
            event_playerLevelUp.Invoke();
        }
        else
        {
            _UI.UpdatePlayerEXP();
        }
    }

    /// <summary>
    /// Check if all nessecary requirements are met to open the resturant
    /// </summary>
    public bool CheckIfSafeToOpen()
    {
        bool safe = true;
        if (_SM.ReturnActiveChefCount() < 1)
        {
            safe = false;
            _UI.ErrorText("No chefs active");
        }
        if (_SM.ReturnActiveWaiterCount() < 1)
        {
            safe = false;
            _UI.ErrorText("No waiters active");
        }
        if (_FM.menu.Count < 1)
        {
            safe = false;
            _UI.ErrorText("No menu items");
        }

        int totalOfAllProduce = _FM.grainTotal_produce + _FM.dairyTotal_produce + _FM.fruitTotal_produce + _FM.vegTotal_produce + _FM.protienTotal_produce;

        if (totalOfAllProduce == 0)
        {
            safe = false;
            _UI.ErrorText("No produce bought");
        }


        return safe;
    }

}
