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
        UpdateAttackSpeed();

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
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            haveTarget = false;
            target = null;
        }

    }

    /// <summary>
    /// 攻击
    /// </summary>
    protected override void Attack()
    {
        //todo: 这个还没搞，但我先写了
        //anim.speed = attackSpeed;
        //anim.Play("attack");    // 对于防御塔来说，这个动画就是防守动画


        // 目前就直接扣血了。没写其他的

        target.GetComponent<Obj>().Bleed(attack);
        canAttack = false;
        Debug.Log("攻击");
    }

    /// <summary>
    /// 静息状态
    /// </summary>
    public void Idle()
    {
        // todo: 这个也是，就是之后加一个动画机，有一些是动画事件
        //anim.speed = 1;
        //anim.Play("idle");
    }

    /// <summary>
    /// 动画事件，发射子弹
    /// </summary>
    public void Shoot()
    {
        // todo: 射子弹
    }
}