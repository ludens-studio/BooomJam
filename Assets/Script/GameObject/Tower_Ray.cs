using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ҷ���λ
/// </summary>
public class Tower_Ray : Tower
{

    private void Update()
    {
        UpdateAttackSpeed();

        // ��һ����Ŀ��,Attack
        if (canAttack)
        {
            checkAndAttack();

        }
        else
        {
            // ����Idle
            Idle();

        }

    }


    /// <summary>
    /// ��Mgr��ɾ��
    /// </summary>
    public override void DelFromBattleMgr()
    {
        Vector3 pos = gameObject.transform.position;
        BattleMgr.GetInstance().towers.Remove(gameObject);
        MapMgr.GetInstance().RemoveTower((int)pos.x, (int)-pos.y);
        MapMgr.GetInstance().ReleaseGrid((int)pos.x, (int)-pos.y);
        SetDefaultHP();
        string name = "Prefabs/Towers/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // ȥ��(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }

    /// <summary>
    /// ��⹥����Χ�����й���
    /// </summary>
    public void checkAndAttack()
    {
        // ! �ⲿ�����ļ��⳯�Ҵ򣬹ֵļ��⳯�����������������ٸ�
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit[] hits = Physics.RaycastAll(ray,attackRange,layerMask); // ��÷�Χ�����еĵ���
        if(hits.Length> 0 )
        {
            canAttack = false;
            foreach (RaycastHit hit in hits)
            {
                GameObject _target = hit.collider.gameObject;
                if (_target.CompareTag("Enemy"))
                {
                    _target.GetComponent<Enemy>().Bleed(attack);
                }
            }
        }
        else
        {
            canAttack = true;
        }




        Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);


    }

    /// <summary>
    /// ����
    /// </summary>
    protected override void Attack()
    {

        // ������ü��⹥���ˣ�֮�����Ķ����üӵ�����ȥ
        // ���߸�����Ĳ���Ҳ��

        //todo: �����û�㣬������д��
        //anim.speed = attackSpeed;
        //anim.Play("attack");    // ���ڷ�������˵������������Ƿ��ض���


        // Ŀǰ��ֱ�ӿ�Ѫ�ˡ�ûд������

/*        target.GetComponent<Obj>().Bleed(attack);
        canAttack = false;
        Debug.Log("����");*/
    }

    /// <summary>
    /// ��Ϣ״̬
    /// </summary>
    public void Idle()
    {
        // todo: ���Ҳ�ǣ�����֮���һ������������һЩ�Ƕ����¼�
        //anim.speed = 1;
        //anim.Play("idle");
    }

    /// <summary>
    /// �����¼��������ӵ�
    /// </summary>
    public void Shoot()
    {
        // todo: ���ӵ�
        PoolMgr.GetInstance().GetObj("Prefabs/Bullets/" + Bullet.name, o =>
        {
            o.gameObject.transform.position = firePoint.position;
            o.transform.parent = GameObject.Find("PoolBullet").transform;
        });

        canAttack = false; 
        // _bullet.GetComponent<Bullet>().

    }
}