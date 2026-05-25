using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Transform healthBar;
    public int maxHealth = 30;

    private bool dead = false;
    
    private int health;
    private NavMeshAgent agent;

    private Animator animator;
    //enemy ataka
    public int damage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float detectionRange = 10f;

    private float attackTimer;
    private Transform player;

    public GameObject hitParticles;

    public AudioClip hitSound;
    private AudioSource audioSource;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();

        health = maxHealth;
        StartCoroutine(WalkAround());

        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    IEnumerator WalkAround()
    {
        while (true)
        {
            var target = new Vector3();
            target.x = Random.Range(10, 90);
            target.y = 500;
            target.z = Random.Range(10, 90);

            if (Physics.Raycast(target, Vector3.down, out RaycastHit hit))
            {
                agent.destination = hit.point;
            }
            
            yield return new WaitForSeconds(Random.Range(10, 20));
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;

            dead = true;

            agent.isStopped = true;

            animator.SetTrigger("Death");

            Destroy(gameObject, 2f);
        }
        
        healthBar.localScale = new Vector3((float)health / maxHealth, 1, 1);

        Instantiate(hitParticles, transform.position + Vector3.up, Quaternion.identity);
        audioSource.PlayOneShot(hitSound);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    void Update()
    {
        if (dead) return;

        if (player == null) return;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (player.GetComponent<PlayerController>().IsDead)
        {
            agent.isStopped = true;

            animator.SetFloat("Speed", 0);

            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // detect player
        if (distance <= detectionRange)
        {
            // chase player
            if (distance > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                // stop and attack
                agent.isStopped = true;

                transform.LookAt(player);

                attackTimer += Time.deltaTime;

                if (attackTimer >= attackCooldown)
                {
                    animator.SetTrigger("Attack");

                    player.GetComponent<PlayerController>().TakeDamage(damage);

                    attackTimer = 0;
                }
            }
        }
    }
}
