using UnityEngine;
using System.Collections.Generic;


public enum Neighbour { Forward, Backward, Left, Right, Up, Down, ForwardUp, ForwardDown, BackwardDown, BackwardUp, LeftDown, LeftUp, RightDown, RightUp }
public enum Direction { None, Up, Down, Left, Right, Forward, Backward }
public enum BlockType { Ramp, Floor, Wall }

[System.Serializable]
public class GridNode
{
    public Vector3 center;
    public GameObject block;
    public bool ignorConditions = false;
    public BlockHealth blockHealth;

    public GridNode(Vector3 center, bool blocked)
    {
        this.center = center;
        this.isUsable = blocked;
    }


    public Dictionary<BlockType, Dictionary<Direction, GameObject>> useCase =
        new Dictionary<BlockType, Dictionary<Direction, GameObject>>()
        {
            {BlockType.Ramp, new Dictionary<Direction, GameObject>()
            {

            {Direction.Forward, null},
            {Direction.Left, null},
            {Direction.Right, null},
            {Direction.Backward, null}

            } },
            {BlockType.Floor, new Dictionary<Direction, GameObject>()
            {
            {Direction.Down, null}

            } },
            {BlockType.Wall, new Dictionary<Direction, GameObject>()
            {

            {Direction.Forward, null},
            {Direction.Left, null},
            {Direction.Right, null},
            {Direction.Backward, null}

            } }
        };

    public Dictionary<Neighbour, GridNode> staticNeighbour = new Dictionary<Neighbour, GridNode>();

    

    public Dictionary<BlockType, Stack<GameObject>> requiredNeighbours = new Dictionary<BlockType, Stack<GameObject>>()
    {
        {BlockType.Floor, new Stack<GameObject>() },
        {BlockType.Ramp,new Stack<GameObject>() },
        {BlockType.Wall, new Stack<GameObject>() }
    };

    public struct NodeBool
    {
        public bool isAvailable;
        public GameObject neighbour;

        public NodeBool(bool value, GameObject gbj)
        {
            isAvailable = value;
            neighbour = gbj;
        }
    }


    public bool InUse(BlockType blockType, Direction direction)
    {
        bool inUse = false;
        if (useCase[blockType].ContainsKey(direction))
        {
            if (useCase[blockType][direction] != null)
            {
                return true;
            }
        }


        return inUse;
    }


    public bool isUsable;

    public void AddRequiredNighbour(BlockType type, GameObject gameObject)
    {
        if (requiredNeighbours[type].Contains(gameObject)) return;
        requiredNeighbours[type].Push(gameObject);
    }
    public void AddNodeUseCase(BlockType blockType, Direction dir, GameObject block)
    {
        if (useCase.ContainsKey(blockType))
        {
            if (useCase[blockType][dir] == null)
            {
                //Debug.LogWarning("Node is not in use, add " + block.name + ". Dir :" + dir);
                Debug.Log("cant live without "+requiredNeighbours[BlockType.Floor].Count + " Floors");
                Debug.Log("cant live without "+requiredNeighbours[BlockType.Ramp].Count + " Ramps");
                Debug.Log("rcant live without "+requiredNeighbours[BlockType.Wall].Count+ " Walls");

                useCase[blockType][dir] = block;
            }
            else
            {
                Debug.LogWarning("Node is in use.");
            }
        }
        else
        {
            Debug.Log("Add Direction " + dir + " to default Settings.");
        }
    }

