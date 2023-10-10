using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 我方单位
/// </summary>
public class Tower_Ray : Tower
{

    private void Update()
    {
        UpdateAttackSpeed();

        // 这一行有目标,Attack
        if (canAttack)
        {
            checkAndAttack();

        }
        else
        {
            // 否则Idle
            Idle();

        }

    }


    /// <summary>
    /// 从Mgr中删除
    /// </summary>
    public override void DelFromBattleMgr()
    {
        Vector3 pos = gameObject.transform.position;
        BattleMgr.GetInstance().towers.Remove(gameObject);
        MapMgr.GetInstance().RemoveTower((int)pos.x, (int)-pos.y);
        MapMgr.GetInstance().ReleaseGrid((int)pos.x, (int)-pos.y);
        SetDefaultHP();
        string name = "Prefabs/Towers/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }

    /// <summary>
    /// 检测攻击范围并进行攻击
    /// </summary>
    public void checkAndAttack()
    {
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit[] hits = Physics.RaycastAll(ray,attackRange,layerMask); // 获得范围内所有的敌人
        if(hits.Length> 0 )
        {
            canAttack = false;
            foreach (RaycastHit hit in hits)
            {
                GameObject _target = hit.collider.gameObject;
                if (_target.CompareTag("Enemy"))
                {
                    _target.GetComponent<Enemy>().Bleed(attack);
                }
            }
        }
        else
        {
            canAttack = true;
        }




        Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);


    }

    /// <summary>
    /// 攻击
    /// </summary>
    protected override void Attack()
    {

        // 这个塔用激光攻击了，之后这块的动画得加到上面去
        // 或者改上面的部分也行

        //todo: 这个还没搞，但我先写了
        //anim.speed = attackSpeed;
        //anim.Play("attack");    // 对于防御塔来说，这个动画就是防守动画


        // 目前就直接扣血了。没写其他的

/*        target.GetComponent<Obj>().Bleed(attack);
        canAttack = false;
        Debug.Log("攻击");*/
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
        PoolMgr.GetInstance().GetObj("Prefabs/Bullets/" + Bullet.name, o =>
        {
            o.gameObject.transform.position = firePoint.position;
            o.transform.parent = GameObject.Find("PoolBullet").transform;
        });

        canAttack = false; 
        // _bullet.GetComponent<Bullet>().

    }
}