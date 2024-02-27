using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float explosionDelay;
    public GameObject explosionPrefab;
    public GameObject smallCubePrefab;
    public float explosionForce;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Innits"))
        {

            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);

        Destroy(gameObject);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        for (int i = 0; i < 5; i++)
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
}
