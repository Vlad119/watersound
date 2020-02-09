using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FonIDScript : MonoBehaviour
{
    public LoopScrollRect scroll;
    public TMP_Text parent;
    public GameObject searchPref;
    public GameObject ParentSearchPref;
    public GameObject plusPref;
    public TMP_Dropdown org;
    public TMP_Dropdown level;
    public TMP_InputField mainValue;
    public int count;

    private void Start()
    {
        count = 0;
    }

    private void OnEnable()
    {
        ParentSearchPref.transform.ClearChildren();
        DropdownData();
    }

    public void PlusSearchPref()
    {
        if (count < 6)
        {
            Instantiate(searchPref, ParentSearchPref.transform);
            Instantiate(plusPref, ParentSearchPref.transform);
            count++;
        }
    }

    public void Search()
    {
        var AM = AppManager.Instance;
        AM.orgName = org.captionText.text;
        AM.mainValue = mainValue.text;
        AM.screens[12].SetActive(true);
    }

    public void DropdownData()
    {
        org.ClearOptions();
        org.options.Add(new TMP_Dropdown.OptionData("--------------------------------------------------------------"));
        var com_names = AppManager.Instance.allowed_values.allowed_values.com_names;
        for (int i = 0; i < com_names.Length; i++)
        {
            org.options.Add(new TMP_Dropdown.OptionData(com_names[i]));
            org.RefreshShownValue();
        }
    }

    public void DropdownValue()
    {
        count = 0;
        count+=2;
        AppManager.Instance.orgName = org.captionText.text;
        ParentSearchPref.transform.ClearChildren();
        Instantiate(searchPref, ParentSearchPref.transform);
        Instantiate(plusPref, ParentSearchPref.transform);
    }

    public void BackBTN()
    {
        gameObject.SetActive(false);
    }
}
