using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Greys out one heart icon per lost health point.
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] List<Image> heartIcons;
    [SerializeField] PlayerHealth pHealth;
    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;

    void OnEnable()
    {
        GameEvents.PlayerHealthChanged += Refresh;
        if (pHealth != null)
        {
            Refresh(pHealth.CurrentHealth, pHealth.MaxHealth);
        }
    }

    void OnDisable()
    {
        GameEvents.PlayerHealthChanged -= Refresh;
    }

    void Refresh(int current, int max)
    {
        int lost = max - current;
        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIcons[i].color = i < lost ? inActiveColor : activeColor;
        }
    }
}
