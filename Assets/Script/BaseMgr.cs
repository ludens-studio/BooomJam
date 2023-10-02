using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1.C#中 泛型的知识
//2.设计模式中 单例模式的知识
public class BaseMgr<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if( instance == null )
        {
            GameObject obj = new GameObject();
            //设置对象的名字为脚本名
            obj.name = typeof(T).ToString();
            //让这个单例模式对象 过场景 不移除
            //因为 单例模式对象 往往 是存在整个程序生命周期中的
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }
        return instance;
    }

    public virtual void Awake()
    {
        if(instance==null) instance=this as T;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

}