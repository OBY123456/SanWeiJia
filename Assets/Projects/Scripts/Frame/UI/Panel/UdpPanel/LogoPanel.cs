using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class LogoPanel : BasePanel
{
    protected override void Start()
    {
        base.Start();
        if(Config.Instance)
        {
            if(Config.Instance.configData.IsVideoRayCastTarget)
            {

            }
            else
            {
                Hide();
            }
        }
    }
}
