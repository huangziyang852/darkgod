/****************************************************
    文件：LoginSys.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 11:38:37
	功能：登陆注册业务系统系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;
    public LoginWindow loginWnd;
    public CreateWindow createWnd;

    public override void InitSys()
    {
        base.InitSys();  //注意基类的调用
        Instance = this;
        PECommon.Log("Init LoginSys");

    }

    /// <summary>
    /// 进入登陆场景
    /// </summary>
    public void EnterLogin()
    {
        
        //异步加载登陆场景
        resSvc.AsynLoadScene(Constants.SceneLogin,()=> {
            loginWnd.SetWndState(true);
            audioSvc.PlayBGMusic(Constants.BGLogin,true);
            GameRoot.AddTips("Load Done");

        });

    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登陆成功");
        //将玩家数据传入gameRoot
        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        //新账号重新创建角色
        if (msg.rspLogin.playerData.name == "")
        {
            createWnd.SetWndState(true);
        }
        else
        {
            MainCitySys.Instance.EnterMainCity();
        }
        //关闭登陆界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
        //打开主城界面
        MainCitySys.Instance.EnterMainCity();
        //关闭创建界面
        createWnd.SetWndState(false);
    }


}