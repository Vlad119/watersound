using TMPro;
using UnityEngine;

public class FindPrefScript : MonoBehaviour
{
    public TMP_Text number;
    public GameObject namePref;
    public GameObject ParentLevelPref;
    public string id;

    public void CreatePref(searchids place, int index)
    {
        number.text = "№ " + index.ToString();
        id = place.id;
        var fName = Instantiate(namePref, ParentLevelPref.transform);
        fName.GetComponent<NamePrefScript>().CreatePref(place.fullName);
    }

    public void TakeID()
    {
        var AM = AppManager.Instance;
        AM.needID = id;
        AM.screens[12].GetComponent<FonID2Script>().OnEnable();
    }
}
