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
        checkTarget(); // 检测目标 !!! 目前只是使用激光+攻击距离检测

        // 这一行有目标,Attack
        if (haveTarget && canAttack)
        {
            Attack(); 
        }
        else
        {
            // 否则Idle
            Idle();

        }

    }

    /// 
    /// 加入到Mgr中
    ///
    public void AddToBattleMgr()
    {
        BattleMgr.GetInstance().towers.Add(gameObject);
    }
    /// <summary>
    /// 从Mgr中删除
    /// </summary>
    public void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().towers.Remove(gameObject);
    }

    /// <summary>
    /// 检测攻击范围内是否有目标
    /// </summary>
    public void checkTarget()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy"); // 只检测Enemy层
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
        {
            if (hit.collider.CompareTag("enemy"))
            {
                // 范围内有enemy
                haveTarget = true;
            }else
            {
                haveTarget= false;
            }
        }

    }

    /// <summary>
    /// 攻击
    /// </summary>
    protected override void Attack()
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