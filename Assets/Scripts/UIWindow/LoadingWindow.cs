/****************************************************
    文件：LoadingWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 12:8:37
	功能：加载进度界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : WindowRoot
{
    public Text textTips;
    public Image imgFG;
    public Image imgPoint;
    public Text textPrg;

    private float fgWidth;


    protected override void InitWnd()
    {
        base.InitWnd();

        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        SetText(textTips, "这是一条游戏Tips");
        SetText(textPrg, "0%");
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector3(-550f, 0, 0);
    }

    public void SetProgress(float prg)
    {
        SetText(textPrg, (int)(prg * 100) + "%");
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth - 550;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);

    }
}