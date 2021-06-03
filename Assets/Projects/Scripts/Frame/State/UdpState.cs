using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;
using System;

namespace MTFrame
{
    public enum PanelName
    {
        WaitPanel,
    }

    public enum EventType
    {
        /// <summary>
        /// 切换页面
        /// </summary>
        PanelSwitch,

        /// <summary>
        /// Udp转换消息传出去
        /// </summary>
        DataToPanel,
    }
}


public class UdpState : BaseState
{
    //注意state一定要在get里面监听事件，没有的话就写成下面样子
    //这里一般用来监听Panel切换
    
    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {
                //事件名string类型
                MTFrame.EventType.PanelSwitch.ToString(),
            };
        }
        set { }
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {

        //接收监听事件的数据，然后用swich判断做处理

        if (parameteData.EvendName == MTFrame.EventType.PanelSwitch.ToString())
        {
            PanelName panelName = parameteData.GetParameter<PanelName>()[0];
            switch (panelName)
            {
                case PanelName.WaitPanel:
                    
                    break;
                default:
                    break;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new UdpTask(this));
    }


    /// <summary>
    /// 切换UI Panel
    /// </summary>
    /// <param name="panelName"></param>
    public static void SwitchPanel(PanelName panelName)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter(panelName);
        EventManager.TriggerEvent(GenericEventEnumType.Message, MTFrame.EventType.PanelSwitch.ToString(), eventParamete);
    }
}
