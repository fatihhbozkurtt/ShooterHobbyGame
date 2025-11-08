using System.Collections.Generic;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace EssentialManagers.CustomPackages.ResourceManager.Scripts
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        [Header("Debug")]
        [SerializeField] ResourceController[] resourceControllers;

        Dictionary<ResourceType, ResourceController> resourceDict;

        protected override void Awake()
        {
            base.Awake();

            resourceControllers = GetComponentsInChildren<ResourceController>(true);
            resourceDict = new Dictionary<ResourceType, ResourceController>();

            for (int i = 0; i < resourceControllers.Length; i++)
            {
                ResourceController resource = resourceControllers[i];
                resourceDict[resource.resourceType] = resource;
            }
        }

        public ResourceController GetResource(ResourceType resourceType)
        {
            return resourceDict[resourceType];
        }

        public Sprite GetResourceSprite(ResourceType resourceType)
        {
            ResourceController resource = GetResource(resourceType);
            return resource.iconSprite;
        }
    }
}
