using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeThemeManager : MonoBehaviour
{
    public TreeTheme[] themes;

    [System.Serializable]
    public class TreeTheme
    {
        public Color color;
        public float intensity;

        public TreeTheme(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
    }
}
