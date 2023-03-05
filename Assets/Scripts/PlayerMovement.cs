using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _radius;

    private Rigidbody _rigidbody;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _onGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        var movement = transform.TransformDirection(new Vector3(_horizontalInput * _speed,
            _rigidbody.velocity.y, _verticalInput * _speed));

        _rigidbody.velocity = movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _onGround = false;
        }
    }
}