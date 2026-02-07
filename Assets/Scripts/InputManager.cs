using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;

public class InputManager : MonoBehaviour
{
    public static InputManager inst = null;

    Vector2 robotMovement;
    Vector2 zeusMovement;

    public UnityEvent smite = new UnityEvent();
    public UnityEvent interact = new UnityEvent();

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
        if (context.started) smite.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started) interact.Invoke();
    }
}
