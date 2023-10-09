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
            if(_obj.name != this.gameObject.name && _obj.CompareTag("Tower"))
            {
                Tower t = col.gameObject.GetComponent<Tower>();
                if (!towers.Contains(t))
                {
                    towers.Add(t);
                }
                t.AddBuff(addBuff);


            }
        }
        // 播放受击特效.该特效位于子节点的最后一个，不要调整
        target.transform.GetChild(target.transform.childCount-1).GetComponent<ParticleSystem>().Play();
        canAttack = false;
    }
}
