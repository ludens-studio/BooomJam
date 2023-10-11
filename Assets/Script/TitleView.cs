using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleView : MonoBehaviour
{

    public GameObject storyBoard;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("firstPlay", 0) == 0)    // 第一次
        {
            return;
        }
        if (PlayerPrefs.GetInt("firstPlay", 0) == 1)    // 不是第一次
        {
            PlayerPrefs.SetInt("firstPlay", 1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        // 0：第一次游戏
        // 1：不是第一次
        PlayerPrefs.SetInt("firstPlay", 0);
        Application.Quit();
    }

    /// <summary>
    /// 开始游戏,播放剧情故事
    /// </summary>
    public void StartGame()
    {
        storyBoard.gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().Play("Story",-1,0.0f);
    }

    /// <summary>
    /// 加载下一关
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
