/****************************************************
    文件：TimerSvc.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/30 16:40:17
	功能：计时服务
*****************************************************/

using System;
using UnityEngine;

public class TimerSvc : SystemRoot
{
    public static TimerSvc Instance = null;

    private PETimer pt;

    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        //日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimerSvc...");

    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond,int count =1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
}