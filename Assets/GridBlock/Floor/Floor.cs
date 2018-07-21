using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Floor : GridObject
{

    Grid GridBase { get { return grid; } }

    public override void CreateBlock(GridNode node, Vector3 rotation)
    {
        base.CreateBlock(node, rotation);
        node.AddNodeUseCase(BlockType.Floor, Direction.Down, LastCreatedBlock);
    }
    public override void RotateGizmo(KeyCode inputKey, Transform camera)
    {
        base.RotateGizmo(inputKey, camera);
    }
    public override bool Validate(GridNode gridNode, Transform gizmo)
    {
        GridNode gNode = gridNode;
        var valid = gridNode.HasNeighbour(gizmo, GridBase, GetBlockType()).isAvailable;

        bool inUse = gNode.InUse(BlockType.Floor, Direction.Down);

        if (gNode != null)
        {
            
            if (gNode.isUsable && !inUse || valid && !inUse)
            {
                return true;
            }
        }
        return false;
    }

    public override BlockType GetBlockType()
    {
        return BlockType.Floor;
    }
}
