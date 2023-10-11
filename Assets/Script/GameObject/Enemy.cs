using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人
/// </summary>
public class Enemy : Obj
{
    public Buff testBuff; 

    /// <summary>
    /// 检测是否因为击杀数而升级
    /// </summary>
    public void LevelUp(float _upAttack , float _upHp , int levelup)
    {

        if (!HasLevelup)
        {
            attack = attack * (1 + _upAttack * levelup *0.01f);
            hp = defaultHP * (1 + _upHp * levelup * 0.01f);

            // 更新
            if(hp > 0)
            {
                defaultHP = hp;
            }
            else
            {
                defaultHP = -hp; 
            }

            if(attack > 0)
            {
                defaultAttack = attack;

            }
            else
            {
                defaultAttack = -attack;

            }

            hpUI.maxValue = hp;
            HasLevelup = true;
        }

        /*        hpUI.maxValue = hp;
                attack = attack * (1 + _upAttack * levelup);
                hp = hp * (1 + _upHp * levelup);
                hasLevelup = true;*/

    }
    
    
    /// <summary>
    /// 从Mgr中删除
    /// 回收进对象池
    /// </summary>
    public override void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Remove(gameObject);
        BattleMgr.GetInstance().enemyKilled(); 
        SetDefault();
        string name = "Prefabs/Enemys/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }


    private void Update()
    {
        SkillCheck();
        
        checkTarget(); // 检测目标 !!! 目前只是使用激光+攻击距离检测
        UpdateAttackSpeed();

        if (Input.GetKeyDown(KeyCode.K)){
            testBuff.SetBuffTarget(this); 
            AddBuff(testBuff); 
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RemoveBuff(testBuff);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            List<int> test = getBuffList();
            String s =" "; 
            for(int i = 0; i < test.Count; i++)
            {
                s += test[i] + ","; 
            }
            Debug.Log(gameObject.name + ":"  + s);

        }

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

    private void FixedUpdate()
    {
        // 需要精确时间计算/FixedDeltaTime 的放这里
        UseBuffTimer();
    }

    /// <summary>
    /// 检测攻击范围内是否有目标
    /// </summary>
    public virtual void checkTarget()
    {
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Tower");
        Ray ray = new Ray(transform.position, -transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange,layerMask))
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
        // 播放受击特效.该特效位于子节点的最后一个，不要调整
        target.transform.GetChild(target.transform.childCount-1).GetComponent<ParticleSystem>().Play();
        canAttack = false;

    }

    private void Walk()
    {

        transform.position += Vector3.left * speed * Time.deltaTime;

    }
    
    /// <summary>
    /// 检测是否释放技能(如果有)
    /// </summary>
    public virtual void SkillCheck()
    {

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
            // BattleMgr.GetInstance().hp -= gameObject.name.Contains("0-") ? 3 : 1;
            if (gameObject.name.Contains("0-"))
            {
                BattleMgr.GetInstance().PlayerDamage(3);
            }
            else
            {
                BattleMgr.GetInstance().PlayerDamage(1);
            }
            DelFromBattleMgr();
        }
    }



}
