using TMPro;
using UnityEngine;

public class SearchPrefScript : MonoBehaviour
{
    public TMP_Dropdown level;
    public TMP_Dropdown contains;
    public void ValueChanged()
    {
        var fID = AppManager.Instance.fID;
        if (level.captionText.text != "--------------------------------------------------------------" &&
            contains.captionText.text != "--------------------------------------------------------------")
            fID.levelss.Add(new UserSearch(level.captionText.text, contains.captionText.text));
    }

    private void OnEnable()
    {
        var levels = AppManager.Instance.search.levels;
        level.options.Clear();
        contains.options.Clear();
        level.options.Add(new TMP_Dropdown.OptionData("--------------------------------------------------------------"));
        contains.options.Add(new TMP_Dropdown.OptionData("--------------------------------------------------------------"));
        level.options.Add(new TMP_Dropdown.OptionData(levels.level1_name));
        level.options.Add(new TMP_Dropdown.OptionData(levels.level2_name));
        level.options.Add(new TMP_Dropdown.OptionData(levels.level3_name));
        level.options.Add(new TMP_Dropdown.OptionData(levels.level4_name));
        level.options.Add(new TMP_Dropdown.OptionData(levels.level5_name));

        foreach (searchids search in AppManager.Instance.search.searchids)
        {
            if (search.parentName.Contains(AppManager.Instance.orgName))
            {
                contains.options.Add(new TMP_Dropdown.OptionData(search.level1));
            }
        }
    }
}
