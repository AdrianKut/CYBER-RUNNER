using System.Collections;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    [SerializeField] private Transform armTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f; 

    [SerializeField] private GameObject impactEffect;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private Vector2 maxArmRotations;

    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioClip audioLaserShoot;
    [SerializeField] private AudioClip audioRifleShoot;

    [SerializeField] private GameObject bulletPrefab;

    private float timer;
    private string CurrentWeapon = "rifle";

    public string GetNameOfCurrentWeapon() => CurrentWeapon;
    public float GetValueOfFireRate() => fireRate;

    [SerializeField] private static int Damage;
    public static int GetCurrentValueOfDamage() => Damage;
    public void UseLaser()
    {
        Damage = 50;
        CurrentWeapon = "laser";
        fireRate = 0.7f;
    }

    public void UseRifle()
    {
        Damage = 25;
        CurrentWeapon = "rifle";
        fireRate = 0.9f;
    }

    private void Start()
    {
        UseLaser();
        UseRifle();
    }

    private void Update()
    {
        if (timer <= 0 && !GameManager.Instance.isGameOver)
        {
            if (PowerUpManager.Instance.isSuperAmmoActivated == true)
                timer = 0.15f;
            else
                timer = fireRate;

            if (CurrentWeapon == "laser")
                StartCoroutine(LaserShoot());
            else if (CurrentWeapon == "rifle")
                RifleShoot();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void RifleShoot()
    {
        shootAudioSource.PlayOneShot(audioRifleShoot);

        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(bullet, 2f);
    }

    private IEnumerator LaserShoot()
    {
        shootAudioSource.PlayOneShot(audioLaserShoot);
        var hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, 9999, collisionLayerMask);
        GameObject impactGameObject = null;
        if (hitInfo)
        {
            var enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null && enemy.GetEnemyType() == EnemyType.Monster)
            {
                enemy.TakeDamage(Damage);
                impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            }

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(0.75f);
        if (impactGameObject != null)
        {
            Destroy(impactGameObject);
        }
    }
}