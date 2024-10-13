using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float distance;
    public float rangeRadius = 12;
    public GameObject gameOverUI;
    public string enemyTag = "Enemy"; // Tag your enemy prefabs with "Enemy"

    private Renderer playerRenderer; // Renderer for the player to change color
    private Color currentColor; // Current color of the player
    public List<Color> colors; // List of colors to cycle through
    private int colorIndex = 0; // Index to track the current color
    private bool isGameOver = false; // Flag to indicate if the game is over

    void Start()
    {
        gameOverUI.SetActive(false);

        // Get the Renderer component of the player
        playerRenderer = GetComponent<Renderer>();

        // Initialize with the first color in the list
        if (colors.Count > 0)
        {
            currentColor = colors[colorIndex];
            playerRenderer.material.color = currentColor;
        }
    }

    void Update()
    {
        // If the game is over, stop all updates
        if (isGameOver)
        {
            return; // Do not process anything further
        }

        DetectEnemies();

        // Detect mouse click to change player and bullet color
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            ChangeColor();
        }
    }

    public void DetectEnemies()
    {
        // Find all GameObjects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        // Loop through all enemies to find the closest one
        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

            // If the enemy is within range and is closer than the previous closest enemy
            if (enemyDistance < minDistance)
            {
                minDistance = enemyDistance;
                closestEnemy = enemy;
            }
        }

        // If a closest enemy was found and is within the rangeRadius
        if (closestEnemy != null && minDistance <= rangeRadius)
        {
            // Rotate to face the closest enemy
            transform.LookAt(closestEnemy.transform.position);

            // If the closest enemy is very close, trigger game over
            if (minDistance <= 1)
            {
                GameOver();
            }
        }
    }

    // Change the color of the player and bullet
    void ChangeColor()
    {
        // Cycle through the color list
        colorIndex = (colorIndex + 1) % colors.Count;
        currentColor = colors[colorIndex];

        // Change the player's color
        playerRenderer.material.color = currentColor;
    }

    // Get the current player color
    public Color GetCurrentColor()
    {
        return currentColor;
    }

    private void GameOver()
    {
        // Show the game over UI
        gameOverUI.SetActive(true);

        // Set the flag to true to stop further updates
        isGameOver = true;

        // Optionally, freeze the game
        Time.timeScale = 0f; // This pauses the entire game
    }

    private void OnDrawGizmos()
    {
        // Draw a flat circle on the X-Z plane to represent the range
        Gizmos.color = Color.white;

        // Draw the wireframe circle on the X-Z plane
        Vector3 center = transform.position;
        Vector3 forward = Vector3.forward;  // X-Z plane forward direction
        Vector3 right = Vector3.right;      // X-Z plane right direction

        // Draw circle in the X-Z plane (Range visualization)
        int segments = 360; // Number of segments for smoother circle
        float angle = 0f;
        float radius = rangeRadius;
        Vector3 prevPoint = center + forward * radius;

        for (int i = 0; i <= segments; i++)
        {
            // Compute angle and next point on the circle
            angle += 360f / segments;
            float radian = angle * Mathf.Deg2Rad;
            Vector3 nextPoint = center + (forward * Mathf.Cos(radian) + right * Mathf.Sin(radian)) * radius;

            // Draw the line between previous and next point
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
