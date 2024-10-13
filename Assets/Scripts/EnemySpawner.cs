using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array to store the four enemy prefabs
    public Transform player;          // Reference to the player's transform
    public float spawnDistance = 10f; // Distance from the player to spawn enemies
    public Color[] enemyColors;       // Array of 4 possible colors

    private Vector3[] spawnPositions; // Array to store the North, East, South, and West positions

    void Start()
    {
        // Define the four positions relative to the player
        spawnPositions = new Vector3[]
        {
            new Vector3(0, 0, spawnDistance),    // North
            new Vector3(spawnDistance, 0, 0),    // East
            new Vector3(0, 0, -spawnDistance),   // South
            new Vector3(-spawnDistance, 0, 0)    // West
        };

        InvokeRepeating("SpawnRandomEnemy", 2f, 1.5f); // Start spawning after 2 seconds, then every 3 seconds
    }

    void SpawnRandomEnemy()
    {
        // Choose a random enemy prefab from the array
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Choose a random position from the spawn positions array
        int randomPositionIndex = Random.Range(0, spawnPositions.Length);

        // Calculate the spawn position relative to the player
        Vector3 spawnPosition = player.position + spawnPositions[randomPositionIndex];

        // Instantiate the enemy at the spawn position with no rotation
        GameObject enemy = Instantiate(enemyPrefabs[randomEnemyIndex], spawnPosition, Quaternion.identity);

        // Randomly assign one of the 4 colors to the enemy
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        Color randomColor = enemyColors[Random.Range(0, enemyColors.Length)];
        enemyRenderer.material.color = randomColor;

        // Set the enemy's color in the EnemyScript
        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
        enemyScript.SetColor(randomColor);
    }
}
