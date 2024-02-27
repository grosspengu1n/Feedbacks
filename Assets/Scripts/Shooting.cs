using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunTransform;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    public float recoilForce = 10f;
    public float recoilDuration = 0.1f;
    public float bulletSpeed = 20f;
    public float reloadTime = 2f;
    public AudioClip shootSound;

    private bool isReloading = false;
    private AudioSource audioSource;
    private Camera mainCamera;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!isReloading && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (!isReloading && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        StartCoroutine(ApplyRecoil());
        StartCoroutine(CameraShake(0.1f, 0.2f));

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletSpawnPoint.gameObject.GetComponentInParent<ParticleSystem>().Play();

        if (bulletRb != null)
        {
            bulletRb.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Impulse);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }


        Vector3 randomPosition = bulletSpawnPoint.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-4f, 4f));

  
        GameObject numberObject = new GameObject("Number");
        numberObject.transform.position = randomPosition;
        numberObject.transform.rotation =bulletSpawnPoint.rotation;
        TextMesh textMesh = numberObject.AddComponent<TextMesh>();
        textMesh.color = Color.red;
        textMesh.text = "99999";


        StartCoroutine(MoveNumberObject(numberObject.transform, Vector3.up, 1.0f));
    }

    IEnumerator MoveNumberObject(Transform transform, Vector3 direction, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction+ new Vector3(0f,5f,0f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        Destroy(transform.gameObject);
    }

    IEnumerator ApplyRecoil()
    {
        Quaternion initialRotation = gunTransform.localRotation;
        Quaternion recoilRotation = Quaternion.Euler(Vector3.back * recoilForce);

        float elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            gunTransform.localRotation = Quaternion.Slerp(initialRotation, recoilRotation, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gunTransform.localRotation = initialRotation;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        gunTransform.Rotate(Vector3.up, 360f / reloadTime * Time.deltaTime);

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}



