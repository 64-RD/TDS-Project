using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text HpText;
    public int MaxHealth;
    public int Health;
    public void SetMaxHealth(int health)
    {
        this.MaxHealth = health;
        slider.maxValue = health;
        slider.value = health;
        HpText.text = $"{health}/{health}";
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetHealth(int health)
    {
        this.Health = health;
        slider.value = health;
        HpText.text = $"{health}/{MaxHealth}";

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
