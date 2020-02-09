using System;
using System.Collections.Generic;
using System.IO;



[Serializable]
public class AnswerPlace
{
    public List<Place> places = new List<Place>();
}


[Serializable]
public class F
{
    public byte[] file;
}


[Serializable]
public class AnswerValues
{
    public Allowed_values allowed_values = new Allowed_values();
}


[Serializable]
public class Place
{
    public int index;
    public bool saved = false;
    public bool edit = false;
    public string status;
    public string id;
    public string name;
    public string comment;
    public List<Surface> surface = new List<Surface>();
    public List<Box> box = new List<Box>();
    public List<Pipe> pipe = new List<Pipe>();
    public List<Channel> channel = new List<Channel>();
    public string way_type;
    public bool original;
    public string path;
    public string repl;


    public void ForeachSurface(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var surface in surface)
        {
            surface.SetTargetID(path, repl);
        }
    }

    public void ForeachNoteSurface(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var surface in surface)
        {
            surface.SetNoteSurfaceTargetID(path, repl);
        }
    }

    public void ForeachBox(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var box in box)
        {
            box.SetTargetID(path, repl);
        }
    }

    public void ForeachNoteBox(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var box in box)
        {
            box.SetNoteBoxTargetID(path, repl);
        }
    }

    public void ForeachPipe(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var pipe in pipe)
        {
            pipe.SetTargetID(path, repl);
        }
    }

    public void ForeachNotePipe(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var pipe in pipe)
        {
            pipe.SetNotePipeTargetID(path, repl);
        }
    }

    public void ForeachChannel(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var channel in channel)
        {
            channel.SetTargetID(path, repl);
        }
    }

    public void ForeachNoteChannel(string path, string repl)
    {
        this.path = path;
        this.repl = repl;
        foreach (var channel in channel)
        {
            channel.SetNoteChannelTargetID(path, repl);
        }
    }

    public Place(string _id, string _name, string _comment, string _way_type, string _status)
    {
        this.id = _id;
        this.name = _name;
        this.comment = _comment;
        this.way_type = _way_type;
        this.status = _status;
    }

    public Place() { }

}


[Serializable]
public class Surface
{
    public string id;
    public string name;
    public List<Files> files = new List<Files>();
    public string surface_type;
    public string terra_type;
    public string surface_humidity;
    public string surface_couldpass;
    public string surface_ground;
    public string surface_tok;
    public string surface_sodk;
    public float air_t;
    public float surface_t;
    public List<Note> note = new List<Note>();
    public string date;
    public string comment;
    public float ground_res;
    public bool saved = false;
    public bool edit = false;
    public int need_delete = 0;


    public void SetTargetID(string path, string repl)
    {
        foreach (var file in files)
        {
            if (file.path.Contains(path))
            {
                file.target_id = repl;
            }
        }
    }

    public void SetNoteSurfaceTargetID(string path, string repl)
    {
        foreach (var note in note)
        {
            note.SetTargetID(path, repl);
        }
    }

    public Surface(
    string _name, List<Files> _files, string _surface_type,
    string _terra_type, string _surface_Humidity, string _surface_couldpass,
    string _surface_Ground, string _surface_Tok, string _surface_Sodk,
    float _air_t, float _surface_t, List<Note> _note,
    string _comment, string _date, float _ground_res)
    {
        this.name = _name;
        this.files = _files;
        this.surface_type = _surface_type;
        this.terra_type = _terra_type;
        this.surface_humidity = _surface_Humidity;
        this.surface_couldpass = _surface_couldpass;
        this.surface_ground = _surface_Ground;
        this.surface_tok = _surface_Tok;
        this.surface_sodk = _surface_Sodk;
        this.air_t = _air_t;
        this.surface_t = _surface_t;
        this.note = _note;
        this.comment = _comment;
        this.date = _date;
        this.ground_res = _ground_res;
    }
    public Surface() { }
}


[Serializable]
public class Box
{
    public string id;
    public string name;
    public List<Files> files;
    public float gpsx;
    public float gpsy;
    public float gpsz;
    public string x;
    public string y;
    public string z;
    public float air_humidity;
    public float air_t;
    public List<Note> note;
    public string date;
    public string address;
    public string comment;
    public bool saved = false;
    public int need_delete = 0;

    public void SetTargetID(string path, string repl)
    {
        foreach (var file in files)
        {
            if (file.path.Contains(path))
            {
                file.target_id = repl;
            }
        }
    }

    public void SetNoteBoxTargetID(string path, string repl)
    {
        foreach (var note in note)
        {
            note.SetTargetID(path, repl);
        }
    }


