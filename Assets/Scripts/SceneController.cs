using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public enum GameLoadState { NewGame, LoadSaveFile}
    // Start is called before the first frame update
    void Start()
    {
        //print(SceneManager.GetActiveScene().name);
    }

    public void LoadingScreen()
    {
        LoadAsyncScene("LoadingScene");
    }


    public IEnumerator LoadGameScene(GameLoadState gameLoadState)
    {
        //unload titlescreen
        UnLoadScene("TitleScene");
        print("LoadGameScene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_GM.gameSceneName, LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        if (asyncLoad.isDone)
        {
            print("Game Scene Loaded");

            _GM.gameState = GameManager.GameState.Playing;
            _GM.event_gameStateOpenGameScene.Invoke();

            switch(gameLoadState)
            {
                case GameLoadState.NewGame:
                    print("New game");
                    _SAVEM.NewSaveFile();
                    break;
                    case GameLoadState.LoadSaveFile:
                    print("Load previous save");
                    _SAVEM.LoadGame();
                    break;
            }

            UnLoadScene("LoadingScene");

        }

    }

    public void LoadEssentials()
    {
        Scene essentials = SceneManager.GetSceneByName("Essentials");
        SceneManager.LoadSceneAsync(1);
    }

    public bool CheckIfSceneIsLoaded(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    public void SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public void UnLoadScene(string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded) SceneManager.UnloadSceneAsync(sceneName);

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void LoadAsyncScene(string sceneName)
    {

        if (!SceneManager.GetSceneByName(sceneName).isLoaded) StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if(asyncLoad.isDone && sceneName != "LoadingScene")
        {
            yield return true;

            UnLoadScene("LoadingScene");
        }

    }


}
