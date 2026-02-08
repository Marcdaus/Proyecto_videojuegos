using UnityEngine;

public class Personaje : MonoBehaviour
{
    // Movimiento
    public float vel = 5f;
    public float rotationSpeed = 10f;

    // Salto
    public float jumpForce = 7f;
    private bool isGrounded;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Leer input (WASD)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        movement = new Vector3(h, 0f, v).normalized;

        // Rotar hacia donde camina
        if (movement.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Mover con físicas
        rb.MovePosition(rb.position + movement * vel * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
        }
    }
}
