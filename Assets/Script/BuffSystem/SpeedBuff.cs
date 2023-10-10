using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float amount;
    [SerializeField] private float amount_real; //ʵ�ʵĵ�����ֵ
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
