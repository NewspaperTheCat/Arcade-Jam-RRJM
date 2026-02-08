using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMeter : MonoBehaviour
{
    Image image;
    public List<Sprite> frames = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        SetCharge(0);
    }

    public void SetCharge(float percent) {
        int index = Mathf.RoundToInt(percent * (frames.Count - 1));
        image.sprite = frames[index];
    }
}
