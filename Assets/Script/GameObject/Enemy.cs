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

    /// <summary>
    /// 检测是否因为击杀数而升级
    /// </summary>
    public void checkLevelUp()
    {
        if(BattleMgr.GetInstance().hasReachKilled)
        {
            // 如果已经到达
            float _upAttack = BattleMgr.GetInstance().enemyAttackUp; 
            attack = attack * (1 + _upAttack);
        }
    }

    /// <summary>
    /// 加入到Mgr中
    /// </summary>
    public override void AddToBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Add(gameObject); 
    }
    
    /// <summary>
    /// 从Mgr中删除
    /// 回收进对象池
    /// </summary>
    public override void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Remove(gameObject);
        BattleMgr.GetInstance().enemyKilled(); 
        SetDefaultHP();
        string name = "Prefabs/Enemys/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
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
    
    /// <summary>
    /// 进入回收器
    /// 玩家扣血，敌人回满血后回收进池子
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            BattleMgr.GetInstance().hp -= 1;
            DelFromBattleMgr();
        }
    }



}
