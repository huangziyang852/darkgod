/****************************************************
    文件：WindowRoot.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 14:41:31
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour 
{
    protected ResSvc resSvc = null;// 获取资源加载服务
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    protected TimerSvc timerSvc = null;

    public void SetWndState(bool isActive)
    {
        if (gameObject.activeSelf != isActive)
        {
           SetActive(gameObject,isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    public bool GetWndState()
    {
        return gameObject.activeSelf;
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
        timerSvc = null;
    }

    #region Tool Functions
    //激活物体
    protected void SetActive(GameObject go,bool isActive)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans,bool state)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text text, bool state)
    {
        text.transform.gameObject.SetActive(state);
    }

    //设置text的重载
    protected void SetText(Text text,string context = "")
    {
        text.text = context;
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans,string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text text,int num = 0)
    {
        SetText(text, num.ToString());
    }

    //设置图片
    protected void SetSprite(Image img,string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }

    //注意通配符的作用是限定类型
    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    //获取子物体的TRANSFORM
    protected Transform GetTrans(Transform trans,string name)
    {
        if(trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }
    #endregion


    #region Click Evts
    //此事件需要获取点击的物体，Action<>内为参数
    protected void Onclick(GameObject go, Action<object> callback,object args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = callback;
        listener.args = args;
    }

    protected void OnClickDown(GameObject go,Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = callback;
    }

    protected void OnClickUp(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = callback;
    }

    protected void OnDrag(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = callback;
    }
    #endregion
}