using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

namespace Main {
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
        public enum AxisOption {
            Both,
            OnlyHorizontal,
            OnlyVertical
        }

        public float HandleRange {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }

        public int MovementRange = 100;
        public AxisOption axesToUse = AxisOption.Both;
        public string horizontalAxisName = "Horizontal";
        public string verticalAxisName = "Vertical";
        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        private RectTransform baseRect = null;
        private Canvas canvas;
        private Camera cam;
        private Vector3 input;

        Vector3 m_OriginalPos;
        Vector3 m_StartPos;
        bool m_UseX;
        bool m_UseY;
        CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;
        CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

        private void OnEnable() {
            CreateVirtualAxes();
        }

        private void Start() {
            HandleRange = handleRange;
            DeadZone = deadZone;
            m_OriginalPos = transform.position;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null) {
                Debug.LogError("the Joystick is not placed inside a canvas");
            }
            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;

            background.gameObject.SetActive(false);
        }

        protected void UpdateVirtualAxes(Vector3 value) {
            var delta = Vector3.zero - value;
            delta.y = -delta.y;
            delta /= MovementRange;
            if (m_UseX) {
                m_HorizontalVirtualAxis.Update(-delta.x);
            }
            if (m_UseY) {
                m_VerticalVirtualAxis.Update(delta.y);
            }
        }

        protected void CreateVirtualAxes() {
            m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

    	    if (m_UseX) {
                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            }
            if (m_UseY) {
                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
            }
        }

        public void OnDrag(PointerEventData data) {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera) {
                cam = canvas.worldCamera;
            }
            Vector2 newPos = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;

            input = (data.position - newPos) / (radius * canvas.scaleFactor);
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
            UpdateVirtualAxes(handle.anchoredPosition);
        }

        public void OnPointerUp(PointerEventData data) {
            Reset();
            UpdateVirtualAxes(Vector2.zero);
            background.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData data) {
            background.anchoredPosition = ScreenPointToAnchoredPosition(data.position);
            m_OriginalPos = background.anchoredPosition;
            m_StartPos = background.anchoredPosition;
            background.gameObject.SetActive(true);
            OnDrag(data);
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition) {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint)) {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam) {
            if (magnitude > deadZone) {
                if (magnitude > 1) {
                    input = normalised;
                }
            } else {
                input = Vector2.zero;
            }
        }

        private void OnDisable() {
            if (m_UseX) {
                m_HorizontalVirtualAxis.Remove();
            }
            if (m_UseY) {
                m_VerticalVirtualAxis.Remove();
            }
        }

        public void Reset() {
            handle.anchoredPosition = Vector2.zero;
            input = Vector2.zero;           
        }
    }
}