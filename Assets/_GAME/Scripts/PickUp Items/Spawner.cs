using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Spawner : MonoBehaviour
{
    [SerializeField]protected Transform t_SpawnPos;
    [SerializeField]protected List<GameObject> m_SpawnObjects;
    //[SerializeField]protected GameObject SpawnObject;
    private float newSpawnDuration = 1f;

    #region Singleton

    public static Spawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        //v_SpawnPos = transform.position;
        SpawnNewObject();
    }

    void SpawnNewObject()
    {
        int RandomNumber = Random.Range(0, m_SpawnObjects.Capacity);
        Instantiate(m_SpawnObjects[RandomNumber], t_SpawnPos.position, t_SpawnPos.rotation, t_SpawnPos.transform.parent);
        //Instantiate(SpawnObject, SpawnPos, Quaternion.identity);
    }

    public void NewSpawnRequest()
    {
        Invoke("SpawnNewObject", newSpawnDuration);
    }
}