using TMPro;
using UnityEngine;

public class ModifyTextScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TMP_Text output;

    public void ChangeText()
    {
        string memText = input.text;
        if (memText.Length < 12)
        {
            if (input.text.Length > 1) memText = memText.Insert(1, "(");
            if (input.text.Length > 4) memText = memText.Insert(5, ")");
            if (input.text.Length > 6) memText = memText.Insert(9, "-");
            if (input.text.Length > 8) memText = memText.Insert(12, "-");
            output.text = memText;
            if (memText.Length == 15)
            {
                Debug.Log(memText);
            }
        }
    }
}