    public void AddStaticGridNeighbour(Neighbour direction, GridNode newNeighbour)
    {
        if (staticNeighbour.ContainsKey(direction))
        {
            staticNeighbour[direction] = newNeighbour;
        }
        else
        {
            staticNeighbour.Add(direction, newNeighbour);
        }
    }
    /// <summary>Chechk if on a Nighbour has the required block</summary>
    /// <param name="grid">Grid in use.</param>
    /// <param name="transform">Gizmo </param>
    /// <param name="neighbour">Wich Nighbour from current Node.</param>
    /// <param name="currentType">Wich BlockType is used.</param>
    /// <param name="nighbourType">Target BlockType</param>
    /// <param name="direction">In Wich Direction the Target Block is Facing.</param>
    /// <param name="nodebool">To pass the same reference through diffrent conditions</param>
    private void DynamicPermissionCheck(Grid grid, Transform transform, Neighbour neighbour, BlockType currentType, BlockType nighbourType, Direction direction, ref NodeBool nodebool)
    {
        var m_dynamicNeighbour = DynamicNeighbour(transform, grid, neighbour);

        if (m_dynamicNeighbour != null)
        {
            bool m_inUse = m_dynamicNeighbour.InUse(nighbourType, direction);

            if (m_inUse)
            {
                var m_requiredNeighbour = m_dynamicNeighbour.useCase[nighbourType][direction];
                //!!m_dynamicNeighbour.AddRelayOn(nighbourType,);
                AddRequiredNighbour(currentType, m_requiredNeighbour);
                nodebool = new NodeBool(true, m_requiredNeighbour);
            }
        }
    }
    /// <summary>Chechk if on a Nighbour has the required block</summary>
    /// <param name="grid">Grid in use.</param>
    /// <param name="transform">Gizmo </param>
    /// <param name="neighbour">Wich Nighbour from current Node.</param>
    /// <param name="currentType">Wich BlockType is used.</param>
    /// <param name="nighbourType">Target BlockType</param>
    /// <param name="direction">In Wich Direction the Target Block is Facing.</param>
    /// <param name="nodebool">To pass the same reference through diffrent conditions</param>
    private void StaticPermissionCheck(Grid grid, Transform transform, Neighbour neighbour, BlockType currentType, BlockType nighbourType, Direction direction, ref NodeBool nodebool)
    {
        var m_staticNeighbour = staticNeighbour[neighbour];

        if (m_staticNeighbour != null)
        {
            bool m_inUse = m_staticNeighbour.InUse(nighbourType, direction);

            if (m_inUse)
            {
                var m_requiredNeighbour = m_staticNeighbour.useCase[nighbourType][direction];
                AddRequiredNighbour(currentType, m_requiredNeighbour);
                nodebool = new NodeBool(true, m_requiredNeighbour);

            }
        }
    }
    private void LocalPermissionCheck(BlockType currentType, BlockType nighbourType, Direction direction, ref NodeBool nodebool)
    {
        var m_localNeighbour = useCase[nighbourType][direction];

        if (m_localNeighbour != null)
        {
            AddRequiredNighbour(currentType, m_localNeighbour);
            nodebool = new NodeBool(true, m_localNeighbour);
        }
    }


    public NodeBool HasNeighbour(Transform transform, Grid grid, BlockType blocktype)
    {
        var m_sameDirection = transform.GetDirection();

        var m_oppositDirection = transform.GetOppositDirection();
        NodeBool m_validation = new NodeBool(false, null);

        switch (blocktype)
        {
            case BlockType.Ramp:
                m_validation = new NodeBool(false, null);

                //check for nighbour Ramps
                //                     local grid, gizmo transform, grid neighbour,current BlockType, Neighbour BlockType, Neighbour Direction, ...
                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Ramp, m_oppositDirection, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.ForwardUp, BlockType.Ramp, BlockType.Ramp, m_sameDirection, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Ramp, BlockType.Ramp, m_sameDirection, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Ramp, BlockType.Ramp, m_sameDirection, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.BackwardDown, BlockType.Ramp, BlockType.Ramp, m_sameDirection, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Ramp, m_oppositDirection, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Up, BlockType.Ramp, BlockType.Ramp, m_oppositDirection, ref m_validation);


                //check for Local walls
                LocalPermissionCheck(BlockType.Ramp, BlockType.Wall, Direction.Right, ref m_validation);
                LocalPermissionCheck(BlockType.Ramp, BlockType.Wall, Direction.Left, ref m_validation);
                LocalPermissionCheck(BlockType.Ramp, BlockType.Wall, Direction.Forward, ref m_validation);
                LocalPermissionCheck(BlockType.Ramp, BlockType.Wall, Direction.Backward, ref m_validation);


                //Check for Nighbour Walls
                DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Ramp, BlockType.Wall, Direction.Left, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Ramp, BlockType.Wall, Direction.Right, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Ramp, BlockType.Wall, Direction.Forward, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Ramp, BlockType.Wall, Direction.Backward, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Ramp, BlockType.Wall, Direction.Left, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Ramp, BlockType.Wall, Direction.Right, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Ramp, BlockType.Wall, Direction.Forward, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Ramp, BlockType.Wall, Direction.Backward, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Wall, Direction.Left, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Wall, Direction.Right, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Wall, Direction.Forward, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Wall, Direction.Backward, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Wall, Direction.Left, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Wall, Direction.Right, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Wall, Direction.Forward, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Wall, Direction.Backward, ref m_validation);


                //Check for local Floors
                LocalPermissionCheck(BlockType.Ramp, BlockType.Floor, Direction.Down, ref m_validation);
                //LocalPermissionCheck(BlockType.Ramp, BlockType.Floor, Direction.Up, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.ForwardUp, BlockType.Ramp, BlockType.Floor, Direction.Down, ref m_validation);
                //DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Ramp, BlockType.Floor, Direction.Up, ref m_validation);

