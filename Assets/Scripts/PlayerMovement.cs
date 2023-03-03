using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _time = 10;
    
    private Rigidbody _rigidbody;

    private float _horizontalInput;
    private float _verticalInput;

    private bool CanAttracted;
    
    public event EventHandler MovingEnded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FindObjectOfType<Harpoon>().HookHooked += OnHookHooked;
    }

    private void OnHookHooked(object sender, Vector3 hookPosition)
    {
        gameObject.LeanMove(hookPosition, _time);
        CanAttracted = true;
    }

    private void Update()
    {
        Move();

        if (!gameObject.LeanIsTweening() && CanAttracted)
        {
            MovingEnded?.Invoke(this, null);
            CanAttracted = false;
        }
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        var movement = transform.TransformDirection(new Vector3(_horizontalInput * _speed,
            _rigidbody.velocity.y, _verticalInput * _speed));

        _rigidbody.velocity = movement;
    }
}