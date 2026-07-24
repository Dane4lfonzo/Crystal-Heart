using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private GameObject AttackRangeRight;
    [SerializeField] private GameObject AttackRangeLeft;
    private bool LookAtLeft, LookAtRight;
    private bool attacking = true;
    private float timer;
    [SerializeField] private Rigidbody2D PlayerRigidbody;
    [SerializeField] float cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            attacking = true;
        }

        if (PlayerRigidbody.linearVelocityX < 0)
        {
            LookAtLeft = true;
            LookAtRight = false;
        }
        else if (PlayerRigidbody.linearVelocityX > 0)
        {
            LookAtLeft = false;
            LookAtRight = true;
        }

        if (attacking)
        {
            if (LookAtLeft && timer < 0.5)
            {
                AttackRangeLeft.SetActive(true);
                AttackRangeRight.SetActive(false);
                Debug.Log("Left");
            }
            else if (LookAtRight && timer < 0.5)
            {
                AttackRangeLeft.SetActive(false);
                AttackRangeRight.SetActive(true);
                Debug.Log("Right");
            }
            else
            {
                AttackRangeLeft.SetActive(false);
                AttackRangeRight.SetActive(false);
            }

            timer += Time.deltaTime;

            if (timer >= cooldown)
            {
                timer = 0;
                attacking = false;
            }
        }
    }
    
}
