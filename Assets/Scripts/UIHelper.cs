using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RetroPlatform
{
    public class UIHelper
    {
        float alpha = 0f;

        public void FadeOut(bool fadeOut, Texture2D FadeTexture)
        {
            float fadespeed = 0.5f;

            if (!fadeOut) return;

            alpha -= -1 * fadespeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color newColor = GUI.color;
            newColor.a = alpha;
            GUI.color = newColor;
            GUI.depth = -1000;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }
    }
}
