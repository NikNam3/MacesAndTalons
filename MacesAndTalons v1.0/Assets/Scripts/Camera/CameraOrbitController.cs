using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Scripts.Camera
{
    public class CameraOrbitController : MonoBehaviour
    {
        private Transform _xFormCamera;
        private Transform _xFormParent;

        private Vector3 _localRotation;
        private float _camreraDistance = 10f;

        [FormerlySerializedAs("MouseSensitivity")] public float mouseSensitivity = 4;
        [FormerlySerializedAs("ScrollSensitivity")] public float scrollSensitivity = 2;
        [FormerlySerializedAs("OrbitSpeed")] public float orbitSpeed = 10f;
        [FormerlySerializedAs("ScrollSpeed")] public float scrollSpeed = 6f;

        [FormerlySerializedAs("CameraDisabled")] public bool cameraDisabled = false;

        // Start is called before the first frame update
        void Start()
        {
            _xFormCamera = transform;
            _xFormParent = transform.parent;
        }

        // LateUpdate is called once per frame, after Update() on every object. This is beacause the rendering has to be happening at the end.
        void LateUpdate()
        {
            cameraDisabled = !Input.GetButton("Fire2");

            if (!cameraDisabled) 
            {
                // Rotation
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    _localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                    _localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                    _localRotation.y = Mathf.Clamp(_localRotation.y, 0f, 90f);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f) 
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                // Scroll Faster if we get further away.
                scrollAmount *= (_camreraDistance * 0.3f);

                _camreraDistance += scrollAmount * -1f;
            
                // Clamps the Camere Distance
                _camreraDistance = Mathf.Clamp(_camreraDistance, 1.5f, 100f);
            }

            // Actual Camare Rig Transformations
            Quaternion qt = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
            _xFormParent.rotation = Quaternion.Lerp(this._xFormParent.rotation, qt, Time.deltaTime * orbitSpeed);

            if (_xFormCamera.localPosition.z != _camreraDistance* -1f) {
                _xFormCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_xFormCamera.localPosition.z, _camreraDistance * -1f, Time.deltaTime * scrollSpeed));
            }

        }
    }
}
