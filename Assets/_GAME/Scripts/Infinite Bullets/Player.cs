using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;
using System.Threading.Tasks;
public class Player : MonoBehaviour
{
	public static Action playerDeathEvent;
	public static Action playerLevelEvent;
	public static Action<int> playerGainExp;
	public static bool playerDeath = false;

	[Tooltip("Determines movement speed.")]
	[SerializeField] private float forwardMoveSpeed;
	[Range(1.0f,60f)]
	[Tooltip("Determines rotation speed.")]
	[SerializeField] private float rotateSpeed = 5.0f;
	[Range(0f,1f)]
	[SerializeField] private float bulletSpawnInterval = 0.2f, spawnTimer = 0f;

	[SerializeField] private GameObject m_BulletObject;
	[SerializeField] private ParticleSystem m_PS;
	[SerializeField] private Transform m_BulletSpawnPoint;
	[SerializeField] private GameObject m_LaserSight;
	[SerializeField] private Transform m_BulletPool;
	[SerializeField] private int expPerLevelModifier;

	private static Player _instance;
	private Joystick m_Joystick;
	private Rigidbody m_Rigid;
	private Animator m_Animator;
	private float activeVelocity;
	private bool gameStarted = false;
	private float experienceCurrent = 0, experienceMax = 100;
	private int playerLevel = 1;
	private bool rainbowMode = false;

	#region Getter-Setters
	public static Player Instance
	{
		get { return _instance; }
	}

	private void GameStarted()
	{
		gameStarted = true;
	}

	public Vector3 PlayerPosition
	{
		get { return transform.position; }
		set { transform.position = value; }
	}

	public float BulletSpawnSpeed
	{
		get { return bulletSpawnInterval; }
		set 
		{
			//Decrease time by %20
			bulletSpawnInterval = bulletSpawnInterval - (bulletSpawnInterval / 5);
		}
	}

	public bool PlayerIsDead
	{
		set { playerDeath = true;}
	}

	public float CurrentExperience => experienceCurrent;
	public float MaxExperience => experienceMax;
	public int PlayerLevel => playerLevel;

	//EXPERIMENTAL PART
	public bool RainbowMode => rainbowMode;

	public async void StartRainbowMode()
	{
		rainbowMode = true;
		Debug.Log("Rainbow Start");
		await Task.Delay(5000);
		rainbowMode = false;
		Debug.Log("Rainbow End");
	}

	#endregion

	#region MonoBehaviour
	private void Start()
	{
		_instance = this;
		playerDeath = false;
		m_Joystick = Joystick.Instance;
		m_Rigid = GetComponent<Rigidbody>();
		m_Animator = GetComponent<Animator>();
		//m_BulletPool = GameObject.Find("BulletPool").transform;
		GameManager.startGame += GameStarted;
		playerGainExp += UpdateExperienceSystem;
	}

	private void OnDestroy()
	{
		GameManager.startGame -= GameStarted;
		playerGainExp -= UpdateExperienceSystem;
	}

	void Update()
	{
		if (m_BulletPool == null) m_BulletPool = GameObject.Find("BulletPool").transform;
		if (!gameStarted) return;
		if (playerDeath) return;

		spawnTimer += Time.deltaTime;
		m_Animator.SetBool("Walking", (false));
		m_Animator.SetFloat("Velocity", (0));

		if (Input.touchCount > 0 || Input.GetMouseButton(0))
		{
			//Determine the direction
			float horizontal = m_Joystick.Horizontal;
			float forward = m_Joystick.Vertical;
			Vector3 direction = new Vector3(horizontal, 0, forward).normalized;
			//Vector3 direction = new Vector3(horizontal, 0, forward).normalized;
			if (direction.magnitude == 0)
				return;

			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

			//Move towards the direction
			transform.Translate(Vector3.forward * forwardMoveSpeed * Time.deltaTime, Space.Self);

			m_Animator.SetFloat("Velocity", (forwardMoveSpeed * Time.deltaTime));
			m_Animator.SetBool("Walking", (true));

		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (RainbowMode) return;

		if(collision.gameObject.tag == "Enemy")
		{
			//Game Ends
			Debug.Log("Player is Dead");
			m_PS.Play();
			m_Animator.SetBool("Death", (true));
			playerDeathEvent.Invoke();
			GameManager.levelEndStatus.Invoke(false);
			//GameManager.levelEndStatus.Invoke(false);
			playerDeath = true;
			m_LaserSight.SetActive(false);
		}
	}

	private void UpdateExperienceSystem(int exp)
	{
		if (playerDeath) return;
		experienceCurrent += exp;
		if(experienceCurrent >= experienceMax)
		{
			experienceCurrent -= experienceMax;
			playerLevel++;
			playerLevelEvent.Invoke();
			experienceMax = (playerLevel * expPerLevelModifier) + 100;
			GameManager.Instance.UpdateExperienceSystem(playerLevel);
			//GameManager.Instance.SkillsPanel(true);
		}
	}
	#endregion
}
