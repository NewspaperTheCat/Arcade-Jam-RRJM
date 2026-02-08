using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;

public class InputManager : MonoBehaviour
{
    public static InputManager inst = null;

    Vector2 robotMovement;
    Vector2 zeusMovement;

    public UnityEvent smite = new UnityEvent();

    // Interact button events
    public UnityEvent drop = new UnityEvent();
    public UnityEvent buildStart = new UnityEvent();
    public UnityEvent buildEnd = new UnityEvent();
    public UnityEvent escape = new UnityEvent();
    float double_tap_start_time = -10;
    const float DOUBLE_TAP_MAX = .5f;

    // Singleton
    void Awake() {
        if (inst == null) {
            inst = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Getters
    public Vector2 GetRobotMovement() { return robotMovement; }
    public Vector2 GetZeusMovement() { return zeusMovement; }

    // ================================================================
    // Input System Interactions
    public void OnRobotMove(InputAction.CallbackContext context) {
        robotMovement = context.ReadValue<Vector2>();
    }

    public void OnZeusMove(InputAction.CallbackContext context) {
        zeusMovement = context.ReadValue<Vector2>();
    }

    public void OnSmite(InputAction.CallbackContext context) {
        if (context.performed) smite.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started) {
            buildStart.Invoke();
        }

        if (context.performed) {
            if (Time.time - double_tap_start_time <= DOUBLE_TAP_MAX) {
                drop.Invoke();
                double_tap_start_time = -10;
            } else {
                double_tap_start_time = Time.time;
            }
        }

        if (context.canceled) {
            buildEnd.Invoke();
        }
    }

    
    public void OnEscape(InputAction.CallbackContext context) {
        if (context.performed) {
            escape.Invoke();
        }
    }

}