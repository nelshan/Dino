using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public SpawnableObject[] objects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    public float birdHeightOffset = 1.5f; // Height offset for birds (can be adjusted)

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        foreach (SpawnableObject obj in objects)
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);

                // Check if the obstacle is a bird
                if (obj.prefab.name == "Bird")
                {
                    // Randomly decide if bird should spawn at the regular height or above it
                    float spawnY = Random.Range(0f, 1f) > 0.5f ? transform.position.y : transform.position.y + birdHeightOffset;
                    obstacle.transform.position = new Vector3(transform.position.x, spawnY, transform.position.z);
                }
                else
                {
                    // Spawn the obstacle at the regular position of the spawner
                    obstacle.transform.position = transform.position;
                }

                break; // Exit after spawning the first obstacle
            }

            spawnChance -= obj.spawnChance;
        }

        // Schedule the next spawn
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    // Draw Gizmos in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set the color to green for visualization

        // Draw a line from the spawner's position to the offset height
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * birdHeightOffset);

        // Draw a label to show the bird height offset distance
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + Vector3.up * birdHeightOffset, 0.1f); // Small sphere to mark the offset position
    }
}
