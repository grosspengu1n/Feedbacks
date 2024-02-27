using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject smallCubePrefab;
    public GameObject explosionPrefab;
    public float explosionDuration = 3f;
    public float explosionForce = 10f;
    private Transform player;
    public float speed = 4f;
    public float attackSpeed = 6f;
    public int damage = 20;
    bool touchPlayer;
    private bool canAttack = true;
    private float lastAttackTime;
    Rigidbody rb;
    public int currentHealth;

    private void Start()
    {
        currentHealth = 100;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (touchPlayer)
        {
            if (touchPlayer && canAttack)
            {
                DealDamage();
                lastAttackTime = Time.time;
                canAttack = false;
            }
        }
        else if (!touchPlayer && !canAttack)
        {
            canAttack = true;
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.MovePosition(pos);
        }
        transform.LookAt(player);
    }
    private void Update()
    {
        if (!canAttack && Time.time - lastAttackTime > attackSpeed)
        {
            canAttack = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchPlayer = true;
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Bullet"))
        {

            StartCoroutine(Explode());
        }
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDuration);

        Destroy(gameObject);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        for (int i = 0; i < 30; i++)
        {
            GameObject smallCube = Instantiate(smallCubePrefab, transform.position, Quaternion.identity);
            Rigidbody cubeRb = smallCube.GetComponent<Rigidbody>();

            if (cubeRb != null)
            {
                // Apply force in random directions
                cubeRb.AddForce(Random.onUnitSphere * explosionForce, ForceMode.Impulse);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touchPlayer = false;
        }
    }

    void DealDamage()
    {
    }
    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
