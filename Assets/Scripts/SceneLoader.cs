using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : GameBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //if Essentials Scene is not loaded
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            _SC.LoadingScreen();

            _SC.LoadAsyncScene("Essentials");

        }
        else
        {

            switch (SceneManager.GetActiveScene().name)
            {
                case "TitleScene":

                    print("Scene");
                    _GM.gameState = GameManager.GameState.Title;
                    _GM.event_gameStateTitleScreen.Invoke();
                    break;
                case "GameScene":

                    _GM.gameState = GameManager.GameState.Playing;
                    _GM.event_gameStateOpenGameScene.Invoke();
                    break;
            }
        }

        
    }


}
