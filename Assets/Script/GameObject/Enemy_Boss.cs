using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy_Boss : Enemy
{

    [Header("Boss相关的数值设计")]
    public float ToNextLevelHp; // 什么时候进入二阶段(血量)
    [SerializeField] private bool UpLevel; //进入二阶段
    public float newSpeed; //二阶段修改移速至xx
    public float healEnemy_Time; //使用回血的时候停止移动的时间
    public float healEnemy_Range; //回血范围   【我这里写了20代表全屏】
    public float healEnemy_amount; //回多少血
    public float healEnemy_CD; //使用CD ;  
    private float healEnemy_CD_Timer; //计时器 ;  


    public override void checkTarget()
    {
        // ! 这部分塔的激光朝右打，怪的激光朝左打。如果有特殊需求再改
        //==================================================
        int layerMask = 1 << LayerMask.NameToLayer("Tower"); 

        // ! 我这里直接写轮流三条射线检测了，可能有bug

        Ray ray = new Ray(transform.position, -transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Tower"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            haveTarget = false;
            target = null;
        }

        // ==================
        ray = new Ray(transform.position + new Vector3(0,1,0), -transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Tower"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            haveTarget = false;
            target = null;
        }
        //=======================

        //=======================
        ray = new Ray(transform.position + new Vector3(0, -1, 0), -transform.right);
        if (Physics.Raycast(ray, out hit, attackRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            if (hit.collider.CompareTag("Tower"))
            {
                haveTarget = true;
                target = hit.collider.gameObject;
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * attackRange, Color.blue);
            haveTarget = false;
            target = null;
        }
        //==============================
        canAttack= false;
    }

    public override void Bleed(float harm)
    {
        hp -= harm;

        if(!UpLevel)
        {
            if(hp <= ToNextLevelHp)
            {
                // 更新速度并标记进入二阶段
                speed = newSpeed;
                UpLevel= true;
                healEnemy_CD_Timer = healEnemy_CD; 
            }
        }

        if (hp <= 0)
        {
            state = ObjState.Death;
        }
    }


    public override void SkillCheck()
    {
        if (UpLevel)
        {
            if (healEnemy_CD_Timer <= 0)
            {
                //放回血技能
                HealEnemy();
                healEnemy_CD_Timer = healEnemy_CD;
            }
            else
            {
                healEnemy_CD_Timer -= Time.fixedDeltaTime;

            }

            if (healEnemy_CD_Timer <= healEnemy_CD - healEnemy_Time)
            {
                speed = newSpeed; // 恢复移动
            }
        }

    }

    private void HealEnemy()
    {
        speed = 0; //停止移动
        Collider[] colliders = Physics.OverlapSphere(transform.position , healEnemy_Range); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject;

            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().hp += healEnemy_amount; 
                // 回血
            }


        }
    }
}
