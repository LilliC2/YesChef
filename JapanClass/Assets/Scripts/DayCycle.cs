using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DayCycle : Singleton<DayCycle>
{
    public bool beginRotation;
    float waveTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_GM.activeWave)
        {
            if(!beginRotation)
            {
                beginRotation = true;

                transform.DORotate(new Vector3(180, 0, 0), waveTime);

            }
        }

        
        //calculate time from start of wave until end of wave

        //foodInWave * secondsBetweenWave = time it takes for all the food to appear



        //time with food speed 1 and secondsbetween wave 1 = 36

        //calculate time for final food to reach end of the wave

        //add them together

        //lerp sun with that time
    }

    public void CalculateRotationTime(int _foodInWave, int _secondsBetweenFood, float _conveyerbeltSpeed)
    {
        var timeForFoodToAllSpawn = _foodInWave * _secondsBetweenFood;

        var travelTime = 40 / _conveyerbeltSpeed;

        waveTime = timeForFoodToAllSpawn + travelTime;


    }
}
