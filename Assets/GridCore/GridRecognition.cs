using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridRecognition : Orientation
{


    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] Vector3 m_positionOffset;
    [SerializeField] private HorizontalPoint m_horizontalPoint;
    [SerializeField] private VerticalPoint m_verticalPoint;
    public Transform Player;
    [SerializeField] private Grid m_grid;
    private float m_yAngel;
    private float m_xAngel;

    private GridNode m_currentNode;

    private GridNode m_positionNode;
    private GridNode m_forwardNode;
    private GridNode m_forwardDownNode;
    private GridNode m_forwardUpNode;

    private GridNode m_forLeftNode;
    private GridNode m_forRightNode;

    private GridNode m_leftNode;
    private GridNode m_rightNode;

    private GridNode m_backRight;
    private GridNode m_backward;
    private GridNode m_backLeft;


    private GridNode m_downNode;
    private GridNode m_upNode;




    public GridNode GridRecognitionUpdate(BlockType type)
    {
        FindGridNode();
        SightField(type);
        m_currentNode = GetVerticalPoint(GetHorizontalPoint(), type);
        return m_currentNode;
    }

    private void FindGridNode()
    {
        var t = Player.position + (Player.forward + m_positionOffset);
        m_positionNode = m_grid.GetNode(t);
        if (m_positionNode == null) return;


        m_positionNode.GetNighbourNode(Neighbour.ForwardUp, out m_forwardUpNode);
        m_positionNode.GetNighbourNode(Neighbour.Forward, out m_forwardNode);
        m_positionNode.GetNighbourNode(Neighbour.Left, out m_leftNode);
        m_positionNode.GetNighbourNode(Neighbour.Right, out m_rightNode);
        m_positionNode.GetNighbourNode(Neighbour.Forward, out m_forwardNode);

        m_rightNode.GetNighbourNode(Neighbour.Forward, out m_forRightNode);

        m_leftNode.GetNighbourNode(Neighbour.Forward, out m_forLeftNode);

        //if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.ForwardUp))
        //m_forwardUpNode = m_positionNode.staticNeighbour[Neighbour.ForwardUp];

        //if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Forward))
        //  m_forwardNode = m_positionNode.staticNeighbour[Neighbour.Forward];


        // if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Left))
        //   m_leftNode = m_positionNode.staticNeighbour[Neighbour.Left];

        // if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Right))
        //   m_rightNode = m_positionNode.staticNeighbour[Neighbour.Right];

        //if (m_rightNode.staticNeighbour.ContainsKey(Neighbour.Forward))
        //m_forRightNode = m_rightNode.staticNeighbour[Neighbour.Forward];

        //if (m_leftNode.staticNeighbour.ContainsKey(Neighbour.Forward))
        //  m_forLeftNode = m_leftNode.staticNeighbour[Neighbour.Forward];

        m_rightNode.GetNighbourNode(Neighbour.Backward, out m_backRight);
        m_positionNode.GetNighbourNode(Neighbour.Backward, out m_backward);
        m_leftNode.GetNighbourNode(Neighbour.Backward, out m_backLeft);

        m_positionNode.GetNighbourNode(Neighbour.Down, out m_upNode);
        m_positionNode.GetNighbourNode(Neighbour.Up, out m_upNode);


        //if (m_rightNode.staticNeighbour.ContainsKey(Neighbour.Backward))
        //m_backRight = m_rightNode.staticNeighbour[Neighbour.Backward];

        // if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Backward))
        //   m_backward = m_positionNode.staticNeighbour[Neighbour.Backward];

        //if (m_leftNode.staticNeighbour.ContainsKey(Neighbour.Backward))
        //  m_backLeft = m_leftNode.staticNeighbour[Neighbour.Backward];

        //if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Down))
        //  m_downNode = m_positionNode.staticNeighbour[Neighbour.Down];

        //if (m_positionNode.staticNeighbour.ContainsKey(Neighbour.Up))
        //  m_upNode = m_positionNode.staticNeighbour[Neighbour.Up];
    }

    private void SightField(BlockType type)
    {
        m_yAngel = Player.transform.eulerAngles.y;
        m_xAngel = transform.eulerAngles.x;

        m_horizontalPoint = Horizontal(m_yAngel);
        m_verticalPoint = Vertical(m_xAngel);

        switch (type)
        {
            case BlockType.Ramp:

                break;
            case BlockType.Floor:

                break;

            case BlockType.Wall:

                break;
        }
    }
    private GridNode GetVerticalPoint(GridNode gridNode, BlockType type)
    {
        GridNode m_targetNode = gridNode;
        GridNode m_cachedNode = null;

        switch (m_verticalPoint)
        {
            case VerticalPoint.None:
                Debug.LogWarning("Somthing went wrong!");
                return null;
            case VerticalPoint.Forward:

                if (Physics.Raycast(Player.position, Player.forward, m_grid.nodeSize, m_layerMask))
                {
                    Debug.Log("hit");

                    m_targetNode.GetNighbourNode(Neighbour.Up, out m_cachedNode);
                    //return m_cachedNode;
                }

                if (type == BlockType.Wall)
                {
                    return m_positionNode;
                }
                return m_targetNode;
            case VerticalPoint.ForwardUp:

                m_targetNode.GetNighbourNode(Neighbour.Up, out m_cachedNode);

                return m_cachedNode;
            case VerticalPoint.ForwardDown:

                if (type == BlockType.Floor)
                {
                    return m_targetNode;
                }
                if (type == BlockType.Wall)
                {
                    return m_positionNode;
                }
                if (type == BlockType.Ramp) return null;

                m_targetNode.GetNighbourNode(Neighbour.Down, out m_cachedNode);
                return m_cachedNode;


            case VerticalPoint.Down:

                return m_positionNode;

            case VerticalPoint.Up:

                m_positionNode.GetNighbourNode(Neighbour.Up, out m_cachedNode);
                return m_cachedNode;
        }
        return null;
    }

    private GridNode GetHorizontalPoint()
    {
        switch (m_horizontalPoint)
        {
            case HorizontalPoint.None:
                return m_positionNode;
            case HorizontalPoint.North:
                return m_forwardNode;
            case HorizontalPoint.West:
                return m_leftNode;
            case HorizontalPoint.East:
                return m_rightNode;
            case HorizontalPoint.NorthWest:
                return m_forLeftNode;
            case HorizontalPoint.NorthEast:
                return m_forRightNode;
            case HorizontalPoint.South:
                return m_backward;
            case HorizontalPoint.SouthEast:
                return m_backRight;
            case HorizontalPoint.SouthWest:
                return m_backLeft;
            case HorizontalPoint.Up:
                return m_upNode;
            case HorizontalPoint.Down:
                return m_downNode;
            case HorizontalPoint.ForwardUp:
                return m_forwardUpNode;
        }

        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Player.position + (Player.forward * m_positionOffset.z) +
                                                (Player.up * m_positionOffset.y), 0.2f);
    }
}
