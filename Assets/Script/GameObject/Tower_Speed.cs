using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower_Speed : Tower
{
    public Buff addBuff;

    protected override void Attack()
    {
        //todo: 这个还没搞，但我先写了
        //anim.speed = attackSpeed;
        //anim.Play("attack");    // 对于防御塔来说，这个动画就是防守动画


        // 目前就直接扣血了。没写其他的
        target.GetComponent<Obj>().Bleed(attack);
        target.GetComponent<Obj>().AddBuff(addBuff);
        canAttack = false;
        Debug.Log("攻击");
    }
}
