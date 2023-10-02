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
    public void AddToBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Add(gameObject); 
    }
    /// <summary>
    /// 从Mgr中删除
    /// </summary>
    public void DelFromBattleMgr()
    {
        BattleMgr.GetInstance().enemies.Remove(gameObject);
    }


    private void Update()
    {
        throw new NotImplementedException();
    }

    private void Walk()
    {
        
    }
    
    
}
