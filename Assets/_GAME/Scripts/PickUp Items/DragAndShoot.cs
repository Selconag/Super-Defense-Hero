using UnityEngine;
using Retroket.Managers;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 touchPressPos;
    private Vector3 touchReleasePos;

    private Rigidbody rb;
    private bool isFirstClick = true;
    private bool isShoot;

    private Vector3 m_TargetPos;

    private GameObject m_DragText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        if (m_UseObjectMass)
            forceMultiplier = rb.mass;
    }

    private void Update()
    {
        //if(Time.timeScale > 0)
        //{
        //    //OnMouseUp Event
        //    if (Input.touchCount == 0 && !isFirstClick)
        //    {
        //        DrawTrajectory.Instance.HideLine();
        //        //touchReleasePos = Input.GetTouch(0).position;
        //        touchPressPos = Input.mousePosition;
        //        Shoot(touchReleasePos - touchPressPos);
        //        rb.useGravity = true;
        //        isFirstClick = true;
        //    }
        //    //OnMouseDown Event
        //    else if (Input.touchCount > 0 && isFirstClick)
        //    {
        //        //touchPressPos = Input.GetTouch(0).position;
        //        touchPressPos = Input.mousePosition;
        //        isFirstClick = false;
        //    }
        //    //OnMouseDrag Event
        //    else if (Input.touchCount > 0 && !isFirstClick)
        //    {
        //        Vector3 forceInit = (Input.mousePosition - touchPressPos);
        //        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;

        //        if (!isShoot)
        //            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        //    }
        //    //if (Input.touchCount > 0)
        //    //{
        //    //    Touch touch = Input.GetTouch(0);
        //    //    //OnMouseDown Event
        //    //    if (isFirstClick)
        //    //    {
        //    //        //touchPressPos = Input.GetTouch(0).position;
        //    //        touchPressPos = Input.mousePosition;
        //    //        isFirstClick = false;
        //    //    }
        //    //    if (touch.phase == TouchPhase.Moved)
        //    //    {
        //    //        Vector3 forceInit = (Input.mousePosition - touchPressPos);
        //    //        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;

        //    //        if (!isShoot)
        //    //            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        //    //    }


        //    //}
        //}
    }

    private void OnMouseDown()
    {
        if (Time.timeScale > 0)
        {
            touchPressPos = Input.mousePosition;
        }
    }
    private void OnMouseUp()
    {
        if (Time.timeScale > 0)
        {
            //Hides previous Line for reference
            DrawTrajectory.Instance.HideLine();
            touchReleasePos = Input.mousePosition;
            Shoot(touchReleasePos - touchPressPos);
            //Shoot(Input.mousePosition - m_TargetPos);
            rb.useGravity = true;
        }

    }
    private void OnMouseDrag()
    {
        if (Time.timeScale > 0)
        {
            //Used for Parabola Line
            Vector3 forceInit = (Input.mousePosition - touchPressPos);
            Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;
            if (!isShoot)
                DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);

            ////Used for Straight Line
            //Vector3 forceInit = (Input.mousePosition - touchPressPos);
            //Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;
            //if (!isShoot)
            //    m_TargetPos = DrawTrajectoryFixed.Instance.UpdateTrajectory(forceV, rb, transform.position);
        }

        ////Draw a raycast from here
        //RaycastHit hit;

        //if (Physics.Raycast(this.gameObject.transform.position, Input.mousePosition - touchPressPos, out hit, 100))
        //{
        //    Debug.DrawRay(this.gameObject.transform.position, Input.mousePosition - touchPressPos,Color.blue);
        //}

    }

    [Tooltip("Use objects Rigidbody Mass value as Force Multiplier?")]
    [SerializeField] protected bool m_UseObjectMass = false;
    [SerializeField] protected float forceMultiplier = 2;

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;
        
        //X => Force.x / Y => Force.y / Z => Force.y because on screen it is only X&Y Dimensions
        rb.AddForce(new Vector3(Force.x, Force.y, Force.y) * forceMultiplier);
        
        Spawner.Instance.NewSpawnRequest();
        isShoot = true;
        HapticManager.Light();
            
    }

}