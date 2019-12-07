/****************************************************
    文件：SystemRoot.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 23:25:21
	功能：Nothing
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimerSvc TimerSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }
}