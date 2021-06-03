using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using AOT;
using System.Threading;
using System.IO;
public class RhCapWrap : MonoBehaviour {

  

    private bool ifHasBegin = false;
    void Start () {
        
    }


    void Update () {
        StartCoroutine(ReturnWaitForEndOfFrame());
    }
    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (ifHasBegin) {

            RhCapNative.GetInstance().StopShare();
            Thread.Sleep(1000);
            ifHasBegin = false;
        }
    }
    IEnumerator ReturnWaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        if (!ifHasBegin)
        {
            if (RhCapNative.GetInstance().InitShare() == 0)
            {
          
                ifHasBegin = true;

            }

        }
        if (ifHasBegin)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            RhCapNative.GetInstance().SetTextureToShareMem(screenShot);
        }
       
    }
}
