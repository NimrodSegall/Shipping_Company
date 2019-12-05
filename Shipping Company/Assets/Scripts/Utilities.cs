using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static Texture2D RotateTextureClockwise(Texture2D originalTexture, int times)
    {
        Texture2D rotatingTexture = originalTexture;
        for (int ii = 0; ii < times; ii++)
        {
            Color32[] original = rotatingTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = rotatingTexture.width;
            int h = rotatingTexture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = original.Length - 1 - (j * w + i);
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            rotatingTexture = rotatedTexture;
        }
        return rotatingTexture;
    }

    public static bool IsAlmostOne(float num)
    {
        if (num > 0.98f && num < 1.02f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsInArray<T>(T[] arr, T val) where T : class
    {
        foreach(T member in arr)
        {
            if(val == member)
            {
                return true;
            }
        }
        return false;
    }

    public static string FormatTime(int timeInt)
    {
        string time = "";
        if ((timeInt / 10) < 1)
        {
            time = time + "0";
        }
        time = time + timeInt + ":00";
        return time;
    }

}
