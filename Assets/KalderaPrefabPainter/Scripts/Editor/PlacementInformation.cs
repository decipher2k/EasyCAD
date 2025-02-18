﻿using System.Collections.Generic;
using UnityEngine;

namespace CollisionBear.WorldEditor
{
    [System.Serializable]
    public class PlacementInformation
    {
        public PaletteItem Item;
        public GameObject PrefabObject;
        public GameObject GameObject;
        public Vector3 Offset;
        public Quaternion Rotation;
        public float ScaleFactor;

        public Vector3 FixedOffset;
        public Vector3 NormalizedOffset;
        public Vector3 NormalizedHeightOffset;
        public Quaternion NormalizedRotation;
        public Vector3 NormalizedScale;

        public List<Collider> Colliders = new List<Collider>();

        public PlacementInformation(PaletteItem item, GameObject prefab, Vector3 offset, Vector3 rotationEuler, Vector3 scale, float scaleFactor)
        {
            Item = item;
            PrefabObject = prefab;
            FixedOffset = offset;
            NormalizedOffset = offset;
            Offset = offset;

            Rotation = Quaternion.Euler(rotationEuler);
            NormalizedRotation = Rotation;
            NormalizedScale = scale;
            ScaleFactor = scaleFactor;

            if (Item.AdvancedOptions.UsePrefabHeight) {
                var height = PrefabObject.transform.localPosition.y * ScaleFactor;
                NormalizedHeightOffset.y = height;
            }
        }

        public void RotateTowardsPosition(Vector3 position)
        {
            if (GameObject == null) {
                return;
            }

            position.y = GameObject.transform.position.y;
            GameObject.transform.LookAt(position);
            GameObject.transform.rotation *= Quaternion.Euler(Item.AdvancedOptions.RotationOffset);
            Rotation = GameObject.transform.rotation;
        }

        public void SetRotation(Vector3 eulerRotation)
        {
            Rotation = Quaternion.Euler(eulerRotation);

            if (GameObject == null) {
                return;
            }

            GameObject.transform.rotation = Rotation;
        }

        public GameObject CreatePlacementGameObject(Vector3 position, float scaleFactor)
        {
            if (PrefabObject == null) {
                return null;
            }

            var result = GameObject.Instantiate(PrefabObject, Vector3.zero, Rotation);
            result.hideFlags = HideFlags.HideAndDontSave;

            SetSaleFactor(scaleFactor);

            result.transform.localScale = NormalizedScale * scaleFactor;
            result.transform.position = position + Offset;
            ScaleFactor = scaleFactor;

            result.name = PrefabObject.name;
            result.SetActive(false);

            // Static object occasionally causes horrible performance 
            SetNonStaticRecursiveAndDisableCollider(result);

            GameObject = result;
            return result;
        }

        public void ClearPlacementGameObject()
        {
            if (GameObject == null) {
                return;
            }

            GameObject.DestroyImmediate(GameObject);
        }

        public void ReplacePlacementObject(int newVariantIndex, Vector3 position, float scaleFactor)
        {
            ClearPlacementGameObject();
            var validObjects = Item.ValidObjects();
            PrefabObject = validObjects[newVariantIndex];
            CreatePlacementGameObject(position, scaleFactor);
        }

        private void SetNonStaticRecursiveAndDisableCollider(GameObject gameObject)
        {
            gameObject.isStatic = false;
            foreach (var collider in gameObject.GetComponents<Collider>()) {
                collider.enabled = false;
            }

            foreach (Transform child in gameObject.transform) {
                SetNonStaticRecursiveAndDisableCollider(child.gameObject);
            }
        }

        public void SetNormalizedOffset(Vector3 offset)
        {
            NormalizedOffset = offset + NormalizedHeightOffset * ScaleFactor;
        }

        public void SetSaleFactor(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
            Offset = NormalizedOffset + NormalizedHeightOffset * scaleFactor;
        }
    }
}