/****************************************************
    文件：PlayerController.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/11/9 10:41:21
	功能：人物控制
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private  Transform camTrans;
    //相机偏移量
    private Vector3 camOffest;
    public Animator ani;
    public CharacterController ctrl;

    public bool isMove = false; //移动标志位
    private Vector2 dir = Vector2.zero;
    //平滑动画的blend值
    private float targetBlend;
    private float currentBlend;

    public Vector2 Dir
    {
        get
        {
            return dir;
        }

        set
        {
            if(value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffest = transform.position - camTrans.position;
    }

    private void Update()
    {
        //平滑动画
        if(currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }
       

        if (isMove)
        {
            //控制方向
            SetDir();
            //移动
            SetMove();
            //相机跟随
            SetCam();
        }
       
    }

    private void SetDir()
    {
        //方向需要加上相机的角度
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1))+camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward*Time.deltaTime*Constants.PlayerMovesSpeed);
    }
    //设置相机位置
    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffest;
        }
    }

    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }
    //
    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }else if(currentBlend > targetBlend){
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;  //根据帧数加减
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }

        ani.SetFloat("Blend", currentBlend);
    }
}