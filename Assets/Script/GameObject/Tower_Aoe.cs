using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Tower_Aoe : Obj
{
    [Header("Boom")]
    public bool isBoom = false; //是否为炸弹
    public float BoomTimer; 

    [Header("Dark Type")]
    public bool isDark = false; // 是否是暗属性
    public float dark_debuff_Hp; // 如果是暗属性，一秒扣多少血
    private float dark_debuff_Hp_Timer = 1.0f;

    private void Update()
    {
        darkTimer(); // 暗属性扣血

        if (isBoom)
        {
            if(BoomTimer> 0)
            {
                BoomTimer -= Time.fixedDeltaTime;
            }
            else
            {
                Attack(); 
                Death();
                // 炸弹用完即销毁
            }
        }
        else
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

        

    }

    private void darkTimer()
    {
        if (dark_debuff_Hp_Timer <= 0f)
        {
            Bleed(dark_debuff_Hp);
            dark_debuff_Hp_Timer = 1.0f;
        }
        else
        {
            dark_debuff_Hp_Timer -= Time.fixedDeltaTime;
        }
    }

    /// 
    /// 加入到Mgr中
    ///
    public override void AddToBattleMgr()
    {
        BattleMgr.GetInstance().towers.Add(gameObject);
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

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;

            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().Bleed(attack);
            }
        }

        canAttack = false;

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
}
