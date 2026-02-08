using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;
    // variables

    public float vel = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // asignamos a la variable rb el componenete rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.Translate(movement * vel * Time.deltaTime);
        // detectamos que este en el suelo y que este el espacio pulsado
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        //salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    //comprobar que toque el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
        }
    }
}
