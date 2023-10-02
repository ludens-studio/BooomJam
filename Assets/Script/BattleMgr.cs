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
public class BattleMgr : BaseMgr<BattleMgr>
{
    [System.Serializable]
    public class Wave
    {
        public float waveRate;  // 与下一波之间的间隔
        public float rate;      // 波次内生成间隔
        public List<GameObject> enemyObj;     // 波内会出现的敌人（可以按顺序也可以纯随机）
    }
    
    /// <summary>
    /// 波数配置
    /// </summary>
    public Wave[] waves;

    [Header("UI interface")]
    public int hp;

    public int timer;

    public string Buff { get; set; }

    [Header("Dices")]
    public Dice[] diceList;
    
    [Header("Game Objects Management")]
    public List<GameObject> towers;
    
    public List<GameObject> enemies;

    /// <summary>
    /// 骰子
    /// </summary>
    [System.Serializable]
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
        StartCoroutine(WaveSpawner());
        StartCoroutine(Timer());
        
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 怪物攻击波
    /// </summary>
    /// <returns></returns>
    IEnumerator WaveSpawner()
    {
        while (hp > 0)
        {
            int n = waves.Length;
            // 打乱顺序
            for (int i = 0; i < n; i++)
            {
                int r = i + Random.Range(0, n - i);
                (waves[i], waves[r]) = (waves[r], waves[i]);
            }
            // 波之间
            for (int i = 0; i < waves.Length; i++)
            {
                // 波内生成的
                for (int j = 0; j < waves[i].enemyObj.Count; j++)
                {
                    PoolMgr.GetInstance().GetObj("Prefabs/" + waves[i].enemyObj[j].name, o=>{
                        int row = Random.Range(-2, 3);    // 随机生成敌人所在的行,-2~2

                        o.transform.position = new Vector3(4, row, -0.1f);
                        o.transform.parent = GameObject.Find("PoolEnemy").transform;
                
                        enemies.Add(o);
                    });
                    yield return new WaitForSeconds(waves[i].rate);
                }
                yield return new WaitForSeconds(waves[i].waveRate);
            }

        }
    }

    /// <summary>
    /// 游戏计时器
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        while (true)
        {
            timer += 1;
            yield return new WaitForSeconds(1.0f);
        }
    }

    // 回对象池以及死亡效果结算
    public void EnemyDeath()
    {
        
    }

    public void TowerDeath()
    {
        
    }

    /// <summary>
    /// 获取当前选择的骰子是否在冷却期
    /// </summary>
    /// <returns></returns>
    public bool IsDiceFreeze(string diceName)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;
        return diceList[id].freeze;
    }

    /// <summary>
    /// 冷却骰子
    /// </summary>
    /// <param name="diceName"></param>
    public void FreezeDice(string diceName)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;
        diceList[id].freeze = true;
    }
    
}
