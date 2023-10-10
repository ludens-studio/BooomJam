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
        PlayerPrefs.SetInt("firstPlay", 1);
        print(PlayerPrefs.GetInt("firstPlay"));
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
        Application.Quit();
    }

    /// <summary>
    /// 开始游戏,播放剧情故事
    /// </summary>
    public void StartGame()
    {
        storyBoard.gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().Play("Story");
    }

    /// <summary>
    /// 加载下一关
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
