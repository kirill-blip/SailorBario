using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private List<MouseLook> _mouseLook;
    private GrapplingGun _grapplingGun;
    private bool _gameIsNotPaused = true;

    public event EventHandler EscapePressed;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _grapplingGun = FindObjectOfType<GrapplingGun>();

        _mouseLook = FindObjectsOfType<MouseLook>().ToList();
        
        FindObjectOfType<UI>().ReturnButtonClicked += OnReturnButtonClicked;

        ActivateCursor();
    }

    private void OnReturnButtonClicked(object sender, EventArgs e)
    {
        _gameIsNotPaused = !_gameIsNotPaused;
            
        ActivateCursor();
        StopRotating();
        ChangeTimeScale();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePressed?.Invoke(this, null);
            
            OnReturnButtonClicked(this, null);
        }

        if (!_gameIsNotPaused) return;

        _playerMovement.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
            _playerMovement.Jump();

        if (Input.GetMouseButtonDown(0))
        {
            _grapplingGun.StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _grapplingGun.StopGrapple();
        }
    }

    private void ActivateCursor()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void StopRotating()
    {
        _mouseLook.ForEach(x => x.CanRotate = !x.CanRotate);
    }

    private void ChangeTimeScale()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public void DisableMovingAndShooting()
    {
        _gameIsNotPaused = true;
        StopRotating();
    }
}