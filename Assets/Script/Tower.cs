using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 我方单位
/// </summary>
public class Tower : Obj
{
    /// <summary>
    /// 塔的类型
    /// </summary>
    public enum TowerType
    {
        AttackTower,
        DefenceTower
    }
    private void Update()
    {
        // 这一行有敌人,Attack
        
        // 否则Idle
        
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Attack()
    {
        anim.speed = attackSpeed;
        anim.Play("attack");    // 对于防御塔来说，这个动画就是防守动画
    }

    /// <summary>
    /// 静息状态
    /// </summary>
    public void Idle()
    {
        anim.speed = 1;
        anim.Play("idle");
    }

    /// <summary>
    /// 动画事件，发射子弹
    /// </summary>
    public void Shoot()
    {
        // todo: 射子弹
    }
}