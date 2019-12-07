/****************************************************
    文件：CreateWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/2 17:37:36
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWindow : WindowRoot 
{
    public InputField iptName;

    protected override void InitWnd()
    {
        base.InitWnd();

        iptName.text = resSvc.GetRDNameData(false);
    }
    //随机生成名字
    public void ClickRandBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);

        string rdName = resSvc.GetRDNameData(false);
        iptName.text = rdName;
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);

        if (iptName.text != "")
        {
            //发送数据到服务器
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不合法");
        }

    }
}