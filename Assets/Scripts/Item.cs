using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // FOR RICHIE
    // Currently:
    // there is no implementation of the cargo variable
    // The player has a functioning OnInteract() binding, but it does nothing currently
    // The item is liften onto the player's head on contact, and the item can be carried
    // the item can not be placed down nor used.

    public StationType cargo; // The station the item is actually holding;

    // Drop Parameters
    const float DROP_DISTANCE = 2f;
    const float DROP_TIME = .5f;
    float time_dropped;

    // Passive animation
    const float BOB_HEIGHT = .375f;
    const float BOB_ROTATION = 45f;

    enum ItemState {
        DROPPING,
        IDLE,
        GRABBED
    }
    ItemState state = ItemState.IDLE;
    float time_alive = 0;

    Vector3 anchor; // local space

    void Start() {
        Drop();
    }

    public void Drop() {
        transform.SetParent(WorldManager.Instance.transform);
        state = ItemState.DROPPING;
        time_dropped = Time.time;

        Vector2 dir = Random.insideUnitCircle * DROP_DISTANCE;
        SetAnchor(new Vector3(dir.x + transform.localPosition.x, 0, dir.y + transform.localPosition.z));
    }

    // Update is called once per frame
    void Update() {
        time_alive += Time.deltaTime;
        
        Vector3 target =  anchor + Vector3.up * (Mathf.Sin(time_alive) * .5f + 1)  * BOB_HEIGHT;
        transform.localPosition += (target - transform.localPosition) * Time.deltaTime / .25f;
        transform.Rotate(Vector3.up * BOB_ROTATION * Time.deltaTime);

        if (state == ItemState.DROPPING && Time.time - time_dropped > DROP_TIME) {
            state = ItemState.IDLE;
        }
    }

    // Sets this item's anchor where it currently is
    public void SetAnchor() {
        anchor = transform.localPosition;
    }
    public void SetAnchor(Vector3 setPos) {
        anchor = setPos;
    }

    // handle collisions, ideally with robot
    void OnTriggerEnter(Collider other) {
        if (state != ItemState.IDLE) return;

        if (other.transform.GetComponent<Robot>() != null) {
            Robot rb = other.transform.GetComponent<Robot>();
            if (rb.PickUpItem(this))
                state = ItemState.GRABBED;
        }
    }
}
