using TMPro;
using UnityEngine;

public class LevelPrefScript : MonoBehaviour
{
    public TMP_Text levelTitle;
    public TMP_Text levelValue;

    public void CreatePref(string level, string value)
    {
        levelTitle.text = level;
        levelValue.text =  value;
    }
}

