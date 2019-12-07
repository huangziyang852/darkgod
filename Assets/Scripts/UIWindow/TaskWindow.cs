/****************************************************
    文件：TaskWindow.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/12/1 10:19:3
	功能：任务奖励界面
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindow : WindowRoot 
{
    public Transform scrollTrans;

    private PlayerData pd = null;
    private List<TaskRewardData> trdList = new List<TaskRewardData>();

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        trdList.Clear();

        List<TaskRewardData> todoList = new List<TaskRewardData>();
        List<TaskRewardData> doneList = new List<TaskRewardData>();

        for(int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };

            if (trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }
        }
        //注意addrange的作用
        trdList.AddRange(todoList);
        trdList.AddRange(doneList);
        //清空数据
        for(int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }

        for(int i = 0; i < trdList.Count; i++)
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trf = resSvc.GetTaskRewardCfg(trd.ID);

            SetText(GetTrans(go.transform, "txtName"), trf.taskName);
            SetText(GetTrans(go.transform, "txtPrg"), trd.prgs+"/"+trf.count);
            SetText(GetTrans(go.transform, "txtExp"), "奖励：        经验"+trf.exp);
            SetText(GetTrans(go.transform, "txtCoin"), "金币"+trf.coin);


            Image imgPrg = GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trf.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() =>
            {
                ClickTakeBtn(go.name);
            });

            Transform transComp = GetTrans(go.transform, "imgComp");
            if (trd.taked)
            {
                //已领取关闭交互
                btnTake.interactable = false;
                SetActive(transComp, true);
            }
            else
            {
                SetActive(transComp, false);
                //判断任务的完成情况
                if(trd.prgs == trf.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }

        }
    }

    //此处通过代码关联事件
    private void ClickTakeBtn(string name)
    {
        Debug.Log("Name:"+name);
        //获取任务的序号
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdList[index].ID
            }
        };

        netSvc.SendMsg(msg);

        TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trdList[index].ID);
        int coin = trc.coin;
        int exp = trc.exp;
        GameRoot.AddTips(Constants.Color("获得奖励：", TxtColor.Blue) + Constants.Color("金币 +"+coin+" 经验 +"+exp, TxtColor.Blue));
    }

    public void ClickCloseBtn()
    {
        SetWndState(false);
    }
}