    public Box(string _id, string _name, List<Files> _Files,
    float _gpsx, float _gpsy, float _gpsz,
    string _x, string _y, string _z,
    float _air_humidity, float _air_t, List<Note> _note,
    string _date, string _address, string _comment)
    {
        this.id = _id;
        this.name = _name;
        this.date = _date;
        this.files = _Files;
        this.gpsx = _gpsx;
        this.gpsy = _gpsy;
        this.gpsz = _gpsz;
        this.x = _x;
        this.y = _y;
        this.z = _z;
        this.air_humidity = _air_humidity;
        this.air_t = _air_t;
        this.note = _note;
        this.date = _date;
        this.comment = _comment;
        this.address = _address;
    }
    public Box() { }
}


[Serializable]
public class Pipe
{
    public string id;
    public string name;
    public string pipe_direction;
    public string pipe_isolate_type;
    public string pipe_metal_type;
    public string pipe_type;
    public List<Note> note;
    public string date;
    public float pipe_t;
    public float pipe_elec_pot;
    public float pipe_lenght;
    public float pipe_diametr;
    public string comment;
    public List<Files> files;
    public bool saved = false;
    public bool edit = false;
    public int need_delete = 0;

    public void SetTargetID(string path, string repl)
    {
        foreach (var file in files)
        {
            if (file.path.Contains(path))
            {
                file.target_id = repl;
            }
        }
    }

    public void SetNotePipeTargetID(string path, string repl)
    {
        foreach (var note in note)
        {
            note.SetTargetID(path, repl);
        }
    }

    public Pipe(string _id, string _name, string _pipe_Direction,
        string _pipe_isolate_Type, string pipe_metal_Type, string _pipe_Type,
        List<Note> _note, string _date, float _pipe_t,
        float _pipe_elec_pot, float _pipe_lenght, float _pipe_diametr,
        string _comment, List<Files> _files)
    {
        this.id = _id;
        this.name = _name;
        this.pipe_direction = _pipe_Direction;
        this.pipe_isolate_type = _pipe_isolate_Type;
        this.pipe_metal_type = pipe_metal_Type;
        this.pipe_type = _pipe_Type;
        this.note = _note;
        this.date = _date;
        this.pipe_t = _pipe_t;
        this.pipe_elec_pot = _pipe_elec_pot;
        this.pipe_lenght = _pipe_lenght;
        this.pipe_diametr = _pipe_diametr;
        this.comment = _comment;
        this.files = _files;
    }
    public Pipe() { }
}


[Serializable]
public class Channel
{
    public string id;
    public string date;
    public string comment;
    public string name;
    public string x;
    public string y;
    public List<Files> files;
    public string cha_type;
    public List<Note> note;
    public int need_delete = 0;

    public void SetTargetID(string path, string repl)
    {
        foreach (var file in files)
        {
            if (file.path.Contains(path))
            {
                file.target_id = repl;
            }
        }
    }

    internal void SetNoteChannelTargetID(string path, string repl)
    {
        foreach (var note in note)
        {
            note.SetTargetID(path, repl);
        }
    }


    public Channel(string _id, string _date, string _comment,
        string _name, string _x, string _y,
        List<Files> _Files, string _cha_type, List<Note> _Note)
    {
        this.id = _id;
        this.date = _date;
        this.comment = _comment;
        this.name = _name;
        this.x = _x;
        this.y = _y;
        this.files = _Files;
        this.cha_type = _cha_type;
        this.note = _Note;
    }
    public Channel() { }
}

[Serializable]
public class Note
{
    public string id;
    public string name; 
    public string note_box_defect;
    public string note_channel_defect;
    public string note_surface_defect;
    public string note_pipe_defect;
    public float gpsx;
    public float gpsy;
    public float gpsz;
    public float place_long_from;
    public float place_long_to;
    public float place_transverse_from;
    public float place_transverse_to;
    public float wall_thickness;
    public float wall_hardness;
    public string comment;
    public List<Files> files = new List<Files>();
    public string parent_type;

    public void SetTargetID(string path, string repl)
    {
        foreach (var file in files)
        {
            if (file.path.Contains(path))
            {
                file.target_id = repl;
            }
        }
    }