                //DynamicPermissionCheck(grid, transform, Neighbour.BackwardDown, BlockType.Ramp, BlockType.Floor, Direction.Up, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Ramp, BlockType.Floor, Direction.Down, ref m_validation);

                return m_validation;

            case BlockType.Floor:
                m_validation = new NodeBool(false, null);




                //check for nighbour floors => down
                StaticPermissionCheck(grid, transform, Neighbour.Left, BlockType.Floor, BlockType.Floor, Direction.Down, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Right, BlockType.Floor, BlockType.Floor, Direction.Down, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Floor, BlockType.Floor, Direction.Down, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Floor, BlockType.Floor, Direction.Down, ref m_validation);

                //Permission through Ramp
                LocalPermissionCheck(BlockType.Floor, BlockType.Ramp, Direction.Left, ref m_validation);
                LocalPermissionCheck(BlockType.Floor, BlockType.Ramp, Direction.Right, ref m_validation);
                LocalPermissionCheck(BlockType.Floor, BlockType.Ramp, Direction.Forward, ref m_validation);
                LocalPermissionCheck(BlockType.Floor, BlockType.Ramp, Direction.Backward, ref m_validation);

                Direction[] rampDir = new Direction[] { Direction.Left, Direction.Right, Direction.Forward, Direction.Backward };

                for (int i = 0; i < rampDir.Length; i++)
                {
                    StaticPermissionCheck(grid, transform, Neighbour.Down, BlockType.Floor, BlockType.Ramp, rampDir[i], ref m_validation);
                }
                //Permission thourgh Nighbour Ramp

                StaticPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Floor, BlockType.Ramp, Direction.Forward, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Left, BlockType.Floor, BlockType.Ramp, Direction.Left, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Right, BlockType.Floor, BlockType.Ramp, Direction.Right, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Floor, BlockType.Ramp, Direction.Backward, ref m_validation);

                StaticPermissionCheck(grid, transform, Neighbour.BackwardDown, BlockType.Floor, BlockType.Ramp, Direction.Forward, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.ForwardDown, BlockType.Floor, BlockType.Ramp, Direction.Backward, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.RightDown, BlockType.Floor, BlockType.Ramp, Direction.Left, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.LeftDown, BlockType.Floor, BlockType.Ramp, Direction.Right, ref m_validation);


                //Wall permission Check
                Direction[] wallDir = new Direction[] { Direction.Left, Direction.Right, Direction.Forward, Direction.Backward };

                for (int i = 0; i < wallDir.Length; i++)
                {
                    StaticPermissionCheck(grid, transform, Neighbour.Down, BlockType.Floor, BlockType.Wall, wallDir[i], ref m_validation);
                    LocalPermissionCheck(BlockType.Floor, BlockType.Wall, wallDir[i], ref m_validation);
                }
                StaticPermissionCheck(grid, transform, Neighbour.Backward, BlockType.Floor, BlockType.Wall, Direction.Forward, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Floor, BlockType.Wall, Direction.Backward, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Right, BlockType.Floor, BlockType.Wall, Direction.Left, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Left, BlockType.Floor, BlockType.Wall, Direction.Right, ref m_validation);

                return m_validation;

            case BlockType.Wall:

                m_validation = new NodeBool(false, null);


                //Floor check
                LocalPermissionCheck(BlockType.Wall, BlockType.Floor, Direction.Down, ref m_validation);
                StaticPermissionCheck(grid, transform, Neighbour.Up, BlockType.Wall, BlockType.Floor, Direction.Down, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Wall, BlockType.Floor, Direction.Down, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.ForwardUp, BlockType.Wall, BlockType.Floor, Direction.Down, ref m_validation);

                //Ramp Check
                LocalPermissionCheck(BlockType.Wall, BlockType.Ramp, Direction.Left, ref m_validation);
                LocalPermissionCheck(BlockType.Wall, BlockType.Ramp, Direction.Right, ref m_validation);
                LocalPermissionCheck(BlockType.Wall, BlockType.Ramp, Direction.Backward, ref m_validation);
                LocalPermissionCheck(BlockType.Wall, BlockType.Ramp, Direction.Forward, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Wall, BlockType.Ramp, m_oppositDirection, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.Forward, BlockType.Wall, BlockType.Ramp, m_sameDirection, ref m_validation);
                DynamicPermissionCheck(grid, transform, Neighbour.ForwardDown, BlockType.Wall, BlockType.Ramp, m_oppositDirection, ref m_validation);

                DynamicPermissionCheck(grid, transform, Neighbour.Up, BlockType.Wall, BlockType.Ramp, m_oppositDirection, ref m_validation);

                //Wall Check


                if (transform.GetDirection() == Direction.Forward)
                {
                    DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Wall, BlockType.Wall, Direction.Forward, ref m_validation);
                    DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Wall, BlockType.Wall, Direction.Forward, ref m_validation);
                }
                if (transform.GetDirection() == Direction.Backward)
                {
                    DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Wall, BlockType.Wall, Direction.Backward, ref m_validation);
                    DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Wall, BlockType.Wall, Direction.Backward, ref m_validation);
                }

                if (transform.GetDirection() == Direction.Left)
                {
                    //Debug.Log("Left");
                    DynamicPermissionCheck(grid, transform, Neighbour.Left, BlockType.Wall, BlockType.Wall, Direction.Left, ref m_validation);
                    DynamicPermissionCheck(grid, transform, Neighbour.Right, BlockType.Wall, BlockType.Wall, Direction.Left, ref m_validation);
                }
                if (transform.GetDirection() == Direction.Right)
                {
                    //Debug.Log("Right");
                }
                StaticPermissionCheck(grid, transform, Neighbour.Down, BlockType.Wall, BlockType.Wall, m_sameDirection, ref m_validation);

                return m_validation;

        }
        Debug.Log("End");
        return m_validation;
    }

    private GridNode DynamicNeighbour(Transform transform, Grid grid, Neighbour neighbour)
    {

        Vector3 down = grid.GetNodeCenter(transform.position - (transform.up * grid.nodeSize));
        Vector3 up = grid.GetNodeCenter(transform.position + (transform.up * grid.nodeSize));


        Vector3 leftUp = grid.GetNodeCenter(transform.position - (transform.right * grid.nodeSize) + (transform.up * grid.nodeSize));
        Vector3 left = grid.GetNodeCenter(transform.position - transform.right * grid.nodeSize);
        Vector3 leftDown = grid.GetNodeCenter(transform.position - (transform.right * grid.nodeSize) + (-transform.up * grid.nodeSize));


        Vector3 rightUp = grid.GetNodeCenter(transform.position + (transform.right * grid.nodeSize) + (transform.up * grid.nodeSize));
        Vector3 right = grid.GetNodeCenter(transform.position + transform.right * grid.nodeSize);
        Vector3 rightDown = grid.GetNodeCenter(transform.position + (transform.right * grid.nodeSize) + (-transform.up * grid.nodeSize));


        Vector3 forwardUp = grid.GetNodeCenter(transform.position + (transform.forward * grid.nodeSize) + (transform.up * grid.nodeSize));
        Vector3 forward = grid.GetNodeCenter(transform.position + (transform.forward * grid.nodeSize));
        Vector3 forwardDown = grid.GetNodeCenter(transform.position + (transform.forward * grid.nodeSize) + (-transform.up * grid.nodeSize));


        Vector3 backwardUp = grid.GetNodeCenter(transform.position - (transform.forward * grid.nodeSize) + (transform.up * grid.nodeSize));
        Vector3 backward = grid.GetNodeCenter(transform.position - (transform.forward * grid.nodeSize));
        Vector3 backwardDown = grid.GetNodeCenter(transform.position - (transform.forward * grid.nodeSize) + (-transform.up * grid.nodeSize));


        switch (neighbour)
        {
            case Neighbour.Forward:
                return grid.GetNode(forward);

            case Neighbour.Backward:
                return grid.GetNode(backward);

            case Neighbour.Left:
                return grid.GetNode(left);

            case Neighbour.Right:
                return grid.GetNode(right);

            case Neighbour.Up:
                return grid.GetNode(up);

            case Neighbour.Down:
                return grid.GetNode(down);

            case Neighbour.ForwardUp:
                return grid.GetNode(forwardUp);

            case Neighbour.ForwardDown:
                return grid.GetNode(forwardDown);

            case Neighbour.BackwardDown:
                return grid.GetNode(backwardDown);

            case Neighbour.BackwardUp:
                return grid.GetNode(backwardUp);

            case Neighbour.LeftDown:
                return grid.GetNode(leftDown);

            case Neighbour.LeftUp:
                return grid.GetNode(leftUp);

            case Neighbour.RightDown:
                return grid.GetNode(rightDown);

            case Neighbour.RightUp:
                return grid.GetNode(rightUp);
        }
        return null;
    }

}
