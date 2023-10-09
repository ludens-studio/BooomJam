using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_flag : Tower
{
    [Header("Buff设计")]
    public Buff addBuff;

    protected override void Attack()
    {
        Debug.Log("图腾");
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
