using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{

    [Header("Player Progress")]
    public int dayCount;
    public float money;

    public int playerLevel;
    public int currentPlayerEXP; //how much exp the player currently has
    int nextLvlEXPCap; //how much EXP the player needs to level up

    public float resturantRating;

    //[Header("Game States")]
    public enum PlayState { Open, Closed };
    public PlayState playState; //while game is actively being played
    public enum GameState { Playing, Paused}
    public GameState gameState;

    public float openDayLength = 300; //300 = 5 minutes
    public float currentTime_OpenDay; 
    float currentTime_lerp;

    //called to change states
    public UnityEvent event_playStateOpen;
    public UnityEvent event_playStateClose;
    public UnityEvent event_gameStatePlaying;
    public UnityEvent event_gameStatePause;
    public UnityEvent event_playerLevelUp;

    [Header("Moving Food")]
    public Transform[] conveyerbeltPoints;



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
        switch(gameState)
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
        if(_SM.chefActiveStaff.Count < 1)
        {
            safe = false;
            _UI.ErrorText("No chefs active");
        }
        if(_SM.waiterActiveStaff.Count < 1)
        {
            safe = false;
            _UI.ErrorText("No waiters active");
        }
        if(_FM.menu.Count < 1)
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

    //public float reputation = 100;
    //public List<GameObject> receipesUnlocked;

    //[Header("Wave Stats")]
    //public bool autoPlayEnabled = false;
    //public int[] foodPerWave;
    //[SerializeField]
    //bool waveComplete;
    //public bool activeWave;
    //public bool playerReady;
    //public Transform[] conveyerbeltPoints;

    //[Header("Layers")]
    //public LayerMask rawFood;
    //[Header("Game State")]
    //public float currentTimeScale;
    //public enum GameState { Playing, GameOver, Pause}
    //public GameState gameState;

    //[Header("Events")]
    //public UnityEvent event_endOfDay;
    //public UnityEvent event_startOfDay;
    //public UnityEvent event_foodToBeServed;
    //public UnityEvent event_updateMoney;
    //public UnityEvent event_kneadForSpeed;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    LoadShaders();
    //    currentTimeScale = 1;
    //    _UI.UpdateDay();
    //    _GM.event_updateMoney.Invoke();
    //    _UI.UpdateReputationSlider();

    //    event_endOfDay.AddListener(EndOfDayReset);
    //    event_startOfDay.AddListener(StartOfDayReset);


    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //activate pause
    //    if (Input.GetKeyDown(KeyCode.Escape)) _UI.Pause();

    //    switch (gameState)
    //    {
    //        #region Playing Game State
    //        case GameState.Playing:

    //            _UI.gameOverPanel.SetActive(false);



    //            if (!activeWave)
    //            {
    //                waveComplete = false;

    //                //print("Player Ready = " + playerReady);

    //                if(!autoPlayEnabled)
    //                {
    //                    //check if player is ready
    //                    if (playerReady)
    //                    {
    //                        print("start of day");
    //                        event_startOfDay.Invoke();

    //                    }


    //                }
    //                else
    //                {
    //                    //start wave automatically
    //                    print("start of day");
    //                    event_startOfDay.Invoke();

    //                }

    //            }


    //            //check if wave is complete
    //            if (activeWave)
    //            {
    //                //timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

    //                if (_FM.foodInWave.Count == 0 && _CustM.customersList.Count == 0)
    //                {
    //                    print("wave done");

    //                    event_endOfDay.Invoke();

    //                    if (dayCount == 20) print("Tutorial compelte");


    //                }
    //            }


    //            //reputation check
    //            if (reputation <= 0) gameState = GameState.GameOver;

    //            break;
    //        #endregion
    //        case GameState.GameOver:



    //            _UI.gameOverPanel.SetActive(true);
    //            break;

    //        case GameState.Pause:

    //            break;

    //    }

    //}
    //void StartOfDayReset()
    //{
    //    //print("player is ready");
    //    playerReady = false;
    //    activeWave = true;
    //    //set conveyerbelt speed

    //    //_DC.CalculateRotationTime(foodPerWave[dayCount], secondsInBetweenPerWave[dayCount],conveyrbeltSpeedPerWave[dayCount]);
    //    //spawn wave
    //    ExecuteAfterSeconds(2, () => StartCoroutine(SummonWave(dayCount)));

    //}

    //public void UpdateTimeScale(int  timeScale)
    //{
    //    Time.timeScale = timeScale;
    //}

    //void EndOfDayReset()
    //{

    //    activeWave = false;
    //    waveComplete = true;
    //    playerReady = false;
    //    Time.timeScale = 1; //reset speed

    //    dayCount++;

    //}
    //public float CalculateConveyerbeltSpeed()
    //{
    //    float b = 1.2f;
    //    float a = 5;
    //    float c = 5;
    //    float h = -10;
    //    //y=a(b^(x+h)+c
    //    /*y = current speed
    //     *a = base speed
    //     *x = day count
    //    */
    //    float waveSpeed = a*(Mathf.Pow(b,(dayCount+h) + c));
    //    return waveSpeed;
    //}

    //float CalculateTimeBetweenFood()
    //{
    //    float b = 0.9f;
    //    float a = 1f;
    //    //y=a(b^x)
    //    /*y = seconds between
    //     * a = base seconds
    //     *x = day count
    //    */
    //    float waveTimeBetweenFood = a * Mathf.Pow(b, dayCount);
    //    return waveTimeBetweenFood;
    //}

    //IEnumerator SummonWave(int _waveNum)
    //{
    //    if(dayCount <= foodPerWave.Length)
    //    {

    //        for (int i = 0; i < foodPerWave[_waveNum]; i++)
    //        {
    //            //give buffer for orders to come in
    //            _FM.InstantiateFood(i);
    //            //if (i == 0)
    //            //else _FM.InstantiateFood(i);

    //            yield return new WaitForSeconds(CalculateTimeBetweenFood());
    //        }
    //    }


    //}

    //void LoadShaders()
    //{
    //    Resources.Load("Shaders/Illustrate Alpha Clipped");
    //    Resources.Load("Shaders/Illustrate Opaque Traditional Outline");
    //    Resources.Load("Shaders/Illustrate Opaque Traditional Outline");
    //    Resources.Load("Shaders/Illustrate Opaque");
    //    Resources.Load("Shaders/Illustrate Simple");
    //    Resources.Load("Shaders/Illustrate Transparent");
    //}
}
