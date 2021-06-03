using UnityEngine;
/// <summary>
/// 入口类
/// </summary>
public class Main : MonoBehaviour
{
    public static Main Instance;
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    [Header("屏幕分辨率")]
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    /// <summary>
    /// 是否全屏
    /// </summary>
    [Header("是否全屏")]
    public bool fullScreen = true;

    //[Header("是否显示鼠标图标")]
    //[SerializeField]
    
    //private bool isCursor=true;
    //public bool IsCursor
    //{
    //    get { return isCursor; }
    //    set
    //    {
    //        isCursor = value;
    //        Cursor.visible = isCursor;
    //    }
    //}

#endif
    //[Header("是否显示Debug面板")]
    //[SerializeField]
    //private bool isDebug;
    //public bool IsDebug
    //{
    //    get { return isDebug; }
    //    set
    //    {
    //        isDebug = value;
    //        Debuger.Instance.IsVisible = isDebug;
    //    }
    //}


    void Awake()
    {
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

#if UNITY_STANDALONE_WIN
        Screen.SetResolution(resolution.x, resolution.y, fullScreen);
#endif
       
        //AudioManager.Init();//音效初始化

    }
    private void Start()
    {
        Init();
    }
    /// <summary>
    ///初始化
    /// </summary>
    public void Init()
    {

//#if UNITY_STANDALONE_WIN
//        IsCursor = isCursor;
//#endif
        //在这里更改场景入口
        StateManager.ChangeState(new UdpState());

    }

    private void Update()
    {

    }

    /// <summary>
    /// 退出
    /// </summary>
    public void Quit()
    {
        Debug.Log("退出");
        Application.Quit();
    }
}
