/****************************************************
    文件：GameRoot.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 11:36:52
	功能：游戏系统的入口
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;

    public LoadingWindow loadingWnd;
    public DynamicWindow dynamicWnd;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");

        ClearUIRoot();

        Init();
    }


    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for(int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.gameObject.SetActive(true);
    }

    private void Init()
    {
        //服务模块初始化
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();

        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();

        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();

        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();


        //业务系统初始化
        LoginSys loginSys = GetComponent<LoginSys>();
        loginSys.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        FubenSys fubenSys = GetComponent<FubenSys>();
        fubenSys.InitSys();


        //进入登陆场景并加载UI
        loginSys.EnterLogin();
        
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }
    //保存玩家数据
    private PlayerData playerData = null; 
    //玩家数据属性
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }
    //登陆后设置玩家数据
    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }
    //改名后设置玩家名字
    public void SetPlayerName(string name)
    {
        PlayerData.name = name;
    }
    //引导后设置玩家数据
    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.guideid = data.guideid;
    }
    //强化后设置玩家数据
    public void SetPlayerDataByStrong(RspStrong data)
    {
        playerData.coin = data.coin;
        playerData.crystal = data.crystal;
        playerData.hp = data.hp;
        playerData.ad = data.ad;
        playerData.addef = data.addef;
        playerData.apdef = data.apdef;

        playerData.strongArr = data.strongArr;
    }
    //购买后设置玩家数据
    public void SetPlayerDataByBuy(RspBuy data)
    {
        playerData.diamond = data.diamond;

        playerData.coin = data.coin;
        playerData.Power = data.power;
    }
    //体力增加
    public void SetPlayerDataByPower(PshPower data)
    {
        playerData.Power = data.power;
    }
    //领取奖励
    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.taskArr = data.taskArr;
    }
    //更新进度
    public void SetPlayerDataByTaskPsh(PshTaskPrgs data)
    {
        playerData.taskArr = data.taskArr;
    }
}