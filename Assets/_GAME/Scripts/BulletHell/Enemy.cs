using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lean.Pool;
using System.ComponentModel;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    private bool lastEnemy;
    private bool firstEnable = false;
    [SerializeField] private float health;
    [SerializeField] private bool walkEnd = false;
    [SerializeField] private Vector3 MoveDirection;
    [Range(0.01f,2f)]
    [SerializeField] private float speedCoefficient;
    public List<string> DamageTags;

    [Header("References")]
    [SerializeField] private EntityProperties m_EnemyProperties;
    [SerializeField] private Transform m_Target;
    [SerializeField] private ParticleSystem m_PS;


    [SerializeField] private Animator m_Anim;
    //[SerializeField] private Collider m_Coll;
    //[SerializeField] private Rigidbody m_Rigid;
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private Renderer m_Rend;


    public bool IsLastEnemy
	{
		get { return lastEnemy; }
		set { lastEnemy = true; }
	}

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float HealthChange
    {
        set { health -= value; }
    }

    public Transform Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }


 //   private void Awake()
	//{
 //       m_Anim = GetComponent<Animator>();
 //       //m_Coll = GetComponent<Collider>();
 //       //m_Rigid = GetComponent<Rigidbody>();
 //       m_Rend = transform.GetChild(0).GetComponent<Renderer>();
 //   }

	//void Start()
 //   {
 //       health = m_EnemyProperties.Health;
 //   }

	void Update()
	{
		if (!walkEnd)
		{
			transform.LookAt(m_Target);
            MoveDirection = Target.position - transform.position;
            m_Controller.Move(new Vector3(MoveDirection.x, transform.position.y, MoveDirection.z) * Time.deltaTime * speedCoefficient);
        }
	}
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //if (!col.transform.gameObject.GetComponent<Bullet>().IsPiercingBullet)
            //    LeanPool.Despawn(col.gameObject);
            //col.gameObject.GetComponent<Bullet>().DecreasePierce = 1;
            StartCoroutine(FlashEffect());
            health--;
            //If no health left, enemy dies
            if (health <= 0)
            {
                //Player.playerGainExp.Invoke(m_EnemyProperties.Experience);
                StartCoroutine(DespawnWaiter());
            }
            //If enemy survives, apply knockback
            else
            {
                //m_Rigid.AddForce(Vector3.back * 1000f, ForceMode.Force);

            }

        }
    }

    private IEnumerator FlashEffect()
	{
        //Apply flash effects
        MaterialPropertyBlock MPB = new MaterialPropertyBlock();
        Color c = transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_Color");
        m_Rend.GetPropertyBlock(MPB);
        MPB.SetColor("_Color", Color.yellow);
        m_Rend.SetPropertyBlock(MPB);
        yield return new WaitForSeconds(0.1f);
        MPB.SetColor("_Color", c);
        m_Rend.SetPropertyBlock(MPB);
    }


    IEnumerator DespawnWaiter()
	{
        m_PS.Play();
        m_Anim.SetBool("Death", (true));
        m_Controller.enabled = false;
        StopEnemy();
        yield return new WaitForSeconds(1.5f);
        LeanPool.Despawn(this.gameObject);
    }

    private void StopEnemy()
	{
        walkEnd = true;
        m_Target = this.transform;
        m_Anim.SetBool("Walking", (false));
        transform.LookAt(m_Target);
	}

  //  private void OnEnable()
  //  {
  //      if(firstEnable)
  //      m_Target = GameObject.FindGameObjectWithTag("Player").transform;
  //      firstEnable = true;
  //      walkEnd = false;
  //      m_Controller.enabled = true;
  //      health = m_EnemyProperties.Health;

  //      if (Player.playerDeath) StopEnemy();
  //      else Player.playerDeathEvent += StopEnemy;
  //  }

  //  private void OnDisable()
  //  {
  //      //If this was last enemy, Invoke level end with success
  //      if (lastEnemy)
		//{
  //          if(GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
  //              GameManager.levelEndStatus.Invoke(true);
  //          Debug.Log("Last Enemy Killed");
  //          lastEnemy = false;
  //      }
  //      walkEnd = false;
  //      Player.playerDeathEvent -= StopEnemy;
  //  }


    #region TakeDamage Efects
    public void TakeDamageAndEffect(float damageAmount, DamageEffects effect = DamageEffects.None)
    {
        HealthChange = damageAmount;
        StartCoroutine(FlashEffect());
        if (Health <= 0)
        {
            StartCoroutine(DespawnWaiter());
        }
    }
    #endregion

    #region Behavioral Parts

    #endregion
}
