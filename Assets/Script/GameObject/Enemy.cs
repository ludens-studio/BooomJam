using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人
/// </summary>
public class Enemy : Obj
{

    /// 
    /// 加入到Mgr中
    ///
    protected override void AddToBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Add(gameObject); 
    }
    /// <summary>
    /// 从Mgr中删除
    /// </summary>
    protected override void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Remove(gameObject);
    }


    private void Update()
    {
        throw new NotImplementedException();
        checkTarget(); // 检测目标 !!! 目前只是使用激光+攻击距离检测

        // 这一行有目标,Attack
        if (haveTarget && canAttack)
        {
            Attack();
        }
        else
        {
            // 否则Idle
            // Idle();

        }
    }

    /// <summary>
    /// 检测攻击范围内是否有目标
    /// </summary>
    public void checkTarget()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tower");

    }

    private void Walk()
    {
        
    }
    
    
}
