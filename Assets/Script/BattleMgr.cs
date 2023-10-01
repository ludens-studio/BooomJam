using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


/// <summary>
/// 1. 存玩家HP，全局Buff信息，游戏时间，骰子状态。
/// 2. 获取以上提到的所有信息，然后扔色子接口
/// 3. UI一直Update，或者数据状态改变发一个事件通知UI更新
/// 4. 管理所有游戏单位的对象，包括塔，小怪，魔王。管理包括创建销毁以及更新
/// 5. 游戏中所有的单位作为对象，存储在BattleMgr中。其存储单位的状态信息
/// </summary>
public class BattleMgr : MonoBehaviour
{
    [Header("UI interface")]
    public int hp;

    public float timer;

    /// <summary>
    /// 固定刷怪时间间隔
    /// </summary>
    public float interval;
    
    public string Buff { get; set; }

    [Header("Dices")]
    public Dice[] diceList;
    
    [Header("Game Objects Management")]
    public GameObject[] towers;
    
    public GameObject[] enemies;

    private UnityAction<GameObject> _enemyInitEvent;

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
        _enemyInitEvent += (GameObject o) =>
        {
            int row = Random.Range(0, 5);    // 随机生成敌人所在的行
            
            o = Instantiate(o);
            o.transform.position = new Vector3(0, row, 0.1f);
            o.transform.parent = GameObject.Find("PoolEnemy").transform;
        };
        
        diceList = new Dice[2];
        StartCoroutine(EnemyWave());
    }

    private void Update()
    {
        
    }

    // UI 功能
    public void PauseGameFake()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 固定刷新
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyWave()
    {
        while (true)
        {
            int row = Random.Range(0, 5);    // 生成敌人所在的行
            PoolMgr.GetInstance().GetObj("Prefabs/Enemy", o=>{
                int row = Random.Range(-2, 3);    // 随机生成敌人所在的行,-2~2

                o.transform.position = new Vector3(0, row, -0.1f);
                o.transform.parent = GameObject.Find("PoolEnemy").transform;});
            yield return new WaitForSeconds(interval);
        }

        yield return null;
    }

}
