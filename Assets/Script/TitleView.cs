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
        // 第一次打开游戏
        if (PlayerPrefs.GetInt("firstPlay", 1) == 0)
        {
            return;
        }
        else
        {
            print("hello world");
            PlayerPrefs.SetInt("firstPlay", 0);
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
        PlayerPrefs.SetInt("firstPlay", 1);
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
