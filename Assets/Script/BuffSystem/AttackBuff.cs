using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class AttackBuff : Buff
{
    public float amount;
    [SerializeField] private float amount_real; //实际的调整数值

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

        target.attack -= amount_real;

        if(target.attack - amount <0)
        {
            target.attack = 1;
        }
        else
        {
            target.attack -= amount; 
        }
    }
}
