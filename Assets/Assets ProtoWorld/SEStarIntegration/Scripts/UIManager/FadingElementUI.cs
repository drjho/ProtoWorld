﻿/*
 * 
 * ELEMENT UI
 * FadingElementUI.cs
 * Miguel Ramos Carretero
 * 
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Script to control the fade in/fade out of an element in the UI.
/// </summary>
public class FadingElementUI : MonoBehaviour
{
    public int fadingSpeedFactor = 1;
    private bool isFadingIn = false;
    private bool fading;
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Start the script.
    /// </summary>
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    /// <summary>
    /// Update method.
    /// </summary>
    void Update()
    {
        if (fading)
        {
            if (isFadingIn)
            {
                if (canvasGroup.alpha == 1.0f)
                    fading = false;
                else
                    canvasGroup.alpha += 0.05f * fadingSpeedFactor;
            }
            else
            {
                if (canvasGroup.alpha == 0.0f)
                    fading = false;
                else
                    canvasGroup.alpha -= 0.05f * fadingSpeedFactor;
            }
        }
    }

    /// <summary>
    /// Fades in the canvas.
    /// </summary>
    public void fadeInCanvas()
    {
        isFadingIn = true;
        fading = true;
    }

    /// <summary>
    /// Fades out the canvas.
    /// </summary>
    public void fadeOutCanvas()
    {
        isFadingIn = false;
        fading = true;
    }

    /// <summary>
    /// Show the canvas without fade in.
    /// </summary>
    public void showCanvasInmediately()
    {
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    /// Hide the canvas without fade out.
    /// </summary>
    public void hideCanvasInmediately()
    {
        canvasGroup.alpha = 0.0f;
    }
}
