using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

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

    // Debugging
    [SerializeField] TextMeshProUGUI debugText = null;

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

    public void OnRobotMoveY(InputAction.CallbackContext context) {
        robotMovement.y = context.ReadValue<float>();
    }

    public void OnRobotMoveX(InputAction.CallbackContext context) {
        robotMovement.x = context.ReadValue<float>();
    }


    public void OnZeusMove(InputAction.CallbackContext context) {
        zeusMovement = context.ReadValue<Vector2>();
    }

    public void OnSmite(InputAction.CallbackContext context) {
        writeDebug("Smite: " + context.phase);
        if (context.phase == InputActionPhase.Performed) smite.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        writeDebug("Interact: " + context.phase);
        if (context.phase == InputActionPhase.Started) {
            buildStart.Invoke();
        }

        if (context.phase == InputActionPhase.Performed) {
            if (Time.time - double_tap_start_time <= DOUBLE_TAP_MAX) {
                drop.Invoke();
                double_tap_start_time = -10;
            } else {
                double_tap_start_time = Time.time;
            }
        }

        if (context.phase == InputActionPhase.Canceled) {
            buildEnd.Invoke();
        }
    }

    
    public void OnEscape(InputAction.CallbackContext context) {
        writeDebug("Escape: " + context.phase);
        if (context.phase == InputActionPhase.Performed) {
            escape.Invoke();
        }
    }

    // Debug Text
    private void writeDebug(string message) {
        if (debugText == null) return;

        debugText.text = message + "\n" + debugText.text;
    }

}