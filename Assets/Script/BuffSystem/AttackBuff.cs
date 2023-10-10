using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class AttackBuff : Buff
{
    public float amount;

    public override void EnterBuff()
    {
    }

    public override void UseBuff()
    {
        int n = target.CheckBuffCount("Attack_Up"); 
        target.addAttack(amount * n);
        Debug.Log(target.name + "||Atk:  " + n+"|||"+target.attack);
        count--;
    }

    public override void ExitBuff()
    {

    }
}
