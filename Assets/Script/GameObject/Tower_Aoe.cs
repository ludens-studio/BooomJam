using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Tower_Aoe : Tower
{
    [Header("AOE塔设计以及Boom")]
    public List<GameObject> targets = new List<GameObject>();
    public bool isBoom = false; //是否为炸弹
    public float BoomTimer; 

    private void Update()
    {
        if(!isBoom){
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

    private void FixedUpdate()
    {
        darkTimer(); // 暗属性扣血


        if (isBoom)
        {
            if (BoomTimer > 0)
            {
                BoomTimer -= Time.fixedDeltaTime;
            }
            else
            {
                BoomAttack(); // 炸弹攻击
                Death();
                // 炸弹用完即销毁
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


    /// <summary>
    /// 从Mgr中删除
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
    public override void checkTarget()
    {
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");


        Ray ray = new Ray(transform.position, transform.right);


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }
        }

        // ==================
        ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);

            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }

        }
        //=======================

        //=======================
        ray = new Ray(transform.position + new Vector3(0, -1, 0), transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);

            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }
        }

        if(targets.Count > 0)
        {
            haveTarget= true;
        }

    }

    /// <summary>
    /// 攻击
    /// </summary>
    protected override void Attack()
    {
            foreach(var _target in targets)
            {
                _target.GetComponent<Enemy>().Bleed(attack); 
                _target.transform.GetChild(_target.transform.childCount - 1).GetComponent<ParticleSystem>().Play();
                // 播放受击特效.该特效位于子节点的最后一个，不要调整

            }

        targets.Clear();

        canAttack = false;

    }

    private void BoomAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;

            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().Bleed(attack);
            }

        }
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
