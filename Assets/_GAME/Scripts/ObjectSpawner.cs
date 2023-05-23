using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float newSpawnDuration = 1f;
    private float spawnTimer;

    [Header("References")]
    [SerializeField] private Image SkillTimerImage;
    [SerializeField] protected Transform t_SpawnPos;
    [SerializeField] protected List<GameObject> m_SpawnObjects;
    //[SerializeField]protected GameObject SpawnObject;
    private GameObject activeThrowable;

    #region Singleton

    public static ObjectSpawner Instance;

    private void Awake() => Instance = this;

    #endregion

    private void Start()
    {
        //v_SpawnPos = transform.position;
        SpawnNewObject();
    }

    private void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.unscaledDeltaTime;
            SkillTimerImage.fillAmount = spawnTimer / newSpawnDuration;
        }
    }

    //async Task SpawnNewObject()
    //{
    //    int RandomNumber = Random.Range(0, m_SpawnObjects.Capacity);
    //    activeThrowable = Instantiate(m_SpawnObjects[RandomNumber], t_SpawnPos.position, t_SpawnPos.rotation, t_SpawnPos.transform.parent);
    //    //Instantiate(SpawnObject, SpawnPos, Quaternion.identity);
    //    //await Task.Yield();
    //}
    public void SpawnNewObject()
    {
        int RandomNumber = Random.Range(0, m_SpawnObjects.Capacity);
        activeThrowable = Instantiate(m_SpawnObjects[RandomNumber], t_SpawnPos.position, t_SpawnPos.rotation, t_SpawnPos.transform.parent);
    }

    public IEnumerator NewSpawnRequest()
    {
        //if (activeThrowable != null) return;
        //await Task.Delay((int)Mathf.Round(newSpawnDuration * 1000));
        spawnTimer = newSpawnDuration;
        yield return new WaitForSeconds(newSpawnDuration);
        SpawnNewObject();
    }
    //public void NewSpawnRequest() => spawnTimer = newSpawnDuration;
}