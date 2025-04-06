using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; 
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    /* Jumping Mechanics */
    public float jumpForce = 5.0f;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool onGround;
    private bool jump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

    }
 
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // Called every frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            jump = true;
        }
    }

    // Called every physics timestep
    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        if(jump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            jump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

 
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);

            count = count + 1;

            SetCountText();
        }
    }

    void SetCountText() 
    {
        if (countText != null)
            countText.text = "Count: " + count.ToString();

        if (count >= 8 && winTextObject != null)
        {
            winTextObject.SetActive(true);
        }
    }

}
