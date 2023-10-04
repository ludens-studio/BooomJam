using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// UI组件的更新
/// 骰子相关事件
/// 商店
/// </summary>
public class BattleView : MonoBehaviour
{
    public Slider hp;

    public TMP_Text timer;

    public Button setting;

    public Button shop;

    public GameObject[] dices;

    // 骰子的冷却时间
    public int diceTimer;

    public GameObject retryWindow;  // 重开面板

    /// <summary>
    /// 场景中生成的骰子（唯一
    /// </summary>
    private GameObject _dice;
    
    // Start is called before the first frame update
    void Start()
    {
        hp.maxValue = BattleMgr.GetInstance().hp;
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = BattleMgr.GetInstance().hp;
        // show time in tmp
        int time = BattleMgr.GetInstance().timer;
        string min = (time / 60 < 10) ? "0" + (time / 60): (time / 60).ToString();
        string sec = (time % 60 < 10) ? "0" + (time % 60) : (time % 60).ToString();

        timer.text = min + ":" + sec;

        if (BattleMgr.GetInstance().hp == 0)
        {
            retryWindow.SetActive(true);
        }
    }

    /// <summary>
    /// 显示所有子节点
    /// 可用于显示骰子各个面的状态
    /// </summary>
    public void ShowChild(GameObject o)
    {
        Animator animator = o.GetComponent<Animator>();
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.normalizedTime >= 1.0f)
        {
            animator.Play("ShowState");
        }
        
        // 骰子非冷却，且场上没有已生成的骰子 -> 生成骰子实例
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name) && !GameObject.FindWithTag("Dice"))
        {
            //在鼠标处生成一个骰子，阴影指向落点
            _dice = ResMgr.GetInstance().Load<GameObject>("Prefabs/Dice");
            // todo: 这个阴影我看直接用平行光投影吧
        }

    }
    
    /// <summary>
    /// 隐藏所有子节点
    /// </summary>
    public void CloseChild(GameObject o)
    {
        o.GetComponent<Animator>().Play("CloseState");
    }

    /// <summary>
    /// 拖拽骰子事件
    /// 骰子跟随鼠标移动
    /// </summary>
    public void OnDiceDrag(GameObject o)
    {
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name))
        {
            _dice = GameObject.FindWithTag("Dice");
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            var objectPosition = Camera.main.ScreenToWorldPoint(mousePos);
            _dice.transform.position = objectPosition;
            // todo: 格子高亮或者阴影落点
        }
    }

    /// <summary>
    /// 投掷骰子事件
    /// 落点矫正（四舍五入）
    /// </summary>
    public void OnDiceRelease(GameObject o)
    {
        Vector3 position = _dice.transform.position;
        int x = Mathf.RoundToInt(position.x);
        int y = -Mathf.RoundToInt(position.y);

        // if MapMgr return empty grid/ valid grid, then player can put the dice, or dice will be hide.
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name) && MapMgr.GetInstance().IsEmptyGrid(x,y))
        {
            // 播放冷却动画
            o.GetComponent<Animator>().Play("CountDown");
            // random tower
            int towerType = Random.Range(1, 5); //1~4

            // random face
            int rdFace = Random.Range(0, 6);
            int[] stateList = BattleMgr.GetInstance().GetDiceState(o.name);
            
            switch (stateList[rdFace])
            {
                case 0: // 资源-》恶魔
                    print("投到资源面" + towerType.ToString());
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 1);
                    BattleMgr.GetInstance().InitTower(x, y, towerType.ToString());
                    o.transform.GetChild(rdFace).GetComponent<Image>().color = Color.yellow;
                    break;
                case 1: // 恶魔-》资源
                    print("投到恶魔面" + towerType.ToString());
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 2);
                    BattleMgr.GetInstance().InitDarkTower(x, y, towerType.ToString());
                    // init 精英怪
                    o.transform.GetChild(rdFace).GetComponent<Image>().color = Color.green;
                    break;
                case 2: // 魔王-》全净化为资源
                    print("投到魔王面");
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 3);
                    // init 魔王
                    for (int i = 0; i < 5; i++)
                    {
                        o.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                    }
                    break;
            }

            // 判断是否除魔王面外全为恶魔面
            for (int i = 0; i < 5; i++)
            {
                if (stateList[i] == 0)
                {
                    return;
                }
                if (i == 4 && stateList[i] == 1)
                {
                    // 除魔王面外全部为恶魔面，则所有面都转换为魔王面
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 4);
                    for (int j = 0; j < 5; j++)
                    {
                        o.transform.GetChild(j).GetComponent<Image>().color = Color.red;
                    }
                }
            }

        }
        else
        {
            // 隐藏骰子（只是放到了远方
            GameObject.FindWithTag("Dice").transform.position = new Vector3(100, 100, 0);
        }
        
    }
    
    /// <summary>
    /// 打开商店时暂停游戏
    /// </summary>
    public void PauseGame()
    {
        // 至少有三个塔才允许进行交易
        if (BattleMgr.GetInstance().towers.Count >= 3)
        {
            Time.timeScale = 0;
            // 播放交易询问的动画，上面有对话框和按钮，按钮有别的事件
        }
        else
        {
            // 播放一个无法交易的动画
        }

    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 重试游戏
    /// </summary>
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    /// <summary>
    /// 回到标题
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }
    
    // 随便测什么都可以
    public void TestFunc(GameObject o)
    {
        BattleMgr.GetInstance().timer = 100;
    }

}
