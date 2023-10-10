using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Tower_Aoe : Tower
{
    [Header("AOE������Լ�Boom")]
    public List<GameObject> targets = new List<GameObject>();
    public bool isBoom = false; //�Ƿ�Ϊը��
    public float BoomTimer; 

    private void Update()
    {
        if(!isBoom){
            checkTarget(); // ���Ŀ�� !!! Ŀǰֻ��ʹ�ü���+����������
            UpdateAttackSpeed();

            // ��һ����Ŀ��,Attack
            if (haveTarget && canAttack)
            {
                Attack();
            }
            else
            {
                // ����Idle
                Idle();

            }
        }

        

    }

    private void FixedUpdate()
    {
        darkTimer(); // �����Կ�Ѫ


        if (isBoom)
        {
            if (BoomTimer > 0)
            {
                BoomTimer -= Time.fixedDeltaTime;
            }
            else
            {
                BoomAttack(); // ը������
                Death();
                // ը�����꼴����
            }
        }
    }

    private void darkTimer()
    {
        if (dark_debuff_Hp_Timer <= 0f)
        {
            Bleed(dark_debuff_Hp);
            dark_debuff_Hp_Timer = 1.0f;
        }
        else
        {
            dark_debuff_Hp_Timer -= Time.fixedDeltaTime;
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
        SetDefault();
        string name = "Prefabs/Towers/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // ȥ��(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }

    /// <summary>
    /// ��⹥����Χ���Ƿ���Ŀ��
    /// </summary>
    public override void checkTarget()
    {
        // ! �ⲿ�����ļ��⳯�Ҵ򣬹ֵļ��⳯�����������������ٸ�
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");


        Ray ray = new Ray(transform.position, transform.right);


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }
        }

        // ==================
        ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);

            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }

        }
        //=======================

        //=======================
        ray = new Ray(transform.position + new Vector3(0, -1, 0), transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Enemy"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
                targets.Add(target);

            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            if (!haveTarget)
            {
                haveTarget = false;
                target = null;
            }
        }

        if(targets.Count > 0)
        {
            haveTarget= true;
        }

    }

    /// <summary>
    /// ����
    /// </summary>
    protected override void Attack()
    {
            foreach(var _target in targets)
            {
                _target.GetComponent<Enemy>().Bleed(attack); 
                _target.transform.GetChild(_target.transform.childCount - 1).GetComponent<ParticleSystem>().Play();
                // �����ܻ���Ч.����Чλ���ӽڵ�����һ������Ҫ����

            }

        targets.Clear();

        canAttack = false;

    }

    private void BoomAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange); //��÷�Χ����������

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;

            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().Bleed(attack);
            }

        }
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
}
