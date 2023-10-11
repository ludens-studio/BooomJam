using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMgr : BaseMgr<CameraMgr>
{
    public GameObject guideWindow;
    public Vector3 worldPos1920;

    private void Start()
    {
        transform.position = worldPos1920;
    }

    public void PauseGame()
    {
        guideWindow.SetActive(true);
        Time.timeScale = 0;
    }

}
