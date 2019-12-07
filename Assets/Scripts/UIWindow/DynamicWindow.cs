/****************************************************
    文件：DynamicWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/2 11:35:1
	功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWindow : WindowRoot 
{
    public Animation tipsAni;
    public Text textTips;

    //标志位
    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(textTips, false);
    }

    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if(tipsQue.Count > 0&&isTipsShow ==false)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    public void SetTips(string tips)
    {
        SetActive(textTips, true);
        SetText(textTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();
        //延时关闭激活状态

        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            SetActive(textTips, false);
            isTipsShow = false;
        }));

    }
    //延时携程
    private IEnumerator AniPlayDone(float sec,Action cb)
    {
        yield return new WaitForSeconds(sec);
        if(cb != null)
        {
            cb();
        }
    }
}