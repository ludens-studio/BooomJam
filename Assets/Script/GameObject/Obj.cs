using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

/// <summary>
/// 对象总类
/// </summary>
public class Obj : MonoBehaviour
{
    // 攻击力
    public float attack;

    // 攻击范围内是否有目标
    public bool haveTarget = false; 

    // 血量
    public float hp;
    public Slider hpUI;

    // 移速
    public float speed;

    // 攻速
    public float attackSpeed;
    [SerializeField] private float attackSpeed_true  ; 

    [SerializeField]private float attackSpeedTimer;
    public bool canAttack;
    public GameObject target; // 通用的，当前攻击的目标
    // attackSpeedTimer倒数，到0，canAttack为真且不倒计时


    // 攻击范围
    public float attackRange; 

    // 状态
    public ObjState state;

    // 动画机
    public Animator anim;

    // 原始血量
    private float defaultHP;
    [SerializeField]private float defaultSpeed;
    private float defaultAttack;
    [SerializeField]protected bool HasLevelup = false; // 是否已经被升级过了

    // Buff
    [SerializeField] private float BuffTimer = 1.0f; 
    public List<Buff> buffs= new List<Buff>();
    public Dictionary<String , int> buffs_Dic = new Dictionary<String , int>(); // 拥有的buff

    [SerializeField]private List<Buff> buff_cache_add =  new List<Buff>();
    [SerializeField] private List<Buff> buff_cache_del =  new List<Buff>();

    // 

    public enum ObjState
    {
        Active,
        Death
    }


    private void Awake()
    {
        state = ObjState.Active;
        defaultHP = hp;
        defaultSpeed = speed;
        defaultAttack = attack;
        HasLevelup = false; 
        hpUI.maxValue = hp;
        InitBuffDic();

    }

    public void UpdateAttackSpeed()
    {
        attackSpeed_true = 1 / attackSpeed;  // 换算，策划案里面的攻速是这里实际表现的 1/speed 秒攻击一次

        if (attackSpeedTimer <= 0.001f)
        {
            canAttack = true;
            attackSpeedTimer = attackSpeed_true;
        }
        else
        {
            attackSpeedTimer -= Time.deltaTime; 
        }
    }
    
    private void LateUpdate()
    {
        hpUI.value = hp;
        if (state == ObjState.Death)
        {
            Death();
        }
    }

    /// <summary>
    /// 对象死亡，可以被覆写
    /// </summary>
    protected virtual void Death()
    {
        DelFromBattleMgr(); // 从对应的列表中删除该对象
        gameObject.SetActive(false);
    }

    public virtual void DelFromBattleMgr()
    {
        // 
    }

    /// <summary>
    /// 回对象池之后需要把hp设回初始值
    /// </summary>
    public void SetDefault()
    {
        print(gameObject.name + " has set default hp: " + defaultHP);
        hp = defaultHP;
        speed = defaultSpeed;
        attack = defaultAttack;
        HasLevelup = false;
    }


