/****************************************************
    文件：NetSvc.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/3 14:59:38
	功能：Nothing
*****************************************************/

using UnityEngine;
using PENet;
using PEProtocol;
using System.Collections.Generic;

public class NetSvc : MonoBehaviour 
{
    public static NetSvc Instance = null;

    PENet.PESocket<ClientSession, GameMsg> client = null;
    //客户端消息队列
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();
    //线程锁
    private static readonly string obj = "lock";

    public void InitSvc()
    {
        Instance = this;

        client = new PESocket<ClientSession, GameMsg>();
        
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });

        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        PECommon.Log("Init NetSvc");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }
    //添加消息包
    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            GameMsg msg = msgQue.Dequeue();
            ProcessMsg(msg);
        }
    }
    //分发消息
    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.ServerDataError:
                    PECommon.Log("服务器数据异常", LogType.Error);
                    GameRoot.AddTips("服务器数据异常");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("数据库更新异常");
                    break;
                case ErrorCode.ClientDataError:
                    PECommon.Log("客户端数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.AcctIsOnline:
                    GameRoot.AddTips("当前账号已上线");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
                case ErrorCode.LackLevel:
                    GameRoot.AddTips("级别不够");
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("金币不够");
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("水晶不够");
                    break;
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("钻石不够");
                    break;
            }
            return;
        }
        switch ((CMD)msg.cmd)
        {
            //收到服务器的回应
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStorng(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
            case CMD.RspTakeTaskReward:
                MainCitySys.Instance.RspTakeTaskReward(msg);
                break;
            case CMD.PshTaskPrgs:
                MainCitySys.Instance.PshTaskPrgs(msg);
                break;
        }
    }
}