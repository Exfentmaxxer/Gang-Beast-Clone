using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TumbleRumble.CameraSystem
{
    /// <summary>
    /// Dynamic camera system that frames all active players
    /// Automatically zooms in/out based on player positions
    /// </summary>
    public class DynamicCamera : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Camera Settings")]
        [SerializeField] private float minZoom = 10f;
        [SerializeField] private float maxZoom = 40f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float followSpeed = 3f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 15f, -10f);

        [Header("Bounds")]
        [SerializeField] private float edgePadding = 3f; // Extra space around players
        [SerializeField] private Vector2 minBounds = new Vector2(15f, 10f);

        [Header("Target Management")]
        [SerializeField] private List<Transform> targets = new List<Transform>();
        #endregion

        #region Private Fields
        private Camera _camera;
        private float _currentZoom;
        private Vector3 _velocity = Vector3.zero;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            _currentZoom = (_camera.orthographic) ? _camera.orthographicSize : Vector3.Distance(transform.position, Vector3.zero);
        }

        private void LateUpdate()
        {
            if (targets.Count == 0) return;

            UpdateCameraPosition();
            UpdateCameraZoom();
        }
        #endregion

        #region Camera Movement
        private void UpdateCameraPosition()
        {
            Vector3 centerPoint = GetCenterPoint();
            Vector3 targetPosition = centerPoint + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f / followSpeed);
        }

        private void UpdateCameraZoom()
        {
            float requiredSize = GetRequiredSize();

            if (_camera.orthographic)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, requiredSize, Time.deltaTime * zoomSpeed);
            }
            else
            {
                // For perspective cameras, adjust distance
                float targetDistance = Mathf.Clamp(requiredSize, minZoom, maxZoom);
                Vector3 direction = transform.forward;
                Vector3 centerPoint = GetCenterPoint();
                Vector3 targetPosition = centerPoint - direction * targetDistance + new Vector3(offset.x, offset.y, 0f);

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);
            }
        }
        #endregion

        #region Calculations
        private Vector3 GetCenterPoint()
        {
            if (targets.Count == 0) return Vector3.zero;
            if (targets.Count == 1) return targets[0].position;

            var bounds = new Bounds(targets[0].position, Vector3.zero);
            foreach (var target in targets)
            {
                if (target != null)
                {
                    bounds.Encapsulate(target.position);
                }
            }

            return bounds.center;
        }

        private float GetRequiredSize()
        {
            if (targets.Count == 0) return minZoom;
            if (targets.Count == 1) return minZoom;

            // Calculate bounding box
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            foreach (var target in targets)
            {
                if (target != null)
                {
                    bounds.Encapsulate(target.position);
                }
            }

            // Add padding
            float width = bounds.size.x + edgePadding;
            float height = bounds.size.z + edgePadding; // Using Z for top-down view

            // Ensure minimum bounds
            width = Mathf.Max(width, minBounds.x);
            height = Mathf.Max(height, minBounds.y);

            // Calculate required size based on aspect ratio
            float requiredWidth = width / _camera.aspect;
            float requiredHeight = height;

            float requiredSize = Mathf.Max(requiredWidth, requiredHeight) * 0.5f;

            return Mathf.Clamp(requiredSize, minZoom, maxZoom);
        }
        #endregion

        #region Target Management
        public void AddTarget(Transform target)
        {
            if (target != null && !targets.Contains(target))
            {
                targets.Add(target);
            }
        }

        public void RemoveTarget(Transform target)
        {
            if (targets.Contains(target))
            {
                targets.Remove(target);
            }
        }

        public void ClearTargets()
        {
            targets.Clear();
        }

        public void SetTargets(List<Transform> newTargets)
        {
            targets = newTargets.Where(t => t != null).ToList();
        }
        #endregion

        #region Debug
        private void OnDrawGizmosSelected()
        {
            if (targets.Count == 0) return;

            // Draw center point
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(GetCenterPoint(), 0.5f);

            // Draw bounds
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            foreach (var target in targets)
            {
                if (target != null)
                {
                    bounds.Encapsulate(target.position);
                }
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        #endregion
    }
}
