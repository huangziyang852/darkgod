/****************************************************
    文件：Constants.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 12:0:49
	功能：常量配置
*****************************************************/

using UnityEngine;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Constants
{
    private const string colorRed = "<color=#FF0000FF>";
    private const string colorgreen = "<color=#00FF00FF>";
    private const string colorBlue = "<color=#00B4FFFF>";
    private const string colorYellow = "<color=#FFFF00FF>";
    private const string colorEnd = "</color>";

    public static string Color(string str,TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = colorRed + str + colorEnd;
                break;
            case TxtColor.Green:
                result = colorgreen + str + colorEnd;
                break;
            case TxtColor.Blue:
                result = colorBlue + str + colorEnd;
                break;
            case TxtColor.Yellow:
                result = colorYellow + str + colorEnd;
                break;
        }
        return result;
    }


    //AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;


    //场景名称/ID
    public const string SceneLogin = "SceneLogin";
    public const int SceneMainCityID = 10000;

    //音效名称
    public const string BGLogin = "bgLogin";
    public const string BgMainCity = "bgMainCity";
    //登陆音效
    public const string UILogin = "uiLoginBtn";

    //普通ui点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string FBItemEnter = "fbitem";

    //屏幕宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;
    //轮盘操作的距离
    public const int ScreenOPDis = 90;

    //角色移动速度
    public const int PlayerMovesSpeed = 8;
    public const int MonsterMoveSpeed = 4;

    //混合参数
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;
    //运动平滑加速度
    public const float AccelerSpeed = 5;

}