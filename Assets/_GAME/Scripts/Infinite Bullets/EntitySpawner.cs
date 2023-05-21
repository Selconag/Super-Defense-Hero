using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
//using System;
public class EntitySpawner : MonoBehaviour
{
    //Spawn Region Values
    [SerializeField]
    protected float minXVal, maxXVal, minZVal, maxZVal;
    //Safe zone values
    [SerializeField]
    protected float minExclXVal, maxExclXVal, minExclZVal, maxExclZVal;
    //Open world Safe zone values
    [SerializeField]
    protected float minOpenXVal, maxOpenXVal, minOpenZVal, maxOpenZVal;
    [Range(0, 10f)]
    [Tooltip("Defines how much seconds the spawner wait for next entity to be spawned.")]
    [SerializeField]
    protected float spawnWaiter = 0.05f;
    [Tooltip("Defines if the map is open world or limited world space? Does not work with Endless Game Mode.")]
    [SerializeField] private bool openWorld;
    [Tooltip("Defines when will the game end based on players level.")]
    [SerializeField] private int targetPlayerLevel;
    [Tooltip("Defines the game mode. Is it endless or limited? Does not work with Open World Game Mode.")]
    [SerializeField] private bool endlessGame;
    [Tooltip("Defines if there is spawn points available or chosen random spawn points.")]
    [SerializeField] private bool spawnPoints;
    [Tooltip("Defines if there is a safe zone for player.")]
    [SerializeField] private bool safeZone;
    [Range(0, 100f)]
    [Tooltip("Defines how many entities will be spawned in this level. When the entities are over and all killed, level ends.")]
    [SerializeField] private int entityAmount;
    


    [SerializeField] private List<Transform> m_SpawnPoints;
    //Holds Entities Spawning List, can be added new ones or extract old ones
    [SerializeField] protected List<GameObject> m_EnemyList;
    [SerializeField] private Transform entityPool;

    private Vector3 position;
    private bool endGame;
    private GameObject lastEnemy;
	private static EntitySpawner _instance;

	public static EntitySpawner Instance =>_instance;
    private void Awake() => _instance = this;

    public int GetTargetPlayerLevel => targetPlayerLevel;

    void Start()
    {
        entityPool = GameObject.Find("EntityPool").transform;
        Player.playerDeathEvent += EndGame;
        GameManager.startGame += StartSpawningSequence;
        //LevelManager.levelChange += DespawnAllEntities;
        position = transform.position;
        //StartCoroutine(StartSpawningSequence());

        //Open world games cannot be endless!
        if (openWorld)
		{
            Player.playerLevelEvent += OpenWorldGameChecker;
            endlessGame = false;
        }
		else
            Player.playerLevelEvent += NewSpawnSystem;
    }
    private void OnDestroy()
    {
        Player.playerDeathEvent -= EndGame;
        GameManager.startGame -= StartSpawningSequence;
        if (openWorld)
            Player.playerLevelEvent -= OpenWorldGameChecker;
        else
            Player.playerLevelEvent -= NewSpawnSystem;
    }

    private void DespawnAllEntities()
    {
        LeanPool.DespawnAll();
    }

    public void StartSpawningSequence()
    {
        StartCoroutine(SpawningSequence());
    }

