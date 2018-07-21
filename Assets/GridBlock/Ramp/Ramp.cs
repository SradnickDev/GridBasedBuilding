using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Ramp : GridObject
{

    GameObject RampBlock { get { return BlockObject; } set { BlockObject = value; } }
    GameObject RampGizmo { get { return BlockGizmo; } set { BlockGizmo = value; } }

    Grid GridBase { get { return grid; } }

    GameObject LastCreatedRamp { get { return LastCreatedBlock; } set { LastCreatedBlock = value; } }

    public override void CreateBlock(GridNode node, Vector3 rotation)
    {
        base.CreateBlock(node, rotation);
        var dir = LastCreatedBlock.transform.GetDirection();
        node.AddNodeUseCase(BlockType.Ramp, dir, LastCreatedBlock);
    }

    public override bool Validate(GridNode gridNode, Transform gizmo)
    {
        GridNode gNode = gridNode;
        var valid = gridNode.HasNeighbour(gizmo, GridBase, GetBlockType()).isAvailable;
        bool inUse = false;
        var dir = new Direction[] { Direction.Left, Direction.Right, Direction.Forward, Direction.Backward };
        foreach (var entry in dir)
        {
            if (!gridNode.InUse(BlockType.Ramp,entry))
            {
                continue;
            }
            inUse = true;
        }


        if (gNode != null)
        {
            if (valid && !inUse || gNode.isUsable && !inUse) return true;
        }
        

        return false;
    }
    public override BlockType GetBlockType()
    {
        return BlockType.Ramp;
    }
}
