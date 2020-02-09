using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class fonPipeScript : MonoBehaviour
{

    public TMP_InputField name;
    public TMP_InputField date;
    public TMP_Dropdown way;
    public TMP_InputField tubeTemperature;
    public TMP_Dropdown tubeType;
    public TMP_InputField electicalPotenctial;
    public TMP_InputField lenght;
    public TMP_InputField diametr;
    public TMP_Dropdown steelType;
    public TMP_Dropdown isolationType;
    public TMP_InputField comment;
    public bool loadValue = false;
    public GameObject content;
    public Allowed_values AV;
    public Pipe pipe;
    public GameObject fonCheck;
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
        AV = AppManager.Instance.allowed_values.allowed_values;
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        {
            await WebHandler.Instance.GetDataWraper((repl) =>
        {
            AppManager.Instance.allowed_values = JsonUtility.FromJson<AnswerValues>(repl);
            Clear();
            ClearDropdowns();
            ChangeDropdowns();
            if (AM.nPipe)
            {
                plusPref.SetActive(false);
                mediaBTN.SetActive(false);
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                LoadPipe();
                ViewPref();
                plusPref.SetActive(true);
                mediaBTN.SetActive(true);
            }
        });
        }
    }

    public void ViewPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].pipe[AM.PipeN].note.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].pipe[AM.PipeN].note.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ClearDropdowns()
    {
        way.options.Clear();
        tubeType.options.Clear();
        steelType.options.Clear();
        isolationType.options.Clear();
    }

    private void Clear()
    {
        name.text = "";
        date.text = "";
        way.captionText.text = "";
        tubeTemperature.text = "";
        tubeType.captionText.text = "";
        electicalPotenctial.text = "";
        lenght.text = "";
        diametr.text = "";
        steelType.captionText.text = "";
        isolationType.captionText.text = "";
        comment.text = "";
    }

    public void ChangeDropdowns()
    {
        ChangeWay();
        ChangeSteelType();
        ChangeTubeTipe();
        ChangeIsolationType();
    }

    public void ChangeWay()
    {
        for (int val = 0; val < AV.pipe_direction.Count; val++)
        {
            way.options.Add(new TMP_Dropdown.OptionData(AV.pipe_direction[val].value));
        }
        way.RefreshShownValue();
    }

    public void ChangeTubeTipe()
    {
        for (int val = 0; val < AV.pipe_type.Count; val++)
        {
            tubeType.options.Add(new TMP_Dropdown.OptionData(AV.pipe_type[val].value));
        }
        tubeType.RefreshShownValue();
    }

    public void ChangeIsolationType()
    {
        for (int val = 0; val < AV.pipe_isolate_type.Count; val++)
        {
            isolationType.options.Add(new TMP_Dropdown.OptionData(AV.pipe_isolate_type[val].value));
        }
        isolationType.RefreshShownValue();
    }

    public void ChangeSteelType()
    {
        for (int val = 0; val < AV.pipe_metal_type.Count; val++)
        {
            steelType.options.Add(new TMP_Dropdown.OptionData(AV.pipe_metal_type[val].value));
        }
        steelType.RefreshShownValue();
    }

    public void LoadPipe()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        name.text = P[AM.PlaceN].pipe[AM.PipeN].name;
        date.text = P[AM.PlaceN].pipe[AM.PipeN].date;
        way.captionText.text = P[AM.PlaceN].pipe[AM.PipeN].pipe_direction;
        tubeTemperature.text = Convert.ToString(P[AM.PlaceN].pipe[AM.PipeN].pipe_t);
        tubeType.captionText.text = P[AM.PlaceN].pipe[AM.PipeN].pipe_type;
        electicalPotenctial.text = Convert.ToString(P[AM.PlaceN].pipe[AM.PipeN].pipe_elec_pot);
        lenght.text = Convert.ToString(P[AM.PlaceN].pipe[AM.PipeN].pipe_lenght);
        diametr.text = Convert.ToString(P[AM.PlaceN].pipe[AM.PipeN].pipe_diametr);
        steelType.captionText.text = P[AM.PlaceN].pipe[AM.PipeN].pipe_metal_type;
        isolationType.captionText.text = P[AM.PlaceN].pipe[AM.PipeN].pipe_isolate_type;
        comment.text = P[AM.PlaceN].pipe[AM.PipeN].comment;
    }

    public void Default()
    {
        date.text = "";
        way.captionText.text = "Направление";
        tubeTemperature.text = "";
        tubeType.captionText.text = "Классификатор типов прокладки";
        electicalPotenctial.text = "";
        lenght.text = "";
        diametr.text = "";
        steelType.captionText.text = "Тип стали";
        isolationType.captionText.text = "Тип изоляции";
        comment.text = "";
    }
    
    
    public void SavePipe()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        Debug.Log("редактирование труб");
        P[AM.PlaceN].pipe[AM.PipeN].name = name.text;
        P[AM.PlaceN].pipe[AM.PipeN].date = date.text;
        P[AM.PlaceN].pipe[AM.PipeN].pipe_direction = way.captionText.text;
        P[AM.PlaceN].pipe[AM.PipeN].pipe_t = Convert.ToSingle(tubeTemperature.text);
        P[AM.PlaceN].pipe[AM.PipeN].pipe_type = tubeType.captionText.text;
        P[AM.PlaceN].pipe[AM.PipeN].pipe_elec_pot = Convert.ToSingle(electicalPotenctial.text);
        P[AM.PlaceN].pipe[AM.PipeN].pipe_lenght = Convert.ToSingle(lenght.text);
        P[AM.PlaceN].pipe[AM.PipeN].pipe_diametr = Convert.ToSingle(diametr.text);
        P[AM.PlaceN].pipe[AM.PipeN].pipe_metal_type = steelType.captionText.text;
        P[AM.PlaceN].pipe[AM.PipeN].pipe_isolate_type = isolationType.captionText.text;
        P[AM.PlaceN].pipe[AM.PipeN].comment = comment.text;
        P[AM.PlaceN].pipe[AM.PipeN].saved = true;
        AM.SwitchScreen(2);
    }

    public void SetPath()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AM.path1 = AM.choisePlace;
        AM.path2 = AM.choisePipe;
        var note = P[AM.PlaceN].pipe[AM.PipeN].note;
        note.Add(new Note(null, null, null, null, null, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, null, null, null));
        AM.editIndex = true;
        AM.SwitchScreen(10);
    }

    public void Media()
    {
        var AM = AppManager.Instance;
        AM.path2 = AM.choisePipe;
        AM.way = 3;
        AM.SwitchScreen(9);
    }

    public void NewPipe()
    {
        Debug.Log("создание новой трубы");
        var AM = AppManager.Instance;
        var NP = AM.places.places[AM.PlaceN].pipe;
        NP.Add(new Pipe(null, name.text, way.captionText.text,
            isolationType.captionText.text,
            steelType.captionText.text, tubeType.captionText.text,
            _note: new System.Collections.Generic.List<Note>(),
            date.text, Convert.ToSingle(tubeTemperature.text),
            Convert.ToSingle(electicalPotenctial.text),
            Convert.ToSingle(lenght.text),
            Convert.ToSingle(diametr.text), comment.text,
            _files: new System.Collections.Generic.List<Files>()));
        AM.nPipe = false;
    }

    public void CheckVoid()
    {
        if (date.text == "")
            date.text = "0";
        if (way.captionText.text == "")
            way.captionText.text = "0";
        if (tubeTemperature.text == "")
            tubeTemperature.text = "0";
        if (electicalPotenctial.text == "")
            electicalPotenctial.text = "0";
        if (lenght.text == "")
            lenght.text = "0";
        if (diametr.text == "")
            diametr.text = "0";
        if (steelType.captionText.text == "")
            steelType.captionText.text = "0";
        if (isolationType.captionText.text == "")
            isolationType.captionText.text = "0";
        if (comment.text == "")
            comment.text = "0";
    }

    public void RecordPipe()
    {
        var AM = AppManager.Instance;
        CheckVoid();
        if (name.text == "")
        {
            fonCheck.SetActive(true);
        }
        else
        {
            if (AM.nPipe)
            {
                NewPipe();
                date.text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                SavePipe();
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
}
