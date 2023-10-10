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
        public GameObject tower;
        public bool lockGrid;
    }

    /// <summary>
    /// 地图数据结构放置塔
    /// BattleMgr放置塔
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// </summary>
    public void SetTower(int x, int y, GameObject tower)
    {
        _gridList[x, y].tower = tower;
    }

    /// <summary>
    /// 地图数据结构移除塔
    /// BattleMgr移除塔
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// </summary>
    public void RemoveTower(int x, int y)
    {
        _gridList[x, y].tower = null;
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

        return _gridList[x, y].tower == null && !_gridList[x,y].lockGrid; // 空的且未被锁住格子
    }

    /// <summary>
    /// 是否在地图内部
    /// </summary>
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// <returns></returns>
    public bool IsValidGrid(int x, int y)
    {
        if (x > 9 || y > 4 || x < 0 || y < 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 暗塔放置
    /// 锁住左右两边的格子
    /// <param name="x">x轴坐标</param>
    /// <param name="y">y轴坐标,记得取绝对值</param>
    /// </summary>
    public void LockGrid(int x, int y)
    {
        if(IsValidGrid(x - 1, y) && IsEmptyGrid(x - 1, y))  // 合法格子而且是空的
            _gridList[x - 1, y].lockGrid = true;
        if(IsValidGrid(x + 1, y) && IsEmptyGrid(x + 1, y))
            _gridList[x + 1, y].lockGrid = true;
    }

    /// <summary>
    /// 暗塔死亡
    /// 释放被锁住的格子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ReleaseGrid(int x, int y)
    {
        if(IsValidGrid(x - 1,y))
            _gridList[x - 1, y].lockGrid = false;
        if(IsValidGrid(x + 1,y))
            _gridList[x + 1, y].lockGrid = false;
    }
}
