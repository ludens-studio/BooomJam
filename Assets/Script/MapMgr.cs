using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 地图/网格管理器
/// 5 * 10
/// </summary>
public class MapMgr : BaseMgr<MapMgr>
{
    /// <summary>
    /// 格子数组
    /// 需要注意的是y轴的数值是取绝对值
    /// 因为在unity里是负的
    /// </summary>
    private Coordinate[,] _gridList = new Coordinate[10, 5];
    
    [System.Serializable]
    public struct Coordinate
    {
        public Tower tower;
    }

    /// <summary>
    /// 地图数据结构放置塔
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// </summary>
    public void SetTower(int x, int y, Tower tower)
    {
        _gridList[x, y].tower = tower;

        // 在ballteMgr中注册塔
        tower.AddToBattleMgr();
    }

    /// <summary>
    /// 地图数据结构移除塔
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// </summary>
    public void RemoveTower(int x, int y)
    {
        _gridList[x, y].tower = null;

        // 在ballteMgr中删除塔塔
        _gridList[x, y].tower.DelFromBattleMgr();

    }
    
    /// <summary>
    /// 是否为空格子
    /// </summary>
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// <returns></returns>
    public bool IsEmptyGrid(int x, int y)
    {
        if (!IsValidGrid(x, y)) // 非法
            return false;
        

        if (_gridList[x, y].tower == null)// 空的
            return true;
        else
            return false;
        
            
    }

    /// <summary>
    /// 是否在地图内部
    /// </summary>
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// <returns></returns>
    public bool IsValidGrid(int x, int y)
    {
        if (x > 4 || y > 9 || x < 0 || y < 0)
            return false;
        else
            return true;
    }
}
