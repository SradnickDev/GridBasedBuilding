using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = (" Grid / new"))]
public class Grid : ScriptableObject {


    public int nodeSize = 10;
    public Vector3 gridSize;

    [SerializeField]Dictionary<Vector3,GridNode> grid = new Dictionary<Vector3,GridNode>();
    public Dictionary<Vector3,GridNode> GridBase { get { return grid; } set { grid = value; } }
}
