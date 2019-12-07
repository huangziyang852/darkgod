/****************************************************
    文件：InfoWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/10 10:22:0
	功能：角色信息展示页面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWindow : WindowRoot 
{
    #region UI Define

    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtHurt;
    public Text txtDef;

    public Button btnClose;
    public Button btnDetail;
    public Button btnCloseDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;
    #endregion


    private Vector2 startPos;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        SetActive(transDetail, false);
        RefreshUI();
    }
    //注册触摸事件
    private void RegTouchEvts()
    {
        //按下
        OnClickDown(imgChar.gameObject, (PointerEventData evt) =>
         {
             startPos = evt.position;
             MainCitySys.Instance.SetStratRoate();
         });
        //拖拽
        OnDrag(imgChar.gameObject, (PointerEventData evt) =>
         {
             float roate = -(evt.position.x - startPos.x)*0.4f;
             MainCitySys.Instance.SetPlayerRoate(roate);
         });
    }

    //

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + "LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp*1.0f/PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.Power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.Power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        //TODO添加职业
        SetText(txtJob, "职业    暗夜刺客");
        SetText(txtFight, "战力    " + PECommon.GetFightByProps(pd));
        SetText(txtHp, "血量    " + pd.hp);
        SetText(txtHurt, "伤害    " + (pd.ad+pd.ap));
        SetText(txtDef, "防御    " + (pd.addef+pd.apdef));

        //TODO detail 
        SetText(dtxhp, pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodge, pd.dodge);
        SetText(dtxpierce, pd.pierce+"%");
        SetText(dtxcritical, pd.critical+"%");
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        SetActive(transDetail,true);
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn, false);
        SetActive(transDetail, false);
    }
}