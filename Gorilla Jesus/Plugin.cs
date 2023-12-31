﻿using System;
using System.ComponentModel;
using BepInEx;
using UnityEngine;
using Utilla;

namespace GorillaJesus
{
    [Description("HauntedModMenu")]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool inRoom;

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            SetWaterObjectsLayer("Default");
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            SetWaterObjectsLayer("Water");
            inRoom = false;
        }

        void OnEnable()
        {
            if (!inRoom)
                return;

            SetWaterObjectsLayer("Default");
        }
        void OnDisable()
        {
            SetWaterObjectsLayer("Water");
        }
        private void SetWaterObjectsLayer(string layerName)
        {
            GameObject[] waterObjects;
            if(inRoom)
            {
                waterObjects = GameObject.FindGameObjectsWithTag("default");
            }
            else
            {
                waterObjects = GameObject.FindGameObjectsWithTag("Water");
            }

            foreach (GameObject waterObject in waterObjects)
            {
                waterObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }
}
