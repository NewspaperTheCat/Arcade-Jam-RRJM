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
    [SerializeField] float death_time;
    [SerializeField] float death_duration;

    Vector3 anchor; // local space

    void Start() {
        Drop();
    }

    public void Drop() {
        transform.SetParent(WorldManager.Instance.transform);
        state = ItemState.DROPPING;
        time_alive = 0;

        transform.localScale = Vector3.one;
        GetComponent<Collider>().enabled = false;

        Vector2 dir = Random.insideUnitCircle * DROP_DISTANCE;
        SetAnchor(new Vector3(dir.x + transform.localPosition.x, 0, dir.y + transform.localPosition.z));
    }

    // Update is called once per frame
    void Update() {
        if (state != ItemState.GRABBED) time_alive += Time.deltaTime;
        else transform.localScale = Vector3.one;
        
        Vector3 target =  anchor + Vector3.up * (Mathf.Sin(time_alive) * .5f + 1)  * BOB_HEIGHT;
        transform.localPosition += (target - transform.localPosition) * Time.deltaTime / .25f;
        transform.Rotate(Vector3.up * BOB_ROTATION * Time.deltaTime);

        if (state == ItemState.DROPPING && time_alive > DROP_TIME) {
            GetComponent<Collider>().enabled = true;
            state = ItemState.IDLE;
        }

        if (time_alive > death_time) {
            float t = (time_alive - death_time) / death_duration;
            t = t * t;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            if (t >= 1) {
                Destroy(gameObject);
            }
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
