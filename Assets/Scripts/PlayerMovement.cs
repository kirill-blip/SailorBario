using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10;

    private Rigidbody _rigidbody;

    private bool _onGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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

    public void Jump()
    {
        if (!_onGround) return;
        
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _onGround = false;
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        var movement = transform.TransformDirection(new Vector3(horizontalInput * _speed,
            _rigidbody.velocity.y, verticalInput * _speed));

        _rigidbody.velocity = movement;
    }
}