using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform playerPos;
    public float distance;
    public float rangeRadius = 24;
    public float moveSpeed = 5f;
    private Color enemyColor; // Store the enemy's current color

    void Start()
    {
        // Automatically find the player GameObject by tag and assign its Transform
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (playerPos != null)
        {
            DistanceCalculator();
            MoveToPlayer();
        }
    }

    public void DistanceCalculator()
    {
        // Calculate the distance between the enemy and the player
        distance = Vector3.Distance(transform.position, playerPos.position);

        // If the player is within range, the enemy will look at the player
        if (distance <= rangeRadius)
        {
            transform.LookAt(playerPos.position);
        }
    }

    public void MoveToPlayer()
    {
        if (playerPos != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
        }
    }

    // Set the color of the enemy
    public void SetColor(Color color)
    {
        enemyColor = color;
        // Ensure the enemy's material color is updated as well
        GetComponent<Renderer>().material.color = color;
    }

    // Detect collision with a bullet
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with is tagged as "Bullet"
        if (other.CompareTag("Bullet"))
        {
            // Get the color of the bullet
            Renderer bulletRenderer = other.GetComponent<Renderer>();
            Color bulletColor = bulletRenderer.material.color;

            // Check if the bullet's color matches the enemy's color
            if (AreColorsSimilar(bulletColor, enemyColor))
            {
                Destroy(gameObject);  // Destroy the enemy if the colors match
            }

            // Destroy the bullet regardless
            Destroy(other.gameObject);  // Destroy the bullet whether it matches or not
        }
    }

    // Function to compare colors with a tolerance
    private bool AreColorsSimilar(Color color1, Color color2, float tolerance = 0.01f)
    {
        return Mathf.Abs(color1.r - color2.r) < tolerance &&
               Mathf.Abs(color1.g - color2.g) < tolerance &&
               Mathf.Abs(color1.b - color2.b) < tolerance;
    }
}
