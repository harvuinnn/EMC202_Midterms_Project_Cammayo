using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet prefab to spawn
    public Transform firePoint;      // The point from which the bullet will be fired
    public float bulletSpeed = 20f;  // The speed at which the bullet will move
    public float fireRate = 1f;      // Time between bullet spawns
    public float bulletLifetime = 0.5f; // Time before the bullet gets destroyed

    private float nextFireTime = 0f; // Time until the next bullet can be fired
    private PlayerScript playerScript;

    void Start()
    {
        // Get the PlayerScript component to retrieve the player's current color
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    void Update()
    {
        // Fire a bullet if enough time has passed
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireBullet()
    {
        // Instantiate the bullet at the fire point with the fire point's rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody component of the bullet to apply movement
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        // Set the bullet's color to match the player's current color
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        bulletRenderer.material.color = playerScript.GetCurrentColor();

        // Destroy the bullet after 'bulletLifetime' seconds
        Destroy(bullet, bulletLifetime);
    }
}
