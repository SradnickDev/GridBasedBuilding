using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GridHelper
{

    public static Direction GetDirection(this Transform transform)
    {
        var YRotation = (int)transform.eulerAngles.y;
        YRotation = Mathf.RoundToInt(YRotation / 90) * 90;

        switch (YRotation)
        {
            case 0:
                return Direction.Forward;
            case 90:
                return Direction.Right;
            case 180:
                return Direction.Backward;
            case 270:
                return Direction.Left;
            case 360:
                return Direction.Forward;
        }
        return Direction.None;
    }
    public static Direction GetDirection(this Vector3 rotation)
    {
        int rot = (int)rotation.y;

        switch (rot)
        {
            case 0:
                return Direction.Forward;
            case 90:
                return Direction.Right;
            case 180:
                return Direction.Backward;
            case 270:
                return Direction.Left;
            case 360:
                return Direction.Forward;
        }
        return Direction.None;
    }
    public static Direction GetOppositDirection(this Transform transform)
    {
        var dir = transform.GetDirection();
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;

            case Direction.Left://
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;

            case Direction.Forward:
                return Direction.Backward;
            case Direction.Backward:
                return Direction.Forward;
        }
        return Direction.None;
    }
    public static void AddGridNeighbourWithPosition(GridNode node, Neighbour direction, Vector3 pos, Grid grid)
    {
        GridNode neighbor = grid.GetNode(pos);
        if (neighbor != null)
        {
            node.AddStaticGridNeighbour(direction, node);
        }

    }
    public static GridNode GetNode(this Grid grid, Vector3 position)
    {
        Vector3 nCenter = grid.GetNodeCenter(position);
        GridNode node;

        if (grid.GridBase.ContainsKey(nCenter))
        {
            node = grid.GridBase[nCenter];
            return node;
        }
        return null;
    }
    public static Vector3 GetNodeCenter(this Grid grid, Vector3 targetPositon)
    {
        int x = Mathf.RoundToInt(targetPositon.x / grid.nodeSize);
        int y = Mathf.RoundToInt(targetPositon.y / grid.nodeSize);
        int z = Mathf.RoundToInt(targetPositon.z / grid.nodeSize);

        Vector3 nodeCenter = new Vector3(x * grid.nodeSize, y * grid.nodeSize, z * grid.nodeSize);

        return nodeCenter;
    }

    public static void GetNighbourNode(this GridNode node, Neighbour TKey, out GridNode TValue)
    {
        TValue = null;
        if (node == null) return;
        Dictionary<Neighbour, GridNode> dic = node.staticNeighbour;
        dic.TryGetValue(TKey, out TValue);
    }
    public static bool InUse(this GridNode gridNode, BlockType blockType, Direction direction)
    {
        GridNode m_cachedNode = gridNode;

        if (m_cachedNode.useCase[blockType].ContainsKey(direction))
        {
            if (m_cachedNode.useCase[blockType][direction] != null)
            {
                return true;
            }
        }
        return false;
    }
    public static GameObject InUseGetGameObject(this GridNode gridNode, BlockType blockType, Direction direction)
    {
        GridNode m_cachedNode = gridNode;

        Dictionary<Direction, GameObject> m_t = new Dictionary<Direction, GameObject>();

        if (m_cachedNode.useCase.TryGetValue(blockType, out m_t))
        {
                return m_t[direction];
        }
        return null;
    }
}