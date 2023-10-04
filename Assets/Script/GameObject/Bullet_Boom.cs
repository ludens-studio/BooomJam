using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boom : MonoBehaviour
{
    public float attack; //子弹攻击
    public float boomRange; // 爆炸范围

    public float speed = 10f;
    private Vector3 direction = Vector3.right;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// 给子弹一个新位置
    /// </summary>
    /// <param name="newDirection"></param>
    public void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // other.gameObject.GetComponent<Obj>().Bleed(attack);
            // 若不注释上面这行，则对击中目标还会造成一次伤害

            Debug.Log("击中");
            string name = "Prefabs/Bullets/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
            PoolMgr.GetInstance().PushObj(name, gameObject);

            Vector3 boomLocation = other.transform.position; // 爆炸点
            boomAttack(boomLocation); 
        }
        else
        {
            // 
        }
    }

    private void boomAttack(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, boomRange); //获得范围内所有物体

        foreach (Collider col in colliders)
        {
            GameObject _obj = col.gameObject; 
            
            if (_obj.CompareTag("Enemy"))
            {
                _obj.GetComponent<Obj>().Bleed(attack);
            }
            

        }
    }
}
