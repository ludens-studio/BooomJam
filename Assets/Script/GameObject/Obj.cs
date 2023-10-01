using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象总类
/// </summary>
public class Obj : MonoBehaviour
{
    // 攻击力
    public float attack;

    // 血量
    public float hp;

    // 移速
    public float speed;

    // 攻速
    public float attackSpeed;

    // 状态
    public ObjState state;

    // 动画机
    public Animator anim;

    public enum ObjState
    {
        Active,
        Death
    }
    
    private void LateUpdate()
    {
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
        gameObject.SetActive(false);
        // todo: obj pool
    }

    /// <summary>
    /// 攻击
    /// 可以是远程（塔）或近战（敌人）
    /// </summary>
    protected virtual void Attack()
    {
        
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void Bleed(float harm)
    {
        hp -= harm;
        if (hp <= 0)
        {
            state = ObjState.Death;
        }
    }
    
}




