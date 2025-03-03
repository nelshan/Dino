using UnityEngine;

public class PlayerAnimatedSprite : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    public Sprite[] dieSprites;
    public Sprite[] crouchSprites;

    private SpriteRenderer spriteRenderer;
    private int frame;
    private Sprite[] currentAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAnimation(idleSprites); // Default to idle animation
    }

    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        if (currentAnimation == null || currentAnimation.Length == 0) return;

        frame++;

        if (frame >= currentAnimation.Length)
        {
            frame = 0;
        }

        spriteRenderer.sprite = currentAnimation[frame];

        Invoke(nameof(Animate), 1f / GameManager.Instance.gameSpeed);
    }

    public void SetAnimation(Sprite[] newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        currentAnimation = newAnimation;
        frame = 0;
        Animate();
    }

    public void SwitchToRunning()
    {
        SetAnimation(runningSprites);
    }

    public void SwitchToCrouching()
    {
        SetAnimation(crouchSprites);
    }

    public void SwitchToDie()
    {
        SetAnimation(dieSprites);
    }
}
