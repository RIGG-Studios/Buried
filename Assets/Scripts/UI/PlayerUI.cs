using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject chestUI;

    public Slider healthSlider;
    public Text healthText;

    public Slider oxygenSlider;
    public Text oxygenText;

    public Slider flashLightSlider;
    public Text flashLightText;

    public void ShowFlashlightBattery(float battery, float maxBattery)
    {
        /*/
        flashLightSlider.gameObject.SetActive(true);

        float percentage = (battery / maxBattery) * 100;

        flashLightSlider.maxValue = maxBattery;
        flashLightSlider.value = battery;
        flashLightText.text = ((int)percentage).ToString();

        Invoke("HideFlashlight", 3f);
        /*/
    }

    private void HideFlashlight()
    {
        flashLightSlider.gameObject.SetActive(false);
        flashLightText.gameObject.SetActive(false);
    }

    public void ToggleChest(bool state) => chestUI.SetActive(state);
}
