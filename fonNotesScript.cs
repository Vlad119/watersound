using TMPro;
using UnityEngine;
using System;

public class fonNotesScript : MonoBehaviour
{
    public TMP_InputField name;
    public TMP_InputField parentName;
    public TMP_InputField parentType;
    public TMP_InputField id;
    public TMP_InputField gpsx;
    public TMP_InputField gpsy;
    public TMP_InputField gpsz;
    public TMP_InputField place_long_from;
    public TMP_InputField place_long_to;
    public TMP_InputField place_transverse_from;
    public TMP_InputField place_transverse_to;
    public TMP_InputField wall_thickness;
    public TMP_InputField wall_hardness;
    public TMP_Dropdown defect;
    public TMP_InputField comment;
    public Allowed_values AV;
    public GameObject fonCheck;
    public GameObject content;
    bool enableCompited = false;
    private Action<Note> callback;
    private PrefEdit prefEdit;
    private int editIndex = 0;


    async void OnEnable()
    {
        enableCompited = false;
        callback = null;
        editIndex = 0;
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        var trans = content.GetComponent<RectTransform>().transform;
        trans.position = new Vector3(trans.position.x, 0, trans.position.z);
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AppManager.Instance;
        });
        parentName.text = AM.path2;
        switch (AM.way)
        {
            case 1:
                parentType.text = "Поверхность"; break;
            case 2:
                parentType.text = "Камера"; break;
            case 3:
                parentType.text = "Труба"; break;
            case 4:
                parentType.text = "Канал"; break;
        }
        Clear();
        defect.options.Clear();
        Way();
        enableCompited = true;
    }

    public async void GetLink(PrefEdit _prefEdit, int _index, Action<Note> _callback)
    {
        await new WaitUntil(() =>
        {
            return enableCompited;
        });
        var AM = AppManager.Instance;
        editIndex = _index;
        callback = _callback;
        Debug.Log("callback " + callback);
        prefEdit = _prefEdit;
        name.text = prefEdit.data.name;
        gpsx.text = prefEdit.data.gpsx.ToString();
        gpsy.text = prefEdit.data.gpsy.ToString();
        gpsz.text = prefEdit.data.gpsz.ToString();
        place_long_from.text = prefEdit.data.place_long_from.ToString();
        place_long_to.text = prefEdit.data.place_long_to.ToString();
        place_transverse_from.text = prefEdit.data.place_transverse_from.ToString();
        place_transverse_to.text = prefEdit.data.place_transverse_to.ToString();
        wall_thickness.text = prefEdit.data.wall_thickness.ToString();
        wall_hardness.text = prefEdit.data.wall_hardness.ToString();
        comment.text = prefEdit.data.comment;
        switch (AM.way)
        {
            case 1: defect.captionText.text = prefEdit.data.note_surface_defect;
                    parentType.text = "Поверхность"; break;
            case 2: defect.captionText.text = prefEdit.data.note_box_defect;
                    parentType.text = "Камера"; break;
            case 3: defect.captionText.text = prefEdit.data.note_pipe_defect;
                    parentType.text = "Труба"; break;
            case 4: defect.captionText.text = prefEdit.data.note_channel_defect;
                    parentType.text = "Канал"; break;
        }
    }

    public void Clear()
    {
        var AM = AppManager.Instance;
        parentName.text = AM.path2;
        gpsx.text = "0";
        gpsy.text = "0";
        gpsz.text = "0";
        place_long_from.text = "0";
        place_long_to.text = "0";
        place_transverse_from.text = "0";
        place_transverse_to.text = "0";
        wall_thickness.text = "0";
        wall_hardness.text = "0";
        defect.captionText.text = "0";
        comment.text = "0";
        switch (AM.way)
        {
            case 1:
                parentType.text = "Поверхность"; break;
            case 2:
                parentType.text = "Камера"; break;
            case 3:
                parentType.text = "Труба"; break;
            case 4:
                parentType.text = "Канал"; break;
        }
    } // очищаю поля

    public void ChangeSurfaceDefect()
    {
        AV = AppManager.Instance.allowed_values.allowed_values;
        defect.options.Clear();
        for (int val = 0; val < AV.note_surface_defect.Count; val++)
        {
            defect.options.Add(new TMP_Dropdown.OptionData(AV.note_surface_defect[val].value));
        }
        defect.RefreshShownValue();
    }

    public void ChangeBoxDefect()
    {
        AV = AppManager.Instance.allowed_values.allowed_values;
        defect.options.Clear();
        for (int val = 0; val < AV.note_box_defect.Count; val++)
        {
            defect.options.Add(new TMP_Dropdown.OptionData(AV.note_box_defect[val].value));
        }
        defect.RefreshShownValue();
    }

    public void ChangePipeDefect()
    {
        AV = AppManager.Instance.allowed_values.allowed_values;
        defect.options.Clear();
        for (int val = 0; val < AV.note_pipe_defect.Count; val++)
        {
            defect.options.Add(new TMP_Dropdown.OptionData(AV.note_pipe_defect[val].value));
        }
        defect.RefreshShownValue();
    }

    public void ChangeChannelDefect()
    {
        AV = AppManager.Instance.allowed_values.allowed_values;
        defect.options.Clear();
        for (int val = 0; val < AV.note_channel_defect.Count; val++)
        {
            defect.options.Add(new TMP_Dropdown.OptionData(AV.note_channel_defect[val].value));
        }
        defect.RefreshShownValue();
    }

    public void Way()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: ChangeSurfaceDefect(); break;
            case 2: ChangeBoxDefect(); break;
            case 3: ChangePipeDefect(); break;
            case 4: ChangeChannelDefect(); break;
        }
    }//выбор типа деффектов для dropdown

    public void SaveNote()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: SaveSurface(); break;
            case 2: SaveBox(); break;
            case 3: SavePipe(); break;
            case 4: SaveChannel(); break;
        }
    } //сохраняю изменения в текущей заметке

    public void UpdatePlayerPrefs()
    {
        var AM = AppManager.Instance;
        var place = AM.places;
        string data = JsonUtility.ToJson(place);
        PlayerPrefs.SetString(AM.phone_number, data);
        Debug.Log("player prefs saved");
    }

    public void SaveSurface()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        Debug.Log("SaveSurface");
        if (AM.editIndex && P[AM.PlaceN].surface[AM.SurfaceN].note.Count > 1)
        {
            var note = P[AM.PlaceN].surface[AM.SurfaceN].note[P[AM.PlaceN].surface[AM.SurfaceN].note.Count - 1];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_surface_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].surface[AM.SurfaceN].note[editIndex].id;
            AM.editIndex = false;
        }
        else
        {
            var note = P[AM.PlaceN].surface[AM.SurfaceN].note[editIndex];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_surface_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].surface[AM.SurfaceN].note[editIndex].id;
        }
        UpdatePlayerPrefs();
        AM.screens[4].GetComponent<fonSurfaceScript>().RecordSurface();
        AM.SwitchScreen(4);
    }//заметка поверхности

    public void SaveBox()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        Debug.Log("SaveBox");
        if (AM.editIndex && P[AM.PlaceN].box[AM.BoxN].note.Count > 1)
        {
            var note = P[AM.PlaceN].box[AM.BoxN].note[P[AM.PlaceN].box[AM.BoxN].note.Count - 1];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_box_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].box[AM.BoxN].note[editIndex].id;
            AM.editIndex = false;
        }
        else
        {
            var note = P[AM.PlaceN].box[AM.BoxN].note[editIndex];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_box_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].box[AM.BoxN].note[editIndex].id;
        }
        UpdatePlayerPrefs();
        AM.screens[5].GetComponent<fonBoxScript>().SaveBox();
        AM.SwitchScreen(5);
    }//заметка камеры

    public void SavePipe()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        Debug.Log("SavePipe");
        if (AM.editIndex && P[AM.PlaceN].pipe[AM.PipeN].note.Count > 1)
        {
            var note = P[AM.PlaceN].pipe[AM.PipeN].note[P[AM.PlaceN].pipe[AM.PipeN].note.Count - 1];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_pipe_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].pipe[AM.PipeN].note[editIndex].id;
            AM.editIndex = false;
        }
        else
        {
            var note = P[AM.PlaceN].pipe[AM.PipeN].note[editIndex];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_pipe_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].pipe[AM.PipeN].note[editIndex].id;
        }
        UpdatePlayerPrefs();
        AM.screens[6].GetComponent<fonPipeScript>().SavePipe();
        AM.SwitchScreen(6);
    }//заметка трубы

    public void SaveChannel()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        Debug.Log("SaveChannel");
        if (AM.editIndex && P[AM.PlaceN].channel[AM.ChannelN].note.Count > 1)
        {
            var note = P[AM.PlaceN].channel[AM.ChannelN].note[P[AM.PlaceN].channel[AM.ChannelN].note.Count - 1];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_channel_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].channel[AM.ChannelN].note[editIndex].id;
            AM.editIndex = false;
        }
        else
        {
            var note = P[AM.PlaceN].channel[AM.PipeN].note[AM.ChannelN];
            note.parent_type = parentType.text;
            note.name = name.text;
            note.note_channel_defect = defect.captionText.text;
            note.gpsx = Convert.ToSingle(gpsx.text);
            note.gpsy = Convert.ToSingle(gpsy.text);
            note.gpsz = Convert.ToSingle(gpsz.text);
            note.place_long_from = Convert.ToSingle(place_long_from.text);
            note.place_long_to = Convert.ToSingle(place_long_to.text);
            note.place_transverse_from = Convert.ToSingle(place_transverse_from.text);
            note.place_transverse_to = Convert.ToSingle(place_transverse_to.text);
            note.wall_thickness = Convert.ToSingle(wall_thickness.text);
            note.wall_hardness = Convert.ToSingle(wall_hardness.text);
            note.id = P[AM.PlaceN].channel[AM.ChannelN].note[editIndex].id;
        }
        UpdatePlayerPrefs();
        AM.screens[7].GetComponent<fonChannelScript>().SaveChannel();
        AM.SwitchScreen(7);
    }//заметка канала

    public void GPS()
    {
        gpsx.text = Convert.ToString(Input.location.lastData.longitude);
        gpsy.text = Convert.ToString(Input.location.lastData.latitude);
        gpsz.text = Convert.ToString(Input.location.lastData.altitude);
    }

    public void Media()
    {
        var AM = AppManager.Instance;
        AM.path2 = AM.choiseSurface;
        AM.openNoteMedia = true;
        AM.NoteN = editIndex;
        AM.nNoteMedia = true;
        AM.media.SetActive(true);
    }
}