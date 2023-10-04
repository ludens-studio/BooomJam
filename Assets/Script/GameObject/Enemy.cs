using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 敌人
/// </summary>
public class Enemy : Obj
{

    /// 
    /// 加入到Mgr中
    ///
    public override void AddToBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Add(gameObject); 
    }
    /// <summary>
    /// 从Mgr中删除
    /// </summary>
    public override void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Remove(gameObject);
    }


    private void Update()
    {
        checkTarget(); // 检测目标 !!! 目前只是使用激光+攻击距离检测
        UpdateAttackSpeed(); 

        // 这一行有目标,Attack
        if (haveTarget)
        {
            if (canAttack)
            {
                Attack();

            }
        }
        else
        {
            // 否则移动

            Walk();

        }
    }

    /// <summary>
    /// 检测攻击范围内是否有目标
    /// </summary>
    public void checkTarget()
    {
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Tower");
        Ray ray = new Ray(transform.position, -transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Tower"))
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

    protected override void Attack()
    {
        // 目前就直接扣血了。没写其他的
        target.GetComponent<Obj>().Bleed(attack); 
        canAttack = false;

    }

    private void Walk()
    {

        transform.position += Vector3.left * speed * Time.deltaTime;

    }


}
