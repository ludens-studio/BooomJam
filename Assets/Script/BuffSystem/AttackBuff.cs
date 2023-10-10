using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class AttackBuff : Buff
{
    public float amount;

    public override void EnterBuff()
    {
        //¡Ÿ ±≤πæ»
        if(isFirst) {
            count_Set = count; 
            isFirst= false;
        }
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
        int n = target.CheckBuffCount("Attack_Up");
        target.addAttack(amount * n);
    }
}
