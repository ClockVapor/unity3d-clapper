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
using System.Collections.Generic;

public class ClapReaction : MonoBehaviour
{
    public string SongsFile = "songs.txt";

    private Color backgroundColor = Color.gray;
    private float colorDecay = 0.98f;
    private Dictionary<int, AudioFile> clapSongs;
    public GUIStyle TextStyle;

    private Clapper clapper;

    private void Awake()
    {
        clapper = GetComponent<Clapper>();
    }

    private void Start()
    {
        // Load player prefs
        if (PlayerPrefs.HasKey("volume"))
        {
            clapper.ClapVolumeThreshold = PlayerPrefs.GetFloat("volume");
        }
        if (PlayerPrefs.HasKey("interval"))
        {
            clapper.ClapMinInterval = PlayerPrefs.GetFloat("interval");
        }
        if (PlayerPrefs.HasKey("timeout"))
        {
            clapper.ClapGroupingTimeout = PlayerPrefs.GetFloat("timeout");
        }

        Camera.main.backgroundColor = backgroundColor;
        ParseSongsFile();
    }

    private void Update()
    {
        // Fade background color
        Camera.main.backgroundColor *= colorDecay;
        Camera.main.backgroundColor = new Color(
            Mathf.Clamp(Camera.main.backgroundColor.r, backgroundColor.r, 1),
            Mathf.Clamp(Camera.main.backgroundColor.g, backgroundColor.g, 1),
            Mathf.Clamp(Camera.main.backgroundColor.b, backgroundColor.b, 1));

        // Get input to adjust clap threshold
        if (Input.GetKeyDown(KeyCode.A))
        {
            clapper.ClapVolumeThreshold += 0.005f;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            clapper.ClapVolumeThreshold -= 0.005f;
        }
        clapper.ClapVolumeThreshold = Mathf.Clamp(clapper.ClapVolumeThreshold, 0.01f, 1);

        // Get input to adjust clap timeout
        if (Input.GetKeyDown(KeyCode.D))
        {
            clapper.ClapGroupingTimeout += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            clapper.ClapGroupingTimeout -= 0.1f;
        }
        clapper.ClapGroupingTimeout = Mathf.Clamp(clapper.ClapGroupingTimeout, 0.1f, 1);
        clapper.ClapMinInterval = Mathf.Clamp(clapper.ClapMinInterval, 0, clapper.ClapGroupingTimeout - 0.1f);

        // Get input to adjust clap interval
        if (Input.GetKeyDown(KeyCode.S))
        {
            clapper.ClapMinInterval += 0.05f;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            clapper.ClapMinInterval -= 0.05f;
        }
        clapper.ClapMinInterval = Mathf.Clamp(clapper.ClapMinInterval, 0, 0.9f);
        clapper.ClapGroupingTimeout = Mathf.Clamp(clapper.ClapGroupingTimeout, clapper.ClapMinInterval + 0.1f, 1);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, -80, Screen.width, Screen.height), "Clap amplitude threshold (A/Z keys to modify): " + clapper.ClapVolumeThreshold.ToString(), TextStyle);
        GUI.Label(new Rect(0, -20, Screen.width, Screen.height), "Clap min interval (S/X keys to modify): " + clapper.ClapMinInterval.ToString() + " seconds", TextStyle);
        GUI.Label(new Rect(0, 40, Screen.width, Screen.height), "Clap grouping timeout (D/C keys to modify): " + clapper.ClapGroupingTimeout.ToString() + " seconds", TextStyle);

        clapper.ClapVolumeThreshold = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 60, 200, 20), clapper.ClapVolumeThreshold, 0.01f, 1.0f);
        clapper.ClapMinInterval = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 20), clapper.ClapMinInterval, 0, 1);
        clapper.ClapMinInterval = Mathf.Clamp(clapper.ClapMinInterval, 0, 0.9f);
        clapper.ClapGroupingTimeout = Mathf.Clamp(clapper.ClapGroupingTimeout, clapper.ClapMinInterval + 0.1f, 1);
        clapper.ClapGroupingTimeout = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 60, 200, 20), clapper.ClapGroupingTimeout, 0, 1);
        clapper.ClapGroupingTimeout = Mathf.Clamp(clapper.ClapGroupingTimeout, 0.1f, 1);
        clapper.ClapMinInterval = Mathf.Clamp(clapper.ClapMinInterval, 0, clapper.ClapGroupingTimeout - 0.1f);
    }

    private void OnApplicationQuit()
    {
        // Save player prefs
        PlayerPrefs.SetFloat("volume", clapper.ClapVolumeThreshold);
        PlayerPrefs.SetFloat("interval", clapper.ClapMinInterval);
        PlayerPrefs.SetFloat("timeout", clapper.ClapGroupingTimeout);
        PlayerPrefs.Save();
    }

    private void ParseSongsFile()
    {
        clapSongs = new Dictionary<int, AudioFile>();

        string songsText = System.IO.File.ReadAllText(SongsFile).Trim();

        string[] lines = songsText.Split(new string[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] tokens = line.Trim().Split('@');

            // Get number of claps
            int numClaps = int.Parse(tokens[0].Trim());

            // Get song file
            string songFile = tokens[1].Trim();

#if UNITY_EDITOR
            print(numClaps + " claps: " + songFile);
#endif

            clapSongs.Add(numClaps, new AudioFile(songFile));
        }
    }

    public void Clap(float volume)
    {
        Camera.main.backgroundColor += 2 * volume * Color.white;
    }

    public void ClapGroup(int numClaps)
    {
        if (clapSongs.ContainsKey(numClaps))
        {
            if (audio.isPlaying &&
                audio.clip == clapSongs[numClaps].Clip)
            {
                audio.Stop();
            }
            else
            {
                audio.clip = clapSongs[numClaps].Clip;
                audio.Play();
            }
        }
    }
}
