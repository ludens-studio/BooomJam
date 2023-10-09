using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attack; //子弹攻击

    public float speed = 10f;
    protected Vector3 direction = Vector3.right;

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

    /// <summary>
    /// 初始化子弹数值
    /// </summary>
    /// <param name="other"></param>

    public void InitBullet(float attack)
    {
        this.attack = attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            bulletEffect(other); 
            // 播放受击特效.该特效位于子节点的最后一个，不要调整
            other.transform.GetChild(other.transform.childCount-1).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            // 
        }
    }

    public virtual void bulletEffect(Collider other)
    {
        other.gameObject.GetComponent<Obj>().Bleed(attack);
        string name = "Prefabs/Bullets/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // 去掉(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }
}
