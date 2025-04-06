using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private int count;

    private float movementX;
    private float movementY;

    private bool isGrounded;
    public bool jumpCharge;

    public float speed = 0;
    public float jumpForce = 5f;
    public float dblJumpForce;
    private float yVel;

    public LayerMask groundLayer;
    public float raycastDistance = 0.6f;
    public float raycastEdgeDistance = 0.3f;


    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        winTextObject.SetActive(false);
        count = 0;
        SetCountText();
        jumpCharge = true;
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        rb.AddForce(movement * speed);


        Vector3 northSide = new Vector3(transform.position.x, transform.position.y, (transform.position.z + 0.5f));
        Vector3 southSide = new Vector3(transform.position.x, transform.position.y, (transform.position.z - 0.5f));
        Vector3 westSide = new Vector3((transform.position.x - 0.5f), transform.position.y, transform.position.z);
        Vector3 eastSide = new Vector3((transform.position.x + 0.5f), transform.position.y, transform.position.z);

        RaycastHit hit;
        if ((Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer)) ||
                (Physics.Raycast(northSide, Vector3.down, out hit, raycastEdgeDistance, groundLayer)) ||
                (Physics.Raycast(southSide, Vector3.down, out hit, raycastEdgeDistance, groundLayer)) ||
                (Physics.Raycast(westSide, Vector3.down, out hit, raycastEdgeDistance, groundLayer)) ||
                (Physics.Raycast(eastSide, Vector3.down, out hit, raycastEdgeDistance, groundLayer)))
        {
            isGrounded = true;
            jumpCharge = true;
        } else
        {
            isGrounded = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Activates on Space
    void OnJump(InputValue jumpValue) 
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        } else if (jumpCharge)
        {
            jumpCharge = false;
            yVel = rb.linearVelocity.y;
            dblJumpForce = jumpForce/2;
            if (yVel < 0)
            {
                dblJumpForce = (-1 * yVel) + jumpForce;
            }
            Vector3 dblJumpForceVector = new Vector3(movementX, dblJumpForce, movementY);
            rb.AddForce(dblJumpForceVector, ForceMode.Impulse);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 4)
        {
            winTextObject.SetActive(true);
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
}
