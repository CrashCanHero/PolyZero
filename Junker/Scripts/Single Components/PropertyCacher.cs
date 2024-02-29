using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public partial class PropertyCacher : Node {
    [Export]
    public bool ExcludeNodeDefaultProperties;

    [Export]
    public bool BlackListToWhiteList;

    [Export]
    public string[] PropertyBlacklist;

    protected Dictionary<string, Variant> properties { get; set; }

    string globalSavePath = ProjectSettings.GlobalizePath("user://Saves");
    string filePath(bool temp = false) => globalSavePath + "/" + ( temp ? "TMP.TMP" : GetParent().Name) + ".rpsav";

    public override void _Ready() {
        base._Ready();

        if (PropertyBlacklist == null) {
            PropertyBlacklist = new string[0];
        }

        if (GetParent() == null) {
            GD.PushError("PropertyCacher must have a parent to cache properties from!");
            return;
        }

        properties = new Dictionary<string, Variant>();

        CacheProperties();
    }

    public void CacheProperties(bool clearList = true) {
        if (clearList) {
            properties = new Dictionary<string, Variant>();
        }

        Godot.Collections.Array<Godot.Collections.Dictionary> props = GetParent().GetPropertyList();
        Godot.Collections.Array<Godot.Collections.Dictionary> defaultProps = new Node().GetPropertyList();

        Dictionary<string, Variant> output = new Dictionary<string, Variant>();
        for (int i = 0; i < props.Count; i++) {
            string name = (string)props[i]["name"];
            Variant val = GetParent().Get(name);

            //Anything Nil is not serializable
            if (val.VariantType == Variant.Type.Nil) {
                continue;
            }

            /*
            > Be me
            > Have so much nesting my code blocks stop getting brighter
            Why me man
            TODO: Gotta be a better way
            */
            if (ExcludeNodeDefaultProperties) {
                bool uhoh = false;
                for (int x = 0; x < defaultProps.Count; x++) {
                    if (name == (string)defaultProps[x]["name"]) {
                        uhoh = true;
                        break;;
                    }
                }

                if (uhoh) {
                    continue;
                }
            }

            //Now check the blacklist
            //> In its own scope for the uhoh bool
            {
                bool uhoh = false;
                for (int x = 0; x < PropertyBlacklist.Length; x++) {
                    if ((!BlackListToWhiteList && name == PropertyBlacklist[x]) || (BlackListToWhiteList && name != PropertyBlacklist[x])) {
                        uhoh = true;
                        break;
                    }
                }

                if (uhoh) {
                    continue;
                }
            }

            output.Add(name, val);
        }

        properties = output;

        GD.Print("Cached properties, got:");

        foreach(KeyValuePair<string, Variant> prop in properties) {
            GD.Print(prop.Key, " : ", prop.Value.VariantType, " : ", prop.Value.Obj);
        }
    }

    public void ResetProperties() {
        foreach(KeyValuePair<string, Variant> prop in properties) {
            GetParent().Set(prop.Key, prop.Value);
        }
    }

    public void ResetPropertiesDeferred() {
        foreach(KeyValuePair<string, Variant> prop in properties) {
            GetParent().SetDeferred(prop.Key, prop.Value);
        }
    }

    public void SaveToFile() {
        GD.Print("Being saving...");

        if (!Directory.Exists(globalSavePath)) {
            GD.Print("Creating save directory");
            Directory.CreateDirectory(globalSavePath);
        }

        PropertyData dta = new PropertyData {
            Names = new string[properties.Count],
            Variants = new Variant[properties.Count]
        };

        int index = 0;
        foreach (KeyValuePair<string, Variant> prop in properties) {
            //Only save what we can
            if (!prop.Value.GetType().IsSerializable) {
                GD.Print("Bad type: ", prop.Value.GetType());
                continue;
            }

            dta.Names[index] = prop.Key;
            dta.Variants[index] = prop.Value;
            index++;

            GD.Print("Writing ", prop.Key, " : ", prop.Value.GetType());
        }

        GD.Print("Writing temp file");
        string json = JsonSerializer.Serialize(dta);
        File.WriteAllText(filePath(true), json);

        if (File.Exists(filePath(true))) {
            GD.Print("Deleting previous save file");
            File.Delete(filePath());
        }

        GD.Print("Copying temp file to new save");
        File.Copy(filePath(true), filePath());

        GD.Print("Deleting temp file");
        File.Delete(filePath(true));
    }

    public void LoadFromFile(bool autoApply = true) {
        string globalSavePath = ProjectSettings.GlobalizePath("user://Saves");
        string filePath = globalSavePath + "/" + GetParent().Name + ".rpsav";

        if (!Directory.Exists(globalSavePath)) {
            Directory.CreateDirectory(globalSavePath);
        }

        if (!File.Exists(filePath)) {
            GD.PushError($"File {GetParent().Name} does not exist!");
            return;
        }

        string json = File.ReadAllText(filePath);

        PropertyData props = JsonSerializer.Deserialize<PropertyData>(json);

        properties = props.Convert();

        if (autoApply) {
            ResetProperties();
        }
    }

    [Serializable]
    public class PropertyData {
        public string[] Names { get; set; }
        public Variant[] Variants { get; set; }

        public Dictionary<string, Variant> Convert() {
            Dictionary<string, Variant> lib = new Dictionary<string, Variant>();

            for (int i = 0; i < Names.Length; i++) {
                lib.Add(Names[i], Variants[i]);
            }

            return lib;
        }
    }
}
