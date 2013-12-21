/************************************************************************

 Unity3D Clapper
 Copyright (C) 2013 Nick Lowery (nick.a.lowery@gmail.com)

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 www.gnu.org/copyleft/gpl.html

************************************************************************/

using UnityEngine;
using System.Collections;

public class ClapReaction : MonoBehaviour
{
    private Color backgroundColor = Color.black;
    private float colorDecay = 0.98f;

    private Clapper clapper;

    private void Awake()
    {
        clapper = GetComponent<Clapper>();
    }

    private void Update()
    {
        backgroundColor *= colorDecay;
        Camera.main.backgroundColor = backgroundColor;

        if (Input.GetKeyDown(KeyCode.A))
        {
            clapper.ClapVolumeThreshold += 0.01f;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            clapper.ClapVolumeThreshold -= 0.01f;
        }

        clapper.ClapVolumeThreshold = Mathf.Clamp(clapper.ClapVolumeThreshold, 0.01f, 1);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 500, 50), "Clap volume threshold (A/Z keys to modify): " + clapper.ClapVolumeThreshold.ToString());
    }

    public void Clap(float volume)
    {
        backgroundColor += 2 * volume * Color.white;
    }

    public void Claps2()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        else
        {
            audio.Stop();
        }
    }
}
