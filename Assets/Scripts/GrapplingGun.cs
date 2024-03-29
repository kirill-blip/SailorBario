﻿using System.Linq;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public bool CanShoot = true;

    [Header("Layers")] [SerializeField] private LayerMask _whatIsGrappleable;
    [SerializeField] private LayerMask _whatIsCrabLayer;
    [SerializeField] private LayerMask _whatIsTreeLayer;

    [Space(0.5f)] [Header("Rope Position")] 
    [SerializeField] private Transform _ropePosition;


    [SerializeField] private float _maxDistance = 50f;

    [Space(0.5f)] [Header("Joint settings")] 
    [SerializeField] private float _spring = 4.5f;

    [SerializeField] private float _damper = 7f;
    [SerializeField] private float _massScale = 2f;

    [Space(0.5f)] [SerializeField] private AudioClip _harpoonClip;

    private Transform _camera, _player;

    private LineRenderer _lineRenderer;
    private Vector3 _grapplePoint;

    private SpringJoint _joint;

    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main.transform;
        _player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public void StartGrapple()
    {
        RaycastHit hit;

        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsCrabLayer))
        {
            _audioManager.PlaySound(_harpoonClip);
            _grapplePoint = hit.point;

            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _lineRenderer.positionCount = 2;

            hit.transform.GetComponent<Health>().TakeDamage(10);
        }
        else if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsTreeLayer))
        {
            _audioManager.PlaySound(_harpoonClip);
            _grapplePoint = hit.point;

            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _lineRenderer.positionCount = 2;

            hit.transform.GetComponent<Tree>().DeleteFruits();
        }
        else if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsGrappleable))
        {
            _audioManager.PlaySound(_harpoonClip);
            _grapplePoint = hit.point;

            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapplePoint;

            _player.GetComponent<Rigidbody>().useGravity = false;

            var distanceFromPoint = Vector3.Distance(_player.position, _grapplePoint);
            _joint.maxDistance = distanceFromPoint * 0.8f;
            _joint.minDistance = distanceFromPoint * 0.25f;

            _joint.spring = _spring;
            _joint.damper = _damper;
            _joint.massScale = _massScale;

            _lineRenderer.positionCount = 2;
        }
    }

    public void StopGrapple()
    {
        _lineRenderer.positionCount = 0;
        var joints = _player.GetComponents<Joint>().ToList();
        joints.ForEach(Destroy);
        _player.GetComponent<Rigidbody>().useGravity = true;
    }

    private void DrawRope()
    {
        if (!_joint) return;

        _lineRenderer.SetPosition(0, _ropePosition.position);
        _lineRenderer.SetPosition(1, _grapplePoint);
    }
}