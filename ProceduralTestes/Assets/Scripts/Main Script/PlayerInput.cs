using UnityEngine;
using UnityEngine.Events;

namespace Main {
    public class PlayerInput : InputComponent {
        public static PlayerInput Instance { get { return s_Instance; } }

        protected static PlayerInput s_Instance;

        public bool HaveControl { get { return m_HaveControl; } }
        protected bool m_HaveControl = true;

        public InputButton Pause = new InputButton(KeyCode.Escape, TouchControllerButtons.Cancel);
        public InputButton Shoot = new InputButton(KeyCode.K, TouchControllerButtons.Fire1);
        public InputButton Jump = new InputButton(KeyCode.Space, TouchControllerButtons.Jump);
        public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A, TouchControllerAxis.Horizontal);
        public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S, TouchControllerAxis.Vertical);
        public InputAxis AimHorizontal = new InputAxis(KeyCode.RightArrow, KeyCode.LeftArrow, TouchControllerAxis.AimHorizontal);
        public InputAxis AimVertical = new InputAxis(KeyCode.UpArrow, KeyCode.DownArrow, TouchControllerAxis.AimVertical);
        public UnityEvent OnReleaseInputsExecute;
        public UnityEvent OnGainControlInputExecute;
        protected bool m_DebugMenuIsOpen = false;

        void Awake() {
            if (s_Instance == null) {
                s_Instance = this;
            } else {
                throw new UnityException("There cannot be more than one PlayerInput script");
            }
        }

        void OnEnable() {
            if (s_Instance == null) {
                s_Instance = this;
            } else if (s_Instance != this) {
                throw new UnityException("There cannot be more than one PlayerInput script");
            }
        }

        void OnDisable() {
            s_Instance = null;
        }

        protected override void GetInputs(bool fixedUpdateHappened) {
            Pause.Get(fixedUpdateHappened);
            Shoot.Get(fixedUpdateHappened);
            Jump.Get(fixedUpdateHappened);
            Horizontal.Get();
            Vertical.Get();
            AimHorizontal.Get();
            AimVertical.Get();
        }

        public override void GainControl() {
            m_HaveControl = true;

            GainControl(Pause);
            GainControl(Shoot);
            GainControl(Jump);
            GainControl(Horizontal);
            GainControl(Vertical);
            GainControl(AimHorizontal);
            GainControl(AimVertical);

            OnGainControlInputExecute?.Invoke();
        }

        public override void ReleaseControl(bool resetValues = true) {
            m_HaveControl = false;

            ReleaseControl(Pause, resetValues);
            ReleaseControl(Shoot, resetValues);
            ReleaseControl(Jump, resetValues);
            ReleaseControl(Horizontal, resetValues);
            ReleaseControl(Vertical, resetValues);
            ReleaseControl(AimHorizontal, resetValues);
            ReleaseControl(AimVertical, resetValues);
            OnReleaseInputsExecute?.Invoke();
        }
    }
}