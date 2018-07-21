using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour {

    public GameObject blockObject;

    public GameObject blockGizmo;
    public int coasts;
    public Grid grid;
    [HideInInspector] public GameObject createdBlock;
    [HideInInspector] public int directions;

    public virtual void CreateBlock(GridNode node,Quaternion rotation)
    {
        createdBlock = Instantiate(blockObject, node.center, rotation);
        node.block = createdBlock;
    }
    public abstract bool Validate(GridNode gridNode,Transform gizmo);

}
