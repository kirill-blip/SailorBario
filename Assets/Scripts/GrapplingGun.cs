﻿using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Layers")] 
    [SerializeField] private LayerMask _whatIsGrappleable;
    [SerializeField] private LayerMask _whatIsCrabLayer;
    [SerializeField] private LayerMask _whatIsTreeLayer;

    [Space(0.5f)] [Header("Rope Position")] [SerializeField]
    private Transform _ropePosition;

    private Transform _camera, _player;

    private LineRenderer _lineRenderer;
    private Vector3 _grapplePoint;

    private float _maxDistance = 100f;
    private SpringJoint _joint;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main.transform;
        _player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        RaycastHit hit;

        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsGrappleable))
        {
            _grapplePoint = hit.point;

            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = true;
            _joint.connectedAnchor = _grapplePoint;

            var distanceFromPoint = Vector3.Distance(_player.position, _grapplePoint);
            _joint.maxDistance = distanceFromPoint * 0.8f;
            _joint.minDistance = distanceFromPoint * 0.25f;

            _joint.spring = 4.5f;
            _joint.damper = 7f;
            _joint.massScale = 4.5f;

            _lineRenderer.positionCount = 2;
        }
        else if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsCrabLayer))
        {
            _grapplePoint = hit.point;

            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _lineRenderer.positionCount = 2;

            hit.transform.GetComponent<Health>().TakeDamage(10);

            Invoke(nameof(StopGrapple), .25f);
        }
        else if (Physics.Raycast(_camera.position, _camera.forward, out hit, _maxDistance, _whatIsTreeLayer))
        {
            _grapplePoint = hit.point;
            
            _joint = _player.gameObject.AddComponent<SpringJoint>();
            _lineRenderer.positionCount = 2;

            hit.transform.GetComponent<Tree>().DeleteFruits();
            Invoke(nameof(StopGrapple), .25f);

        }
    }

    private void DrawRope()
    {
        if (!_joint) return;

        _lineRenderer.SetPosition(0, _ropePosition.position);
        _lineRenderer.SetPosition(1, _grapplePoint);
    }

    private void StopGrapple()
    {
        _lineRenderer.positionCount = 0;
        if (_joint is not null)
            Destroy(_joint);
    }
}