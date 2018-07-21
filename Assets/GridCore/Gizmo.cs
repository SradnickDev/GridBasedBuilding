using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{


    [SerializeField] Transform m_transform;     //gizmos Transform
    [SerializeField] MeshRenderer m_meshRenderer;       //gizmos Material
    [SerializeField] Color m_positiv = new Color(0, 1, 0, 0.25f);           //visualize negativ action
    [SerializeField] Color m_negativ = new Color(1, 0, 0, 0.25f);           //visualize negativ action
    [SerializeField] float m_smoothDamp = 0.25f;


    private Vector3 m_targetPositon;
    private Vector3 m_targetRotation;
    private Vector3 m_positionVelocity;         //ref to smooth Position
    private Vector3 m_rotationVelocity;			//ref to smooth Rotation

    /// <summary>Position change will be smooth.</summary>
    /// <param name="targetPosition">Position to go</param>
    public void SetPosition(Vector3 targetPosition)
    {
        var m_newPositon = targetPosition;
        m_targetPositon = Vector3.SmoothDamp(m_transform.position, m_newPositon, ref m_positionVelocity, m_smoothDamp * Time.deltaTime);
        m_transform.position = m_targetPositon;
    }

    /// <summary>Rotation change will be smooth.</summary>
    /// <param name="rotation">rotation to go</param>
    public void SetRotation(Vector3 rotation)
    {
        var m_newRotation = rotation;
        //m_targetRotation.x = Mathf.SmoothDampAngle(m_transform.eulerAngles.x,m_newRotation.x,ref m_rotationVelocity.x,m_smoothDamp * Time.deltaTime);
        //m_targetRotation.y = Mathf.SmoothDampAngle(m_transform.eulerAngles.y, m_newRotation.y, ref m_rotationVelocity.y, m_smoothDamp * Time.deltaTime);
        //m_targetRotation.z = Mathf.SmoothDampAngle(m_transform.eulerAngles.z,m_newRotation.z,ref m_rotationVelocity.z,m_smoothDamp * Time.deltaTime);
        m_transform.eulerAngles = m_newRotation;
    }
    public void VisualePermission(bool positiv)
    {
        m_meshRenderer.material.color = positiv == true ? m_positiv : m_negativ;
    }

}

