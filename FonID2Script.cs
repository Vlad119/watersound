using TMPro;
using UnityEngine;

public class FonID2Script : MonoBehaviour
{
    public GameObject findPref;
    public GameObject levelPref;
    public GameObject spacePref;
    public GameObject ParentFindPref;
    public TMP_Text id;

    private void Start()
    {
        Searching();
    }

    public void BackBTN()
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        id.text = AppManager.Instance.needID;
    }

    public void TakeID()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        P[AM.PlaceN].id = AM.needID;
        AM.screens[8].SetActive(false);
        AM.screens[12].SetActive(false);
        AM.screens[3].GetComponent<fonNewAreaScript>().OnEnable();
    }

    public void Searching()
    {
        var AM = AppManager.Instance;
        int index = 1;
        var levels = AM.fID.levelss;
        if (levels.Count != 0)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                var l = Instantiate(levelPref, ParentFindPref.transform);
                l.GetComponent<LevelPrefScript>().CreatePref(levels[i].needLevel, levels[i].needValue);
            }
        }
        Instantiate(spacePref, ParentFindPref.transform);
        foreach (UserSearch value in AM.fID.levelss)
        {
            foreach (searchids search in AM.search.searchids)
            {
                if (search.level1.Contains(value.needValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index);
                    index++;
                }
                 
                if (search.level2.Contains(value.needValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index); index++;
                }

                if (search.level3.Contains(value.needValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index); index++;
                }
                
                if (search.level4.Contains(value.needValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index); index++;
                }

                if (search.level5.Contains(value.needValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index); index++;
                }

                if (search.fullName.Contains(AM.mainValue))
                {
                    var pref = Instantiate(findPref, ParentFindPref.transform);
                    pref.GetComponent<FindPrefScript>().CreatePref(search, index); index++;
                }
            }
        }
    }
}
