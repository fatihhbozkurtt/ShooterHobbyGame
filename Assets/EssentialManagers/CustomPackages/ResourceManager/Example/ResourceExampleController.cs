using System.Collections;
using EssentialManagers.CustomPackages.ResourceManager.Scripts;
using UnityEngine;

public class ResourceExampleController : MonoBehaviour
{
    public Transform spendTarget;

    ResourceController goldResource;

    private void Start()
    {
        goldResource = ResourceManager.instance.GetResource(ResourceType.Gold);
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return null;

        while (true)
        {
            goldResource.AddResource(5, transform.position, showAmount: true);
            yield return new WaitForSeconds(1);

            goldResource.AddResource(5, transform.position);
            yield return new WaitForSeconds(1);

            goldResource.AddResource(5, transform.position, callback: OnAddComplete);
            yield return new WaitForSeconds(1);

            goldResource.TrySpendResource(5, spendTarget, OnSpendComplete, showAmount: true);
            yield return new WaitForSeconds(1);

            goldResource.TrySpendResource(5, spendTarget, OnSpendComplete);
            yield return new WaitForSeconds(1);

            goldResource.AddScatteredResource(50, transform.position, callback: OnScatterAddComplete);
            yield return new WaitForSeconds(1);

            goldResource.AddScatteredResource(50, transform.position, scatterScale: 0.5f, iconCount: 100);
            yield return new WaitForSeconds(1);

            goldResource.AddScatteredResource(50, transform.position, scatterScale: 1.25f);
            yield return new WaitForSeconds(1);
        }
    }

    void OnSpendComplete(Transform target)
    {
        Debug.Log("spend completed: " + target.name);
    }

    void OnAddComplete()
    {
        Debug.Log("add complete.");
    }

    void OnScatterAddComplete()
    {
        Debug.Log("scatter add complete.");
    }
}
