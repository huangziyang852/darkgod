/****************************************************
    文件：FubenSys.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/12/7 10:57:36
	功能：副本业务系统
*****************************************************/
//注意拆分业务逻辑
public class FubenSys : SystemRoot 
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys()
    {
        base.InitSys();  //注意基类的调用
        Instance = this;
        PECommon.Log("Init FubenSys");
    }

    public void EnterFuben()
    {
        OpenFubenWnd();
    }

    #region 副本窗口
    public void OpenFubenWnd()
    {
        fubenWnd.SetWndState(true);
    }
    #endregion
}