    /*
     * Note: Entities will be spawned randomly, in the area with given formula
     * Spawned X Coordinate "Xs" => minXVal < Xs < maxXVal .
     * if minXVal = -5 and maxXVal = 5
     * Xs will be chosen between -5 to 5 coordinates randomly.
     * 
     * Spawned Y Coordinate will always be "Ys = 0" .
     * 
     * Spawned Z Coordinate "Zs" => minZVal < Zs < maxZVal .
     * * if minZVal = -5 and maxZVal = 5
     * Zs will be chosen between -5 to 5 coordinates randomly.
     * 
     * SafeZone is a zone that no NPC's will be spawn, so the player will be safe from
     * spawn kills.
     */
    private IEnumerator SpawningSequence()
    {
        entityPool = GameObject.Find("EntityPool").transform;
        if (endlessGame)
        {
            while (!endGame)
            {
                if (m_EnemyList.Count == 0) break;

                else
                {
                     if (spawnPoints)
                    {
                        lastEnemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)].position, Quaternion.identity, entityPool);
                    }
                    else
                    {
                        if (safeZone)
                            SpawnWithSafeZone();
                        else
                            lastEnemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(Random.Range(minXVal, maxXVal), 0, Random.Range(minZVal, maxZVal)), Quaternion.identity, entityPool);

                    }
                }
                yield return new WaitForSeconds(spawnWaiter);
            }
        }

        else
        {
            if (openWorld)
            {
                OpenWorldGameChecker();
            }
            else
                NewSpawnSystem();
			//for (int i = 0; i < entityAmount; i++)
			//{
			//	if (endGame) break;

			//	if (spawnPoints)
			//	{
			//		lastEnemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)].position, Quaternion.identity, entityPool);
			//	}
			//	else
			//	{
			//		if (safeZone)
			//			SpawnWithSafeZone();
			//		else
			//			lastEnemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(Random.Range(minXVal, maxXVal), 0, Random.Range(minZVal, maxZVal)), Quaternion.identity, entityPool);

			//	}



			//	if (i == entityAmount - 1) lastEnemy.GetComponent<Enemy>().IsLastEnemy = true;

			//	yield return new WaitForSeconds(spawnWaiter);
			//}
		}
    }

    private void EndGame()
    {
        endGame = true;
    }


    //Note: Exclude zone must be a rectangle
    //Brief: Excludes a safe zone for player, in safe zone no enemy can spawn.
    private void SpawnWithSafeZone()
    {
        int region = Random.Range(0, 5);
        //Defines which zone will be selected for next spawn
        Vector3 zone;
        switch (region)
        {
            //Min X,Min Z
            case 0:
                zone = new Vector3(
                    Random.Range(minXVal, minExclXVal), 0, Random.Range(minZVal, minExclZVal));
                //LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(
                //Random.Range(minXVal, minExclXVal), 0, Random.Range(minZVal, minExclZVal)), Quaternion.identity, entityPool);
                break;
            //Min X,Max Z
            case 1:
                zone = new Vector3(
                    Random.Range(minXVal, minExclXVal), 0, Random.Range(maxExclZVal, maxZVal));
                //LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(
                //    Random.Range(minXVal, minExclXVal), 0, Random.Range(maxExclZVal, maxZVal)), Quaternion.identity, entityPool);
                break;
            //Max X,Min Z
            case 2:
                zone = new Vector3(
                    Random.Range(maxExclXVal, maxXVal), 0, Random.Range(minZVal, minExclZVal));
                //LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(
                //    Random.Range(maxExclXVal, maxXVal), 0, Random.Range(minZVal, minExclZVal)), Quaternion.identity, entityPool);
                break;
            //Max X,Max Z
            case 3:
                zone = new Vector3(
                    Random.Range(maxExclXVal, maxXVal), 0, Random.Range(maxExclZVal, maxZVal));
                //LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], new Vector3(
                //    Random.Range(maxExclXVal, maxXVal), 0, Random.Range(maxExclZVal, maxZVal)), Quaternion.identity, entityPool);
                break;
            default:
                zone = new Vector3(
                    Random.Range(minXVal, minExclXVal), 0, Random.Range(minZVal, minExclZVal));
                break;
        }

        lastEnemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], zone, Quaternion.identity, entityPool);

    }

    private void NewSpawnSystem()
    {
        //When boss defeated and...
        if (Player.Instance.PlayerLevel > targetPlayerLevel)
		{
            //End the level and move on to the next one
            //Player.playerDeathEvent.Invoke();
            //GameManager.levelEndStatus.Invoke(true);
            return;
        }


        //Get Player level and calculate creature amount 
        float creatureAmount = Player.Instance.PlayerLevel;// * 2 + 2;
        //float creatureAmount = Mathf.Pow(Player.Instance.PlayerLevel, 1.7f) + 2;
        for (int i = 0; i < creatureAmount; i++)
        {
            if (endGame) break;
            Enemy enemy;
            foreach (Transform SpawnPoint in m_SpawnPoints)
			{
                enemy = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], SpawnPoint.position, Quaternion.identity, entityPool).GetComponent<Enemy>();
                enemy.Health = (int)((float)enemy.Health + (float)(Player.Instance.PlayerLevel - 1) * 1.5f);
            }
        }


    }


    #region Open World

    private void OpenWorldGameChecker()
	{
        if (Player.Instance.PlayerLevel < targetPlayerLevel)
            OpenWorldSpawnSystem();

    }

    private void OpenWorldSpawnSystem()
    {
        //Get Player level and calculate creature amount
        float creatureAmount = Mathf.Pow(Player.Instance.PlayerLevel, 1.7f) + 2;

		for (int i = 0; i < creatureAmount; i++)
		{
            if (endGame) break;
            Enemy E;
            Vector3 playerPos = Player.Instance.PlayerPosition;
            int region = Random.Range(0, 5);
            //Defines which zone will be selected for next spawn
            Vector3 zone;
            //Z -20 to 40
            //X -12 to 12
            minOpenXVal = playerPos.x - 12f;
            maxOpenXVal = playerPos.x + 12f;
            minOpenZVal = playerPos.z - 20f;
            maxOpenZVal = playerPos.z + 40f;
            switch (region)
            {
                //Min X,Min Z
                case 0:
                    zone = new Vector3(
                        Random.Range(minOpenXVal - 10, minOpenXVal), 0, Random.Range(minOpenZVal - 10, minOpenZVal));
                    break;
                //Min X,Max Z
                case 1:
                    zone = new Vector3(
                        Random.Range(minOpenXVal - 10, minOpenXVal), 0, Random.Range(maxOpenZVal, maxOpenZVal + 10));
                    break;
                //Max X,Min Z
                case 2:
                    zone = new Vector3(
                        Random.Range(maxOpenXVal, maxOpenXVal + 10), 0, Random.Range(minOpenZVal - 10, minOpenZVal));
                    break;
                //Max X,Max Z
                case 3:
                    zone = new Vector3(
                        Random.Range(maxOpenXVal, maxOpenXVal + 10), 0, Random.Range(maxOpenZVal, maxOpenZVal + 10));
                    break;
                default:
                    zone = new Vector3(
                        Random.Range(minOpenXVal - 10, minOpenXVal), 0, Random.Range(minOpenZVal - 10, minOpenZVal));
                    break;
            }
            E = LeanPool.Spawn(m_EnemyList[Random.Range(0, m_EnemyList.Count)], zone, Quaternion.identity, entityPool).GetComponent<Enemy>();
            E.Health = (int)((float)E.Health + (float)(Player.Instance.PlayerLevel - 1) * 1.5f);
        }
    }
	#endregion
}