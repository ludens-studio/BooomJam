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
        //target.attack -= amount;

/*        float tmp = 0;
        tmp = target.attack - amount;
        if (tmp < 0)
        {
            // 超出最低速度
            amount_real = target.attack - 0; //存储最大能够修改的数值
        }
        else
        {
            // 没有超出最低速度的话，amount_real就是amount
            amount_real = amount;
        }

        target.attack -= amount_real;*/

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
