using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    public GameObject PlayerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IgnorePlayerCollision()
    {
        Physics2D.IgnoreCollision(PlayerObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
