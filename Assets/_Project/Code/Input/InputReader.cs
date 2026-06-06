using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mosslight.Input
{
    public enum InputContext
    {
        Driving,
        OnFoot,
        UI,
        ActivityView
    }

    public sealed class InputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private InputContext _defaultContext = InputContext.Driving;

        private InputActionMap _drivingMap;
        private InputActionMap _onFootMap;
        private InputActionMap _uiMap;
        private InputActionMap _activityViewMap;

        private InputAction[] _steer;
        private InputAction[] _accelerate;
        private InputAction[] _brakeReverse;
        private InputAction[] _look;
        private InputAction[] _interact;
        private InputAction[] _exitEnterVehicle;
        private InputAction[] _toggleHeadlights;
        private InputAction[] _toggleWipers;
        private InputAction[] _leftIndicator;
        private InputAction[] _rightIndicator;
        private InputAction[] _toggleHazards;
        private InputAction[] _openSatchel;
        private InputAction[] _openBinder;
        private InputAction[] _radioNext;
        private InputAction[] _radioPrevious;
        private InputAction[] _radioToggle;
        private InputAction[] _pause;

        public float Steer { get; private set; }
        public float Accelerate { get; private set; }
        public float BrakeReverse { get; private set; }
        public Vector2 Look { get; private set; }
        public InputContext CurrentContext { get; private set; }

        public event Action Interact;
        public event Action ExitEnterVehicle;
        public event Action ToggleHeadlights;
        public event Action ToggleWipers;
        public event Action LeftIndicator;
        public event Action RightIndicator;
        public event Action ToggleHazards;
        public event Action OpenSatchel;
        public event Action OpenBinder;
        public event Action RadioNext;
        public event Action RadioPrevious;
        public event Action RadioToggle;
        public event Action Pause;

        private void Awake()
        {
            if (_inputActions == null)
            {
                Debug.LogWarning($"{nameof(InputReader)} needs a Unity Input Actions asset assigned in the Inspector.", this);
                return;
            }

            CacheActionMaps();
            CacheActions();
            BindActions();
        }

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                return;
            }

            SetContext(_defaultContext);
        }

        private void OnDisable()
        {
            DisableAllContexts();
            ResetState();
        }

        public void SetContext(InputContext context)
        {
            DisableAllContexts();
            CurrentContext = context;

            GetMapForContext(context)?.Enable();
        }

        private void CacheActionMaps()
        {
            // Create these action maps in the Unity Input Actions asset with these exact names.
            _drivingMap = _inputActions.FindActionMap(InputActionNames.DrivingMap, throwIfNotFound: false);
            _onFootMap = _inputActions.FindActionMap(InputActionNames.OnFootMap, throwIfNotFound: false);
            _uiMap = _inputActions.FindActionMap(InputActionNames.UIMap, throwIfNotFound: false);
            _activityViewMap = _inputActions.FindActionMap(InputActionNames.ActivityViewMap, throwIfNotFound: false);
        }

        private void CacheActions()
        {
            // Hookup expectation: every prototype action should exist in at least one relevant action map.
            // If the same action name is used in multiple maps, all matching actions are bound.
            _steer = FindActions(InputActionNames.Steer);
            _accelerate = FindActions(InputActionNames.Accelerate);
            _brakeReverse = FindActions(InputActionNames.BrakeReverse);
            _look = FindActions(InputActionNames.Look);
            _interact = FindActions(InputActionNames.Interact);
            _exitEnterVehicle = FindActions(InputActionNames.ExitEnterVehicle);
            _toggleHeadlights = FindActions(InputActionNames.ToggleHeadlights);
            _toggleWipers = FindActions(InputActionNames.ToggleWipers);
            _leftIndicator = FindActions(InputActionNames.LeftIndicator);
            _rightIndicator = FindActions(InputActionNames.RightIndicator);
            _toggleHazards = FindActions(InputActionNames.ToggleHazards);
            _openSatchel = FindActions(InputActionNames.OpenSatchel);
            _openBinder = FindActions(InputActionNames.OpenBinder);
            _radioNext = FindActions(InputActionNames.RadioNext);
            _radioPrevious = FindActions(InputActionNames.RadioPrevious);
            _radioToggle = FindActions(InputActionNames.RadioToggle);
            _pause = FindActions(InputActionNames.Pause);
        }

        private void BindActions()
        {
            BindValue(_steer, value => Steer = value);
            BindValue(_accelerate, value => Accelerate = value);
            BindValue(_brakeReverse, value => BrakeReverse = value);
            BindValue(_look, value => Look = value);

            BindButton(_interact, () => Interact?.Invoke());
            BindButton(_exitEnterVehicle, () => ExitEnterVehicle?.Invoke());
            BindButton(_toggleHeadlights, () => ToggleHeadlights?.Invoke());
            BindButton(_toggleWipers, () => ToggleWipers?.Invoke());
            BindButton(_leftIndicator, () => LeftIndicator?.Invoke());
            BindButton(_rightIndicator, () => RightIndicator?.Invoke());
            BindButton(_toggleHazards, () => ToggleHazards?.Invoke());
            BindButton(_openSatchel, () => OpenSatchel?.Invoke());
            BindButton(_openBinder, () => OpenBinder?.Invoke());
            BindButton(_radioNext, () => RadioNext?.Invoke());
            BindButton(_radioPrevious, () => RadioPrevious?.Invoke());
            BindButton(_radioToggle, () => RadioToggle?.Invoke());
            BindButton(_pause, () => Pause?.Invoke());
        }

        private InputAction[] FindActions(string actionName)
        {
            List<InputAction> actions = new();
            AddMatchingAction(actions, _drivingMap, actionName);
            AddMatchingAction(actions, _onFootMap, actionName);
            AddMatchingAction(actions, _uiMap, actionName);
            AddMatchingAction(actions, _activityViewMap, actionName);

            if (actions.Count == 0)
            {
                Debug.LogWarning($"Input action '{actionName}' was not found. Add it to the assigned Input Actions asset.", this);
            }

            return actions.ToArray();
        }

        private static void AddMatchingAction(List<InputAction> actions, InputActionMap actionMap, string actionName)
        {
            InputAction action = actionMap?.FindAction(actionName, throwIfNotFound: false);

            if (action != null)
            {
                actions.Add(action);
            }
        }

        private static void BindValue(InputAction[] actions, Action<Vector2> setValue)
        {
            foreach (InputAction action in actions)
            {
                action.performed += context => setValue(context.ReadValue<Vector2>());
                action.canceled += _ => setValue(Vector2.zero);
            }
        }

        private static void BindValue(InputAction[] actions, Action<float> setValue)
        {
            foreach (InputAction action in actions)
            {
                action.performed += context => setValue(context.ReadValue<float>());
                action.canceled += _ => setValue(0f);
            }
        }

        private static void BindButton(InputAction[] actions, Action invoke)
        {
            foreach (InputAction action in actions)
            {
                action.performed += _ => invoke();
            }
        }

        private InputActionMap GetMapForContext(InputContext context)
        {
            return context switch
            {
                InputContext.Driving => _drivingMap,
                InputContext.OnFoot => _onFootMap,
                InputContext.UI => _uiMap,
                InputContext.ActivityView => _activityViewMap,
                _ => null
            };
        }

        private void DisableAllContexts()
        {
            _drivingMap?.Disable();
            _onFootMap?.Disable();
            _uiMap?.Disable();
            _activityViewMap?.Disable();
        }

        private void ResetState()
        {
            Steer = 0f;
            Accelerate = 0f;
            BrakeReverse = 0f;
            Look = Vector2.zero;
        }
    }
}
