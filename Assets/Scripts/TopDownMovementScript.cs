using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovementScript : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    PlayerInput Input;
    private Vector2 Movement;
    private Rigidbody2D myRigidbody;

    // Awake() is basically a "private" initializer for all GameObjects and Start() is a "public" initializer
    // Example: Awake() is used to Instantiate variables/objects called within the GameObject itself, whereas Start() is used to reference variables/objects from other GameObjects
    // Lastly, Start() is always called once EVERY Awake() function in all GameObjects has run in Unity

    private void Awake() // Is called in once when the GameObject is created in the Scene
    { 
        Input = new PlayerInput(); // Instantiating a new object
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate() works differently than Update(). Update() loops itself every new frame in Unity, whereas FixedUpdate() ideally (not consistently) runs a loop every 1/50th of a second depending on FPS
    // Typically FixedUpdate is only used for physics simulations involving RigidBodies as well as any other physics simulation calculation that are relevant
    // The reason for this is bacause Unity's physics engine runs in a FixedUpdate() loop, so to match with Unity's timing for simulating physics, u would code ur physics simulations in FixedUpdate() as well
    private void FixedUpdate() 
    {
        myRigidbody.linearVelocity = Movement * movementSpeed;
    }

    private void OnEnable() // Function becomes active when Player is called in and active in Scene
    {
        Input.Enable();

        Input.Gameplay.TopDownMovement.performed += PlayerMovement;
        Input.Gameplay.TopDownMovement.canceled += PlayerMovement;
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    private void PlayerMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }
}
