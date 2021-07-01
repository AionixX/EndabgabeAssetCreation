using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Slider arcSlider;
    [SerializeField] Slider hammerSlider;
    void Update()
    {
        if(!player) return;

        if(arcSlider)
            arcSlider.value = player.actualArcCastTime / player.arcCastTime;

        if(hammerSlider)
            hammerSlider.value = player.actualHammerCastTime / player.hammerCastTime;
    }
}
