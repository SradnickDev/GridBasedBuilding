using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BuildHandler : MonoBehaviour
{

    #region Inspector Reference

    [SerializeField] GridRecognition m_gridRecognition;


    [SerializeField] GameObject slot;
    [SerializeField] Transform buttonPanel;

    private Transform m_shadowGizmo;
    [SerializeField] Color m_active = new Color(1, 1, 1, 1);
    [SerializeField] Color m_inActive = new Color(0.5f, 0.5f, 0.5f, 1);

    [System.Serializable]
    public class GridHud
    {
        public Image image;
        public KeyCode inputKey;
        public Text keyText;


        public GridHud(Image image, KeyCode inputKey)
        {
            this.image = image;
            this.inputKey = inputKey;
        }
    }


    [System.Serializable]
    public class BuildableObject
    {
        [HideInInspector] public string Name;
        public GridObject gridObject;
        public GridHud gridHud;


        public BuildableObject(GridObject gridObject, GridHud gridHud)
        {
            this.gridObject = gridObject;
            this.gridHud = gridHud;
        }
    }

    [SerializeField] BuildableObject[] buildableObject;

    #endregion Inspector Reference
    #region Event


    public delegate void ChangeNodeHandler(GridNode gridNode);
    public event ChangeNodeHandler ChangeNode;

    #endregion;
    [SerializeField] private AudioSource m_audioSource;

    private GridObject m_currentBlock;
    private Vector3 m_postionOnGrid;
    private Quaternion m_currentRotation;
    private GridNode m_currentNode;
    private bool m_isValid;
    private GridNode gNode;
    float m_buildRate = 0.15f;
    float m_lastBuild;
    int m_lastIndex;

    private void OnValidate()
    {
        for (int i = 0; i < buildableObject.Length; i++)
        {
            if (buildableObject[i].gridObject != null)
            {
                buildableObject[i].Name = buildableObject[i].gridObject.GetType().ToString();
            }
        }
    }

    private void Start()
    {
        Initialize();
        m_shadowGizmo = new GameObject("GizmoShadow").transform;
        ChangeNode += OnGridNodeChanged;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Initialize()
    {
        for (int i = 0; i < buildableObject.Length; i++)
        {
            BuildableObject cachedBuildableObject = buildableObject[i];
            GridObject cachedGridObject = cachedBuildableObject.gridObject;
            cachedGridObject.Initialize();
            cachedBuildableObject.gridHud.keyText.text = cachedBuildableObject.gridHud.inputKey.ToString();
        }
    }
    void OnChangeNode(GridNode gridNode)
    {
        if (m_currentNode != gNode)
        {
            gNode = m_currentNode;

            if (ChangeNode == null) return;
            ChangeNode(gNode);

        }
    }
    void Update()
    {
        PlayerInput();

        if (m_currentBlock == null) return;
        m_currentNode = m_gridRecognition.GridRecognitionUpdate(m_currentBlock.GetBlockType());

        OnChangeNode(m_currentNode);
        if (gNode == null) return;
        m_currentBlock.Gizmo.SetPosition(gNode.center);
        m_shadowGizmo.transform.rotation = m_currentBlock.Gizmo.transform.rotation;

        m_currentBlock.RotateGizmo(KeyCode.R, m_gridRecognition.Player);

        if (m_currentBlock.Gizmo.transform.rotation != m_shadowGizmo.transform.rotation)
        {
            m_shadowGizmo.transform.rotation = m_currentBlock.Gizmo.transform.rotation;
            ValidateNode();
        }
        OnBuildInput();


    }
    void OnGridNodeChanged(GridNode gridNode)
    {
        if (gridNode == null) return;
        gNode = gridNode;
        m_shadowGizmo.position = gNode.center;
        ValidateNode();
    }

    void ValidateNode()
    {
        m_isValid = m_currentBlock.Validate(gNode, m_shadowGizmo);

        if (m_isValid)
        {
            m_audioSource.Play();
        }
        OnGizmoChange();
    }
    void PlayerInput()
    {
        for (int i = 0; i < buildableObject.Length; i++)
        {
            if (Input.GetKeyDown(buildableObject[i].gridHud.inputKey))
            {
                OnChangeBlock(buildableObject[i].gridObject, i);
            }
        }
        if (Input.GetKey(KeyCode.End))  //Pressing 'End' key will disable Cursor Lock
            Cursor.lockState = CursorLockMode.None;
    }

    void OnGizmoChange()
    {
        var m_dir = m_currentBlock.GetBlockType() == BlockType.Floor ? Direction.Down : m_currentBlock.Gizmo.transform.GetDirection();
        bool inUse = gNode.InUse(m_currentBlock.GetBlockType(), m_dir);

        m_currentBlock.EnableGizmo(inUse == true ? false : true);
        if (m_isValid)
            m_currentBlock.Gizmo.VisualePermission(true);
        else
            m_currentBlock.EnableGizmo(false);
    }



    void OnBuildInput()
    {
        if (m_isValid)
        {
            if (Input.GetMouseButton(0) && Time.time > m_lastBuild)
            {
                m_currentBlock.CreateBlock(gNode, m_currentBlock.Gizmo.transform.eulerAngles);
                OnGizmoChange();
                m_lastBuild = Time.time + m_buildRate;
            }
        }
    }

    void OnChangeBlock(GridObject gridObject, int index)
    {
        ChangeBlock(gridObject, index);
    }

    void ChangeBlock(GridObject newBlock, int index)
    {
        Vector3 position = new Vector3(0, 0, 0);

        if (m_currentBlock != null)
        {
            m_currentBlock.EnableGizmo(false);
            var m_prevGizmo = m_currentBlock.Gizmo;
            position = m_prevGizmo == null ? new Vector3(0, 0, 0) : m_shadowGizmo.position;
        }

        if (newBlock != m_currentBlock)
        {
            m_currentBlock = newBlock;
            m_currentBlock.UseGizmo(position);

            buildableObject[m_lastIndex].gridHud.image.color = m_inActive;
            buildableObject[index].gridHud.image.color = m_active;
            m_lastIndex = index;
        }
    }

}

