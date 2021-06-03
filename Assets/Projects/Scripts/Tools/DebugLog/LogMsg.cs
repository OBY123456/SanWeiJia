using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMsg : MonoBehaviour
{
    public static LogMsg Instance;

    [Header("是否输出Debug信息")]
    public bool IsLog = true;

    private void Awake()
    {
        Instance = this;
    }

    public void Log(string str)
    {
        if(IsLog)
        {
            Debug.Log(str);
        }   
    }
}
