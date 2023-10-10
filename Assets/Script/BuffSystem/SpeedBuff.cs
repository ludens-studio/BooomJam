using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float amount;

    public override void EnterBuff()
    {
        if (isFirst)
        {
            count_Set = count;
            isFirst = false;
        }

        /*        save_speed = target.GetComponent<Obj>().speed; 

                if(target.GetComponent<Obj>().CheckBuffCount("Speed_down") <= 1) {
                    float tmp = 0;
                    tmp = target.speed + amount;
                    if (tmp < 0)
                    {
                        // ��������ٶ�
                        amount_real = 0 - target.speed; //�洢����ܹ��޸ĵ���ֵ
                    }
                    else
                    {
                        // û�г�������ٶȵĻ���amount_real����amount
                        amount_real = amount;
                    }

                    target.speed = target.speed + amount_real;
                    isUse= true;
                }
                else
                {
                    isUse = false;
                }*/


    }

    public override void UseBuff()
    {
        int n = target.CheckBuffCount("Speed_Up");
        target.addSpeed(amount * n);
        Debug.Log(target.name + "||Speed:  " + n);
        count--; 
    }

    public override void ExitBuff()
    {
/*        if (isUse)
        {
            target.speed -= amount_real;
        }*/

    }



}
