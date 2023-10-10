using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float amount;
    [SerializeField] private float amount_real; //ʵ�ʵĵ�����ֵ
    private float save_speed; 
    public override void EnterBuff()
    {
        save_speed = target.GetComponent<Obj>().speed; 

        float tmp = 0; 
        tmp = target.speed + amount;
        if(tmp < 0)
        {
            // ��������ٶ�
            amount_real = target.speed - 0; //�洢����ܹ��޸ĵ���ֵ
        }
        else
        {
            // û�г�������ٶȵĻ���amount_real����amount
            amount_real = amount; 
        }

        target.speed = target.speed + amount_real;
    }

    public override void UseBuff()
    {
        count--; 
    }

    public override void ExitBuff()
    {
        target.speed -= amount_real;
        if(target.speed < 0 || target.speed > save_speed) {
            target.speed = target.save_speed; 
        }
    }



}
