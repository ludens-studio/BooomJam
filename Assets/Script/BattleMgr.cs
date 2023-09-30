using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 存玩家HP，全局Buff信息， 游戏时间，骰子状态。
/// 2. 获取以上提到的所有信息，然后扔色子接口
/// 3. UI一直Update，或者数据状态改变发一个事件通知UI更新
/// 4. 管理所有游戏单位的对象，包括塔，小怪，魔王。管理包括创建销毁以及更新
/// 5. 游戏中所有的单位作为对象，存储在BattleMgr中。其存储单位的状态信息
/// </summary>
public class BattleMgr : MonoBehaviour
{
    // UI interface
    public int Hp { get; set; }

    public string Buff { get; set; }

    public float Timer { get; set; }
    
    [Header("Dices")]
    public Dice[] diceList;
    
    [Header("Game Objects Management")]
    public GameObject[] towers;
    
    public GameObject[] enemies;

    /// <summary>
    /// 骰子
    /// </summary>
    public struct Dice
    {
        /// <summary>
        /// 0: resource
        /// 1: evil
        /// 2: demo
        /// </summary>
        public int[] state;

        /// <summary>
        /// 冷却状态
        /// </summary>
        public bool freeze;
    }

    // Start is called before the first frame update
    void Start()
    {
        diceList = new Dice[2];
    }
    
    // UI 功能
    public void PauseGameFake()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
