using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents events;
    // Start is called before the first frame update
    void Awake()
    {
        events = this;
    }

    public event Action onPauseGame;
    public event Action onUnpauseGame;

    public void OnPauseGame()
    {
        if(onPauseGame != null)
        {
            onPauseGame();
        }
    }

    public void OnUnpauseGame()
    {
        if (onPauseGame != null)
        {
            onUnpauseGame();
        }
    }
}