    /// <summary>
    /// 攻击
    /// 可以是远程（塔）或近战（敌人）
    /// </summary>
    protected virtual void Attack()
    {
        canAttack = false; 
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public virtual void Bleed(float harm)
    {
        hp -= harm;
        if (hp <= 0)
        {
            state = ObjState.Death;
        }
    }

    /// <summary>
    /// 计算两个物体a和b在地图上的距离（用Postion算的）
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public float calDistance_Postion(GameObject a  , GameObject b)
    {
        float distance;
        distance = Vector3.Distance(a.transform.position, b.transform.position);

        return distance; 
    }

    /// <summary>
    /// 计算当前物体到另外一个物体a直接的距离（用Postion算的）
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public float calDistance_Postion(GameObject a)
    {
        float distance;
        distance = Vector3.Distance(a.transform.position, gameObject.transform.position);

        return distance;
    }


    public void UseBuffTimer()
    {
        RemoveBuff_Real(); 
        if (BuffTimer <= 0.0f)
        {
            BuffTimer = 1.0f;
            UseBuffs();
        }
        else
        {
            BuffTimer -= Time.fixedDeltaTime;
        }

        AddBuff_Real(); 

    }

    public void AddBuff(Buff buff)
    {
        buff_cache_add.Add(buff);
    }

    /// <summary>
    /// 实际上加入Buff的函数
    /// </summary>
    private void AddBuff_Real()
    {
        if(buff_cache_add.Count> 0)
        {
            foreach (Buff buff in buff_cache_add)
            {
                buff.SetBuffTarget(this);
                if (buffs_Dic.ContainsKey(buff.BuffName))
                {
                    buffs_Dic[buff.BuffName]++;
                    Debug.Log("For" +gameObject.name+ buff.name +":"+ buffs_Dic[buff.BuffName]); 
                }
                else
                {
                    Debug.Log("Buff有误");
                }
                buffs.Add(buff);
                buff.EnterBuff();
            }
        }

        buff_cache_add.Clear(); // 清空Buff栏

    }

    public void UseBuffs()
    {
        if(buffs.Count > 0)
        {
            for(int i =  0;  i< buffs.Count; i++)
            {
                Buff _buff = buffs[i];

                if (_buff.count <= 0)
                {
                    Debug.Log(gameObject.name + "REOMOVE:  " + _buff.name);
                    _buff.count = _buff.count_Set; 
                    RemoveBuff(_buff);
                }
                else
                {
                    _buff.UseBuff();
                    Debug.Log(gameObject.name + "USE!!!!!!!!!!:  " + _buff.name);

                }
            }
        }

    }

    public void RemoveBuff(Buff buff)
    {
        buff_cache_del.Add(buff);
    }

    /// <summary>
    /// 实际删除的部分
    /// </summary>
    public void RemoveBuff_Real()
    {
        if(buff_cache_del.Count > 0)
        {
            foreach (Buff buff in buff_cache_del)
            {
                buffs.Remove(buff);
                //buff.ExitBuff();
                if (buffs_Dic.ContainsKey(buff.BuffName))
                {
                    buffs_Dic[buff.BuffName]--;
                    if (buffs_Dic[buff.BuffName] < 0)
                    {
                        Debug.Log("Buff Count Error");
                    }
                }
                else
                {
                    Debug.Log("Buff有误");
                }
                //buffs.Remove(buff);
                buff.ExitBuff();

            }


        }

        buff_cache_del.Clear();


    }


    private void InitBuffDic()
    {
        // 初始化buff子弹
        buffs_Dic.Add("Speed_Up", 0);
        buffs_Dic.Add("Speed_Down", 0);
        buffs_Dic.Add("Attack_Up", 0);
        buffs_Dic.Add("Attack_Down", 0);

    }

    /// <summary>
    /// 得到一个代表buff层数的List,按特定顺序排列，顺序同“InitBuffDic”中的排序
    /// </summary>
    /// <returns></returns>
    public List<int> getBuffList()
    {
        List<int> list = new List<int>();
        list.Add(CheckBuffCount("Speed_Up"));
        list.Add(CheckBuffCount("Speed_Down"));
        list.Add(CheckBuffCount("Attack_Up"));
        list.Add(CheckBuffCount("Attack_Down"));


        return list;
    }

    /// <summary>
    /// 返回"buffName"的在当前单位的层数，如果没有则是0，有的话是一个大于1的数值（buff可叠加）。如果buffName有误则返回-1
    /// </summary>
    /// <param name="buffName"></param>
    /// <returns></returns>
    public int CheckBuffCount(string buffName)
    {
        if (buffs_Dic.ContainsKey(buffName))
        {
            return buffs_Dic[buffName];
        }
        else
        {
            return -1; 
        }
    }

    /// <summary>
    /// 得到已持有buff的List，目前只检测"Speed_Down"，"Attack_Up"
    /// </summary>
    /// <returns></returns>

    public List<String> getBuffList_String()
    {
        List<String> list = new List<String>();
        if(CheckBuffCount("Speed_Down") >= 1){
            list.Add("Speed_Down");
        }

        if(CheckBuffCount("Attack_Up") >= 1)
        {
            list.Add("Attack_Up");
        }

        return list ; 

    }

    public void addAttack(float amount)
    {
        attack = defaultAttack + amount; 
    }

    public void addSpeed(float amount)
    {
        speed = defaultSpeed + amount;
        if(speed <= 0)
        {
            speed = 0.05f; //最低速度
        }
    }


    // 测试用=================
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        /*        Vector3 start = new Vector3(transform.position.x, transform.position.y, 0);
                Vector3 end = new Vector3(transform.lossyScale.x * (transform.position.x + attackRange), transform.position.y, 0);
                Gizmos.DrawLine(start, end);*/

        // Gizmos.DrawWireSphere(transform.position, attackRange);

    }

}
