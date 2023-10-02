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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = BattleMgr.GetInstance().hp;
        // timer.text = BattleMgr.GetInstance().timer
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
    /// 掷骰子
    /// </summary>
    public void UseDice()
    {
        InvokeRepeating(nameof(DiceTimer), 0f, 1f);
    }

    public void DiceTimer()
    {
        BattleMgr.GetInstance().Timer(diceTimer);
    }

    public void TestFunc(GameObject o)
    {
        
        print(o.name);
    }



}
