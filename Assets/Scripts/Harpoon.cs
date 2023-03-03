using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Harpoon : MonoBehaviour
{
    [SerializeField] private Hook _hook;

    [SerializeField] private float _hookSpeed = 6f;
    [SerializeField] private float _ropeLength = 25f;

    private readonly float _defaultX = 0.4012f;

    private bool CanReturn = true;

    private GameObject _hookClone;

    public event EventHandler<Vector3> HookHooked;

    private void Start()
    {
        _hook.CollidedWithGround += OnCollidedWithGround;

        FindObjectOfType<PlayerMovement>().MovingEnded += OnMovingEnded;
    }

    private void OnMovingEnded(object sender, EventArgs e)
    {
        Destroy(_hookClone);
        
        _hook.gameObject.SetActive(true);
        _hook.gameObject.LeanMoveLocalX(_defaultX, _hookSpeed);
        _hook.gameObject.LeanRotate(Vector3.zero, _hookSpeed);
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
        if (Input.GetButtonDown("Fire1"))
        {
            _hook.gameObject.LeanMoveLocalX(_ropeLength, _hookSpeed).setEase(LeanTweenType.linear);
        }

        if (CanReturn && _hook.transform.position.x != _defaultX && !_hook.gameObject.LeanIsTweening())
        {
            _hook.gameObject.LeanMoveLocalX(_defaultX, _hookSpeed).setEase(LeanTweenType.linear);
        }
    }
}