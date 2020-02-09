using System.IO;
using TMPro;
using UnityEngine;

public class MediaEdit : MonoBehaviour
{
    public GameObject editPref;
    public TMP_Text id;
    public TMP_Text type;
    public TMP_Text date;
    public TMP_Text name;
    public Files data;
    public int index;

    public void Edit()
    {
        editPref.SetActive(true);
    }

    public void Back()
    {
        editPref.SetActive(false);
    }

    private void ScrollCellIndex(int _index)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (!AM.openNoteMedia)
        {
            switch (AM.way)
            {
                case 1:
                    if (P[AM.PlaceN].surface[AM.SurfaceN].files.Count > 0)
                        data = P[AM.PlaceN].surface[AM.SurfaceN].files[_index]; break;
                case 2:
                    if (P[AM.PlaceN].box[AM.BoxN].files.Count > 0)
                        data = P[AM.PlaceN].box[AM.BoxN].files[_index]; break;
                case 3:
                    if (P[AM.PlaceN].pipe[AM.PipeN].files.Count > 0)
                        data = P[AM.PlaceN].pipe[AM.PipeN].files[_index]; break;
                case 4:
                    if (P[AM.PlaceN].channel[AM.ChannelN].files.Count > 0)
                        data = P[AM.PlaceN].channel[AM.ChannelN].files[_index]; break;
            }
            index = _index;
            Debug.Log("ScrollCellIndex = " + index);
            SetValues();
        }
        else
        {
            switch (AM.way)
            {
                case 1:
                    if (P[AM.PlaceN].surface[AM.SurfaceN].note[AM.NoteN].files.Count > 0)
                        data = P[AM.PlaceN].surface[AM.SurfaceN].note[AM.NoteN].files[_index]; break;
                case 2:
                    if (P[AM.PlaceN].box[AM.BoxN].note[AM.NoteN].files.Count > 0)
                        data = P[AM.PlaceN].box[AM.BoxN].note[AM.NoteN].files[_index]; break;
                case 3:
                    if (P[AM.PlaceN].pipe[AM.PipeN].note[AM.NoteN].files.Count > 0)
                        data = P[AM.PlaceN].pipe[AM.PipeN].note[AM.NoteN].files[_index]; break;
                case 4:
                    if (P[AM.PlaceN].channel[AM.ChannelN].note[AM.NoteN].files.Count > 0)
                        data = P[AM.PlaceN].channel[AM.ChannelN].note[AM.NoteN].files[_index]; break;
            }
            index = _index;
            Debug.Log("ScrollCellIndex = " + index);
            SetValues();
        }
    }


    private void SetValues()
    {
        id.text = "№ " + (data).target_id;
        type.text = data.type;
        date.text = data.date;
        name.text = data.name;
    }


    public void DeleteThisFile()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: DeleteSurfaceObject(); break;
            case 2: DeleteBoxObject(); break;
            case 3: DeletePipeObject(); break;
            case 4: DeleteChannelObject(); break;
        }
        Check();
    }

    public void DeleteSurfaceObject()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        if (P[AM.PlaceN].surface[AM.SurfaceN].files.Count > 0)
        {
            var DSF = P[AM.PlaceN].surface[AM.SurfaceN].files;
            DSF.Remove(DSF[index]); File.Delete(data.path); Debug.Log("del surface");
        }
    }

    public void DeleteBoxObject()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        if (P[AM.PlaceN].box[AM.BoxN].files.Count > 0)
        {
            var DBF = P[AM.PlaceN].box[AM.BoxN].files;
            DBF.Remove(DBF[index]); File.Delete(data.path);
        }
    }

    public void DeletePipeObject()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        if (P[AM.PlaceN].pipe[AM.PipeN].files.Count > 0)
        {
            var DPF = P[AM.PlaceN].pipe[AM.PipeN].files;
            DPF.Remove(DPF[index]); File.Delete(data.path);
        }
    }

    public void DeleteChannelObject()
    {
        var AM = AppManager.Instance;
        var P = AppManager.Instance.places.places;
        if (P[AM.PlaceN].channel[AM.ChannelN].files.Count > 0)
        {
            var DCF = P[AM.PlaceN].channel[AM.ChannelN].files;
            DCF.Remove(DCF[index]); File.Delete(data.path);
        }
    }

    public void Check()
    {
        var AM = AppManager.Instance;

        switch (AM.way)
        {
            case 1: CheckSurfaceZero(); break;
            case 2: CheckBoxZero(); break;
            case 3: CheckPipeZero(); break;
            case 4: CheckChannelZero(); break;
        }
        editPref.SetActive(false);
        AM.screens[9].GetComponent<fonMediaScript>().ViewPref();
    }

    public void CheckSurfaceZero()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (P[AM.PlaceN].surface[AM.SurfaceN].files.Count == 0)
        {
            AM.screens[9].GetComponent<fonMediaScript>().scroll.ClearCells();
        }
    }

    public void CheckBoxZero()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (P[AM.PlaceN].box[AM.BoxN].files.Count == 0)
        {
            AM.screens[9].GetComponent<fonMediaScript>().scroll.ClearCells();
        }
    }

    public void CheckPipeZero()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (P[AM.PlaceN].pipe[AM.PipeN].files.Count == 0)
        {
            AM.screens[9].GetComponent<fonMediaScript>().scroll.ClearCells();
        }
    }

    public void CheckChannelZero()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (P[AM.PlaceN].channel[AM.ChannelN].files.Count == 0)
        {
            AM.screens[9].GetComponent<fonMediaScript>().scroll.ClearCells();
        }
    }


    public void ChangeFile()
    {
        var AM = AppManager.Instance;
        AM.SwitchScreen(9);
        AM.screens[9].GetComponent<fonMediaScript>().GetLink(this, index, (_file) =>
        {
            data = _file;
            Debug.Log("sended index = " + index);
            AppManager.Instance.SwitchScreen(4);
            SetValues();
        });
    }
}