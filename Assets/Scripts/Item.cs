using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Item : MonoBehaviour
{
    const float BOB_HEIGHT = .375f;
    const float BOB_ROTATION = 45f;

    enum ItemState {
        IDLE,
        GRABBED
    }
    ItemState state = ItemState.IDLE;
    float time_alive = 0;

    // Update is called once per frame
    void Update() {
        time_alive += Time.deltaTime;
        if (state == ItemState.IDLE) {
            transform.position = new Vector3(transform.position.x, (Mathf.Sin(time_alive) * .5f + 1)  * BOB_HEIGHT, transform.position.z);
            transform.Rotate(Vector3.up * BOB_ROTATION * Time.deltaTime); 
        }
        
    }
}
