using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attack; //�ӵ�����

    public float speed = 10f;
    protected Vector3 direction = Vector3.right;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// ���ӵ�һ����λ��
    /// </summary>
    /// <param name="newDirection"></param>
    public void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    /// <summary>
    /// ��ʼ���ӵ���ֵ
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
            // �����ܻ���Ч.����Чλ���ӽڵ�����һ������Ҫ����
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
        string name = "Prefabs/Bullets/" + gameObject.name.Substring(0, gameObject.name.Length - 7);    // ȥ��(Clone)
        PoolMgr.GetInstance().PushObj(name, gameObject);
    }
}
