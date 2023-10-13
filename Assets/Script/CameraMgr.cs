using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMgr : BaseMgr<CameraMgr>
{
    public GameObject guideWindow;
    public Vector3 worldPos1920;
    public Vector3 mapPos;
    public GameObject loadingPanel;

    private void Start()
    {
        transform.position = worldPos1920;
        transform.Find("Map").position = mapPos;
    }

    public void PauseGame()
    {
        guideWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void TransPanel()
    {
        StartCoroutine(Lerp(1339, 2.0f));
    }
    
    IEnumerator Lerp(float b, float time)
    {
        float startTime = Time.time;

        while (Time.time - startTime < time)
        {
            float t = (Time.time - startTime) / time;
            float value = Mathf.Lerp(0, b, t);

            loadingPanel.GetComponent<RectTransform>().offsetMin = new Vector2(loadingPanel.GetComponent<RectTransform>().offsetMin.x, value);
            loadingPanel.GetComponent<RectTransform>().offsetMax = new Vector2(loadingPanel.GetComponent<RectTransform>().offsetMax.x, -1 * value);

            yield return null; // 等待下一帧
        }
    }


}