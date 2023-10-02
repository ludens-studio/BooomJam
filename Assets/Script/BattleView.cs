using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleView : MonoBehaviour
{
    public Slider hp;

    public TMP_Text timer;

    public Button setting;

    public Button shop;
    
    // 骰子的冷却时间
    public int diceTimer;

    private GameObject dice;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = BattleMgr.GetInstance().hp;
        int time = BattleMgr.GetInstance().timer;
        string min = (time / 60 < 10) ? "0" + (time / 60): (time / 60).ToString();
        string sec = (time % 60 < 10) ? "0" + (time % 60) : (time % 60).ToString();
        
        timer.text = min + ":" + sec;
    }

    /// <summary>
    /// 显示所有子节点
    /// 可用于显示骰子各个面的状态
    /// </summary>
    public void ShowChild(GameObject o)
    {
        Animator animator = o.GetComponent<Animator>();
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.normalizedTime >= 1.0f)
        {
            animator.Play("ShowState");
        }

    }
    
    /// <summary>
    /// 隐藏所有子节点
    /// </summary>
    public void CloseChild(GameObject o)
    {
        o.GetComponent<Animator>().Play("CloseState");
    }

    /// <summary>
    /// 点击骰子图标事件
    /// 生成骰子实体
    /// </summary>
    /// <param name="o"></param>
    public void OnDiceClick(GameObject o)
    {
        //如果骰子非冷却
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name))
        {
            //在鼠标处生成一个骰子，阴影指向落点
            dice = ResMgr.GetInstance().Load<GameObject>("Prefabs/Dice");
            // dice = Instantiate(dice);
            BattleMgr.GetInstance().FreezeDice(o.name);
        }
    }

    /// <summary>
    /// 拖拽骰子事件
    /// 骰子跟随鼠标移动
    /// </summary>
    public void OnDiceDrag(GameObject o)
    {
        dice = GameObject.FindWithTag("Dice");
        var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        var objectPosition = Camera.main.ScreenToWorldPoint(mousePos);
        dice.transform.position = objectPosition;

    }


    // 随便测什么都可以
    public void TestFunc(GameObject o)
    {
        BattleMgr.GetInstance().timer = 100;
    }

    /// <summary>
    /// 打开商店时暂停游戏
    /// </summary>
    public void PauseGame()
    {
        
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    


}
