using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Slider arcSlider;
    [SerializeField] Slider hammerSlider;
    [SerializeField] TMP_Text score = null;
    void Update()
    {
        if(!player) return;

        if(arcSlider)
            arcSlider.value = player.actualArcCastTime / player.arcCastTime;

        if(hammerSlider)
            hammerSlider.value = player.actualHammerCastTime / player.hammerCastTime;

        score.text = player.score.ToString();
    }
}
