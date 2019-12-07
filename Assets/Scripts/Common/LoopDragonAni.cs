/****************************************************
    文件：LoopDragonAni.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 23:41:48
	功能：Nothing
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{
    private Animation ani;
    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if(ani != null)
        {
            InvokeRepeating("PlayDragonAni",0,20);//重复
        }
    }

    private void PlayDragonAni()
    {
        if (ani != null)
        {
            ani.Play();
        }
    }
}