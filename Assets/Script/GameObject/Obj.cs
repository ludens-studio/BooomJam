using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

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

    // Buff
    [SerializeField] private float BuffTimer = 1.0f; 
    public List<Buff> buffs= new List<Buff>();

    public enum ObjState
    {
        Active,
        Death
    }

    private void Start()
    {
        defaultHP = hp;
        hpUI.maxValue = hp;
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

    public virtual void AddToBattleMgr()
    {
        // 
    }

    public virtual void DelFromBattleMgr()
    {
        // 
    }

    /// <summary>
    /// 回对象池之后需要把hp设回初始值
    /// </summary>
    public void SetDefaultHP()
    {
        hp = defaultHP;
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
        if(buffs.Count> 0)
        {
            if (BuffTimer <= 0.0f)
            {
                BuffTimer = 1.0f;
                UseBuffs();
            }
            else
            {
                BuffTimer -= Time.fixedDeltaTime;
            }
        }

    }

    public void AddBuff(Buff buff)
    {
        buff.SetBuffTarget(this); 
        buffs.Add(buff);
        buff.EnterBuff();
    }

    public void UseBuffs()
    {
        if(buffs.Count > 0)
        {
            foreach (Buff _buff in buffs)
            {
                if (_buff.count <= 0)
                {
                    RemoveBuff(_buff);
                }
                else
                {
                     _buff.UseBuff();
                }
            }
        }

    }

    public void RemoveBuff(Buff buff)
    {
        buff.ExitBuff();
        buffs.Remove(buff);
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
