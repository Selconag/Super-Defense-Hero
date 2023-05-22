using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]protected Transform t_SpawnPos;
    [SerializeField]protected List<GameObject> m_SpawnObjects;
    //[SerializeField]protected GameObject SpawnObject;
    [SerializeField] private float newSpawnDuration = 1f;
    private GameObject activeThrowable;

    #region Singleton

    public static ObjectSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        //v_SpawnPos = transform.position;
    }

    async Task SpawnNewObject()
    {
        await Task.Delay((int)Mathf.Round(newSpawnDuration * 1000));
        int RandomNumber = Random.Range(0, m_SpawnObjects.Capacity);
        activeThrowable = Instantiate(m_SpawnObjects[RandomNumber], t_SpawnPos.position, t_SpawnPos.rotation, t_SpawnPos.transform.parent);
        //Instantiate(SpawnObject, SpawnPos, Quaternion.identity);
        await Task.Yield();
    }

    public async void NewSpawnRequest()
    {
        //if (activeThrowable != null) return;
        await SpawnNewObject();
    }
}