/****************************************************
    文件：ChatWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/24 16:37:13
	功能：聊天窗口
*****************************************************/

using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : WindowRoot 
{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;
    //存储聊天信息
    private List<string> chatList = new List<string>();


    protected override void InitWnd()
    {
        base.InitWnd();

        chatType = 0;

        RefreshUI();
    }

    private void RefreshUI()
    {
        if(chatType == 0)
        {
            string chatMessage = "";
            for(int i = 0; i < chatList.Count; i++)
            {
                chatMessage += chatList[i] + "\n";
            }
            SetText(txtChat,chatMessage);

            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
      
        else if(chatType == 1)
        {
            SetText(txtChat, "还未加入公会");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype1");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if(chatType == 2)
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype1");
        }
    }

    private bool canSend = true;
    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("聊天消息每5秒可以发一次");
            return;
        }
        if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
        {
            //限制长度和发送间隔
            if(iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入信息不能超过12个字");
            }
            else
            {
                //发送网络消息
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text,
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
                canSend = false;

                timerSvc.AddTimeTask((int tid)=>
                {
                    canSend = true;
                },5,PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("尚未输入聊天信息");
        }
    }
    public void AddChatMsg(string name,string chat)
    {
        chatList.Add(Constants.Color(name+":",TxtColor.Blue)+chat);
        if (chatList.Count > 12)
        {
            chatList.RemoveAt(0);
        }
        if (GetWndState())
        {
            RefreshUI();
        }
       
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        chatType = 0;

        RefreshUI();
    }
    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        chatType = 1;

        RefreshUI();        
    }
    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        chatType = 2;

        RefreshUI();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        chatType = 0;
        SetWndState(false);
    }
}