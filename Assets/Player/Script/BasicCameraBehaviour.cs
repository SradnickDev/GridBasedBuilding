using UnityEngine;

public class BasicCameraBehaviour : MonoBehaviour
{

    [System.Serializable]
    public class TargetOptions
    {
        public Transform Target;                              //target we want to follow
        public Vector3 Offset = new Vector3(2, 2, 8);   //XAxis Left -Right offset, yAxis Height offset, ZAxis distance to Target
    }
    [System.Serializable]
    public class LimitInfo
    {
        public float Minimum = -80;                         //Limit yAxis up
        public float Maximum = 80;                          //Limit yAxis down
        public float Speed = 7;
    }
    [System.Serializable]
    public class Collision
    {
        public LayerMask Layer;
        public float RayThickness = 0.1f;
        public float OffsetFromWall = 0.1f;
        public float smoothTime = 0.1f;
    }
    [SerializeField] TargetOptions targetOptions;
    [SerializeField] LimitInfo limitInfo;
    [SerializeField] Collision collision;
    float m_yDeg;                                                     //should store MouseYaxis
    Vector3 m_targetPosition;                                         //target Camera Position
    Quaternion m_rotation;
    float m_currentDistance;
    float m_velocity;


    private void Start()
    {
        m_currentDistance = targetOptions.Offset.z;
    }

    void LateUpdate()
    {
        if (targetOptions.Target == null) return;
        CameraBehaviour();
    }
    private void CameraBehaviour()
    {

        var startPos = targetOptions.Target.position;
        var endPos = startPos - transform.forward * targetOptions.Offset.z;
        var result = Vector3.zero;

        RayCast(startPos, endPos, ref result, collision.RayThickness);
        float resultDistance = Vector3.Distance(targetOptions.Target.position, result);

        if (resultDistance <= targetOptions.Offset.z)
        {
            m_currentDistance = resultDistance + collision.OffsetFromWall;
            
        }
        else
        {
            m_currentDistance = Mathf.SmoothDamp(m_currentDistance, resultDistance, ref m_velocity, collision.smoothTime);
        }

        Vector3 m_vTargetOffset = new Vector3(0, -targetOptions.Offset.y, 0);                           //Offeset Vector
        m_yDeg -= Input.GetAxis("Mouse Y") * limitInfo.Speed;                                            //Mouse Vertical movement
        m_yDeg = Mathf.Clamp(m_yDeg, limitInfo.Minimum, limitInfo.Maximum);                                     //Clamp betwen min and max
        m_rotation = Quaternion.Euler(m_yDeg, targetOptions.Target.eulerAngles.y, 0);                              //Get Targets Y Euler Angel and add it to this rotation

        m_targetPosition = targetOptions.Target.position + (targetOptions.Target.right * targetOptions.Offset.x) - (m_rotation * Vector3.forward * m_currentDistance + m_vTargetOffset);

        transform.position = m_targetPosition;                                                  //add m_targetPosition to this position
        transform.rotation = m_rotation;                                                        //add m_rotation to this position
    }
    private bool RayCast(Vector3 start, Vector3 end, ref Vector3 result, float thickness)
    {
        var direction = end - start;
        var distance = Vector3.Distance(start, end);

        RaycastHit hit;
        if (Physics.SphereCast(new Ray(start, direction), thickness, out hit, distance, collision.Layer.value,QueryTriggerInteraction.Collide))
        {
            result = hit.point + hit.normal * collision.RayThickness;
            return true;
        }
        else
        {
            result = end;
            return false;
        }
    }
}
