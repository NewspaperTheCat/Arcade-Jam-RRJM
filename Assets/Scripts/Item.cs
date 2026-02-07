using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Item : MonoBehaviour
{
    // FOR RICHIE
    // Currently:
    // there is no implementation of the cargo variable
    // The player has a functioning OnInteract() binding, but it does nothing currently
    // The item is liften onto the player's head on contact, and the item can be carried
    // the item can not be placed down nor used.

    Station cargo; // The station the item is actually holding;

    const float BOB_HEIGHT = .375f;
    const float BOB_ROTATION = 45f;

    enum ItemState {
        IDLE,
        GRABBED
    }
    ItemState state = ItemState.IDLE;
    float time_alive = 0;

    Vector3 anchor; // local space

    void Start() {
        SetAnchor();
    }

    // Update is called once per frame
    void Update() {
        time_alive += Time.deltaTime;
        
        Vector3 target =  anchor + Vector3.up * (Mathf.Sin(time_alive) * .5f + 1)  * BOB_HEIGHT;
        transform.localPosition += (target - transform.localPosition) * Time.deltaTime / .75f;
        transform.Rotate(Vector3.up * BOB_ROTATION * Time.deltaTime); 
    }

    // Sets this item's anchor where it currently is
    public void SetAnchor() {
        anchor = transform.localPosition;
    }

    // handle collisions, ideally with robot
    void OnTriggerEnter(Collider other) {
        if (state == ItemState.GRABBED) return;

        if (other.transform.GetComponent<Robot>() != null) {
            Robot rb = other.transform.GetComponent<Robot>();
            rb.PickUpItem(this);
            state = ItemState.GRABBED;
        }
    }
}
