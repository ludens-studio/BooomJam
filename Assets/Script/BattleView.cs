using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// UI组件的更新
/// UI相关事件
/// 骰子相关事件(伪随机概率、投掷骰子、动画)
/// 商店
/// </summary>
public class BattleView : MonoBehaviour
{
    [Header("战斗主界面")]
    public Slider hp;

    public TMP_Text timer;

    public Button pause;

    public GameObject shopGirl;

    public TMP_Text shopGuide;  // 商店引导面板

    [Header("UI 面板")]
    public GameObject gameWindow;   // 游戏主面板
    
    public GameObject retryWindow;  // 重开面板

    public GameObject settingWindow;    // 设置面板

    public GameObject loadingWindow;    // 加载面板

    public GameObject logWindow;        // 商店对话面板
    
    public GameObject handbookWindow;   // 手册面板

    public GameObject guideWindow;      // 新手引导面板

    /// <summary>
    /// 场景中生成的骰子（唯一
    /// </summary>
    private GameObject _dice;

    /// <summary>
    /// 选中用于交易的塔们
    /// </summary>
    private List<Obj> _towers = new List<Obj>();

    /// <summary>
    /// 判断是否开始交易
    /// 开始则进行射线检测
    /// </summary>
    private bool _beginSelect = false;

    /// <summary>
    /// 被选中的骰子
    /// </summary>
    private GameObject _choosenDice;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("firstPlay",1) == 1)   // 第一次播放loading画面
        {
            PlayerPrefs.SetInt("firstPlay", 0);
            print(loadingWindow.transform.GetChild(0).name);
            loadingWindow.transform.GetChild(0).gameObject.SetActive(true);
            loadingWindow.GetComponent<Animator>().Play("EyeLoading",-1,0.0f);
            gameWindow.SetActive(false);
            BattleMgr.GetInstance().loadTime = 12.0f;
            Invoke(nameof(CameraTransition),10f);
        }
        else
        {
            loadingWindow.transform.GetChild(0).gameObject.SetActive(false);
            BattleMgr.GetInstance().loadTime = 0.1f;
        }
    }

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
        if (BattleMgr.GetInstance().GetBarTime() == 100)
        {
            gameWindow.SetActive(true);
        }

        // 血量为0，游戏结束
        if (BattleMgr.GetInstance().hp <= 0)
        {
/*            if (BattleMgr.GetInstance().hasReachKilled)
            {
                // todo: 特殊剧情(此处可以播动画，在动画末尾加上GameOver()事件即可)
                GameOver();
            }
            else
            {*/
                GameOver();
            //}
        }
        
        // 开始商店逻辑
        // 点击场上的两个塔
        // 点击需要净化的骰子，弹出确定
        // 净化可以直接用ChangeDiceState的第三种类型
        if (_beginSelect)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tower"))
                {
                    // 已选择的塔少于2个，鼠标点击时往list中加入塔
                    if (Input.GetMouseButtonDown(0) && _towers.Count <= 2)
                    {
                        // 不重复地添加塔
                        if (!_towers.Contains(hit.collider.gameObject.GetComponent<Obj>()))
                        {
                            hit.collider.transform.Find("Board").gameObject.SetActive(true);
                            _towers.Add(hit.collider.gameObject.GetComponent<Obj>());
                        }
                        else
                        {
                            hit.collider.transform.Find("Board").gameObject.SetActive(false);
                            _towers.Remove(hit.collider.gameObject.GetComponent<Obj>());
                        }
                    }
                    if(Input.GetMouseButtonDown(0) && _towers.Count > 2)
                    {
                        if (_towers.Contains(hit.collider.gameObject.GetComponent<Obj>()))
                        {
                            hit.collider.transform.Find("Board").gameObject.SetActive(false);
                            _towers.Remove(hit.collider.gameObject.GetComponent<Obj>());
                        }
                    }

                    if(_towers.Count == 2)
                    {
                        
                        shopGuide.text = "Choose the Dice";
                    }
                    
                }
                
            }
            
        }
    }

    /// <summary>
    /// 镜头转场，从底往上
    /// 隐藏伪加载面板
    /// </summary>
    public void CameraTransition()
    {
        Camera.main.GetComponent<Animator>().Play("CameraTransition");
    }

    /// <summary>
    /// 显示所有子节点
    /// 可用于显示骰子各个面的状态
    /// 更改鼠标图标（click）
    /// </summary>
    public void ShowChild(GameObject o)
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Cursors/PointerClick"), Vector2.zero, CursorMode.Auto);
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
    /// 恢复鼠标状态
    /// </summary>
    public void CloseChild(GameObject o)
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Cursors/PointerPut"), Vector2.zero, CursorMode.Auto);
        o.GetComponent<Animator>().Play("CloseState");
    }

    /// <summary>
    /// 点击骰子事件
    /// 用于商店交易
    /// </summary>
    /// <param name="o"></param>
    public void OnDiceClick(GameObject o)
    {
        // 在选择状态且已经选满了两个塔
        if (_beginSelect && _towers.Count == 2)
        {
            _choosenDice = o;
            logWindow.SetActive(true);
        }
    }

    /// <summary>
    /// 拖拽骰子事件
    /// 骰子跟随鼠标移动
    /// 鼠标变为Grab
    /// </summary>
    public void OnDiceDrag(GameObject o)
    {
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name) && Time.timeScale != 0)
        {
            Cursor.SetCursor(Resources.Load<Texture2D>("Cursors/PointerGrab"), Vector2.zero, CursorMode.Auto);
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
    /// 鼠标变为Put
    /// </summary>
    public void OnDiceRelease(GameObject o)
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Cursors/PointerPut"), Vector2.zero, CursorMode.Auto);
        Vector3 position = _dice.transform.position;
        int x = Mathf.RoundToInt(position.x);
        int y = -Mathf.RoundToInt(position.y);

        // if MapMgr return empty grid/ valid grid, then player can put the dice, or dice will be hide.
        if (!BattleMgr.GetInstance().IsDiceFreeze(o.name) && MapMgr.GetInstance().IsEmptyGrid(x,y) && Time.timeScale!=0)
        {
            // 骰子类型、六面状态、骰子三面统计
            int diceType = BattleMgr.GetInstance().GetDiceType(o.name);
            int[] stateList = BattleMgr.GetInstance().GetDiceState(o.name);
            Dictionary<int, List<int>> diceStateNum = CountStateList(stateList);
            // fake random face
            int rdFace = FakeProbability(diceStateNum);    //0~5
            print("raFace is: " + rdFace);
            // random tower
            int towerType = Random.Range(1, 6); //1~5
            string filePath = "";
            
            // 播放冷却动画
            // o.GetComponent<Animator>().Play("CountDown",-1,0.0f);
            o.GetComponent<Animation>().Play("CountDown");
            GameObject.FindWithTag("Dice").GetComponent<Animator>().Play((rdFace+1).ToString(), -1, 0f);

            switch (stateList[rdFace])
            {
                case 0: // 资源-》恶魔
                    print("投到资源面" + towerType.ToString());
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 1);
                    BattleMgr.GetInstance().InitTower(x, y, diceType, towerType.ToString());
                    filePath = diceType == 0? "Dices/DiceTowerDark":"Dices/DiceSoldierDark";
                    o.transform.GetChild(rdFace).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>(filePath);
                    break;
                case 1: // 恶魔-》资源
                    print("投到恶魔面" + towerType.ToString());
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 2);
                    BattleMgr.GetInstance().InitDarkTower(x, y, diceType, towerType.ToString());
                    filePath = diceType == 0? "Dices/DiceTower":"Dices/DiceSoldier";
                    o.transform.GetChild(rdFace).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>(filePath);
                    break;
                case 2: // 魔王-》全净化为资源
                    print("投到魔王面");
                    BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 3);
                    // init 魔王
                    BattleMgr.GetInstance().InitBoss();
                    for (int i = 0; i < 5; i++)
                    {
                        filePath = diceType == 0? "Dices/DiceTower":"Dices/DiceSoldier";
                        o.transform.GetChild(i).GetComponent<Image>().sprite =
                            Resources.Load<Sprite>(filePath);
                    }
                    break;
            }

            // 判断是否除魔王面外全为恶魔面
            if (diceStateNum[0].Count == 0)
            {
                // 除魔王面外全部为恶魔面，则所有面都转换为魔王面
                BattleMgr.GetInstance().ChangeDiceState(o.name, rdFace, 4);
                for (int j = 0; j < 5; j++)
                {
                    o.transform.GetChild(j).GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("Dices/DiceDevilDark");
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
    /// 计算当前骰子各类面的情况
    /// result[0]代表哪些面是资源面
    /// result[1]代表哪些面是恶魔面
    /// result[2] = 5 即唯一的恶魔面
    /// </summary>
    /// <param name="state">骰子各面状态列表</param>
    /// <returns></returns>
    public Dictionary<int,List<int>> CountStateList(int[] state)
    {
        Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
        result.Add(0,new List<int>(){});
        result.Add(1,new List<int>(){});
        result.Add(2,new List<int>(){5});
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == 0)  //资源面数量
            {
                result[0].Add(i);
            }
            if (state[i] == 1)  //恶魔面数量
            {
                result[1].Add(i);
            }
        }
        return result;
    }

    /// <summary>
    /// 对骰子各面分类讨论的伪概率
    /// </summary>
    /// <param name="input">骰子状态字典</param>
    /// <returns></returns>
    public int FakeProbability(Dictionary<int, List<int>> input)
    {
        float random = 0;
        if (input[0].Count == 5)    // 5-0-1 / 95-0-5
        {
            random = Random.Range(0, 100);
            if (random <= 95)
                return input[0][Random.Range(0, 5)];
            else
                return 5;
        }
        if (input[0].Count == 4)    // 4-1-1 / 85-7.5-7.5
        {
            random = Random.Range(0, 100);
            if (random <= 85)
                return input[0][Random.Range(0, 4)];
            else if (random > 85 && random <= 92.5)
                return input[1][0];
            else
                return 5;
        }
        if (input[0].Count == 3)    // 3-2-1 / 70-20-10
        {
            random = Random.Range(0, 100);
            if (random <= 70)
                return input[0][Random.Range(0, 3)];
            else if (random > 70 && random <= 90)
                return input[1][Random.Range(0, 2)];
            else
                return 5;
        }
        if (input[0].Count == 2)    // 2-3-1 / 40-47.5-12.5
        {
            random = Random.Range(0, 100);
            if (random <= 40)
                return input[0][Random.Range(0, 2)];
            else if (random > 40 && random <= 87.5)
                return input[1][Random.Range(0, 3)];
            else
                return 5;
        }
        if (input[0].Count == 1)    // 1-4-1 / 15-70-15
        {
            random = Random.Range(0, 100);
            if (random <= 15)
                return input[0][0];
            else if (random > 15 && random <= 85)
                return input[1][Random.Range(0, 4)];
            else
                return 5;
        }
        if (input[0].Count == 0)    // 0-5-1 / 0-0-100
        {
            return 5;
        }

        print("出错了");
        return 0;
    }

    /// <summary>
    /// 游戏结束，打开面板
    /// 更新最高记录(如果打破了)
    /// </summary>
    public void GameOver()
    {
        retryWindow.SetActive(true);
        int record = PlayerPrefs.GetInt("Record",0);
        int mark = BattleMgr.GetInstance().timer;
        if (mark > record)
        {
            retryWindow.transform.Find("Record").GetComponent<TMP_Text>().text =
                "Best Record: " + mark;
            PlayerPrefs.SetInt("Record", mark);
        }
        else
        {
            retryWindow.transform.Find("Record").GetComponent<TMP_Text>().text =
                "Best Record: " + record;
        }
    }
    
    /// <summary>
    /// 打开商店交易
    /// 暂停游戏
    /// 净化骰子
    /// </summary>
    public void OpenShop()
    {
        // 至少有两个塔才允许进行交易
        if (BattleMgr.GetInstance().towers.Count >= 2)
        {
            Time.timeScale = 0;
            // 引导
            shopGuide.text = "Choose Two Towers or Soldiers in the map";
            _beginSelect = true;
            shopGirl.GetComponent<Animator>().Play("BeginShop",-1,0.0f);
        }
        else
        {
            // 播放无法交易的动画
            shopGirl.GetComponent<Animator>().Play("CantShop",-1,0.0f);
        }
    }

    /// <summary>
    /// 暂停游戏
    /// 替换按钮贴图
    /// </summary>
    public void PauseGame(GameObject o)
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            o.GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>("UIElements/StartUI");
        }
        else
        {
            Time.timeScale = 1;
            o.GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>("UIElements/StopUI");
        }
    }

    /// <summary>
    /// 重试游戏
    /// </summary>
    public void RetryGame()
    {
        PlayerPrefs.SetInt("firstPlay", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    /// <summary>
    /// 回到标题
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }

    /// <summary>
    /// 返回游戏
    /// </summary>
    public void BackToGame()
    {
        settingWindow.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// 打开设置面板
    /// </summary>
    public void ShowSetting()
    {
        Time.timeScale = 0;
        settingWindow.SetActive(true);
    }

    /// <summary>
    /// 关闭商店对话
    /// </summary>
    public void CloseLog()
    {
        logWindow.SetActive(false);
    }

    /// <summary>
    /// 打开图鉴
    /// </summary>
    public void ShowHandbook()
    {
        handbookWindow.SetActive(true);
    }

    /// <summary>
    /// 关闭图鉴
    /// </summary>
    public void CloseHandbook()
    {
        handbookWindow.SetActive(false);
    }

    /// <summary>
    /// 关闭新手引导
    /// </summary>
    public void CloseGuide()
    {
        guideWindow.SetActive(false);
        Time.timeScale = 1;
    }
    
    

    /// <summary>
    /// 完成商店逻辑
    /// </summary>
    public void ShopComplete()
    {
        logWindow.SetActive(false);
        BattleMgr.GetInstance().ChangeDiceState(_choosenDice.name,0,3);
        string filePath = BattleMgr.GetInstance().GetDiceType(_choosenDice.name) == 0? "Dices/DiceTower":"Dices/DiceSoldier";
        // 替换状态贴图
        for (int i = 0; i < 5; i++)
            _choosenDice.transform.GetChild(i).GetComponent<Image>().sprite =
                Resources.Load<Sprite>(filePath);
        // 回收选中的塔
        // 关闭选中的描边效果
        foreach (var tower in _towers)
        {
            tower.transform.Find("Board").gameObject.SetActive(false);
            tower.state = Obj.ObjState.Death;
        }

        _towers.Clear();
        _beginSelect = false;
        shopGuide.text = "";
        pause.GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>("UIElements/StopUI");
        Time.timeScale = 1;
    }

}
