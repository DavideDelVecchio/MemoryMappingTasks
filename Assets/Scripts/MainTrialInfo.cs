﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class MainTrialInfo : ISerializable
{
    [Serializable()]
    public class InfoTrial : ISerializable { 
    
        public string ID;
        public float s_x;
        public float s_y;
        public float s_z;
        public float r_x;
        public float r_y;
        public float r_z;
        public float time;
        public float d;
        public string score;

        public InfoTrial() {
            ID = string.Empty;
            s_x = 0;
            s_y = 0;
            s_z = 0;
            r_x = 0;
            r_y = 0;
            r_z = 0;
            time = 0;
            d = 0;
            score = "0";
        }

           
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("posx", s_x);
            info.AddValue("posy", s_y);
            info.AddValue("posz", s_z);
            info.AddValue("rotx", r_x);
            info.AddValue("roty", r_y);
            info.AddValue("rotz", r_z);
            info.AddValue("time", time);
            info.AddValue("distance", d);
            info.AddValue("score", score);
            
        }
}
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
        }

}
