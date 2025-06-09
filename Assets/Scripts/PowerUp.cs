using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Star,       
        UpgradeShot, 
        Shield,     
        Invincibility 
    }

    [SerializeField] private PowerUpType type;
    [SerializeField] private float fallSpeed = 1f; 
    [SerializeField] private int scoreValue = 200; 
    [SerializeField] private float duration = 5f; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.linearVelocity = new Vector2(0, -fallSpeed); 
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            ApplyPowerUp(player);
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (type)
        {
            case PowerUpType.Star:
                GameManager.Instance.AddScore(scoreValue);
                Debug.Log("Collected Star! +200 points.");
                break;

            case PowerUpType.UpgradeShot:
                int currentShotLevel = player.GetCurrentShotLevel();
                if (currentShotLevel < 3)
                {
                    player.UpgradeShot(); // Nâng cấp đạn
                    Debug.Log("Upgraded to Shot Level " + (currentShotLevel + 1));
                }
                else
                {
                    GameManager.Instance.AddScore(scoreValue * 2);
                    Debug.Log("Max Shot Level reached! +100 points instead.");
                }
                break;

            case PowerUpType.Shield:
                player.ActivateShield(duration);
                Debug.Log("Shield activated for 5 seconds!");
                break;

            case PowerUpType.Invincibility:
                player.ActivateInvincibility(duration);
                Debug.Log("Invincibility and faster firing activated for 5 seconds!");
                break;
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPowerUpSFX();
        }
    }

    
    public void SetPowerUpType(PowerUpType newType)
    {
        type = newType;
    }
}