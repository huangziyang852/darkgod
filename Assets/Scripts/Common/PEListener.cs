/****************************************************
    文件：PEListener.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/9 8:48:5
	功能：UI的触碰监听
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour,IPointerClickHandler,IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<object> onClick;
    public Action<PointerEventData> onClickDown;//这个Action里指定相关操作
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onDrag;

    public object args;  //参数
    //
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(args);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onClickDown != null)
        {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null)
        {
            onClickUp(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    
}