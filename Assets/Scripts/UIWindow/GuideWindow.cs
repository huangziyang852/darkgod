/****************************************************
    文件：GuideWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/16 18:11:15
	功能：引导对话界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWindow : WindowRoot
{
    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;


    private PlayerData pd;
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index;

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
        
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if(talkArr[0] == "0")
        {
            //玩家
            SetSprite(imgIcon,PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else
        {
            //对话NPC
            switch (curtTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小云");
                    break;
            }
        }
        //设置大小
        imgIcon.SetNativeSize();
        //更改对话内容的引用
        SetText(txtTalk,talkArr[1].Replace("$name", pd.name));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);

        index += 1;
        if(index == dialogArr.Length)
        {
            //TODO 向服务器发送请求完成任务的信息
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curtTaskData.ID
                }
            };
            
            netSvc.SendMsg(msg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }        
    }
}