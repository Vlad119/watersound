using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class fonBoxScript : MonoBehaviour
{
    public TMP_InputField cameraName;
    public TMP_InputField date;
    public TMP_InputField address;
    public TMP_InputField GPSx;
    public TMP_InputField GPSy;
    public TMP_InputField GPSz;
    public TMP_InputField airTemperature;
    public TMP_InputField airHumidity;
    public TMP_InputField comment;
    public GameObject content;
    public bool loadValue = false;
    public GameObject fonCheck;
    public Allowed_values AV;
    public LoopScrollRect scroll;
    public GameObject plusPref;
    public GameObject mediaBTN;


    async void OnEnable()
    {
        var trans = content.GetComponent<RectTransform>().transform;
        trans.position = new Vector3(trans.position.x, 0, trans.position.z);
        AV = AppManager.Instance.allowed_values.allowed_values;
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AppManager.Instance;
        });
        Clear();
        if (AM.nBox) //если камера новая
        {
            plusPref.SetActive(false);
            mediaBTN.SetActive(false);
            date.text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        else
        {
            LoadBox();
            ViewPref();
            plusPref.SetActive(true);
            mediaBTN.SetActive(true);
        }
    }


    public void ViewPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].box[AM.BoxN].note.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].box[AM.BoxN].note.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void LoadBox()
    {
        var place = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        
        cameraName.text = place[AM.PlaceN].box[AM.BoxN].name;
        date.text = place[AM.PlaceN].box[AM.BoxN].date;
        address.text = place[AM.PlaceN].box[AM.BoxN].address;
        GPSx.text = place[AM.PlaceN].box[AM.BoxN].gpsx.ToString();
        GPSy.text = place[AM.PlaceN].box[AM.BoxN].gpsx.ToString();
        GPSz.text = place[AM.PlaceN].box[AM.BoxN].gpsx.ToString();
        airTemperature.text = place[AM.PlaceN].box[AM.BoxN].air_t.ToString();
        airHumidity.text = place[AM.PlaceN].box[AM.BoxN].air_humidity.ToString();
        comment.text = place[AM.PlaceN].box[AM.BoxN].comment;
    }//загружаю данные в поля ввода

    public void Clear()
    {
        cameraName.text = "";
        date.text = "";
        address.text = "";
        GPSx.text = "";
        GPSy.text = "";
        GPSz.text = "";
        airTemperature.text = "";
        airHumidity.text = "";
        comment.text = "";
    }//очищаю поля

    public void RecordBox()
    {
        var AM = AppManager.Instance;
        CheckVoid();
        if (cameraName.text == "")
        {
            fonCheck.SetActive(true);
        }
        else
        {
            if (AM.nBox)
            {
                NewBox();
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                SaveBox();
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

    public void CheckVoid()
    { 
        if (date.text == "")
            date.text = "0";
        if (address.text == "")
            address.text = "0";
        if (GPSx.text == "")
            GPSx.text = "0";
        if (GPSy.text == "")
            GPSy.text = "0";
        if (GPSz.text == "")
            GPSz.text = "0";
        if (airTemperature.text == "")
            airTemperature.text = "0";
        if (airHumidity.text == "")
            airHumidity.text = "0";
    }

    public void NewBox()
    {
        Debug.Log("создание новой камеры");
        var NB = AppManager.Instance.places.places[AppManager.Instance.PlaceN].box;
        var AM = AppManager.Instance;
        NB.Add(new Box(null,
        cameraName.text, _Files: new System.Collections.Generic.List<Files>(),
        Convert.ToSingle(GPSx.text),
        Convert.ToSingle(GPSy.text),
        Convert.ToSingle(GPSz.text),
        _x: null, _y: null, _z: null,
        Convert.ToSingle(airHumidity.text),
        Convert.ToSingle(airTemperature.text),
        _note: new System.Collections.Generic.List<Note>(),
        date.text, address.text, comment.text));
        AM.nBox = false;
    }

    public void SaveBox()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        P[AM.PlaceN].box[AM.BoxN].name = cameraName.text;
        P[AM.PlaceN].box[AM.BoxN].date = date.text;
        P[AM.PlaceN].box[AM.BoxN].address = address.text;
        P[AM.PlaceN].box[AM.BoxN].gpsx = Convert.ToSingle(GPSx.text);
        P[AM.PlaceN].box[AM.BoxN].gpsy = Convert.ToSingle(GPSy.text);
        P[AM.PlaceN].box[AM.BoxN].gpsz = Convert.ToSingle(GPSz.text);
        P[AM.PlaceN].box[AM.BoxN].air_t = Convert.ToSingle(airTemperature.text);
        P[AM.PlaceN].box[AM.BoxN].air_humidity = Convert.ToSingle(airHumidity.text);
        P[AM.PlaceN].box[AM.BoxN].comment = comment.text;
        P[AM.PlaceN].box[AM.BoxN].saved = true;
    }

    public void SetPath()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AM.path1 = AM.choisePlace;
        AM.path2 = AM.choiseBox;
        var note = P[AM.PlaceN].box[AM.BoxN].note;
        note.Add(new Note(null, null, null, null, null, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, null, null, null));
        AM.way = 2;
        AM.editIndex = true;
        AM.SwitchScreen(10);
    }
    public void Media()
    {
        var AM = AppManager.Instance;
        AM.path2 = AM.choiseBox;
        AM.way = 2;
        AM.SwitchScreen(9);
    }

    public void GPS()
    {
        GPSx.text = Convert.ToString(Input.location.lastData.longitude);
        GPSy.text = Convert.ToString(Input.location.lastData.latitude);
        GPSz.text = Convert.ToString(Input.location.lastData.altitude);
    }
}
