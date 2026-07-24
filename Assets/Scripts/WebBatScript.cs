using UnityEngine;

public class WebBatScript : MonoBehaviour
{
    private GameObject PlayerObject;
    [SerializeField] GameObject SpiderObject;
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] float moveSpeed;
    private int enemyCollisionLayer;
    private float directionX = 1;
    private float Timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerObject = GameObject.Find("CleaningRook");
        enemyCollisionLayer = LayerMask.NameToLayer("Enemy");
    }
    

    // Update is called once per frame
    void Update()
    {
        IgnoreEntityCollisions();
        DropSpiders();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void IgnoreEntityCollisions()
    {
        Physics2D.IgnoreCollision(PlayerObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        Physics2D.IgnoreLayerCollision(enemyCollisionLayer, enemyCollisionLayer, true);
        // Physics2D.IgnoreLayerCollision(enemyCollisionLayer, LayerMask.NameToLayer("IgnoreCollider"), true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (directionX < 0)
            {
                directionX = 1;
            }
            else
            {
                directionX = -1;
            }
        }
    }

    private void Movement()
    {
        myRigidbody.linearVelocity = new Vector2(directionX * moveSpeed, myRigidbody.linearVelocityY);
    }

    void DropSpiders()
    {
        Timer += Time.deltaTime;

        if (Timer >= 1)
        {
            Instantiate(SpiderObject, new Vector2(gameObject.transform.position.x, SpiderObject.transform.position.y), transform.rotation);
            Timer = 0;
        }
    }

}
