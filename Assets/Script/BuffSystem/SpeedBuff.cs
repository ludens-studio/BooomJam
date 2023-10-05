using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float amount; 
    public override void EnterBuff()
    {
        target.speed += amount; 
    }

    public override void UseBuff()
    {
        count--; 
    }

    public override void ExitBuff()
    {
        target.speed -= amount;
    }



}
