using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class ConfigData
{
    /// <summary>
    /// 是否显示鼠标
    /// </summary>
    public bool IsCursor;

    /// <summary>
    /// 是否打开视频播放器的射线检测
    /// </summary>
    public bool IsVideoRayCastTarget;

    /// <summary>
    /// 返回屏保模式倒计时
    /// </summary>
    public float BackTime;

    /// <summary>
    /// UDP端口号
    /// </summary>
    public int Port;

    public string IP;

    /// <summary>
    /// 接收消息的时间间隔
    /// </summary>
    public float TimeInterval;

    /// <summary>
    /// 按钮的位置信息
    /// </summary>
    public Position[] ButtonPosition;

    /// <summary>
    /// 按钮的位置信息
    /// </summary>
    public Position SwitchButtonPosition;

    /// <summary>
    /// 提示视频的位置信息
    /// </summary>
    public Position TipsVideoPosition;

    /// <summary>
    /// 切换按钮的大小信息
    /// </summary>
    public Position SwitchButtonScale;

    /// <summary>
    /// 提示视频的大小信息
    /// </summary>
    public Position TipsVideoScale;
}


[System.Serializable]
public class Position
{
    public float x;
    public float y;
}


public class Config : MonoBehaviour
{
    public static Config Instance;

    public ConfigData configData;

    private string File_name = "config.txt";
    private string Path;

    private void Awake()
    {
        Instance = this;
        configData = new ConfigData();
#if UNITY_STANDALONE_WIN
        Path = Application.streamingAssetsPath + "/" + File_name;
        if (FileHandle.Instance.IsExistFile(Path))
        {
            string st = FileHandle.Instance.FileToString(Path);
            configData = JsonConvert.DeserializeObject<ConfigData>(st);
        }
#elif UNITY_ANDROID || UNITY_IOS
        Path = Application.persistentDataPath + "/" + File_name;
        if(FileHandle.Instance.IsExistFile(Path))
        {
            string st = FileHandle.Instance.FileToString(Path);
            configData = JsonConvert.DeserializeObject<ConfigData>(st);
        }
        else
        {
            Path = Application.streamingAssetsPath + "/" + File_name;
            if (FileHandle.Instance.IsExistFile(Path))
            {
                string st = FileHandle.Instance.FileToString(Path);
                configData = JsonConvert.DeserializeObject<ConfigData>(st);
            }
        }
#endif


    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = configData.IsCursor;
        LogMsg.Instance.Log("Port==" + configData.Port);
    }

    public void SaveData()
    {
#if UNITY_ANDROID || UNITY_IOS
         Path = Application.persistentDataPath + "/" + File_name;
#endif
        string st = JsonConvert.SerializeObject(configData);
        FileHandle.Instance.SaveFile(st, Path);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
