using UnityEngine;

namespace Scenes.Scripts.Camera
{
    public class CameraOrbitController : MonoBehaviour
    {
        private Transform _XForm_Camera;
        private Transform _XForm_Parent;

        private Vector3 _LocalRotation;
        private float _CamreraDistance = 10f;

        public float MouseSensitivity = 4;
        public float ScrollSensitivity = 2;
        public float OrbitSpeed = 10f;
        public float ScrollSpeed = 6f;

        public bool CameraDisabled = false;

        // Start is called before the first frame update
        void Start()
        {
            _XForm_Camera = transform;
            _XForm_Parent = transform.parent;
        }

        // LateUpdate is called once per frame, after Update() on every object. This is beacause the rendering has to be happening at the end.
        void LateUpdate()
        {
            CameraDisabled = !Input.GetButton("Fire2");

            if (!CameraDisabled) 
            {
                // Rotation
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                    _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                    _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f) 
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

                // Scroll Faster if we get further away.
                scrollAmount *= (_CamreraDistance * 0.3f);

                _CamreraDistance += scrollAmount * -1f;
            
                // Clamps the Camere Distance
                _CamreraDistance = Mathf.Clamp(_CamreraDistance, 1.5f, 100f);
            }

            // Actual Camare Rig Transformations
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            _XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitSpeed);

            if (_XForm_Camera.localPosition.z != _CamreraDistance* -1f) {
                _XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_XForm_Camera.localPosition.z, _CamreraDistance * -1f, Time.deltaTime * ScrollSpeed));
            }

        }
    }
}
