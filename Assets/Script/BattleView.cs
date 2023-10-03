using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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
        hp.maxValue = BattleMgr.GetInstance().hp;
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = BattleMgr.GetInstance().hp;
        // show time in tmp
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
        
        // 骰子非冷却，且场上没有已生成的骰子 -> 生成骰子实例
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name) && !GameObject.FindWithTag("Dice"))
        {
            //在鼠标处生成一个骰子，阴影指向落点
            dice = ResMgr.GetInstance().Load<GameObject>("Prefabs/Dice");
            // todo: 这个阴影我看直接用平行光投影吧
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
    /// 拖拽骰子事件
    /// 骰子跟随鼠标移动
    /// </summary>
    public void OnDiceDrag(GameObject o)
    {
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name))
        {
            dice = GameObject.FindWithTag("Dice");
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            var objectPosition = Camera.main.ScreenToWorldPoint(mousePos);
            dice.transform.position = objectPosition;
            // todo: 格子高亮或者阴影落点
        }
    }

    /// <summary>
    /// 投掷骰子事件
    /// 落点矫正（四舍五入）
    /// </summary>
    public void OnDiceRelease(GameObject o)
    {
        // if MapMgr return empty grid/ valid grid, then player can put the dice, or dice will be hide.
        if (MapMgr.GetInstance().IsEmptyGrid() && MapMgr.GetInstance().IsValidGrid())
        {
            // random face
            int rdFace = Random.Range(0, 6);
            int[] stateList = BattleMgr.GetInstance().GetDiceState(o.name);
            switch (stateList[rdFace])
            {
                case 0:
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 1);
                    break;
                case 1:
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 2);
                    break;
                case 2:
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 3);
                    break;
            }
        }
        else
        {
            GameObject.FindWithTag("Dice").transform.position = new Vector3(100, 100, 0);
        }
        
    }
    
    /// <summary>
    /// 打开商店时暂停游戏
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // 随便测什么都可以
    public void TestFunc(GameObject o)
    {
        BattleMgr.GetInstance().timer = 100;
    }

}
