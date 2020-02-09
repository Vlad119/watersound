using TMPro;
using UnityEngine;

public class FonLoginScript : MonoBehaviour
{
    public TMP_InputField codeText;

    private void Start()
    {
        codeText.text = PlayerPrefs.GetString("code");
    }
}
