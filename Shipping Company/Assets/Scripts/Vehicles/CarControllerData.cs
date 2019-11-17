using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerData : MonoBehaviour
{
    public int turnRandomNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "carControllerData";
    }

    // Update is called once per frame
    void Update()
    {
        turnRandomNum = GetNewRandomNum();
    }

    private int GetNewRandomNum()
    {
        return Random.Range(0, 2147483647);
    }
}
