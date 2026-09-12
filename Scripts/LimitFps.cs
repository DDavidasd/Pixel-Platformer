using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFps : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60; //120 60 stb
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
