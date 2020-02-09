using TMPro;
using UnityEngine;

public class PrefEdit : MonoBehaviour
{
    public GameObject editPref;
    public TMP_Text id;
    public TMP_Text place;
    public TMP_Text defect;
    public Note data;
    public int index;
    bool isActive;


    public void Edit()
    {
        editPref.SetActive(true);
        isActive = true;
    }

    public void Back()
    {
        editPref.SetActive(false);
        isActive = false;
    }

    private void OnEnable()
    {
        if (isActive)
        {
            editPref.SetActive(false);
        }
    }

    private void ScrollCellIndex(int _index)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        try
        {
            switch (AM.way)
            {
                case 1:
                    data = P[AM.PlaceN].surface[AM.SurfaceN].note[_index];
                    index = _index; SetValues(); break;
                case 2:
                    data = P[AM.PlaceN].box[AM.BoxN].note[_index];
                    index = _index; SetValues(); break;
                case 3:
                    data = P[AM.PlaceN].pipe[AM.PipeN].note[_index];
                    index = _index; SetValues(); break;
                case 4:
                    data = P[AM.PlaceN].channel[AM.ChannelN].note[_index];
                    index = _index; SetValues(); break;
            }
        }
        catch { }
    }



    private void SetValues()
    {
        var AM = AppManager.Instance;
        id.text = "№ " + (index + 1);
        place.text = data.name;
        switch (AM.way)
        {
            case 1: defect.text = data.note_surface_defect; break;
            case 2: defect.text = data.note_box_defect; break;
            case 3: defect.text = data.note_pipe_defect; break;
            case 4: defect.text = data.note_channel_defect; break;
        }
    }



    public void DeleteThisNote()
    {
        Debug.Log("DeleteNote");
        var AM = AppManager.Instance;
        var P = AM.places.places;
        switch (AM.way)
        {
            case 1:
                var DSN = P[AM.PlaceN].surface[AM.SurfaceN].note;
                DSN.Remove(DSN[index]);
                AM.screens[4].GetComponent<fonSurfaceScript>().ViewPref();
                if (P[AM.PlaceN].surface[AM.SurfaceN].note.Count == 0)
                    AM.screens[4].GetComponent<fonSurfaceScript>().scroll.ClearCells();
                break;
            case 2:
                var DBN = P[AM.PlaceN].box[AM.BoxN].note;
                DBN.Remove(DBN[index]);
                AM.screens[5].GetComponent<fonBoxScript>().ViewPref();
                if (P[AM.PlaceN].box[AM.BoxN].note.Count == 0)
                    AM.screens[5].GetComponent<fonBoxScript>().scroll.ClearCells();
                break;
            case 3:
                var DPN = P[AM.PlaceN].pipe[AM.PipeN].note;
                DPN.Remove(DPN[index]);
                AM.screens[6].GetComponent<fonPipeScript>().ViewPref();
                if (P[AM.PlaceN].pipe[AM.PipeN].note.Count == 0)
                    AM.screens[6].GetComponent<fonPipeScript>().scroll.ClearCells();
                break;
            case 4:
                var DCN = P[AM.PlaceN].channel[AM.ChannelN].note;
                DCN.Remove(DCN[index]);
                AM.screens[7].GetComponent<fonChannelScript>().ViewPref();
                if (P[AM.PlaceN].channel[AM.ChannelN].note.Count == 0)
                    AM.screens[7].GetComponent<fonChannelScript>().scroll.ClearCells();
                break;
        }
    }

    public void Change()
    {
        var AM = AppManager.Instance;
        AM.SwitchScreen(10);
        switch (AM.way)
        {
            case 1:
                AM.screens[10].GetComponent<fonNotesScript>().GetLink(this, index, (_note) =>
                {
                    data = _note;
                    AppManager.Instance.SwitchScreen(4);
                    SetValues();
                });
                break;
            case 2:
                AM.screens[10].GetComponent<fonNotesScript>().GetLink(this, index, (_note) =>
                {
                    data = _note;
                    AppManager.Instance.SwitchScreen(5);
                    SetValues();
                });
                break;
            case 3:
                AM.screens[10].GetComponent<fonNotesScript>().GetLink(this, index, (_note) =>
                {
                    data = _note;
                    AppManager.Instance.SwitchScreen(6);
                    SetValues();
                });
                break;
            case 4:
                AM.screens[10].GetComponent<fonNotesScript>().GetLink(this, index, (_note) =>
                {
                    data = _note;
                    AppManager.Instance.SwitchScreen(7);
                    SetValues();
                });
                break;
        }
    }
}
