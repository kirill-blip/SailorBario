using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private RotationAxes _rotationAxes;
    [SerializeField] private float _sencitivity = 9.0f;
    [SerializeField] private float _angle = 45f;

    public bool CanRotate = true;
    private float _rotationX = 0;

    private void Update()
    {
        if (!CanRotate) return;

        switch (_rotationAxes)
        {
            case RotationAxes.MouseX:
                transform.Rotate(0, Input.GetAxis("Mouse X") * _sencitivity, 0);
                break;
            case RotationAxes.MouseY:
                _rotationX -= Input.GetAxis("Mouse Y") * _sencitivity;
                _rotationX = Mathf.Clamp(_rotationX, -_angle, _angle);

                var rotationY = transform.localEulerAngles.y;

                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
                break;
        }
    }
}