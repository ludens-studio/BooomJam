using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_flag : Tower
{

    public Buff addBuff;

    protected override void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;

            if (_obj.CompareTag("Tower"))
            {
                _obj.GetComponent<Obj>().AddBuff(addBuff);
            }
        }

        canAttack = false;
    }
}
