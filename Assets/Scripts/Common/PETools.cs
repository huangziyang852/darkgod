/****************************************************
    文件：PETools.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/2 18:36:48
	功能：工具类
*****************************************************/

public class PETools 
{
    public static int RDInt(int min,int max,System.Random rd =null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min, max+1);
        return val;
    }
}