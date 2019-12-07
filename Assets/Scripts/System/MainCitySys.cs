/****************************************************
    文件：MainCitySys.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/4 22:57:54
	功能：主城业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWindow maincityWnd;
    public InfoWindow infoWnd;
    public GuideWindow guideWnd;
    public StrongWindow strongWnd;
    public ChatWindow chatWnd;
    public BuyWindow buyWnd;
    public TaskWindow taskWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curtTaskData; //当前任务
    private Transform[] npcPosTrans; //npc位置
    private NavMeshAgent nav;

    public override void InitSys()
    {
        base.InitSys();  //注意基类的调用
        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }
    //进入主城的初始化事件
    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfg(Constants.SceneMainCityID);
        resSvc.AsynLoadScene(mapData.sceneName,()=> {
            PECommon.Log("Enter MainCity...");
            //加载主角
            LoadPlayer(mapData);
            //打开主城UI
            maincityWnd.SetWndState(true);

            //播放主城音乐
            audioSvc.PlayBGMusic(Constants.BgMainCity,true);
            //加载npc位置
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;
            //设置人物展示相机
            if(charCamTrans != null){
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }
    
    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab,true);
        Debug.Log(player);
        player.transform.position = mapData.PlayerBornPos;
        player.transform.localEulerAngles = mapData.PlayerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        //停止寻路导航
        StopNavTask();
        if(dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }

        playerCtrl.Dir = dir;
    }

    #region 调用副本系统
    public void EnterFuben()
    {
        StopNavTask();
        FubenSys.Instance.EnterFuben();
    }
    #endregion
    #region Task Wnd 任务窗口
    public void OpenTaskRewardWnd()
    {
        StopNavTask();
        taskWnd.SetWndState(true);
    }
    //领取奖励
    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.RspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);

        taskWnd.RefreshUI();
        maincityWnd.RefreshUI();
    }
    //更新进度
    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTaskPsh(data);

        if (taskWnd.GetWndState())
        {
            //拿到PSH消息后检测窗口是否打开，打开刷新界面
            taskWnd.RefreshUI();
        }
    }
    #endregion
    #region Buy Window购买窗口
    public void OpenBuyWindow(int type)
    {
        StopNavTask();
        buyWnd.SetByType(type);
        buyWnd.SetWndState(true);
    }
    public void RspBuy(GameMsg msg)
    {
        RspBuy rspBuy = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(rspBuy);
        GameRoot.AddTips("购买成功");

        maincityWnd.RefreshUI();
        buyWnd.SetWndState(false);

        //第二个包
        GameRoot.Instance.SetPlayerDataByTaskPsh(msg.pshTaskPrgs);
        if (taskWnd.GetWndState())
        {
            //拿到PSH消息后检测窗口是否打开，打开刷新界面
            taskWnd.RefreshUI();
        }
    }
    #endregion

    #region Chat Window 聊天窗口
    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState(true);
    }

    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
    }
    #endregion

    #region 强化窗口 Strong Window
    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState(true);
    }

    public void RspStorng(GameMsg msg)
    {
        //处理强化回应
        int ZhanliPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int ZhanliNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战斗力提升了"+(ZhanliNow-ZhanliPre), TxtColor.Blue));
        //刷新界面
        strongWnd.UpdateUI();
        maincityWnd.RefreshUI();
    }
    #endregion

    #region 人物属性面板 Info Window
    //打开属性窗口
    public void OpenInfoWnd()
    {
        //中断导航
        StopNavTask();
        
        if(charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        //设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f+new Vector3(0,1.2f,0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState(true);
    }

    //关闭人物展示相机
    public void CloseInfoWnd()
    {
        if(charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }
    //记录旋转开始的位置,设置人物旋转
    private float startRoate;
    public void SetStratRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;

    }
    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(3, startRoate + roate, 0);
    }
    #endregion

    #region 任务引导 guide window
 //寻路标志变量
    private bool isNavGuide = false;

    //自动任务
    public void RunTask(AutoGuideCfg agc)
    {
        if(agc != null)
        {
            curtTaskData = agc;
        }

        //解析任务数据
        nav.enabled = true; //防止无法关闭，先激活寻路组件
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMovesSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }
    //判断是否到达目标点,中断寻路
    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;
            OpenGuideWnd();
        }
    }

    //中断导航
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;

            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    //打开对话界面
    private void OpenGuideWnd()
    {
        guideWnd.SetWndState(true);
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Constants.Color("任务奖励 金币 +" + curtTaskData.coin + "经验值 +" + curtTaskData.exp,TxtColor.Blue));

        switch (curtTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //进入副本
                EnterFuben();
                break;
            case 2:
                //打开强化界面    
                OpenStrongWnd();
                break;
            case 3:
                //购买体力
                OpenBuyWindow(0);
                break;
            case 4:
                //购买金币
                OpenBuyWindow(1);
                break;
            case 5:
                //TODO 进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        maincityWnd.RefreshUI();
    }
    #endregion

    #region 体力增加处理
    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        if (maincityWnd.GetWndState())
        {
            maincityWnd.RefreshUI();
        }
    }
    #endregion
}