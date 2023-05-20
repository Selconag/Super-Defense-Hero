using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    [SerializeField] 
    protected LineRenderer _lineRenderer;
    [SerializeField]
    [Range(3, 30)] protected int _lineSegmentCount = 20;
    [SerializeField] protected GameObject m_ShadowObject;


    private List<Vector3> _linePoints = new List<Vector3>();

    #region Singleton

    public static DrawTrajectory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Update()
    {
        //SURFACE GLOW SYSTEM
        //Adds a glowing shadow shape of the object to the ground that will be thrown
        if (m_ShadowObject != null && _linePoints.Count != 0)
        {
            m_ShadowObject.transform.position = _linePoints[_linePoints.Count - 1];

        }
    }

    public void UpdateTrajectory(Vector3 forceVector,Rigidbody rb,Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector / rb.mass) * Time.fixedDeltaTime;
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = FlightDuration / _lineSegmentCount;

        _linePoints.Clear();
        //Uncomment below if you are not using interaction system
        _linePoints.Add(startingPoint);

        //for (int i = 0; i < _lineSegmentCount; i++)
        for (int i = 1; i < _lineSegmentCount; i++)
            {
            float stepTimePassed = stepTime * i * 3;

            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed);

            RaycastHit hit;


            //INTERACTION SYSTEM
            //If it interacts with any object, this part makes it last position for line
            Vector3 NewPointOnLine = -MovementVector + startingPoint;
            if (Physics.Raycast(_linePoints[i - 1], NewPointOnLine - _linePoints[i - 1], out hit, (NewPointOnLine - _linePoints[i - 1]).magnitude))
            {
                _linePoints.Add(hit.point);
                break;
            }
            //Debug.DrawLine(_linePoints[i - 1], NewPointOnLine, Color.yellow, 0.0f, true);
            _linePoints.Add(NewPointOnLine);


            //NOT-INTERACTION SYSTEM
            //This part doesn't care above
            //if (Physics.Raycast(startingPoint, -MovementVector, out hit, MovementVector.magnitude))
            //{
            //    break;
            //}
            //_linePoints.Add(-MovementVector + startingPoint);

        }
        m_ShadowObject.GetComponent<MeshRenderer>().enabled = true;

        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    public void HideLine()
    {
        _lineRenderer.positionCount = 0;
        m_ShadowObject.GetComponent<MeshRenderer>().enabled = false;

    }
}
