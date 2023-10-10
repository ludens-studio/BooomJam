using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Buff : ScriptableObject
{

    public Obj target;
    public string BuffName;
    public int count_Set;
    public bool isFirst = true; 
    public int count; // buff的持续时间，写x秒

    public void SetBuffTarget(Obj target )
    {
        this.target = target;
    }

    public void SetBuffCount(int count)
    {
        this.count = count;
    }

    public abstract void EnterBuff();

    public abstract void UseBuff();

    public abstract void ExitBuff();

    
    
}
