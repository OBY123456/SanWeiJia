using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using UnityEngine.Video;
using RenderHeads.Media.AVProVideo;
using DG.Tweening;

enum VideoName
{
    Video = 0,
    Video1 = 1,
    Video2 = 2,
    Video3 = 3,
    Video4 = 4,
    Video5 = 5,
}

public enum VideoModel
{
    屏保模式,
    点播模式,
}

// 顺序：数字化营销--3D云设计软件--数字供应链--3D云制造软件--数控系统——五合一 —— 模式切换 === 0~6

public class UdpPanel : BasePanel
{
    public MediaPlayer[] videoPlayers;

    public MediaPlayer PingBaoVideo;

    public Button[] buttons;

    public Image SwitchButton;

    public CanvasGroup Tips;

    public string[] VideoPath;

    public VideoModel videoModel;

    protected override void Start()
    {
        base.Start();
        if(Config.Instance.configData.ButtonPosition != null && Config.Instance.configData.ButtonPosition.Length > 0)
        {
            for (int i = 0; i < Config.Instance.configData.ButtonPosition.Length; i++)
            {
                Vector3 vector = new Vector3(Config.Instance.configData.ButtonPosition[i].x, Config.Instance.configData.ButtonPosition[i].y, 0);
                buttons[i].gameObject.GetComponent<RectTransform>().anchoredPosition = vector;
                if (Config.Instance.configData.IsVideoRayCastTarget)
                {
                    buttons[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    buttons[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }
        else
        {
            Position[] position = new Position[buttons.Length];
            Config.Instance.configData.ButtonPosition = position;
        }

        if(Config.Instance)
        {
            Vector3 vector = new Vector3(Config.Instance.configData.SwitchButtonPosition.x, Config.Instance.configData.SwitchButtonPosition.y, 0);
            SwitchButton.gameObject.GetComponent<RectTransform>().anchoredPosition = vector;

            if (Config.Instance.configData.IsVideoRayCastTarget)
            {
                SwitchButton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                SwitchButton.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        CountDwon = Config.Instance.configData.TimeInterval;

        BackTime = Config.Instance.configData.BackTime;

        Tips.gameObject.GetComponent<MediaPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "AVProVideoSamples/拍我一下.mp4");

        Enter_PingBao_Model();
    }

    public override void InitFind()
    {
        base.InitFind();
        videoPlayers = FindTool.FindChildNode(transform, "Videos").GetComponentsInChildren<MediaPlayer>();
        buttons = FindTool.FindChildNode(transform, "Buttons").GetComponentsInChildren<Button>();
        SwitchButton = FindTool.FindChildComponent<Image>(transform, "SwitchButton");
        Tips = FindTool.FindChildComponent<CanvasGroup>(transform, "Tips");
        PingBaoVideo = FindTool.FindChildComponent<MediaPlayer>(transform, "PingBaoVideo");
    }

    public override void InitEvent()
    {
        base.InitEvent();

        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    InitButton(buttons[i], i);
        //}

        foreach (var item in videoPlayers)
        {
            item.Events.AddListener(Complete) ;
        }

        //SwitchButton.onClick.AddListener(() => {
        //    if (videoModel == VideoModel.点播模式)
        //    {
        //        Enter_PingBao_Model();
        //    }
        //    else
        //    {
        //        Enter_DianBo_Model();
        //    }
        //});
    }

    private void Complete(MediaPlayer arg0, MediaPlayerEvent.EventType arg1, ErrorCode arg2)
    {
        switch (arg1)
        {
            case MediaPlayerEvent.EventType.FinishedPlaying:
                arg0.Stop();
                arg0.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                Tips.alpha = 1;
                PingBaoStart();
                VideoName name = (VideoName)Enum.Parse(typeof(VideoName), arg0.name);
                switch (name)
                {
                    case VideoName.Video:
                        Button_Open(0);
                        break;
                    case VideoName.Video1:
                        Button_Open(1);
                        break;
                    case VideoName.Video2:
                        Button_Open(2);
                        break;
                    case VideoName.Video3:
                        Button_Open(3);
                        break;
                    case VideoName.Video4:
                        Button_Open(4);
                        break;
                    case VideoName.Video5:

                        for (int i = 0; i < buttons.Length; i++)
                        {
                            Button_Open(i);
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 打开水波纹
    /// </summary>
    /// <param name="number"></param>
    private void Button_Open(int number)
    {
        buttons[number].gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    //private void InitButton(Button button,int number)
    //{
    //    if(number < videoPlayers.Length)
    //    button.onClick.AddListener(() => {
    //        if (videoModel == VideoModel.点播模式)
    //        PlayVideo(number);
            
    //    });
    //}

    private void PlayVideo(int number)
    {
        PingBaoClose();

        foreach (var item in videoPlayers)
        {
            item.Stop();
            item.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        if(number < buttons.Length - 1)
        {
            foreach (var item in buttons)
            {
                item.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            }
            buttons[number].gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            foreach (var item in buttons)
            {
                item.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
        Tips.alpha = 0;
        videoPlayers[number].gameObject.GetComponent<CanvasGroup>().alpha = 1;
        videoPlayers[number].OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[number]);
    }

    public override void Open()
    {
        base.Open();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, MTFrame.EventType.DataToPanel.ToString(), callback);
    }

    public override void Hide()
    {
        base.Hide();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, MTFrame.EventType.DataToPanel.ToString(), callback);
    }

    private float CountDwon;
    private bool IsComplete = true;
    private void callback(EventParamete parameteData)
    {
        //如果需要判断事件名就用parameteData.EvendName
        //如果需要判断数据内容就用string data = parameteData.GetParameter<string>()[0];
        //要接收什么类型的数据就定义什么类型的数据，这里只会获取你选择的数据类型的数据
        string data = parameteData.GetParameter<string>()[0];
        switch (data)
        {
            case "5":
                if (IsComplete)
                {
                    StartCoroutine("CountDown");
                    if(videoModel == VideoModel.屏保模式)
                    {
                        videoModel = VideoModel.点播模式;
                        PingBaoVideo.Stop();
                        PingBaoVideo.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    }

                    if (videoPlayers[5].Control.IsPlaying())
                    {
                        Reset();
                        for (int i = 0; i < buttons.Length; i++)
                        {
                            Button_Open(i);
                            PingBaoStart();
                        }
                    }
                    else
                    {
                        PlayVideo(5);
                    }
                }

                break;

            case "6":
                if (IsComplete)
                {
                    StartCoroutine("CountDown");
                    if (videoModel == VideoModel.点播模式)
                    {
                        Enter_PingBao_Model();
                        PingBaoClose();
                    }
                    else
                    {
                        Enter_DianBo_Model();
                        PingBaoStart();
                    }
                }
                break;
            default:
                break;
        }


        if (videoModel == VideoModel.点播模式)
        {           
            switch (data)
            {
                case "0":
                    if (!videoPlayers[0].Control.IsPlaying() && !videoPlayers[5].Control.IsPlaying())
                        PlayVideo(0);
                    break;
                case "1":
                    if (!videoPlayers[1].Control.IsPlaying() && !videoPlayers[5].Control.IsPlaying())
                        PlayVideo(1);
                    break;
                case "2":
                    if (!videoPlayers[2].Control.IsPlaying() && !videoPlayers[5].Control.IsPlaying())
                        PlayVideo(2);
                    break;
                case "3":
                    if (!videoPlayers[3].Control.IsPlaying() && !videoPlayers[5].Control.IsPlaying())
                        PlayVideo(3);
                    break;
                case "4":
                    if (!videoPlayers[4].Control.IsPlaying() && !videoPlayers[5].Control.IsPlaying())
                        PlayVideo(4);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 设置消息接收间隔
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountDown()
    {
        IsComplete = false;
        yield return new WaitForSeconds(CountDwon);
        IsComplete = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        //保存位置信息
        if(Config.Instance)
        {
            for (int i = 0; i < Config.Instance.configData.ButtonPosition.Length; i++)
            {
                Config.Instance.configData.ButtonPosition[i].x = buttons[i].GetComponent<RectTransform>().anchoredPosition.x;
                Config.Instance.configData.ButtonPosition[i].y = buttons[i].GetComponent<RectTransform>().anchoredPosition.y;
            }

            Config.Instance.configData.SwitchButtonPosition.x = SwitchButton.GetComponent<RectTransform>().anchoredPosition.x;
            Config.Instance.configData.SwitchButtonPosition.y = SwitchButton.GetComponent<RectTransform>().anchoredPosition.y;

            Config.Instance.SaveData();
        }
    }

    private void Reset()
    {
        foreach (var item in videoPlayers)
        {
            item.Stop();

            item.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    private void Enter_PingBao_Model()
    {
        videoModel = VideoModel.屏保模式;
        Reset();
        Tips.alpha = 1;

        foreach (var item in buttons)
        {
            item.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }

        PingBaoVideo.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[VideoPath.Length - 1]);
        PingBaoVideo.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void Enter_DianBo_Model()
    {
        videoModel = VideoModel.点播模式;
        Tips.alpha = 1;
        PingBaoVideo.Stop();

        PingBaoVideo.gameObject.GetComponent<CanvasGroup>().alpha = 0;

        foreach (var item in buttons)
        {
            item.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

    }

    /** 视频播放完成返回屏保模式 **/
    private float BackTime = 5;
    private float Back_Time;
    private bool IsBack = false;

    private void Update()
    {
        if (Back_Time > 0 && IsBack)
        {
            Back_Time -= Time.deltaTime;
            if (Back_Time <= 0)
            {
                IsBack = false;
                Enter_PingBao_Model();
            }
        }
    }

    private void PingBaoStart()
    {
        IsBack = true;
        Back_Time = BackTime;
    }

    private void PingBaoClose()
    {
        IsBack = false;
        Back_Time = BackTime;
    }
}
