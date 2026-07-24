using UnityEngine;

public class DustBunnyScript : MonoBehaviour
{
    private GameObject PlayerObject;
    [SerializeField] float moveSpeed; 
    [SerializeField] Rigidbody2D myRigidbody; 
    [SerializeField] float jumpHeight;
    private int enemyCollisionLayer;
    private float health = 10;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerObject = GameObject.Find("CleaningRook");
        enemyCollisionLayer = LayerMask.NameToLayer("Enemy");
        IgnoreEntityCollisions();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        PlayerTracking();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            BunnyJump();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            health =- 2;
            Debug.Log("HIT");
        }
    }

    void IgnoreEntityCollisions()
    {
        Physics2D.IgnoreCollision(PlayerObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreLayerCollision(enemyCollisionLayer, enemyCollisionLayer, true);
        // Physics2D.IgnoreLayerCollision(enemyCollisionLayer, LayerMask.NameToLayer("IgnoreCollider"), true);
    }

    public void PlayerTracking()
    {
        float directionX = 0f;

        if (PlayerObject.transform.position.x > transform.position.x)
        {
            directionX = 1f;
        }
        else
        {
            directionX = -1f;
        }

        // Vector2 pos = Vector2.MoveTowards(transform.position, new Vector2(PlayerObject.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
        // myRigidbody.MovePosition(pos);

        myRigidbody.linearVelocity = new Vector2(directionX * moveSpeed, myRigidbody.linearVelocity.y);
    }

    private void BunnyJump()
    {
        Vector2 jumpDirection = (Vector2.up * jumpHeight).normalized;
        myRigidbody.AddForce(jumpDirection * 5, ForceMode2D.Impulse);
    }
}
