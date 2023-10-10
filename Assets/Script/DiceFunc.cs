using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFunc : MonoBehaviour
{

    /// <summary>
    /// 动画完成之后让骰子消失
    /// </summary>
    public void WayBack()
    {
        gameObject.transform.position = new Vector3(100, 100, -1);
    }
}
