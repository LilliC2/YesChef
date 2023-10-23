using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int dayCount;
    public float money;
    public float reputation = 100;

    [SerializeField]
    int[] foodPerWave;
    [SerializeField]
    int[] secondsInBetweenPerWave;
    public int[] conveyrbeltSpeedPerWave;
    bool waveComplete;
    public bool activeWave;

    public bool playerReady;

    public Transform[] conveyerbeltPoints;

    public List<GameObject> receipesUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        _UI.UpdateDay();
        _UI.UpdateMoney();
        _UI.UpdateReputationSlider();

    }

    // Update is called once per frame
    void Update()
    {

        if(!activeWave)
        {
            waveComplete = false;

            //check if player is ready
            if (playerReady)
            {
                playerReady = false;
                activeWave = true;
                //set conveyerbelt speed


                //spawn wave
                StartCoroutine(SummonWave(dayCount, secondsInBetweenPerWave[dayCount]));

            }
        }
        

        //check if wave is complete
        if(activeWave)
        {
            if(_FM.foodInWave.Count ==0)
            {
                print("wave done");
                activeWave = false;
                waveComplete = true;
                Time.timeScale = 1; //reset speed
            }
        }

        //update day
        if (waveComplete)
        {
            dayCount++;
            _UI.UpdateDay();
            _UI.UpdateMoney(); //shouldnt need this here but just in case;
        }
    }

    IEnumerator SummonWave(int _waveNum, int _timeBetweenFood)
    {
        for (int i = 0; i < foodPerWave[_waveNum]; i++)
        {
            _FM.InstantiateFood();
            yield return new WaitForSeconds(_timeBetweenFood);
        }
    }
}
