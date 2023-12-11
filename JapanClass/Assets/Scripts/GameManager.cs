using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{

    [Header("Player Stats")]
    public int dayCount;
    public float money;
    public float reputation = 100;
    public List<GameObject> receipesUnlocked;

    [Header("Wave Stats")]
    public bool autoPlayEnabled = false;
    public int[] foodPerWave;
    [SerializeField]
    bool waveComplete;
    public bool activeWave;
    public bool playerReady;
    public Transform[] conveyerbeltPoints;

    [Header("Game State")]
    public float currentTimeScale;
    public enum GameState { Playing, GameOver, Pause}
    public GameState gameState;

    [Header("Events")]
    public UnityEvent event_endOfDay;
    public UnityEvent event_startOfDay;
    public UnityEvent event_foodToBeServed;

    // Start is called before the first frame update
    void Start()
    {
        _UI.UpdateDay();
        _UI.UpdateMoney();
        _UI.UpdateReputationSlider();

        event_endOfDay.AddListener(EndOfDayReset);
        event_startOfDay.AddListener(StartOfDayReset);


    }

    // Update is called once per frame
    void Update()
    {
        //activate pause
        if (Input.GetKeyDown(KeyCode.Escape)) _UI.Pause();

        switch (gameState)
        {
            #region Playing Game State
            case GameState.Playing:

                _UI.gameOverPanel.SetActive(false);



                if (!activeWave)
                {
                    waveComplete = false;

                    //print("Player Ready = " + playerReady);

                    if(!autoPlayEnabled)
                    {
                        //check if player is ready
                        if (playerReady)
                        {
                            event_startOfDay.Invoke();

                        }


                    }
                    else
                    {
                        //start wave automatically

                        event_startOfDay.Invoke();

                    }

                }


                //check if wave is complete
                if (activeWave)
                {
                    //timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

                    if (_FM.foodInWave.Count == 0 && _CustM.customersList.Count == 0)
                    {
                        print("wave done");

                        event_endOfDay.Invoke();

                    }
                }


                //reputation check
                if (reputation <= 0) gameState = GameState.GameOver;

                break;
            #endregion
            case GameState.GameOver:

                _UI.gameOverPanel.SetActive(true);
                break;

            case GameState.Pause:

                break;

        }

    }
    void StartOfDayReset()
    {
        //print("player is ready");
        playerReady = false;
        activeWave = true;
        //set conveyerbelt speed

        //_DC.CalculateRotationTime(foodPerWave[dayCount], secondsInBetweenPerWave[dayCount],conveyrbeltSpeedPerWave[dayCount]);
        //spawn wave
        StartCoroutine(SummonWave(dayCount));
    }

    void EndOfDayReset()
    {

        activeWave = false;
        waveComplete = true;
        playerReady = false;
        Time.timeScale = 1; //reset speed

        dayCount++;

    }
    public float CalculateConveyerbeltSpeed()
    {
        float b = 1.2f;
        float a = 5;
        float c = 5;
        float h = -10;
        //y=a(b^(x+h)+c
        /*y = current speed
         *a = base speed
         *x = day count
        */
        float waveSpeed = a*(Mathf.Pow(b,(dayCount+h) + c));
        return waveSpeed;
    }

    float CalculateTimeBetweenFood()
    {
        float b = 0.9f;
        float a = 1f;
        //y=a(b^x)
        /*y = seconds between
         * a = base seconds
         *x = day count
        */
        float waveTimeBetweenFood = a * Mathf.Pow(b, dayCount);
        return waveTimeBetweenFood;
    }

    IEnumerator SummonWave(int _waveNum)
    {
        if(dayCount <= foodPerWave.Length)
        {

            for (int i = 0; i < foodPerWave[_waveNum]; i++)
            {
                _FM.InstantiateFood(i);
                yield return new WaitForSeconds(CalculateTimeBetweenFood());
            }
        }


    }
}
