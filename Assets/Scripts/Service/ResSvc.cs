/****************************************************
    文件：ResSvc.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 11:40:7
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour 
{
    public static ResSvc Instance = null;

    private Action prgCB = null;
    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfg(PathDefine.TaskRewardCfg);
        PECommon.Log("Init ResSvc...");
    }
    //场景加载及进度条设置，场景通常使用异步加载
    public void AsynLoadScene(string sceneName,Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState(true);

        //取出异步操作
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);

        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1)
            {
                if (loaded != null)
                {
                    //如果不为空就调用委托
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
 
    }

    public void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }
    //音乐缓存字典
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path,bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au)){
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    //加载预制体
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();  
    public GameObject LoadPrefab(string path,bool cache = false)
    {
        GameObject prefab = null;
        if(!goDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if(prefab == null)
            {
                Debug.LogError("can not find the prefab");
            }
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }
        GameObject go = null;
        if(prefab != null)
        {
            go = Instantiate(prefab);
        }

        return go;
    }
    //加载图片
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path,bool cache =false)
    {
        Sprite sp = null;
        if(!spDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region InitCfgs
    #region 随机名字
    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();
    private void InitRDNameCfg(string path)
    {
        TextAsset xml= Resources.Load<TextAsset>(path);

        if (!xml)
        {
            PECommon.Log("Xml file:" + path + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for(int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                //int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach(XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }

            }
        }
    }

    public string GetRDNameData(bool man = true)
    {
        //System.Random rd = new System.Random();
        string rdName = surnameList[PETools.RDInt(0, surnameList.Count - 1)];
        if (man)
        {
            rdName += manList[PETools.RDInt(0, manList.Count - 1)];
        }
        else
        {
            rdName += womanList[PETools.RDInt(0, womanList.Count - 1)];
        }

        return rdName;
    }
    #endregion
    #region 地图
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);

        if (!xml)
        {
            PECommon.Log("Xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                //定义地图配置类
                MapCfg mc = new MapCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            {
                                string[] valArr = e.InnerText.Split(','); 
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.PlayerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                                Debug.Log(mc.PlayerBornPos);
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.PlayerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                    }
                }

                mapCfgDataDic.Add(ID, mc);
                
            }
        }
    }
    public MapCfg GetMapCfg(int id)
    {
        MapCfg data;
        //注意out 的用法
        if(mapCfgDataDic.TryGetValue(id,out data))
        {
            //Debug.Log(data.PlayerBornPos);
            return data;
            
        }
        return null;
    }
    #endregion
    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);

        if (!xml)
        {
            PECommon.Log("Xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                AutoGuideCfg mc = new AutoGuideCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            mc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            mc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            mc.actID = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideTaskDic.Add(ID, mc);
            }
        }
    }

    public AutoGuideCfg GetAutoGuideCfg(int id)
    {
        AutoGuideCfg agc =null;
        if(guideTaskDic.TryGetValue(id,out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    #region 强化配置
    //字典以位置分成6个字典
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);

        if (!xml)
        {
            PECommon.Log("Xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                StrongCfg sd = new StrongCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            sd.pos = val;
                            break;
                        case "starlv":
                            sd.startlv = val;
                            break;
                        case "addhp":
                            sd.addhp = val;
                            break;
                        case "addhurt":
                            sd.addhurt = val;
                            break;
                        case "adddef":
                            sd.adddef = val;
                            break;
                        case "minlv":
                            sd.minlv = val;
                            break;
                        case "coin":
                            sd.coin = val;
                            break;
                        case "crystal":
                            sd.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDic.TryGetValue(sd.pos, out dic))
                {
                    dic.Add(sd.startlv, sd);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sd.startlv, sd);

                    strongDic.Add(sd.pos, dic);
                }
            }
        }
    }
    //根据部位获取数据
    public StrongCfg GetStrongCfg(int pos, int startlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(startlv))
            {
                sd = dic[startlv];
            }
        }
        return sd;
    }
    //获取累计的强化数据
    public int GetPropAddValPreLv(int pos,int starlv,int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if(strongDic.TryGetValue(pos,out posDic))
        {
            for(int i = 0; i < starlv; i++)
            {
                StrongCfg sd;
                if(posDic.TryGetValue(i,out sd))
                {
                    switch (type)
                    {
                        case 1://hp
                            val += sd.addhp;
                            break;
                        case 2://hurt
                            val += sd.addhurt;
                            break;
                        case 3://def
                            val += sd.adddef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);

        if (!xml)
        {
            PECommon.Log("Xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                TaskRewardCfg trc = new TaskRewardCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        
                        case "taskName":
                            trc.taskName = e.InnerText;
                            break;
                        case "count":
                            trc.count = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            trc.exp = int.Parse(e.InnerText);
                            break;
                        case "coin":
                            trc.coin = int.Parse(e.InnerText);
                            break;
                    }
                }
                taskRewardDic.Add(ID, trc);
            }
        }
    }

    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg trc = null;
        if (taskRewardDic.TryGetValue(id, out trc))
        {
            return trc;
        }
        return null;
    }
    #endregion

    #endregion
}