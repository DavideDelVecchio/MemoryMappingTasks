using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable()]
public class CompletePath : ISerializable
{

    [Serializable()]
    public class PathMapping : ISerializable
    {

        public string name;
        public float s_x;
        public float s_y;
        public float s_z;
        public float r_x;
        public float r_y;
        public float r_z;
        public float t;

        public PathMapping()
        {
            //name = "Sub01";
            name = PlayerPrefs.GetString("SubjID");
            s_x = 0;
            s_y = 0;
            s_z = 0;
            r_x = 0;
            r_y = 0;
            r_z = 0;
            t = 0;

        }


        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", name);
            info.AddValue("posx", s_x);
            info.AddValue("posy", s_y);
            info.AddValue("posz", s_z);
            info.AddValue("rotx", r_x);
            info.AddValue("roty", r_y);
            info.AddValue("rotz", r_z);
            info.AddValue("time", t);
        }
    }
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
    }



}


