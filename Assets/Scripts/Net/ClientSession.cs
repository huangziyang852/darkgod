/****************************************************
    文件：ClientSession.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/3 15:5:39
	功能：客户端网络会话
*****************************************************/

using PENet;
using PEProtocol;


public class ClientSession :PESession<GameMsg>
{
    protected override void OnConnected()
    {
        GameRoot.AddTips("连接服务器成功");
        PECommon.Log("Connect to Server succeed");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Recive Package CMD:"+((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRoot.AddTips("断开服务器连接");
        PECommon.Log("DisConnect to server");
    }
}