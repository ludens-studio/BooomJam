using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_flag : Tower
{
    [Header("Buff相关")]
    public Buff addBuff;
    public List<Tower> towers = new List<Tower>();

    protected override void Attack()
    {
        Debug.Log("图腾");
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;
            if(_obj.name != this.gameObject.name)
            {
                Tower t = col.gameObject.GetComponent<Tower>();
                towers.Add(t);
                t.AddBuff(addBuff);
            }
        }

        canAttack = false;
    }

    protected override void Death()
    {
        DelFromBattleMgr(); // 从对应的列表中删除该对象
        gameObject.SetActive(false);

        if (isFlag)
        {
            foreach(Tower t in towers)
            {
                t.RemoveBuff(addBuff);
            }
        }
    }
}
