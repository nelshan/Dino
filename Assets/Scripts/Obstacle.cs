using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }

    // Detect collision with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver(); // Trigger game over
        }
    }
}
