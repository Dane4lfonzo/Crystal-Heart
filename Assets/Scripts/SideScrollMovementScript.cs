using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollMovementScript : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpHeight;
    PlayerInput Input;
    private Vector2 Movement;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer characSprite;
    private int jumpCount = 0;

    // Awake() is basically a "private" initializer for all GameObjects and Start() is a "public" initializer
    // Example: Awake() is used to Instantiate variables/objects called within the GameObject itself, whereas Start() is used to reference variables/objects from other GameObjects
    // Lastly, Start() is always called once EVERY Awake() function in all GameObjects has run in Unity

    private void Awake() // Is called in once when the GameObject is created in the Scene
    { 
        Input = new PlayerInput(); // Instantiating a new object
        myRigidbody = GetComponent<Rigidbody2D>();
        characSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.Gameplay.PlayerJump.triggered)
        {
            PlayerJump();
        } 

        if (myRigidbody.linearVelocityX > 0)
        {
            characSprite.flipX = false;
        }       
        else if (myRigidbody.linearVelocityX < 0)
        {
            characSprite.flipX = true;
        }
    }

    // FixedUpdate() works differently than Update(). Update() loops itself every new frame in Unity, whereas FixedUpdate() ideally (not consistently) runs a loop every 1/50th of a second depending on FPS
    // Typically FixedUpdate is only used for physics simulations involving RigidBodies as well as any other physics simulation calculation that are relevant
    // The reason for this is bacause Unity's physics engine runs in a FixedUpdate() loop, so to match with Unity's timing for simulating physics, u would code ur physics simulations in FixedUpdate() as well
    private void FixedUpdate() 
    {
        float currentYVelocity = myRigidbody.linearVelocity.y;

        //myRigidbody.linearVelocity = Movement * movementSpeed;
        myRigidbody.linearVelocity = new Vector2(Movement.x * movementSpeed, currentYVelocity);
    }

    private void OnEnable() // Function becomes active when Player is called in and active in Scene
    {
        Input.Enable();

        Input.Gameplay.SideScrollMovement.performed += PlayerMovement;
        Input.Gameplay.SideScrollMovement.canceled += PlayerMovement;
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    private void PlayerMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    private void PlayerJump()
    {
        if (jumpCount < 3)
        {
            myRigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            jumpCount += 1;
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            jumpCount = 0;
        }
    }

}