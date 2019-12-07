/****************************************************
    文件：BuyWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/26 22:57:57
	功能：购买窗口
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindow : WindowRoot
{
    public Text txtInfo;
    public Button btnSure;

    private int buyType;//0:体力 1:金币

    public void SetByType(int type)
    {
        this.buyType = type;
    }
    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                //体力
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买"+ Constants.Color("100体力", TxtColor.Green)+"?";
                break;
            case 1:
                //金币
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("1000金币", TxtColor.Green)+"?";
                break;
        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        //发送网络消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost = 10
            }
        };
        netSvc.SendMsg(msg);
        //防止多次操作
        btnSure.interactable = false;
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        SetWndState(false);
    }
}