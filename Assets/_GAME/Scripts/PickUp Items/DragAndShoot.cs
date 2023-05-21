using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndShoot : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Vector3 touchPressPos;
    [SerializeField] private Vector3 touchReleasePos;

    [Tooltip("Use objects Rigidbody Mass value as Force Multiplier?")]
    [SerializeField] protected bool m_UseObjectMass = false;
    [SerializeField] protected float forceMultiplier = 2;

    private bool isFirstClick = true;
    [SerializeField] private bool isShoot;
    private Vector3 m_TargetPos;
    private bool isClicking;

    [Header("References")]
    [SerializeField] private Rigidbody objectToThrow;
    [SerializeField] private Camera activeCamera;

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                //Vector3 hitPos = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 0);
                //We dont have an object to throw
                if(objectToThrow == null)
                {
                    touchPressPos = Input.mousePosition;
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touchPressPos);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.transform.tag == "Throwable")
                        {
                            objectToThrow = hit.transform.GetComponent<Rigidbody>();
                            Debug.Log("Hit");
                        }
                    }
                }
                //We have an object to throw
                else
                {
                    //Used for Parabola Line
                    Vector3 forceInit = (Input.mousePosition - touchPressPos);
                    Vector3 forceV = (new Vector3(forceInit.x, Mathf.Abs(forceInit.y), Mathf.Abs(forceInit.y))) * forceMultiplier;
                    if (!isShoot)
                        DrawTrajectory.Instance.UpdateTrajectory(forceV, objectToThrow, objectToThrow.transform.position);

                }
            }
            // Release the object
            else if (Input.touchCount == 0 && objectToThrow != null)
            {
                //Hides previous Line for reference
                DrawTrajectory.Instance.HideLine();
                touchReleasePos = Input.mousePosition;
                Shoot(touchReleasePos - touchPressPos);
                //Shoot(Input.mousePosition - m_TargetPos);
                objectToThrow.useGravity = true;
                objectToThrow = null;
                isShoot = false;
            }


            
        }
    }

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;
        
        //X => Force.x / Y => Force.y / Z => Force.y because on screen it is only X&Y Dimensions
        objectToThrow.AddForce(new Vector3(Force.x, Force.y, Force.y) * forceMultiplier);
        
        Spawner.Instance.NewSpawnRequest();
        isShoot = true;        
    }

    //UNUSED
    //private void OnMouseDown()
    //{
    //    if (Time.timeScale > 0)
    //    {
    //        touchPressPos = Input.mousePosition;
    //        //Vector3 hitPos = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 0);
    //        RaycastHit hit;
    //        Ray ray = activeCamera.ScreenPointToRay(touchPressPos);
    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            if (hit.transform.tag == "Throwable")
    //            {
    //                objectToThrow = hit.transform.GetComponent<Rigidbody>();
    //            }
    //        }
    //    }
    //}
    //private void OnMouseUp()
    //{
    //    if (Time.timeScale > 0)
    //    {
    //        //Hides previous Line for reference
    //        DrawTrajectory.Instance.HideLine();
    //        touchReleasePos = Input.mousePosition;
    //        Shoot(touchReleasePos - touchPressPos);
    //        //Shoot(Input.mousePosition - m_TargetPos);
    //        objectToThrow.useGravity = true;
    //        objectToThrow = null;
    //    }

    //}
    //private void OnMouseDrag()
    //{
    //    if (Time.timeScale > 0)
    //    {
    //        //Used for Parabola Line
    //        Vector3 forceInit = (Input.mousePosition - touchPressPos);
    //        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;
    //        if (!isShoot)
    //            DrawTrajectory.Instance.UpdateTrajectory(forceV, objectToThrow, transform.position);

    //        ////Used for Straight Line
    //        //Vector3 forceInit = (Input.mousePosition - touchPressPos);
    //        //Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;
    //        //if (!isShoot)
    //        //    m_TargetPos = DrawTrajectoryFixed.Instance.UpdateTrajectory(forceV, rb, transform.position);
    //    }

    //    ////Draw a raycast from here
    //    //RaycastHit hit;

    //    //if (Physics.Raycast(this.gameObject.transform.position, Input.mousePosition - touchPressPos, out hit, 100))
    //    //{
    //    //    Debug.DrawRay(this.gameObject.transform.position, Input.mousePosition - touchPressPos,Color.blue);
    //    //}

    //}

}