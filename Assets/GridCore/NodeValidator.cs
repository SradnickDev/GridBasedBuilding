using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeValidator : MonoBehaviour
{

    [SerializeField] LayerMask m_layerMask;

    [SerializeField] private Grid m_grid;
    private float m_rayLength = 1;

    [SerializeField] Color m_debugColor = new Color(1, 0, 0);

    private RaycastHit m_hit;
    Vector3[] m_offset;
    Vector3[] m_direction;



    public void Offset(Transform trf)
    {
        var t = Mathf.Floor(m_grid.nodeSize / 2);
        t += m_grid.nodeSize * 0.1f;
        m_rayLength = m_grid.nodeSize * 1.1f;
        m_offset = new Vector3[]{

        trf.position  + (trf.up * t),
        trf.position  + (trf.forward * t),
        trf.position  + (trf.right * t)

        };
    }
    public void Direction(Transform trf)
    {
        m_direction = new Vector3[]{
                -trf.up,
                -trf.forward,
                -trf.right
            };
    }

    public bool CheckNode(Vector3 position)
    {
        transform.position = position;

        Offset(this.transform);
        Direction(this.transform);

        for (int i = 0; i < 3; i++)
        {
            if (Physics.Raycast(new Ray(m_offset[i],m_direction[i] * m_rayLength), out m_hit, m_grid.nodeSize, m_layerMask))
            {
                if (m_hit.transform != null)
                {
                    return true;
                }
            }
        }


        return false;

    }

    void OnDrawGizmos()
    {
        Offset(this.transform);
        Direction(this.transform);

        Gizmos.color = m_debugColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        for (int i = 0; i < 3; i++)
        {
            Gizmos.DrawRay(m_offset[i], m_direction[i] * m_rayLength);
        }
    }
}
