using UnityEngine;
using UnityEngine.UI;

public class AddEditToPref : MonoBehaviour
{
    public Button edit;
    private bool listenerAdded = false;

    public void Start()
    {
        try
        {
            if (!listenerAdded)
            {
                edit.onClick.AddListener(ChangeScreen);
            }
        }
        catch { }
    }

    private void ChangeScreen()
    {
        AppManager.Instance.SwitchScreen(8);
        listenerAdded = true;
    }
}
