using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>
/// 1. 存玩家HP，全局Buff信息，游戏时间，骰子状态。
/// 2. 获取以上提到的所有信息，然后扔骰子接口
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
    
    /// <summary>
    /// 骰子冷却时间
    /// </summary>
    public float diceTime;
    
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
                        int row = Random.Range(-4, 1);    // 随机生成敌人所在的行, 0~-4

                        o.transform.position = new Vector3(9, row, -0.1f);
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

    /// <summary>
    /// 骰子冷却倒计时
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IEnumerator DiceTime(int id)
    {
        float t = 0.0f;
        while (t < diceTime)
        {
            yield return new WaitForSeconds(1.0f);
            t += 1.0f;
        }

        diceList[id].freeze = false;
    }

    /// <summary>
    /// 实例化塔并放置在地图上
    /// </summary>
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标</param>
    /// <param name="type">塔的类型</param>
    public void InitTower(int x, int y, string type)
    {
        GameObject tower;
        print(type);
        tower = ResMgr.GetInstance().Load<GameObject>("Prefabs/T" + type);
        // Instantiate(tower);
        tower.transform.position = new Vector3(x, -y, -0.1f);
        MapMgr.GetInstance().SetTower(x, y, tower.GetComponent<Tower>());
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
    /// <param name="diceName">例如Dice1</param>
    /// <returns></returns>
    public bool IsDiceFreeze(string diceName)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;
        return diceList[id].freeze;
    }

    /// <summary>
    /// 冷却骰子
    /// 同时开始倒计时，计时结束取消冷却
    /// </summary>
    /// <param name="diceName">例如Dice1</param>
    /// <returns></returns>
    public void FreezeDice(string diceName)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;
        diceList[id].freeze = true;
        StartCoroutine(DiceTime(id));
    }

    /// <summary>
    /// 获取骰子状态
    /// </summary>
    /// <param name="diceName">例如Dice1</param>
    /// <returns></returns>
    public int[] GetDiceState(string diceName)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;
        return diceList[id].state;
    }

    /// <summary>
    /// 掷骰时修改骰子的面的状态
    /// 冻结骰子（已使用）
    /// </summary>
    /// <param name="diceName">例如Dice1</param>
    /// <param name="faceId">第几个面(记得是从0开始的)</param>
    /// <param name="type">1：资源-恶魔；2：恶魔-资源；3：魔王全净化</param>
    public void ChangeDiceState(string diceName, int faceId, int type)
    {
        int id = Int32.Parse(diceName.Substring(diceName.Length-1,1)) - 1;

        switch (type)
        {
            case 1:
                diceList[id].state[faceId] = 1;
                break;
            case 2:
                diceList[id].state[faceId] = 0;
                break;
            case 3:
                // 全部净化
                for (int i = 0; i < 5; i++)
                {
                    diceList[id].state[i] = 0;
                }
                break;
        }
        FreezeDice(diceName);
    }

    
}
