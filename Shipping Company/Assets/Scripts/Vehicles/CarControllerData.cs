using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerData : MonoBehaviour
{
    public static CarControllerData current;
    public int turnRandomNum = 0;

    private void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        turnRandomNum = GetNewRandomNum();
    }

    public event Action onRandomCarTurn;
    public void RandomCarTurn()
    {
        onRandomCarTurn?.Invoke();
    }

    public static int GetNewRandomNum()
    {
        return UnityEngine.Random.Range(0, 2147483647);
    }
}
