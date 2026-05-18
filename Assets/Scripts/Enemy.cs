using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Transform healthBar;
    public int maxHealth = 30;
    
    private int health;
    private NavMeshAgent agent;

    private Animator animator;
    //enemy ataka
    public int damage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private float attackTimer;
    private Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();

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
            //TODO: death animation
            //TODO: loot
            Destroy(gameObject);
        }
        
        healthBar.localScale = new Vector3((float)health / maxHealth, 1, 1);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            attackTimer += Time.deltaTime;

            transform.LookAt(player);

            if (attackTimer >= attackCooldown)
            {
                animator.SetTrigger("Attack");

                player.GetComponent<PlayerController>().TakeDamage(damage);

                attackTimer = 0;
            }
        }
    }
}
