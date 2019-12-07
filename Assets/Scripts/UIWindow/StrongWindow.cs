/****************************************************
    文件：StrongWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/17 11:43:32
	功能：强化窗口
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrongWindow : WindowRoot 
{
    #region UI Define
    public Image imgCurtPos; //当前部位的图片
    public Text txtStarLv; //当前部位的图片
    public Transform starTransGrp; //星星
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1; //箭头
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv; //需要等级
    public Text txtCosCoin;
    public Text txtCostCrystal;

    public Transform costTransRoot;//信息窗口
    public Text txtCoin;//剩余的数量


    #endregion

    #region Data Area
    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currrentIndex; //当前点击的图片
    private PlayerData pd;
    StrongCfg nextSd;
    #endregion

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RegClickEvts();

        ClickPosItem(0);
    }
    //循环注册点击事件
    private void RegClickEvts()
    {
        for(int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            //参数i为点击的位置的位置
            Onclick(img.gameObject, (object args) =>
             {
                 ClickPosItem((int)args);
                 audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
             },i);
            imgs[i] = img;// 保存图片数组
        }
    }
    //点击之后设置图片
    private void ClickPosItem(int index)
    {
        PECommon.Log("Click Item"+index);
        //给当前点击对象赋值
        currrentIndex = index;
        for(int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == currrentIndex)
            {
                //箭头显示
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(10.0f,trans.localPosition.y,0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250,95);
            }
            else
            {
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshItem();
    }
    //点击刷新强化界面
    private void RefreshItem()
    {
        SetText(txtCoin, pd.coin);
        switch (currrentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemTouKui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strongArr[currrentIndex] + "星级");

        int curtStarLv = pd.strongArr[currrentIndex];
        for(int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.spStar2);
            }
            else
            {
                SetSprite(img, PathDefine.spStar1);
            }
        }

        int nextStarLV = curtStarLv + 1;
        int sumAddHp = resSvc.GetPropAddValPreLv(currrentIndex, nextStarLV, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currrentIndex, nextStarLV, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(currrentIndex, nextStarLV, 3);
        SetText(propHP1, "生命   +" + sumAddHp);
        SetText(propHurt1, "伤害   +" + sumAddHurt);
        SetText(propDef1, "防御   +" + sumAddDef);

        nextSd = resSvc.GetStrongCfg(currrentIndex, nextStarLV);
        if (nextSd != null)
        {
            //启动组件
            SetActive(propHP2, true);
            SetActive(propHurt2, true);
            SetActive(propDef2, true);

            SetActive(costTransRoot, true);
            SetActive(propArr1, true);
            SetActive(propArr2, true);
            SetActive(propArr3, true);

            SetText(propHP2, "强化后 +" + nextSd.addhp);
            SetText(propHurt2, " +"+nextSd.addhurt);
            SetText(propDef2, " +" +nextSd.adddef);

            SetText(txtNeedLv,"需要等级：" +nextSd.minlv);
            SetText(txtCosCoin, "需要消耗:        " + nextSd.coin);
            //TODO水晶
            SetText(txtCostCrystal, pd.crystal + "/" + nextSd.crystal);
        }
        else
        {
            //关闭组件
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);

        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        SetWndState(false);
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAuido(Constants.UIClickBtn,false);
        //判断是否满级
        if (pd.strongArr[currrentIndex] < 10)
        {
            if (pd.lv < nextSd.minlv)
            {
                GameRoot.AddTips("角色等级不够");
                return;
            }
            if (pd.coin < nextSd.coin)
            {
                GameRoot.AddTips("金币不够");
                return;
            }
            if (pd.crystal < nextSd.crystal)
            {
                GameRoot.AddTips("水晶不够");
                return;
            }

            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = currrentIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("星级已经升满");
        }

    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAuido(Constants.FBItemEnter,false);
        //相当于重新点击一次该部位
        ClickPosItem(currrentIndex);

    }
}