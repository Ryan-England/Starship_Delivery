using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;

    // Corrected constructor where the parameter 'pc' is assigned to 'player_coordinates'
    public PlayerData(Vector3 pc)
    {
        position = new float[3];
        position[0] = pc.x;
        position[1] = pc.y;
        position[2] = pc.z;
    }
}
