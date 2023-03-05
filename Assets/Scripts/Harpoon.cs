using System;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private Hook _hook;
    [SerializeField] private Transform _hookPosition;

    [SerializeField] private float _hookSpeed = 6f;
    [SerializeField] private float _ropeLength = 25f;

    [SerializeField] private Vector3 _localPosition;
    [SerializeField] private Vector3 _localRotation;

    private readonly float _defaultX = 0.4f;

    private bool CanReturn = true;

    private GameObject _hookClone;
    
    public event EventHandler<Vector3> HookHooked;

    private void Awake()
    {
        _localPosition = _hook.transform.localPosition;
        _localRotation = _hook.transform.localEulerAngles;
    }

    private void Start()
    {
        _hook.CollidedWithGround += OnCollidedWithGround;
        _hook.CollidedWithCrab += OnCollidedWithCrab;
        print($"Start {CanReturn}");
        FindObjectOfType<PlayerMovement>().MovingEnded += OnMovingEnded;
    }

    private void OnCollidedWithCrab(object sender, EventArgs e)
    {
        _hook.IsMoving = !_hook.IsMoving;
        _hook.gameObject.LeanMoveLocalX(_defaultX, _hookSpeed).setEase(LeanTweenType.linear);
    }

    private void OnMovingEnded(object sender, EventArgs e)
    {
        Destroy(_hookClone);
        
        _hook.gameObject.SetActive(true);
        
        _hook.transform.localPosition = _localPosition;
        _hook.transform.localEulerAngles = _localRotation;
        
        CanReturn = true;
    }

    private void OnCollidedWithGround(object sender, EventArgs e)
    {
        _hookClone = Instantiate(_hook.gameObject, _hook.transform.position, _hook.transform.rotation);
        
        _hook.gameObject.SetActive(false);

        HookHooked?.Invoke(this, _hook.transform.position);
        CanReturn = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !_hook.IsMoving)
        {
            _hook.IsMoving = true;
            _hook.gameObject.LeanMoveLocalX(_ropeLength, _hookSpeed);
            print(CanReturn);
        }

        if (CanReturn && _hook.transform.position.x != _defaultX && !_hook.gameObject.LeanIsTweening())
        {
            _hook.IsMoving = false;
            _hook.gameObject.LeanMoveLocalX(_defaultX, _hookSpeed);
        }
    }
}