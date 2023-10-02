using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    public Slider hp;

    public TMP_Text timer;

    public Button setting;

    public Button shop;
    
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
    public void ShowChild()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
            print("For loop: " + transform.GetChild(i));
    }

    public void TestFunc()
    {
        print(transform.gameObject.name);
    }
}
