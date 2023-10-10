using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boom : Bullet
{
    public float boomRange; 

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public override void bulletEffect(Collider other)
    {
        string name = "Prefabs/Bullets/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // ȥ��(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);

        Vector3 boomLocation = other.transform.position; // ��ը��
        boomAttack(boomLocation);
    }

    private void boomAttack(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, boomRange); //��÷�Χ����������

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject; 
            
            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().Bleed(attack);
                // �����ܻ���Ч.����Чλ���ӽڵ�����һ������Ҫ����
                _obj.transform.GetChild(_obj.transform.childCount-1).GetComponent<ParticleSystem>().Play(true);
            }
            

        }
    }
}
