/****************************************************
    文件：MainCityWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/4 22:56:42
	功能：主城UI界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWindow : WindowRoot 
{
    #region UIDefine;
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Animation menuAni;
    public Button btnMenu;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    public Button btnGuide; //任务引导图标


    #endregion
    public bool menuState = true;
    private float pointDis; //轮盘点的距离
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    private AutoGuideCfg curtTaskData;  //当前任务引导的数据


    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis; //根据屏幕的比例算出距离
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);
        RegisterTouchEvts();

        RefreshUI();
    }
    //需要外部获取
    public void RefreshUI()
    {
        //玩家数据存放于gameroot
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtPower, "体力:" + pd.Power+"/"+PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.Power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtLevel,pd.lv);
        SetText(txtName, pd.name);

        //expprg
        #region ExpPrg
        int expPrgVal =(int)(pd.exp * 1.0f/PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");

        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        //获取屏幕的缩放比
        //UI的缩放设置为了基于高度
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;
        grid.cellSize = new Vector2( width,7);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }else if(i == index){
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion

        //设置自动任务图标
        curtTaskData = resSvc.GetAutoGuideCfg(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }

    }
    //设置引导图标
    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(img, spPath);

        //
    }

    #region ClickEvts
    //点击购买体力
    public void ClickBuyPowerBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIOpenPage,false);
        MainCitySys.Instance.OpenBuyWindow(0);
    }
    //点击购买金币
    public void ClickMKCoinBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIOpenPage, false);
        MainCitySys.Instance.OpenBuyWindow(1);
    }
    //副本
    public void ClickFubenBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIOpenPage, false);
        MainCitySys.Instance.EnterFuben();
    }
    //任务
    public void ClickTaskBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIOpenPage, false);
        MainCitySys.Instance.OpenTaskRewardWnd();
    }
    //强化
    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIExtenBtn, false);

        MainCitySys.Instance.OpenStrongWnd();
    }
    //引导
    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);

        if(curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务，正在开发中...");
        }
    }
    //菜单
    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIExtenBtn,false);

        menuState = !menuState;

        AnimationClip clip = null;
        if (menuState)
        {
            clip = menuAni.GetClip("OpenMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMenu");
        }
        menuAni.Play(clip.name);
    }
    //点击头像打开属性框
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIExtenBtn, false);

        MainCitySys.Instance.OpenInfoWnd();
    }
    //点击聊天窗口
    public void ClickChatBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIOpenPage,false);
        MainCitySys.Instance.OpenChatWnd();
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
         {
             startPos = evt.position;
             SetActive(imgDirPoint,true);
             //注意全局坐标和本地坐标
             imgDirBg.transform.position = evt.position;
         });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
         {
             imgDirBg.transform.position = defaultPos;
             SetActive(imgDirPoint, false);
             imgDirPoint.transform.localPosition = Vector2.zero;

             MainCitySys.Instance.SetMoveDir(Vector2.zero);
         });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
         {
             Vector2 dir = evt.position - startPos;
             float len = dir.magnitude;
             if (len > pointDis)
             {
                 //限制移动距离
                 Vector2 clampDir = Vector2.ClampMagnitude(dir,pointDis );
                 imgDirPoint.transform.position = startPos + clampDir;
             }
             else
             {
                 imgDirPoint.transform.position = evt.position;
             }

             //方向信息传递
             MainCitySys.Instance.SetMoveDir(dir.normalized);
         });
    }
    #endregion

}