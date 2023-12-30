using UnityEngine;

public class Harvest : MonoBehaviour, IHarvestable
{
    [SerializeField] private float harvestTime = 1.5f;
    
    public (bool, float) Harvestable(Vector3 playerPosition, float harvestDistance)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= harvestDistance)
        {
            return (true, harvestTime);
        }

        return (false, 0.0f);
    }
}
