using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class fonSurfaceScript : MonoBehaviour
{
    public GameObject content;

    public TMP_InputField name;
    public TMP_InputField date;
    public TMP_Dropdown teraType;
    public TMP_Dropdown surfaceType;
    public TMP_Dropdown passPossibillity;
    public TMP_Dropdown currentSource;
    public TMP_Dropdown ground;
    public TMP_Dropdown wetGround;
    public TMP_InputField groundResistace;
    public TMP_InputField temperature;
    public TMP_InputField temperatureGround;
    public TMP_Dropdown SODK;
    public TMP_InputField comment;
    public GameObject fonCheck;
    public bool loadValue = false;
    public bool edit = false;
    public Allowed_values AV;
    public LoopScrollRect scroll;
    public GameObject plusPref;
    public GameObject mediaBTN;

    async void OnEnable()
    {
        var trans = content.GetComponent<RectTransform>().transform;
        trans.position = new Vector3(trans.position.x, 0, trans.position.z);
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        AV = AppManager.Instance.allowed_values.allowed_values;
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AppManager.Instance;
        });
        {
            Clear();
            ClearDropdowns();
            ChangeDropdowns();
            if (AM.nSurface) // если поверхность новая
            {
                plusPref.SetActive(false);
                mediaBTN.SetActive(false);
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                LoadSurface();
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
            if (P[AM.PlaceN].surface[AM.SurfaceN].note.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].surface[AM.SurfaceN].note.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }//обновление префаба


    public void ClearDropdowns()
    {
        teraType.options.Clear();
        surfaceType.options.Clear();
        passPossibillity.options.Clear();
        currentSource.options.Clear();
        ground.options.Clear();
        SODK.options.Clear();
    } //очищаю dropdown

    public void LoadSurface()
    {
        Debug.Log("LoadSurface");
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        name.text = P[AM.PlaceN].surface[AM.SurfaceN].name;
        date.text = P[AM.PlaceN].surface[AM.SurfaceN].date;
        teraType.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].terra_type;
        surfaceType.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_type;
        passPossibillity.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_couldpass;
        currentSource.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_tok;
        ground.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_ground;
        wetGround.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_humidity;
        groundResistace.text = Convert.ToString(P[AM.PlaceN].surface[AM.SurfaceN].ground_res);
        temperature.text = Convert.ToString(P[AM.PlaceN].surface[AM.SurfaceN].air_t);
        temperatureGround.text = Convert.ToString(P[AM.PlaceN].surface[AM.SurfaceN].surface_t);
        SODK.captionText.text = P[AM.PlaceN].surface[AM.SurfaceN].surface_sodk;
        comment.text = P[AM.PlaceN].surface[AM.SurfaceN].comment;
    } // загрузка данных в поля ввода

    private void Clear()
    {
        name.text = "";
        date.text = "";
        teraType.captionText.text = "";
        surfaceType.captionText.text = "";
        passPossibillity.captionText.text = "";
        currentSource.captionText.text = "";
        ground.captionText.text = "";
        wetGround.captionText.text = "";
        groundResistace.text = "";
        temperature.text = "";
        temperatureGround.text = "";
        SODK.captionText.text = "";
        comment.text = "";
    } //очистка полей ввода

    public void ChangeTeraType()
    {
        for (int val = 0; val < AV.terra_type.Count; val++)
        {
            teraType.options.Add(new TMP_Dropdown.OptionData(AV.terra_type[val].value));
        }
        teraType.RefreshShownValue();
    }

    public void ChangeSurfaceType()
    {
        for (int val = 0; val < AV.surface_type.Count; val++)
        {
            surfaceType.options.Add(new TMP_Dropdown.OptionData(AV.surface_type[val].value));
        }
        surfaceType.RefreshShownValue();
    }

    public void ChangePassPosibillity()
    {
        for (int val = 0; val < AV.surface_couldpass.Count; val++)
        {
            passPossibillity.options.Add(new TMP_Dropdown.OptionData(AV.surface_couldpass[val].value)); ;
        }
        passPossibillity.RefreshShownValue();
    }

    public void ChangeCurrentSource()
    {
        for (int val = 0; val < AV.surface_tok.Count; val++)
        {
            currentSource.options.Add(new TMP_Dropdown.OptionData(AV.surface_tok[val].value));
        }
        currentSource.RefreshShownValue();
    }

    public void ChangeGround()
    {
        for (int val = 0; val < AV.surface_ground.Count; val++)
        {
            ground.options.Add(new TMP_Dropdown.OptionData(AV.surface_ground[val].value));
        }
        ground.RefreshShownValue();
    }

    public void ChangeWetGround()
    {
        for (int val = 0; val < AV.surface_humidity.Count; val++)
        {
            var optionData = new TMP_Dropdown.OptionData(AV.surface_humidity[val].value);
            wetGround.options.Add(optionData);
        }
        wetGround.RefreshShownValue();
    }

    public void ChangeSODK()
    {
        for (int val = 0; val < AV.surface_sodk.Count; val++)
        {
            var optionData = new TMP_Dropdown.OptionData(AV.surface_sodk[val].value);
            SODK.options.Add(optionData);
        }
        SODK.RefreshShownValue();
    }

    public void ChangeDropdowns()
    {
        ChangeTeraType();
        ChangeSurfaceType();
        ChangePassPosibillity();
        ChangeCurrentSource();
        ChangeGround();
        ChangeWetGround();
        ChangeSODK();
    } //обновляю dropdowns

    public void RecordSurface()
    {
        var AM = AppManager.Instance;
        if (groundResistace.text == "")
            groundResistace.text = "0";
        if (temperature.text == "")
            temperature.text = "0";
        if (temperatureGround.text == "")
            temperatureGround.text = "0";
        if (name.text == "")
        {
            fonCheck.SetActive(true);   
        }
        else
        {
            if (AM.nSurface)
            {
                NewSurface();
            }
            else
            {
                SaveSurface();
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

    public void NewSurface() //создаю новую поверхность
    {
        var NS = AppManager.Instance.places.places[AppManager.Instance.PlaceN].surface;
        var AM = AppManager.Instance;
        Debug.Log("создание новой поверхности");
        NS.Add(new Surface(name.text,
        _files: new System.Collections.Generic.List<Files>(),
        surfaceType.captionText.text,
        teraType.captionText.text,
        wetGround.captionText.text,
        passPossibillity.captionText.text,
        ground.captionText.text,
        currentSource.captionText.text,
        SODK.captionText.text,
        Convert.ToSingle(temperature.text),
        Convert.ToSingle(temperatureGround.text),
        _note: new System.Collections.Generic.List<Note>(),
        comment.text, date.text,
        Convert.ToSingle(groundResistace.text)));
        AM.nSurface = false;
    }

    public void SaveSurface()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        Debug.Log("редактирование поверхности");
        P[AM.PlaceN].surface[AM.SurfaceN].name = name.text;
        P[AM.PlaceN].surface[AM.SurfaceN].date = date.text;
        P[AM.PlaceN].surface[AM.SurfaceN].terra_type = teraType.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].surface_type = surfaceType.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].surface_couldpass = passPossibillity.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].surface_ground = ground.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].surface_humidity = wetGround.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].surface_tok = currentSource.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].ground_res = Convert.ToSingle(groundResistace.text);
        P[AM.PlaceN].surface[AM.SurfaceN].air_t = Convert.ToSingle(temperature.text);
        P[AM.PlaceN].surface[AM.SurfaceN].surface_t = Convert.ToSingle(temperatureGround.text);
        P[AM.PlaceN].surface[AM.SurfaceN].surface_sodk = SODK.captionText.text;
        P[AM.PlaceN].surface[AM.SurfaceN].comment = comment.text;
        P[AM.PlaceN].surface[AM.SurfaceN].saved = true;
    }//сохраняю изменения поверхности

    public void SetPath() // запоминаю путь для отображения в Note
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AM.path1 = AM.choisePlace;
        AM.path2 = AM.choiseSurface;
        var note = P[AM.PlaceN].surface[AM.SurfaceN].note;
        note.Add(new Note(null, null, null, null, null, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, null, null, null));
        AM.way = 1;
        AM.editIndex=true;
        AM.SwitchScreen(10);
    }

    public void Media()
    {
        var AM = AppManager.Instance;
        AM.path2 = AM.choiseSurface;
        AM.way = 1;
        AM.SwitchScreen(9);
    }
}



