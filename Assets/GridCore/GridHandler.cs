using System.Collections.Generic;
using UnityEngine;


public class GridHandler : MonoBehaviour
{

    
    public Grid grid;
    [SerializeField] NodeValidator nodeValidator;
    
    bool m_usableCheck;
    private List<Vector3> usableNode = new List<Vector3>();

    [Header("Visuals")]
    [SerializeField] bool visualizeGrid = true;
    [SerializeField] bool visualizeUsableGrid = true;
    [SerializeField] Color gridColor;
    //    [SerializeField] Color usableNodeColor = new Color(1f, 0f, 0.04f, 0.5f);
    GridNode gNode;
    private void Start()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        

        for (int x = 0; x < grid.gridSize.x * grid.nodeSize; x += grid.nodeSize)
        {
            for (int z = 0; z < grid.gridSize.z * grid.nodeSize; z += grid.nodeSize)
            {
                for (int y = 0; y < grid.gridSize.y * grid.nodeSize; y += grid.nodeSize)
                {
                                  
                    var point = grid.GetNodeCenter(new Vector3(x, y, z) + transform.position);
                    m_usableCheck = nodeValidator.CheckNode(point);
                    gNode = new GridNode(point, m_usableCheck);
                    if(m_usableCheck) usableNode.Add(point);
                    grid.GridBase.Add(point, gNode);
                }
            }

        }
        sw.Stop();
        Debug.Log("Scanning done in " + sw.ElapsedMilliseconds + "ms");
        sw.Reset();
        sw.Start();
        
        
        for (int x = 0; x < grid.gridSize.x * grid.nodeSize; x += grid.nodeSize)
        {
            for (int z = 0; z < grid.gridSize.z * grid.nodeSize; z += grid.nodeSize)
            {
                for (int y = 0; y < grid.gridSize.y * grid.nodeSize; y += grid.nodeSize)
                {
                    var m_thisPosition = transform.position;

                    var point = grid.GetNodeCenter(new Vector3(x, y, z) + m_thisPosition);
                    GridNode gridNode = grid.GetNode(point);

                    var forward = grid.GetNodeCenter(new Vector3(x, y, z + grid.nodeSize) + m_thisPosition);
                    GridNode nodeForward = grid.GetNode(forward);
                    gridNode.AddStaticGridNeighbour(Neighbour.Forward, nodeForward);

                    var backward = grid.GetNodeCenter(new Vector3(x, y, z - grid.nodeSize) + m_thisPosition);
                    GridNode nodeBackward = grid.GetNode(backward);
                    gridNode.AddStaticGridNeighbour(Neighbour.Backward, nodeBackward);
                    

                    var right = grid.GetNodeCenter(new Vector3(x + grid.nodeSize, y, z) + m_thisPosition);
                    GridNode nodeRight = grid.GetNode(right);
                    gridNode.AddStaticGridNeighbour(Neighbour.Right, nodeRight);

                    var left = grid.GetNodeCenter(new Vector3(x - grid.nodeSize, y, z) + m_thisPosition);
                    GridNode nodeLeft = grid.GetNode(left);
                    gridNode.AddStaticGridNeighbour(Neighbour.Left, nodeLeft);


                    var up = grid.GetNodeCenter(new Vector3(x, y + grid.nodeSize, z) + m_thisPosition);
                    GridNode nodeUp = grid.GetNode(up);
                    gridNode.AddStaticGridNeighbour(Neighbour.Up, nodeUp);


                    var down = grid.GetNodeCenter(new Vector3(x, y - grid.nodeSize, z) + m_thisPosition);
                    GridNode nodeDown = grid.GetNode(down);
                    gridNode.AddStaticGridNeighbour(Neighbour.Down, nodeDown);

                    var forwardUp = grid.GetNodeCenter(new Vector3(x, y + grid.nodeSize, z + grid.nodeSize) + m_thisPosition);
                    GridNode nodeForwardUp = grid.GetNode(forwardUp);
                    gridNode.AddStaticGridNeighbour(Neighbour.ForwardUp, nodeForwardUp);

                    var forwardDown = grid.GetNodeCenter(new Vector3(x, y - grid.nodeSize, z + grid.nodeSize) + m_thisPosition);
                    GridNode nodeForwardDown = grid.GetNode(forwardDown);
                    gridNode.AddStaticGridNeighbour(Neighbour.ForwardDown, nodeForwardDown);

                    var backwardDown = grid.GetNodeCenter(new Vector3(x, y - grid.nodeSize, z - grid.nodeSize) + m_thisPosition);
                    GridNode nodeBackwardDown = grid.GetNode(backwardDown);
                    gridNode.AddStaticGridNeighbour(Neighbour.BackwardDown, nodeBackwardDown);
                    
                    var backwardUp = grid.GetNodeCenter(new Vector3(x, y + grid.nodeSize, z - grid.nodeSize) + m_thisPosition);
                    GridNode nodeBackwardUp = grid.GetNode(backwardUp);
                    gridNode.AddStaticGridNeighbour(Neighbour.BackwardUp, nodeBackwardUp);

                    var leftDown = grid.GetNodeCenter(new Vector3(x - grid.nodeSize, y - grid.nodeSize, z)+ m_thisPosition);
                    GridNode nodeLeftDown = grid.GetNode(leftDown);
                    gridNode.AddStaticGridNeighbour(Neighbour.LeftDown, nodeLeftDown);
                    
                    var leftUp = grid.GetNodeCenter(new Vector3(x - grid.nodeSize, y + grid.nodeSize, z)+ m_thisPosition);
                    GridNode nodeLeftUp = grid.GetNode(leftUp);
                    gridNode.AddStaticGridNeighbour(Neighbour.LeftUp, nodeLeftUp);
                    

                    var rightDown = grid.GetNodeCenter(new Vector3(x + grid.nodeSize, y - grid.nodeSize, z)+ m_thisPosition);
                    GridNode nodeRightDown = grid.GetNode(rightDown);
                    gridNode.AddStaticGridNeighbour(Neighbour.RightDown, nodeRightDown);
                    
                    var rightUp = grid.GetNodeCenter(new Vector3(x + grid.nodeSize, y + grid.nodeSize, z)+ m_thisPosition);
                    GridNode nodeRightUp = grid.GetNode(rightUp);
                    gridNode.AddStaticGridNeighbour(Neighbour.RightUp, nodeRightUp);
                }
            }

        }
        sw.Stop();
        Debug.Log("Scanning done in " + sw.ElapsedMilliseconds + "ms");

    }

    public Vector3 GetGridCenter()
    {
        return transform.position + new Vector3(grid.gridSize.x* grid.nodeSize - grid.nodeSize,grid.gridSize.y* grid.nodeSize -grid.nodeSize,grid.gridSize.z * grid.nodeSize - grid.nodeSize) / 2;
    }

    private Vector3 SnapPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(transform.position.x / grid.nodeSize);
        int y = Mathf.RoundToInt(transform.position.y / grid.nodeSize);
        int z = Mathf.RoundToInt(transform.position.z / grid.nodeSize);
        
        Vector3 snappedPoint = new Vector3(x * grid.nodeSize, y * grid.nodeSize, z * grid.nodeSize);
        return snappedPoint;
    }
    
    private void OnDrawGizmos()
    {   
        transform.position = SnapPosition(transform.position);
        if (visualizeGrid)
        {
            Gizmos.color = gridColor;
            for (float x = 0; x < grid.gridSize.x * grid.nodeSize; x += grid.nodeSize)
            {
                for (float z = 0; z < grid.gridSize.z * grid.nodeSize; z += grid.nodeSize)
                {
                    for (float y = 0; y < grid.gridSize.y * grid.nodeSize; y += grid.nodeSize)
                    {
                        var point = transform.position + grid.GetNodeCenter(new Vector3(x, y, z));

                        Gizmos.DrawWireCube(point, new Vector3(grid.nodeSize, grid.nodeSize, grid.nodeSize));
                    }
                }

            }
        }

        if (visualizeUsableGrid)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < usableNode.Count; i++)
            {
                Gizmos.DrawWireCube(usableNode[i], new Vector3(grid.nodeSize, grid.nodeSize, grid.nodeSize));
            }
        }
    }
}
