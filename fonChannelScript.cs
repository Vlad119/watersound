using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class fonChannelScript : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_Dropdown channelType;
    public TMP_InputField date;
    public TMP_InputField height;
    public TMP_InputField width;
    public TMP_InputField comment;
    public GameObject content;
    public Allowed_values AV;
    public GameObject fonCheck;
    public bool loadValue = false;
    public LoopScrollRect scroll;
    public GameObject plusPref;
    public GameObject mediaBTN;


    async void OnEnable()
    {
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AppManager.Instance;
        });
        var trans = content.GetComponent<RectTransform>().transform;
        trans.position = new Vector3(trans.position.x, 0, trans.position.z);
        var AM = AppManager.Instance;
        AV = AppManager.Instance.allowed_values.allowed_values;
        {
            Clear();
            channelType.options.Clear();
            ChangeChannelType();
            if (AM.nChannel)
            {
                plusPref.SetActive(false);
                mediaBTN.SetActive(false);
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                LoadChannel();
                ViewPref();
                plusPref.SetActive(true);
                mediaBTN.SetActive(true);
            }
        }
    }

    public void ViewPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].channel[AM.ChannelN].note.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].channel[AM.ChannelN].note.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }//обновление префаба

    public void Clear()
    {
        name.text = "";
        date.text = "";
        channelType.captionText.text = "";
        height.text = "";
        width.text = "";
        comment.text = "";
    }//очистка полей ввода

    public void LoadChannel()
    {
        Debug.Log("server load");
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        name.text = P[AM.PlaceN].channel[AM.ChannelN].name;
        channelType.captionText.text = P[AM.PlaceN].channel[AM.ChannelN].cha_type;
        date.text = P[AM.PlaceN].channel[AM.ChannelN].date;
        height.text = P[AM.PlaceN].channel[AM.ChannelN].x;
        width.text = P[AM.PlaceN].channel[AM.ChannelN].y;
        comment.text = P[AM.PlaceN].channel[AM.ChannelN].comment;
    }//загрузка данных в поля ввода

    public void ChangeChannelType()
    {
        for (int val = 0; val < AV.cha_type.Count; val++)
        {
            channelType.options.Add(new TMP_Dropdown.OptionData(AV.cha_type[val].value));
        }
        channelType.RefreshShownValue();
    }

    public void CheckVoid()
    {
        if (channelType.captionText.text == "")
            channelType.captionText.text = "0";
        if (date.text == "")
            date.text = "0";
        if (width.text == "")
            width.text = "0";
        if (height.text == "")
            height.text = "0";
        if (comment.text == "")
            comment.text = "0";
    }

    public void SaveChannel()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        P[AM.PlaceN].channel[AM.ChannelN].name = name.text;
        P[AM.PlaceN].channel[AM.ChannelN].cha_type = channelType.captionText.text;
        P[AM.PlaceN].channel[AM.ChannelN].date = date.text;
        P[AM.PlaceN].channel[AM.ChannelN].x = width.text;
        P[AM.PlaceN].channel[AM.ChannelN].y = height.text;
        P[AM.PlaceN].channel[AM.ChannelN].comment = comment.text;
        P[AM.PlaceN].surface[AM.ChannelN].saved = true;
        AM.SwitchScreen(2);
    }//сохранение изменений канала

    public void SetPath()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AM.path1 = AM.choisePlace;
        AM.path2 = AM.choiseChannel;
        var note = P[AM.PlaceN].channel[AM.ChannelN].note;
        note.Add(new Note(null, null, null, null, null, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, null, null, null));
        AM.way = 4;
        AM.editIndex = true;
        AM.SwitchScreen(10);
    }

    public void Media()
    {
        var AM = AppManager.Instance;
        AM.path2 = AM.choiseChannel;
        AM.way = 4;
        AM.SwitchScreen(9);
    }

    public void RecordChannel()
    {
        var AM = AppManager.Instance;
        CheckVoid();
        if (name.text == "")
        {
            fonCheck.SetActive(true);
        }
        else
        {
            if (AM.nChannel)
            {
                NewChannel();
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                SaveChannel();
            }
            UpdatePlayerPrefs();
            AM.SwitchScreen(2);
        }
    }

    public void UpdatePlayerPrefs()
    {
        var AM = AppManager.Instance;
        var place = AM.places;
        string data = JsonUtility.ToJson(place);
        PlayerPrefs.SetString(AM.phone_number, data);
        Debug.Log("player prefs saved");
    }

    public void NewChannel()
    {
        Debug.Log("создание нового канала");
        var AM = AppManager.Instance;
        var NC = AM.places.places[AM.PlaceN].channel;
        NC.Add(new Channel(null, name.text, comment.text, name.text,
            height.text, width.text,
            _Files: new System.Collections.Generic.List<Files>(),
            channelType.captionText.text,
            _Note: new System.Collections.Generic.List<Note>()));
        AM.nChannel = false;
    }
}