using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager : Singleton<EventManager>
{
    public UnityEvent event_playStateOpen;
    public UnityEvent event_playStateClose;
    public UnityEvent event_gameStatePlaying;
    public UnityEvent event_gameStatePause;
    public UnityEvent event_gameStateTitleScreen;
    public UnityEvent event_gameStateOpenGameScene;
    public UnityEvent event_playerLevelUp;

}
