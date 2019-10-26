using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_staticFuncs : MonoBehaviour
{
    public static float smooth0to1(float x)
    {
        x = Mathf.Clamp(x, 0f, 1f);
        float y = (Mathf.Sin(Mathf.PI * x - Mathf.PI /2f) + 1f) / 2f;
        return y;
    }
}
