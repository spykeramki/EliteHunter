using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUiCtrl : MonoBehaviour
{
    public Image[] healthImages;
    public Image[] shieldImages;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;

    public Color32 depletionFullColor;
    public Color32 depletionFullColorFade;

    private bool isDepletionEffectStarted = false;
    public bool IsDepletionEffectStarted
    {
        get { return isDepletionEffectStarted; }
    }

    public void SetHealthInUi(float m_health)
    {
        healthText.text = ((int)m_health).ToString();
        foreach (Image item in healthImages)
        {
            item.fillAmount = m_health/100f;
        }
    }

    public void SetShieldInUi(float m_charge)
    {
        StopCoroutine("ShieldDepletionEffect");
        shieldText.text = ((int)m_charge).ToString();
        foreach (Image item in shieldImages)
        {
            item.fillAmount = m_charge / 100f;
        }
    }

    public void StartShieldDepletionEffect()
    {
        if (!isDepletionEffectStarted)
        {
            isDepletionEffectStarted = true;
            StartCoroutine(ShieldDepletionEffect());
        }
    }

    private IEnumerator ShieldDepletionEffect()
    {
        StopCoroutine("ShieldDepletionEffect");
        int fluctuateTime = 20;
        bool isFullColor = false;

        while (fluctuateTime > 0)
        {
            fluctuateTime--;
            isFullColor = !isFullColor;
            AssignAlphaToShieldBars((isFullColor ? 70f : 20f), 1f);
            yield return new WaitForSeconds(0.2f);
        }
        if (fluctuateTime <= 0)
        {
            AssignAlphaToShieldBars(255f, 0);
            StopCoroutine("ShieldDepletionEffect");
            isDepletionEffectStarted = false ;
        }
    }

    private void AssignAlphaToShieldBars(float m_alpha, float m_fill)
    {
        foreach (Image item in shieldImages)
        {
            item.fillAmount = m_fill;
            Color32 m_color = item.color;
            m_color.a = (byte)m_alpha;
            item.color = m_color;
        }
    }
}
