using System.Collections;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform armTransform;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private static int damage;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private LayerMask collisionLayerMask;

    [SerializeField]
    private Vector2 maxArmRotations;

    [SerializeField]
    private AudioSource shootAudioSource;

    [SerializeField]
    private AudioClip audioLaserShoot;

    [SerializeField]
    private AudioClip audioRifleShoot;

    [SerializeField]
    private GameObject bulletPrefab;

    private float timer;
    private string currentWeapon = "rifle";

    public string GetNameOfCurrentWeapon() => currentWeapon;
    public float GetValueOfFireRate() => fireRate;

    public void UseLaser()
    {
        damage = 50;
        currentWeapon = "laser";
        fireRate = 0.7f;
    }

    public void UseRifle()
    {
        damage = 25;
        currentWeapon = "rifle";
        fireRate = 0.9f;
    }

    public static int GetCurrentValueOfDamage() => damage;

    private void Start()
    {
        UseLaser();
        UseRifle();
    }

    private void Update()
    {
        //if (Input.GetButton("Fire1") && timer <= 0 && !GameManager.GameManagerInstance.isGameOver)
        //{
        //    if (PowerUpManager.SuperAmmoActivated() == true)
        //        timer = 0.15f;
        //    else
        //        timer = fireRate;

        //    if (currentWeapon == "laser")
        //        StartCoroutine(LaserShoot());
        //    else if (currentWeapon == "rifle")
        //        RifleShoot();
        //}
        //else
        //{
        //    timer -= Time.deltaTime;
        //}

        if (timer <= 0 && !GameManager.GameManagerInstance.isGameOver)
        {
            if (PowerUpManager.SuperAmmoActivated() == true)
                timer = 0.15f;
            else
                timer = fireRate;

            if (currentWeapon == "laser")
                StartCoroutine(LaserShoot());
            else if (currentWeapon == "rifle")
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

    private void FixedUpdate()
    {
        //if (!GameManager.GameManagerInstance.isGameOver)
        //{
        //    var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    var dir = pos - armTransform.position;
        //    dir.Normalize();

        //    if (transform.eulerAngles.y > 90)
        //    {
        //        dir.x *= -1;
        //    }

        //    float angle = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        //    angle = Mathf.Clamp(angle, maxArmRotations.x, maxArmRotations.y);
        //    armTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        //}
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
                if (enemy.name.Contains("MoneyBank"))
                    GameManager.GameManagerInstance.OnDestroyMoneyPig?.Invoke();

                if (enemy.name.Contains("MysteriousBox"))
                    GameManager.GameManagerInstance.OnDestroyMysteriousBox?.Invoke();



                enemy.TakeDamage(damage);
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