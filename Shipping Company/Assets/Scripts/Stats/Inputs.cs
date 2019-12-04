using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Inputs get;
    private KeyCode[] upKeys = { KeyCode.UpArrow, KeyCode.W };
    private KeyCode[] downKeys = { KeyCode.DownArrow, KeyCode.S };
    private KeyCode[] leftKeys = { KeyCode.LeftArrow, KeyCode.A };
    private KeyCode[] rightKeys = { KeyCode.RightArrow, KeyCode.D };

    private KeyCode[] rotateClockwiseKeys = { KeyCode.RightBracket, KeyCode.E };
    private KeyCode[] rotateCounterClockwiseKeys = { KeyCode.LeftBracket, KeyCode.Q };

    private KeyCode[] zoomInKeys = { KeyCode.Plus, KeyCode.R };
    private KeyCode[] zoomOutKeys = { KeyCode.Minus, KeyCode.F };

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;

    public bool rotClock = false;
    public bool rotCounterClock = false;

    public bool zoomIn = false;
    public bool zoomOut = false;

    private void Awake()
    {
        get = this;
    }

    void Update()
    {
        GetMotionInputs();
    }

    private void GetMotionInputs()
    {
        up = IsKeyPressed(upKeys);
        down = IsKeyPressed(downKeys);
        left = IsKeyPressed(leftKeys);
        right = IsKeyPressed(rightKeys);

        rotClock = IsKeyPressed(rotateClockwiseKeys);
        rotCounterClock = IsKeyPressed(rotateCounterClockwiseKeys);

        zoomIn = IsKeyPressed(zoomInKeys);
        zoomOut = IsKeyPressed(zoomOutKeys);
    }

    private bool IsKeyPressed(KeyCode[] keysArr)
    {
        bool pressed = false;
        foreach (KeyCode key in keysArr)
        {
            pressed = pressed || Input.GetKey(key);
        }
        return pressed;
    }
}
