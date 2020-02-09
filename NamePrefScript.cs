using TMPro;
using UnityEngine;

public class NamePrefScript : MonoBehaviour
{
    public TMP_Text fullName;
    public void CreatePref(string name)
    {
        fullName.text = name;
    }
}
