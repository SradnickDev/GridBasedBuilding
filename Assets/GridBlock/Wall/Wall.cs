using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wall : GridObject {

    Grid GridBase { get { return grid; } }

    private void Start()
    {
        System.Array enumValue = System.Enum.GetValues(typeof(Neighbour));
        Directions = enumValue.Length;
    }
    public override void CreateBlock(GridNode node, Vector3 rotation)
    {
        base.CreateBlock(node, rotation);
        var dir = LastCreatedBlock.transform.GetDirection();
        node.AddNodeUseCase(BlockType.Wall, dir, LastCreatedBlock);
        switch(dir)
        {
            case Direction.Backward:
                node.staticNeighbour[Neighbour.Backward].useCase[BlockType.Wall][Direction.Forward] = LastCreatedBlock;
                break;
            case Direction.Forward:
                node.staticNeighbour[Neighbour.Forward].useCase[BlockType.Wall][Direction.Backward] = LastCreatedBlock;
                break;
            case Direction.Left:
                node.staticNeighbour[Neighbour.Left].useCase[BlockType.Wall][Direction.Right] = LastCreatedBlock;
                break;
            case Direction.Right:
                node.staticNeighbour[Neighbour.Right].useCase[BlockType.Wall][Direction.Left] = LastCreatedBlock;
                break;
        }
    }
    public override BlockType GetBlockType()
    {
        return BlockType.Wall;
    }
    public override bool Validate(GridNode gridNode, Transform gizmo)
    {
        GridNode gNode = gridNode;
        var valid = gridNode.HasNeighbour(gizmo, GridBase, GetBlockType()).isAvailable;

        bool inUse = gNode.InUse(BlockType.Wall, gizmo.GetDirection());

        if (gNode != null)
        {

            if (gNode.isUsable && !inUse || valid && !inUse)
            {
                return true;
            }
        }
        return false;
    }
}
