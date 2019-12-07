/****************************************************
    文件：LoginWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 14:16:28
	功能：登录注册界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : WindowRoot 
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWnd()
    {
        base.InitWnd();
        //获取本地存储的账号密码
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }

        
    }
    /// <summary>
    /// 点击进入游戏
    /// </summary>
    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAuido(Constants.UILogin,false);

        string _acct = iptAcct.text;
        string _pass = iptPass.text;

        if(_acct !=""&& _pass != "")
        {
            //TODO保存账号密码
            PlayerPrefs.SetString("Acct", _acct);
            PlayerPrefs.SetString("Pass", _pass);

            //TODO 发送网络消息
            GameMsg msg = new GameMsg
            {
                //注意Msg里包含什么字段
                cmd=(int)CMD.ReqLogin,
                reqLogin = new ReqLogin  
                {
                    acct =_acct,
                    pass =_pass
                }
            };
            netSvc.SendMsg(msg);

            //LoginSys.Instance.RspLogin();
        }
        else
        {
            GameRoot.AddTips("账号密码不能为空");
        }
    }

    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        GameRoot.AddTips("功能正在开发");
    }
}