using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int dayCount;
    public float money;
    public float reputation = 100;

    public bool autoPlayEnabled = false;

    [SerializeField]
    int[] foodPerWave;
    [SerializeField]
    //float[] secondsInBetweenPerWave;
    //public float[] conveyrbeltSpeedPerWave;
    bool waveComplete;
    public bool activeWave;

    public bool playerReady;

    public Transform[] conveyerbeltPoints;

    public List<GameObject> receipesUnlocked;

    private float initializationTime;

    public enum GameState { Playing, GameOver, Pause}
    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        _UI.UpdateDay();
        _UI.UpdateMoney();
        _UI.UpdateReputationSlider();
        initializationTime = Time.timeSinceLevelLoad;

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
                            //print("player is ready");
                            playerReady = false;
                            activeWave = true;
                            //set conveyerbelt speed

                            //_DC.CalculateRotationTime(foodPerWave[dayCount], secondsInBetweenPerWave[dayCount],conveyrbeltSpeedPerWave[dayCount]);
                            //spawn wave
                            if (dayCount < 12)
                            {
                                StartCoroutine(SummonWave(dayCount));

                            }
                            else
                            {
                                Time.timeScale = 1;
                                _UI.completePanel.SetActive(true);
                            }
                        }

                    
                    }
                    else
                    {
                        //start wave automatically

                        playerReady = false;
                        activeWave = true;
                        //set conveyerbelt speed

                        //_DC.CalculateRotationTime(foodPerWave[dayCount], secondsInBetweenPerWave[dayCount],conveyrbeltSpeedPerWave[dayCount]);
                        //spawn wave

                        StartCoroutine(SummonWave(dayCount));
                    }
                    
                }


                //check if wave is complete
                if (activeWave)
                {
                    //timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

                    if (_FM.foodInWave.Count == 0)
                    {
                        print("wave done");
                        activeWave = false;
                        waveComplete = true;
                        playerReady = false;
                        Time.timeScale = 1; //reset speed
                    }
                }

                //update day
                if (waveComplete)
                {
                    //reset day cycle
                    //_DC.transform.rotation = new Quaternion(10, 0, 0,0);
                    //_DC.beginRotation = false;

                    dayCount++;
                    _UI.UpdateDay();
                    _UI.UpdateMoney(); //shouldnt need this here but just in case;
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
                _FM.InstantiateFood();
                yield return new WaitForSeconds(CalculateTimeBetweenFood());
            }
        }


    }
}
