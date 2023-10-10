using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float amount;
    [SerializeField] private float amount_real; //实际的调整数值
    private float save_speed; 

    private bool isUse = false ;
    public override void EnterBuff()
    {
        save_speed = target.GetComponent<Obj>().speed; 

        if(target.GetComponent<Obj>().CheckBuffCount("Speed_down") <= 1) {
            float tmp = 0;
            tmp = target.speed + amount;
            if (tmp < 0)
            {
                // 超出最低速度
                amount_real = 0 - target.speed; //存储最大能够修改的数值
            }
            else
            {
                // 没有超出最低速度的话，amount_real就是amount
                amount_real = amount;
            }

            target.speed = target.speed + amount_real;
            isUse= true;
        }
        else
        {
            isUse = false;
        }
       

    }

    public override void UseBuff()
    {
        count--; 
    }

    public override void ExitBuff()
    {
        if (isUse)
        {
            target.speed -= amount_real;
        }

    }



}
