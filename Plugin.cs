using System;
using System.Diagnostics;
using BepInEx;
using UnityEngine;
using Utilla;

namespace GorillaTagModTemplateProject
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
        }

        void Update()
        {
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            SetWaterObjectsLayer("Default");
            AddMeshCollidersToWaterObjects();
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            SetWaterObjectsLayer("Water");
            RemoveMeshCollidersFromWaterObjects();
        }

        private void AddMeshCollidersToWaterObjects()
        {
            if (!inRoom)
                return;

            // Get all objects with the layer "Water"
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");

            // Iterate through each water object
            foreach (GameObject waterObject in waterObjects)
            {
                // Add mesh collider if not already present
                if (waterObject.GetComponent<MeshCollider>() == null)
                {
                    MeshFilter meshFilter = waterObject.GetComponent<MeshFilter>();

                    // Create mesh collider if a mesh filter component is present
                    if (meshFilter != null && meshFilter.sharedMesh != null)
                    {
                        waterObject.AddComponent<MeshCollider>();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void RemoveMeshCollidersFromWaterObjects()
        {
            if (!inRoom)
                return;

            // Get all objects with the layer "Water"
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");

            // Iterate through each water object
            foreach (GameObject waterObject in waterObjects)
            {
                // Remove mesh collider if present
                MeshCollider meshCollider = waterObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    Destroy(meshCollider);
                }
            }
        }

        private void SetWaterObjectsLayer(string layerName)
        {
            // Get all objects with the layer "Water"
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");

            // Iterate through each water object
            foreach (GameObject waterObject in waterObjects)
            {
                waterObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }
}
