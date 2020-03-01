using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { ATTACKING, PURSUIT, PATROL, DEFAULT };
public class Enemy : MonoBehaviour
{
    public static float pursuitMaxDistance;
    public bool pursuingTarget;
    public int Creature_Damage;
    public int enemy_hp_max;
    public int enemy_hp;
    public float patrolSpeed;
    public float pursuitSpeed;
    public Animator anim;
    public Transform[] waypoints;
    public int curWaypointIndex;
    public GameObject player;
    public float attackDistance;
    public GameObject healthBar;
    public float originalXscale;

    protected EnemyState state = EnemyState.DEFAULT;

    void Start()
    {
        pursuitMaxDistance = 30.0f;
        pursuingTarget = false;
        Creature_Damage = -10;
        enemy_hp_max = 100;
        enemy_hp = 100;
        patrolSpeed = 2.0f;
        pursuitSpeed = patrolSpeed + 0.5f;
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        curWaypointIndex = 0;
        attackDistance = 8.0f;
        originalXscale = healthBar.transform.localScale.x;
        //healthBar = GameObject.GetChildren();
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.DEFAULT:
                if (Vector3.Distance(transform.position, player.transform.position) <= pursuitMaxDistance)
                {
                    state = EnemyState.PURSUIT;
                }
                else
                {
                    state = EnemyState.PATROL;
                }
                anim.SetBool("RUN", true);
                break;
            case EnemyState.PATROL:
                if (Vector3.Distance(transform.position, player.transform.position) <= pursuitMaxDistance)
                {
                    state = EnemyState.PURSUIT;
                    pursuingTarget = true;
                }
                else if (curWaypointIndex < waypoints.Length)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[curWaypointIndex].position, Time.deltaTime * patrolSpeed);

                    if (!pursuingTarget)
                    {
                        transform.LookAt(waypoints[curWaypointIndex].position);
                    }

                    if (Vector3.Distance(transform.position, waypoints[curWaypointIndex].position) < 1.5f) //set new patrol point if it's getting close
                    {
                        curWaypointIndex++;
                    }
                }
                else
                {
                    curWaypointIndex = 0;
                }
                break;
            case EnemyState.PURSUIT:
                Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
                {
                    state = EnemyState.ATTACKING;
                    anim.SetBool("RUN", false);
                    anim.SetBool("Attack", true);
                }
                else if (Vector3.Distance(transform.position, player.transform.position) > pursuitMaxDistance)
                {
                    state = EnemyState.PATROL;
                    pursuingTarget = false;
                }
                else //move towards player
                {
                    transform.LookAt(player.transform.position);
                    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Time.deltaTime * pursuitSpeed);
                }
                break;
            case EnemyState.ATTACKING: //enemy is standing still
                if (Vector3.Distance(transform.position, player.transform.position) > attackDistance + 0.5f)
                {
                    state = EnemyState.PURSUIT;
                    anim.SetBool("Attack", false);
                    anim.SetBool("RUN", true);
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        } else if (other.CompareTag("Dagger"))
        {
            enemy_hp -= 10;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float xScale = (enemy_hp * originalXscale) / enemy_hp_max;
        healthBar.transform.localScale = new Vector3(xScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    void GetDamage()
    {

    }

}


/**public class Enemy : MonoBehaviour {
    
    public Transform shootElement;
    public GameObject bullet;
    public GameObject Enemybug;
    public int Creature_Damage = 10;    
    public float Speed;
    // 
    public Transform[] waypoints;
    int curWaypointIndex = 0;
    public float previous_Speed;
    public Animator anim;
    public int Enemy_Hp;
    public Transform target;
    public GameObject EnemyTarget;

    public bool targetLocked;
    

    void Start()
    {
        EnemyTarget = null;
        target = null;
        anim = GetComponent<Animator>();
        Enemy_Hp = 100;
        previous_Speed = Speed;
        targetLocked = false;
    }

    // Attack
    void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player") //"Castle")
        {
            
            Speed = 0;
            EnemyTarget = other.gameObject;
            target = other.gameObject.transform;
            Vector3 targetPosition = new Vector3(EnemyTarget.transform.position.x, transform.position.y, EnemyTarget.transform.position.z);            
            transform.LookAt(targetPosition);
            anim.SetBool("RUN", false);
            anim.SetBool("Attack", true);
            targetLocked = true;
            
        }

    }

    // Attack
    void Shooting ()
    {
        //if (EnemyTarget)
       // {           
            GameObject с = GameObject.Instantiate(bullet, shootElement.position, Quaternion.identity) as GameObject;
            с.GetComponent<EnemyBullet>().target = target;
            с.GetComponent<EnemyBullet>().twr = this;
       // }  

    }

    

    void GetDamage ()

    {        
            EnemyTarget.GetComponent<TowerHP>().Dmg_2(Creature_Damage);       
    }

    


    void Update () 
	{

        
        //Debug.Log("Animator  " + anim);


        // MOVING

        if (curWaypointIndex < waypoints.Length){
	        transform.position = Vector3.MoveTowards(transform.position,waypoints[curWaypointIndex].position,Time.deltaTime*Speed);
            
            if (!EnemyTarget)
            {
                transform.LookAt(waypoints[curWaypointIndex].position);
            }
	
	        if(Vector3.Distance(transform.position,waypoints[curWaypointIndex].position) < 0.5f)
	        {
		        curWaypointIndex++;
	        }    
	    }
        else
        {
            anim.SetBool("Victory", true);  // Victory
        }

        // DEATH

        if (Enemy_Hp <= 0)
        {
            Speed = 0;
            Destroy(gameObject, 5f);
            anim.SetBool("Death", true);            
        }

        // Attack to Run

        if (EnemyTarget)        {

          
            if (EnemyTarget.CompareTag("Player"))//"Castle_Destroyed")) // get it from BuildingHp
            {
                anim.SetBool("Attack", false);
                anim.SetBool("RUN", true);
                Speed = previous_Speed;               
                EnemyTarget = null;                
            }
        }


    }
       
   
}*/
