using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameScene : UI_Base
{
    [SerializeField] TextMeshProUGUI _killCountText;
    [SerializeField] Slider _gemSlider;

    public void SetGemCountRatio(float ratio)
    {
        _gemSlider.value = ratio;
    }

    public void SetKillCount(int killCount)
    {
        _killCountText.text = $"{killCount}";
    }

    public void SetInfo()
    {

    }
    public void RefreshUI()
    {

    }
}
