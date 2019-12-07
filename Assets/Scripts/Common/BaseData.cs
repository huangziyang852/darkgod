/****************************************************
    文件：BaseData.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/9 18:43:23
	功能：数据配置类（对应xml解析）
*****************************************************/
using UnityEngine;

//强化配置
public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;//装备位置
    public int startlv;//星级
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv; //最低需求等级
    public int coin;
    public int crystal;
}

//自动引导
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{

    public int npcID;//触发任务的NPC索引号
    public string dilogArr;  //对话数据
    public int actID;  //目标任务ID
    public int coin;
    public int exp;

}

public class MapCfg: BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos ;
    public Vector3 mainCamRote;
    public Vector3 PlayerBornPos;
    public Vector3 PlayerBornRote;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;  //进度
    public bool taked; //是否领取
}

public class BaseData<T>
{
    public int ID;
}