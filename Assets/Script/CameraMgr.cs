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
    public Vector3 loadPos;

    private void Start()
    {
        transform.position = worldPos1920;
        transform.Find("Map").position = mapPos;
        transform.Find("PanelLoading").position = loadPos;
    }

    public void PauseGame()
    {
        guideWindow.SetActive(true);
        Time.timeScale = 0;
    }

}
