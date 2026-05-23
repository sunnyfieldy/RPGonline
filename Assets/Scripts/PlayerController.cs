using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    
    [Header("Movement Settings")]
    public Transform aimVisual;
    public LayerMask groundLayer;
    
    [Header("AI Settings")]
    public float stoppingDistance = 2;
    public float interactDistance = 2.5f;
    
    [Header("Combat Settings")]
    public float attackInterval = 1.5f;
    public float attackRange = 2.5f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    public Transform healthBar;

    private float attackTimer = 0;
    private Transform currentEnemy;
    private NavMeshAgent agent;
    private Target currentTarget;
    private Equipment equipment;

    private Animator animator;

    public AudioClip attackSound;
    private AudioSource audioSource;
    void Start()
    {
        equipment = GetComponent<Equipment>();
        agent = GetComponent<NavMeshAgent>();
        aimVisual.gameObject.SetActive(false);
        attackTimer = attackInterval;

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        HandleInput();
        HandleInteraction();
        HandleCombat();

        animator.SetFloat("Speed", agent.velocity.magnitude, 0.1f, Time.deltaTime);
    }


    void HandleCombat()
    {
        if (currentEnemy == null) return;
        
        var distance = Vector3.Distance(transform.position, currentEnemy.position);
        
        //if enemy moved away - chase
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.transform.position);
            return;
        }
        
        //stop
        agent.SetDestination(transform.position);
        
        //face enemy
        var dir = (currentEnemy.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            animator.SetTrigger("Attack");

            audioSource.PlayOneShot(attackSound);

            attackTimer = 0;

            var enemyScript = currentEnemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(equipment.GetDamage());

            if (enemyScript.IsDead())
            {
                currentEnemy = null;
                currentTarget = null;
            }
        }
    }


    void HandleInteraction()
    {
        if (currentTarget == null || currentTarget.transform == null)
            return;
        
        var dist = Vector3.Distance(transform.position, currentTarget.transform.position);
              
        if (dist <= interactDistance)
        {
            switch (currentTarget.type)
            {
                case TargetType.Item:
                    PickupItem(currentTarget.transform);
                    currentTarget = null;
                    break;
                
                case TargetType.Enemy:
                    StartCombat(currentTarget.transform);
                    break;
            }
        }
    }

    
    private void StartCombat(Transform enemy)
    {
        //print("StartCombat with " + enemy.name);
        currentEnemy = enemy;
    }

    
    private void PickupItem(Transform item)
    {
        //print("Picked up " + item.name);
        var worldItem = item.GetComponent<ItemWorld>();
        inventory.AddItem(worldItem.data);
        Destroy(item.gameObject);
    }


    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentEnemy = null;
            currentTarget = null;
            agent.isStopped = false;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                if (hit.collider.tag == "Enemy")
                {
                    agent.stoppingDistance = stoppingDistance;
                    SetTarget(hit.collider.transform, TargetType.Enemy);
                }
                else if (hit.collider.tag == "Item")
                {
                    agent.stoppingDistance = stoppingDistance;
                    SetTarget(hit.collider.transform, TargetType.Item);
                }
                else
                {
                    agent.stoppingDistance = 0;
                    SetTarget(null, TargetType.Ground);
                    agent.SetDestination(hit.point);
                    aimVisual.position = hit.point + Vector3.up * 0.1f;
                    aimVisual.forward = hit.normal;
                    aimVisual.gameObject.SetActive(true);
                }
               
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            aimVisual.gameObject.SetActive(false);
        }
    }


    void SetTarget(Transform t, TargetType type)
    {
        currentTarget = new Target{transform = t, type = type};
        
        if(t != null)
            agent.SetDestination(t.position);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.localScale = new Vector3((float)currentHealth / maxHealth, 1, 1);
    }

    void Die()
    {
        Debug.Log("Player died");

        // optional:
        // disable movement
        // death animation
    }
}
