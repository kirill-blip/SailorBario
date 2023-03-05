using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _time = 10;

    private Rigidbody _rigidbody;

    private float _horizontalInput;
    private float _verticalInput;

    private bool CanAttracted = false;

    private Health _health;

    public event EventHandler MovingEnded;
    public event EventHandler<int> HealthChanged;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _health = GetComponent<Health>();
        _health.HealthChanged += OnHealthChanged;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var trees = FindObjectsOfType<Tree>().ToList();
        foreach (var item in trees)
        {
            item.FruitCollected += OnFruitCollected;
        }
    }

    private void OnFruitCollected(object sender, int e)
    {
        _health.Hill(e);
    }

    private void OnHealthChanged(object sender, int e)
    {
        HealthChanged?.Invoke(sender, e);
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
            gameObject.LeanCancel();
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