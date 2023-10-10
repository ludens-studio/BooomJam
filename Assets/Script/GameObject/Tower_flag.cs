using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_flag : Tower
{
    [Header("Buff���")]
    public Buff addBuff;
    public List<Tower> towers = new List<Tower>();

    protected override void Attack()
    {
        Debug.Log("ͼ��");
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange); //��÷�Χ����������

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

        canAttack = false;

        // �����ܻ���Ч.����Чλ���ӽڵ�����һ������Ҫ����
        //target.transform.GetChild(target.transform.childCount-1).GetComponent<ParticleSystem>().Play();
        //canAttack = false;
    }
}
