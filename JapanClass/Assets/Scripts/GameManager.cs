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
    bool waveComplete;
    bool activeWave;

    public bool playerReady;
    

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
        if (Input.GetKeyDown(KeyCode.Space)) playerReady = true;

        if(!activeWave)
        {
            //check if player is ready
            if (playerReady)
            {
                playerReady = false;
                activeWave = true;
                //spawn wave
                StartCoroutine(SummonWave(dayCount));

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
    }

    IEnumerator SummonWave(int _waveNum)
    {
        for (int i = 0; i < foodPerWave[_waveNum]; i++)
        {
            _FM.InstantiateFood();
            yield return new WaitForSeconds(2);
        }
    }
}
