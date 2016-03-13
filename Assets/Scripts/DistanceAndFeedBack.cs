using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class DistanceAndFeedBack : ISerializable
{
    [Serializable()]
    public class FeedbackInfo : ISerializable
    {

        public string ID;
        public float g_x;
        public float g_y;
        public float g_z;
        public float e_x;
        public float e_y;
        public float e_z;
        public float distance;
        public string color;

        public FeedbackInfo()
        {
            ID = string.Empty;
            g_x = 0;
            g_y = 0;
            g_z = 0;
            e_x = 0;
            e_y = 0;
            e_z = 0;
            distance = 0;
            color = string.Empty;
        }


        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("GoalPosx", g_x);
            info.AddValue("GoalPosy", g_y);
            info.AddValue("GoalPosz", g_z);
            info.AddValue("EndPosx", e_x);
            info.AddValue("EndPosy", e_y);
            info.AddValue("EndPosz", e_z);
            info.AddValue("Distance", distance);
            info.AddValue("FeedbackGiven", color);
        }
    }
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
    }

}