    public Note(string _name,
      string _note_box_defect,
      string _note_channel_defect,
      string _note_surface_defect,
      string _note_pipe_defect,
      float _gpsx,
      float _gpsy,
      float _gpsz,
      float _place_long_from,
      float _place_long_to,
      float _place_transverse_from,
      float _place_transverse_to,
      float _wall_thickness,
      float _wall_hardness,
      string _comment, List<Files> _Files,
      string _parent_type, string _id = null)
    {
        if (_id != null)
            id = _id;
        this.note_box_defect = _note_box_defect;
        this.note_channel_defect = _note_channel_defect;
        this.note_surface_defect = _note_surface_defect;
        this.note_pipe_defect = _note_pipe_defect;
        this.name = _name;
        this.gpsx = _gpsx;
        this.gpsy = _gpsy;
        this.gpsz = _gpsz;
        this.place_long_from = _place_long_from;
        this.place_long_to = _place_long_to;
        this.place_transverse_from = _place_transverse_from;
        this.place_transverse_to = _place_transverse_to;
        this.wall_thickness = _wall_thickness;
        this.wall_hardness = _wall_hardness;
        this.comment = _comment;
        this.files = _Files;
        this.parent_type = _parent_type;
    }
    public Note() { }
}

public class Defect
{
    public string key;
    public string value;
}

public class Element
{
    public string key;
    public string value;
}

[Serializable]
public class Files
{
    public string target_id;
    public string display;
    public string description;
    public string url;
    public string type;
    public string date;
    public string name;
    public string path;

    public Files(string _path)
    {
        var data = new FileInfo(_path);
        type = data.Extension;
        date = data.CreationTime.ToString();
        name = data.Name;
        path = _path;
    }
}


[Serializable]
public class Allowed_values
{
    public List<Surface_type> surface_type;
    public List<Cha_type> cha_type;
    public List<Terra_type> terra_type;
    public List<Pipe_direction> pipe_direction;
    public List<Pipe_isolate_type> pipe_isolate_type;
    public List<Pipe_metal_type> pipe_metal_type;
    public List<Pipe_type> pipe_type;
    public List<Surface_humidity> surface_humidity;
    public List<Surface_couldpass> surface_couldpass;
    public List<Surface_ground> surface_ground;
    public List<Surface_tok> surface_tok;
    public List<Surface_sodk> surface_sodk;
    public List<Note_box_defect> note_box_defect;
    public List<Note_channel_defect> note_channel_defect;
    public List<Note_surface_defect> note_surface_defect;
    public List<Note_channel_defect> note_pipe_defect;
    public List<Way_type> way_type;
    public string[] com_names;
}


[Serializable]
public class Surface_type
{
    public string key;
    public string value;
}

[Serializable]
public class Cha_type
{
    public string key;
    public string value;
}

[Serializable]
public class Terra_type
{
    public string key;
    public string value;
}

[Serializable]
public class Pipe_direction
{
    public string key;
    public string value;
}

[Serializable]
public class Pipe_isolate_type
{
    public string key;
    public string value;
}

[Serializable]
public class Pipe_metal_type
{
    public string key;
    public string value;
}

[Serializable]
public class Pipe_type
{
    public string key;
    public string value;
}

[Serializable]
public class Surface_humidity
{
    public string key;
    public string value;
}

[Serializable]
public class Surface_couldpass
{
    public string key;
    public string value;
}

[Serializable]
public class Surface_ground
{
    public string key;
    public string value;
}

[Serializable]
public class Surface_tok
{
    public string key;
    public string value;
}


[Serializable]
public class Surface_sodk
{
    public string key;
    public string value;
}


[Serializable]
public class Note_box_defect
{
    public string key;
    public string value;
}


[Serializable]
public class Note_channel_defect
{
    public string key;
    public string value;
}

[Serializable]
public class Note_surface_defect
{
    public string key;
    public string value;
}


[Serializable]
public class Note_pipe_defect
{
    public string key;
    public string value;
}

[Serializable]
public class Way_type
{
    public string key;
    public string value;

    public Way_type(string _key, string _value)
    {
        this.key = _key;
        this.value = _value;
    }
    public Way_type() { }
}


[Serializable]
public class Name
{
    public string name;
}


[Serializable]
public class searchids
{
    public string parentName;
    public string fullName;
    public string id;
    public string level1;
    public string level2;
    public string level3;
    public string level4;
    public string level5;
}

[Serializable]
public class Search
{
    public List <searchids> searchids = new List<searchids>();
    public levels levels = new levels();
}

[Serializable]
public class levels
{
    public string level1_name;
    public string level2_name;
    public string level3_name;
    public string level4_name;
    public string level5_name;
}

[Serializable]
public class FindID
{
    public List<UserSearch> levelss = new List<UserSearch>();
}

[Serializable]
public class UserSearch
{
    public string needLevel;
    public string needValue;

    public UserSearch(string _needLevel, string _needValue)
    {
        this.needLevel = _needLevel;
        this.needValue = _needValue;
    }

    UserSearch() { }
}