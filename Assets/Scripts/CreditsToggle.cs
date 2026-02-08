using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsToggle : MonoBehaviour
{
    [SerializeField] Sprite TitleBackground, CreditsBackground;
    [SerializeField] SpriteRenderer background;
    [SerializeField] Animator screenDisplay;
    [SerializeField] AudioSource startUpSound;
    bool showingCredits;

    void Start()
    {
        InputManager.inst.escape.AddListener(Toggle);
        showingCredits = false;
    }

    // toggles screen display and background using animator
    public void Toggle() {
        if (showingCredits) {
            startUpSound.Play();
            background.sprite = TitleBackground;
            screenDisplay.SetTrigger("ToTitle");
        } else {
            background.sprite = CreditsBackground;
            screenDisplay.SetTrigger("ToCredits");
        }

        showingCredits = !showingCredits;
    }

}
