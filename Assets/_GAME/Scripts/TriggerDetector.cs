using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public string ActivatorTag;
    //The list of colliders currently inside the trigger
    public List<Collider> TriggerList = new List<Collider>();

    public List<Collider> GetList()
    {
        return TriggerList;
    }

    public void ClearCollection()
    {
        TriggerList.Clear();
    }
 
    //called when something enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != ActivatorTag) return;
        //if the object is not already in the list
        if (!TriggerList.Contains(other))
        {
            //add the object to the list
            TriggerList.Add(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit(Collider other)
    {
        if (other.tag != ActivatorTag) return;
        //if the object is not already in the list
        if (!TriggerList.Contains(other))
        {
            //add the object to the list
            TriggerList.Remove(other);
        }
    }
}
