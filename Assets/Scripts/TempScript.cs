using NUnit.Framework;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public GameObject PlayerObject;
    private float playerPosX;
    private float playerPosY;
    [SerializeField] float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerTracking();
    }

    void PlayerTracking()
    {
        playerPosX = PlayerObject.transform.position.x;
        playerPosY = PlayerObject.transform.position.y;

        //transform.position = transform.position + Vector3.left * moveSpeed * Time.deltaTime;

        while (gameObject.transform.position != PlayerObject.transform.position)
        {
            Debug.Log("Running");
        }
    }
}
