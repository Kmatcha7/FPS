using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public AudioClip gunSound;
    public GameObject explosion;
    public GameObject sparks;
    public Transform magazine;
    public Transform muzzlePoint;
    public GameObject tracerPrefab;
    public float readyThreshold = 0.05f;
    public float tracerTime = 0.05f;
    public float fireRate = 10f;
    public float sprintAnimationSpeed = 8f;
    public float aimAnimationSpeed = 12f;
    public float reloadAnimationSpeed = 8f;

    public int magazineSize = 30;

    public float baseSpread = 30f;
    public float moveSpread = 30f;
    public float sprintSpread = 70f;
    public float airSpread = 100f;
    public float adsSpreadMultiplier = 0.2f;

    AudioSource audioSource;
    RaycastHit hit;
    CharacterController characterController;

    Vector3 normalPosition;
    Quaternion normalRotation;

    Vector3 sprintPosition = new Vector3(0.04f, -0.78f, 1.05f);
    Quaternion sprintRotation = Quaternion.Euler(10f, -90f, 14f);

    Vector3 aimPosition = new Vector3(-0.07f, -0.54f, 0.75f);
    Quaternion aimRotation = Quaternion.Euler(0f, 0f, 0f);

    Vector3 reloadPosition = new Vector3(0.162f, -0.1f, 0.85f);
    Quaternion reloadRotation = Quaternion.Euler(-23.62f, -69.02f, -6.207f);

    Vector3 normalMagPosition = new Vector3(0.046f, 0.061f, -0.053f);
    Vector3 reloadMagPosition = new Vector3(0.203f, -0.498f, 0.041f);

    int currentAmmo;
    float nextFireTime;
    bool isReloading;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.35f;

        characterController = GetComponentInParent<CharacterController>();

        normalPosition = transform.localPosition;
        normalRotation = transform.localRotation;

        currentAmmo = magazineSize;

        if (magazine != null)
        {
            magazine.localPosition = normalMagPosition;
        }
    }

    void Update()
    {
        bool isSprinting = IsSprinting();
        bool isAiming = Input.GetMouseButton(1) && !isReloading;
        bool isFiring = Input.GetMouseButton(0) && !isReloading;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }

        bool gunReady;

        if (isAiming)
        {
            gunReady = Vector3.Distance(transform.localPosition, aimPosition) < readyThreshold;
        }
        else
        {
            gunReady = Vector3.Distance(transform.localPosition, normalPosition) < readyThreshold;
        }

        if (isFiring && gunReady && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + 1f / fireRate;
            currentAmmo--;

            audioSource.PlayOneShot(gunSound);
            Shot();

            if (explosion != null)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
        }
        if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (isReloading)
        {
            MoveGun(reloadPosition, reloadRotation, reloadAnimationSpeed);
        }
        else if (isAiming)
        {
            MoveGun(aimPosition, aimRotation, aimAnimationSpeed);
        }
        else if (isFiring)
        {
            MoveGun(normalPosition, normalRotation, sprintAnimationSpeed);
        }
        else if (isSprinting)
        {
            MoveGun(sprintPosition, sprintRotation, sprintAnimationSpeed);
        }
        else
        {
            MoveGun(normalPosition, normalRotation, sprintAnimationSpeed);
        }
    }

    bool IsMoving()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;
        return horizontalVelocity.magnitude > 0.1f;
    }

    bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) &&
               IsMoving() &&
               characterController.isGrounded &&
               !Input.GetMouseButton(1) &&
               !isReloading;
    }

    void MoveGun(Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * speed
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );
    }

    void Shot()
    {
        float spread = CalculateSpread();
        float randomX = Random.Range(-spread, spread);
        float randomY = Random.Range(-spread, spread);

        Vector3 center = new Vector3(
            Screen.width / 2 + randomX,
            Screen.height / 2 + randomY,
            0
        );

        Ray ray = Camera.main.ScreenPointToRay(center);
        float distance = 100f;
        Vector3 endPoint = ray.GetPoint(distance);

        if (Physics.Raycast(ray, out hit, distance))
        {
            endPoint = hit.point;

            if (sparks != null)
            {
                Instantiate(sparks, hit.point, Quaternion.identity);
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.SendMessage("Damage");
            }
        }

        if (tracerPrefab != null && muzzlePoint != null)
        {
            GameObject tracer = Instantiate(tracerPrefab);
            TracerScript tracerScript = tracer.GetComponent<TracerScript>();

            if (tracerScript != null)
            {
                tracerScript.Show(muzzlePoint.position, endPoint, tracerTime);
            }
        }
    }

    public float CalculateSpread()
    {
        float spread = baseSpread;

        bool isMoving = IsMoving();
        bool isGrounded = characterController.isGrounded;
        bool isSprinting = IsSprinting();
        bool isAiming = Input.GetMouseButton(1) && !isReloading;

        if (isMoving)
        {
            spread += moveSpread;
        }

        if (isSprinting)
        {
            spread += sprintSpread;
        }

        if (!isGrounded)
        {
            spread += airSpread;
        }

        if (isAiming)
        {
            spread *= adsSpreadMultiplier;
        }

        return spread;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(0.3f);
        yield return MoveMagazine(normalMagPosition, reloadMagPosition, 0.15f);
        yield return new WaitForSeconds(0.4f);
        yield return MoveMagazine(reloadMagPosition, normalMagPosition, 0.5f);

        currentAmmo = magazineSize;

        yield return new WaitForSeconds(0.3f);

        isReloading = false;
    }

    IEnumerator MoveMagazine(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            magazine.localPosition = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );

            yield return null;
        }

        magazine.localPosition = endPosition;
    }
}