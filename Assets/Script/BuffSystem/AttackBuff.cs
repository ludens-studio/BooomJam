using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class AttackBuff : Buff
{
    public float amount; 
    public override void EnterBuff()
    {
        target.attack += amount;
    }

    public override void UseBuff()
    {
        count--;
    }

    public override void ExitBuff()
    {
        target.attack -= amount;
    }
}
