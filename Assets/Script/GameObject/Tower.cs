using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 我方单位
/// </summary>
public class Tower : Obj
{
    public bool shootTower = false; // 启用该项，tower会使用子弹攻击
    public bool isFlag = false; 

    [Header("Attack Type")]
    public bool isAOE = false;
    public GameObject Bullet; // 生成的子弹
    public Transform firePoint; // 生成子弹的位置

    [Header("Dark Type")]
    public bool isDark = false; // 是否是暗属性
    public float dark_debuff_Hp; // 如果是暗属性，一秒扣多少血
    protected float dark_debuff_Hp_Timer = 1.0f;

    private void Update()
    {
        checkTarget(); // 检测目标 !!! 目前只是使用激光+攻击距离检测

        // 这一行有目标,Attack
        if (isFlag)
        {
            // 图腾时间到了就攻击
            if(canAttack)
            {
                Attack(); 
            }
        }
        else
        {

            if (target == null)
            {
                haveTarget = false;
            }

            if (haveTarget && canAttack)
            {
                if(isAOE)
                {
                    if (shootTower)
                    {
                        Debug.Log(gameObject.name + "||" + haveTarget + "|||" + target.name + "||||" + target.transform.position.y);
                        Shoot();

                    }
                    else
                    {
                        Attack();
                    }
                    canAttack = false;
                }
                else
                {
                    if(target.transform.position.y == transform.position.y)
                    {
                        // 单行攻击的只能在同一行射击
                        if (shootTower)
                        {
                            Debug.Log(gameObject.name + "||" + haveTarget + "|||" + target.name + "||||" + target.transform.position.y);
                            Shoot();

                        }
                        else
                        {
                            Attack();
                        }
                        canAttack = false;
                    }
 
                }

            }
            else
            {
                // 否则Idle
                Idle();

            }
        }
       

    }

    private void FixedUpdate()
    {
        darkTimer(); // 暗属性扣血
        UseBuffTimer();
        UpdateAttackSpeed();
    }

    private void darkTimer()
    {
        if(isDark)
        {
            if (dark_debuff_Hp_Timer <= 0f)
            {
                if (dark_debuff_Hp > 0)
                {
                    Bleed(dark_debuff_Hp);
                    dark_debuff_Hp_Timer = 1.0f;
                }

            }
            else
            {
                dark_debuff_Hp_Timer -= Time.fixedDeltaTime;
            }
        }

    }


    /// <summary>
    /// 从Mgr中删除
    /// 回到对象池
    /// </summary>
    public override void DelFromBattleMgr()
    {
        Vector3 pos = gameObject.transform.position;
        BattleMgr.GetInstance().towers.Remove(gameObject);
        MapMgr.GetInstance().RemoveTower((int)pos.x, (int)-pos.y);
        MapMgr.GetInstance().ReleaseGrid((int)pos.x, (int)-pos.y);
        SetDefault();
        string name = "Prefabs/Towers/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }

    /// <summary>
    /// 检测攻击范围内是否有目标
    /// </summary>
    public virtual void checkTarget()
    {

        if (target == null)
        {
            haveTarget = false;
        }


        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange , layerMask))
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

        if(target == null)
        {
            haveTarget = false; 
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

        print(target.name);
        // 目前就直接扣血了。没写其他的
        target.GetComponent<Obj>().Bleed(attack);
        // 播放受击特效.该特效位于子节点的最后一个，不要调整
        target.transform.GetChild(target.transform.childCount-1).GetComponent<ParticleSystem>().Play();
        canAttack = false;
        Debug.Log("攻击");
    }

    /// <summary>
    /// 静息状态
    /// </summary>
    public void Idle()
    {
        // todo: 未进攻时
        //anim.speed = 1;
        //anim.Play("idle");
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    public void Shoot()
    {
        PoolMgr.GetInstance().GetObj("Prefabs/Bullets/" + Bullet.name, o =>
        {
            o.gameObject.GetComponent<Bullet>().InitBullet(attack);  // 设置子弹的伤害为塔的伤害
            o.gameObject.transform.position = firePoint.position;
            o.transform.parent = GameObject.Find("PoolBullet").transform;
        });

        canAttack = false; 
         // _bullet.GetComponent<Bullet>().

    }


}