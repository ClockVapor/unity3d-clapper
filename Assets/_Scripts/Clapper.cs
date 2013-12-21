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

public class Clapper : MonoBehaviour
{
    public string[] ClapGroupingMethods;
    public string ClapHeardMethod;
    public float AnalyzeInterval = 0.1f;
    public float ClapVolumeThreshold = 0.1f;
    public float ClapMinInterval = 0.2f;
    public float ClapGroupingTimeout = 0.5f;
    private float lastClap = 0;
    private int clapGroupSize = 0;

    private MicrophoneListener listener;
    private float analyzeTimer = 0;

    private void Awake()
    {
        listener = GetComponent<MicrophoneListener>();
    }

    private void Update()
    {
        // Analyze microphone audio for claps
        analyzeTimer += Time.deltaTime;
        if (analyzeTimer >= AnalyzeInterval)
        {
            analyzeTimer = 0;
            AnalyzeAudio(listener.GetNewClipData(), listener.Frequency);
        }

        // Timeout grouping if it's been a while since last clap
        if (Time.time >= lastClap + ClapGroupingTimeout)
        {
            GroupingTimeout();
        }
    }

    /// <summary>
    /// Looks through the given audio data for claps.
    /// </summary>
    /// <param name="data">The audio data.</param>
    /// <param name="frequency">The frequency of sampling for the audio data.</param>
    public void AnalyzeAudio(float[] data, int frequency)
    {
        for (int i = 0; i < data.Length; i++)
        {
            float clapTime;
            float clapVolume;

            if ((clapVolume = Mathf.Abs(data[i])) >= ClapVolumeThreshold
                && (clapTime = Time.time + (float) i / frequency) >= lastClap + ClapMinInterval)
            {
#if UNITY_EDITOR
                print("Clap");
#endif
                lastClap = clapTime;
                clapGroupSize++;
                if (!string.IsNullOrEmpty(ClapHeardMethod))
                {
                    SendMessage(ClapHeardMethod, clapVolume);
                }
            }
        }
    }

    /// <summary>
    /// Called when grouping for claps has timed out, and it's time to call the appropriate method for how many claps were heard.
    /// </summary>
    private void GroupingTimeout()
    {
        if (clapGroupSize > 0)
        {
#if UNITY_EDITOR
            print("Grouping timeout with " + clapGroupSize + " clap(s).");
#endif
            if (ClapGroupingMethods != null &&
                ClapGroupingMethods.Length >= clapGroupSize &&
                !string.IsNullOrEmpty(ClapGroupingMethods[clapGroupSize - 1]))
            {
                SendMessage(ClapGroupingMethods[clapGroupSize - 1]);
            }
        }

        clapGroupSize = 0;
    }
}
