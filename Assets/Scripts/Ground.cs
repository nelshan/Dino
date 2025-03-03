using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Ground : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // Check if the game has started in GameManager
        if (GameManager.Instance.gameStarted)
        {
            // Move the ground when the game has started
            float speed = GameManager.Instance.gameSpeed / transform.localScale.x;
            meshRenderer.material.mainTextureOffset += speed * Time.deltaTime * Vector2.right;
        }
    }

}
