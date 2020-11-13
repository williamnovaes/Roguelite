using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Main {
    public abstract class InputComponent : MonoBehaviour {

        public enum TouchControllerButtons {
            None, Jump, Fire1, Cancel
        }

        public enum TouchControllerAxis {
            None, Horizontal, Vertical, AimHorizontal, AimVertical
        }

        [Serializable]
        public class InputButton {
            public KeyCode key;
            public TouchControllerButtons touchButton;
            public bool Down { get; protected set; }
            public bool Held { get; protected set; }
            public bool Up { get; protected set; }
            public bool Enabled { get { return m_Enabled; } }

            [SerializeField]
            protected bool m_Enabled = true;
            protected bool m_GettingInput = true;

            //This is used to change the state of a button (Down, Up) only if at least a FixedUpdate happened between the previous
            //Frame and this one. Since movement are made in FixedUpdate, without that an input could be missed it get press/release
            //between FixedUpdate
            bool m_AfterFixedUpdateDown;
            bool m_AfterFixedUpdateHeld;
            bool m_AfterFixedUpdateUp;

            protected static readonly Dictionary<int, string> k_ButtonsToName = new Dictionary<int, string> {
                {(int) TouchControllerButtons.Jump, "Jump"},
                {(int) TouchControllerButtons.Fire1, "Fire1"},
                {(int) TouchControllerButtons.Cancel, "Cancel"}
            };

            public InputButton(KeyCode key, TouchControllerButtons touchButton) {
                this.key = key;
                this.touchButton = touchButton;
            }

            public void Get(bool fixedUpdateHappened) {
                if (!m_Enabled) {
                    Down = false;
                    Held = false;
                        Up = false;
                    return;
                }

                if (!m_GettingInput) {
                    return;
                }

                if (fixedUpdateHappened) {
                    Down = CrossPlatformInputManager.GetButtonDown(k_ButtonsToName[(int)touchButton]);
                    Held = CrossPlatformInputManager.GetButton(k_ButtonsToName[(int)touchButton]);
                    Up = CrossPlatformInputManager.GetButtonUp(k_ButtonsToName[(int)touchButton]);

                    if (!Down) {
                        Down = Input.GetKeyDown(key);
                    }
                    if (!Held) {
                        Held = Input.GetKey(key);
                    }
                    if (!Up) {
                        Up = Input.GetKeyUp(key);
                    }

                    m_AfterFixedUpdateDown = Down;
                    m_AfterFixedUpdateHeld = Held;
                    m_AfterFixedUpdateUp = Up;
                } else {
                    Down = CrossPlatformInputManager.GetButtonDown(k_ButtonsToName[(int)touchButton]) || m_AfterFixedUpdateDown;
                    Held = CrossPlatformInputManager.GetButton(k_ButtonsToName[(int)touchButton]) || m_AfterFixedUpdateHeld;
                    Up = CrossPlatformInputManager.GetButtonUp(k_ButtonsToName[(int)touchButton]) || m_AfterFixedUpdateUp;

                    if (!Down) {
                        Down = Input.GetKeyDown(key) || m_AfterFixedUpdateDown;
                    }
                    if (!Held) {
                        Held = Input.GetKey(key) || m_AfterFixedUpdateHeld;
                    }
                    if (!Up) {
                        Up = Input.GetKeyUp(key) || m_AfterFixedUpdateUp;
                    }

                    m_AfterFixedUpdateDown |= Down;
                    m_AfterFixedUpdateHeld |= Held;
                    m_AfterFixedUpdateUp |= Up;
                }
            }

            public void Enable() {
                m_Enabled = true;
            }

            public void Disabled() {
                m_Enabled = false;
            }

            public void GainControl() {
                m_GettingInput = true;
            }

            public IEnumerator ReleaseControl(bool resetValues) {
                m_GettingInput = false;

                if (!resetValues) {
                    yield break;
                }

                if (Down) {
                    Up = true;
                }
                Down = false;
                Held = false;

                m_AfterFixedUpdateDown = false;
                m_AfterFixedUpdateHeld = false;
                m_AfterFixedUpdateUp = false;

                yield return null;
                Up = false;
            }

            public IEnumerator ResetValues() {
                if (Down) {
                    Up = true;
                }
                Down = false;
                Held = false;

                m_AfterFixedUpdateDown = false;
                m_AfterFixedUpdateHeld = false;
                m_AfterFixedUpdateUp = false;

                yield return null;
                Up = false;
            }
        }

        [Serializable]
        public class InputAxis {
            public KeyCode positive;
            public KeyCode negative;
            public TouchControllerAxis controllerAxis;
            public float Value { get; protected set; }
            public float RealValue { get; protected set; }
            public bool ReceivingInput { get; protected set; }
            public bool Enabled { get { return m_Enabled; } }

            protected bool m_Enabled = true;
            protected bool m_GettingInput = true;

            protected readonly static Dictionary<int, string> k_AxisToName = new Dictionary<int, string> {
                {(int) TouchControllerAxis.Horizontal, "Horizontal"},
                {(int) TouchControllerAxis.Vertical, "Vertical"},
                {(int) TouchControllerAxis.AimHorizontal, "AimHorizontal"},
                {(int) TouchControllerAxis.AimVertical, "AimVertical"},
            };

            public InputAxis(KeyCode positive, KeyCode negative, TouchControllerAxis controllerAxis) {
                this.positive = positive;
                this.negative = negative;
                this.controllerAxis = controllerAxis;
            }

            public void Get() {
                if (!m_Enabled) {
                    Value = 0f;
                    return;
                }

                if (!m_GettingInput) {
                    return;
                }

                bool positiveHeld = false;
                bool negativeHeld = false;

                float value = CrossPlatformInputManager.GetAxisRaw(k_AxisToName[(int)controllerAxis]);
                RealValue = Util.Normalize(value, Constants.JOYSTICK_AXIS_MAX_VALUE);

                positiveHeld = Input.GetKey(positive);
                negativeHeld = Input.GetKey(negative);

                if (!positiveHeld) {
                    positiveHeld = value > Single.Epsilon;
                }

                if (!negativeHeld) {
                    negativeHeld = value < -Single.Epsilon;
                }

                if (positiveHeld == negativeHeld) {
                    Value = 0f;
                } else if (positiveHeld) {
                    Value = 1f;
                } else {
                    Value = -1f;
                }

                ReceivingInput = positiveHeld || negativeHeld;
            }

            public void Enable() {
                m_Enabled = true;
            }

            public void Disable() {
                m_Enabled = false;
            }

            public void GainControl() {
                m_GettingInput = true;
            }

            public void ReleaseControl(bool resetValues) {
                m_GettingInput = false;
                if (resetValues) {
                    Value = 0f;
                    RealValue = 0f;
                    ReceivingInput = false;
                }
            }

            public void ResetValues() {
                Value = 0f;
                RealValue = 0f;
                ReceivingInput = false;
            }
        }

        bool m_FixedUpdateHappened;

        void Update() {
            GetInputs(m_FixedUpdateHappened || Mathf.Approximately(Time.timeScale, 0));

            m_FixedUpdateHappened = false;
        }

        void FixedUpdate() {
            m_FixedUpdateHappened = true;
        }

        protected abstract void GetInputs(bool fixedUpdateHappened);

        public abstract void GainControl();

        public abstract void ReleaseControl(bool resetValues = true);

        protected void GainControl(InputButton inputButton) {
            inputButton.GainControl();
        }

        protected void GainControl(InputAxis inputAxis) {
            inputAxis.GainControl();
        }

        protected void ReleaseControl(InputButton inputButton, bool resetValues) {
            StartCoroutine(inputButton.ReleaseControl(resetValues));
        }

        protected void ReleaseControl(InputAxis inputAxis, bool resetValues) {
            inputAxis.ReleaseControl(resetValues);
        }
    }
}