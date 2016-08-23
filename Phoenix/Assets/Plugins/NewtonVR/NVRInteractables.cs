﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NewtonVR
{
    public class NVRInteractables : MonoBehaviour
    {
        private static Dictionary<Collider, NVRInteractable> ColliderMapping;
        //edited; make static if broken
        private static Dictionary<NVRInteractable, Collider[]> NVRInteractableMapping;

#pragma warning disable 0414
        private static bool Initialized = false;
#pragma warning restore 0414

        public static void Initialize()
        {
            ColliderMapping = new Dictionary<Collider, NVRInteractable>();
            NVRInteractableMapping = new Dictionary<NVRInteractable, Collider[]>();

            Initialized = true;
        }

        public static void Register(NVRInteractable interactable, Collider[] colliders)
        {

            NVRInteractableMapping.Add(interactable, colliders);

            for (int index = 0; index < colliders.Length; index++)
            {
                ColliderMapping.Add(colliders[index], interactable);
            }
        }

        public static void Deregister(NVRInteractable interactable)
        {
            NVRPlayer.DeregisterInteractable(interactable);

            ColliderMapping = ColliderMapping.Where(mapping => mapping.Value != interactable).ToDictionary(mapping => mapping.Key, mapping => mapping.Value);
            NVRInteractableMapping.Remove(interactable);
        }

        public static NVRInteractable GetInteractable(Collider collider)
        {
            NVRInteractable interactable;
            ColliderMapping.TryGetValue(collider, out interactable);
            return interactable;
        }
    }
}