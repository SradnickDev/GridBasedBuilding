using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class GridObject : MonoBehaviour
{

    #region PublicMembers
    public GameObject BlockObject;
    public GameObject BlockGizmo;
    public Gizmo Gizmo { get; set; }

    [Header("Scriptable Grid")]
    public Grid grid;

    public GameObject LastCreatedBlock { get; set; }
    public int Directions { get; set; }
    #endregion PublicMembers


    #region PrivateMembers
    private int m_poolSize;
    float m_lastRotation;
    float m_currentRotation;
    #endregion PrivateMembers

    #region Events
    #endregion Events


    public void Initialize()
    {
        Array enumVals = Enum.GetValues(typeof(Direction));
        Directions = enumVals.Length;
    }
    public virtual void CreateBlock(GridNode node, Vector3 rotation)
    {
        if (node == null) return;
        Quaternion rot = Quaternion.Euler(rotation);
        LastCreatedBlock = Instantiate(BlockObject, node.center, rot);
        LastCreatedBlock.name = CreateRandomName(8);
        node.block = LastCreatedBlock;
        node.blockHealth = LastCreatedBlock.GetComponent<BlockHealth>();
        node.blockHealth.SetGridBlock(node);
    }


    public abstract bool Validate(GridNode gridNode, Transform gizmo);
    /// <summary>Rotate Gizmo with</summary><param name="camera"></param>
    bool m_autoRotate = true;
    public virtual void RotateGizmo(KeyCode inputKey, Transform camera)
    {
        if (Input.GetKeyDown(inputKey))
        {
            m_autoRotate = false;
            m_currentRotation += 90;
            if (m_currentRotation > 360) m_currentRotation = 90;
            Gizmo.SetRotation(new Vector3(0, m_currentRotation, 0));

        }
        if (m_autoRotate)
        {
            if (Gizmo != null)
            {
                var t = AutoRotateGizmo(camera);
                if (Gizmo.transform.eulerAngles != t)
                {
                    Gizmo.SetRotation(t);
                }
            }
        }
        if (Gizmo.transform.GetDirection() == camera.GetDirection())
        {
            m_autoRotate = true;
        }

    }
    public virtual Vector3 AutoRotateGizmo(Transform camera)
    {
        Vector3 angle = camera.eulerAngles;
        Vector3 snapAngle = new Vector3(0, 0, 0);
        if (angle.x >= 45f && angle.x <= 90)
        {
            //down

            snapAngle = new Vector3(0, 180, 0);
        }
        if (angle.y < 45 || angle.y > 315)
        {
            //forward

            snapAngle = new Vector3(0, 0, 0);
        }
        if (angle.y > 45 && angle.y < 135)
        {

            //right

            snapAngle = new Vector3(0, 90, 0);
        }
        if (angle.y > 135 && angle.y < 225)
        {

            //back

            snapAngle = new Vector3(0, 180, 0);
        }
        if (angle.y > 225 && angle.y < 315)
        {
            //left

            snapAngle = new Vector3(0, 270, 0);
        }

        return snapAngle;
    }
    /// <summary>If no Gizmo avaible creates one and use it.</summary><param name="initialPosition">Node Position</param>
    public virtual void UseGizmo(Vector3 initialPosition)
    {
        if (Gizmo == null)
        {
            Gizmo = Instantiate(BlockGizmo, initialPosition, Quaternion.identity).GetComponent<Gizmo>();
        }
        else if (!Gizmo.gameObject.activeInHierarchy)
        {
            Gizmo.SetPosition(initialPosition);
            EnableGizmo(false);
        }
    }
    public void EnableGizmo(bool value)
    {
        Gizmo.gameObject.SetActive(value);
    }
    public abstract BlockType GetBlockType();

    private string CreateRandomName(int numChars)
    {
      var retString = String.Format("{0}", (char)UnityEngine.Random.Range(65, 92));  //begin with a capital letter
      do
        retString = String.Format("{0}{1}", retString, (char)UnityEngine.Random.Range(97, 122));
      while (numChars-- > 0);

      return retString;
    }
}
