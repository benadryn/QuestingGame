using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

interface IHarvestable
{
    public (bool, float) Harvestable(Vector3 playerPosition, float harvestDistance);
}

public class DistanceToHarvest : MonoBehaviour
{

    [SerializeField] private Slider harvestSlider;
    [SerializeField] private float harvestDistance = 2.0f;
    
    private int _harvestLayerMask;
    private PlayerControls _playerControls;
    private InputAction _harvest;
    private float _harvestTime = 0f;


    private void Awake()
    {
        _harvestLayerMask = LayerMask.GetMask("Harvestable");
        _playerControls = new PlayerControls();
        _harvest = _playerControls.Player.Harvest;
        harvestSlider.gameObject.SetActive(false);

        

    }

    private void FixedUpdate()
    {
        if (_harvest.IsPressed())
        {
            CheckForHarvestableDistance(transform.position, harvestDistance, _harvestLayerMask);
        }

        if (_harvest.WasReleasedThisFrame())
        {
            ResetHarvestSlider();
        }
    }

    void CheckForHarvestableDistance(Vector3 center, float radius, int layerMask)
    {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
            foreach (var hitCollider in hitColliders)
            {
                Harvest(hitCollider);
            }
    }
    
    private void Harvest(Collider hitCollider)
    {
        if (hitCollider.gameObject.TryGetComponent(out IHarvestable harvestObj))
        {
            var position = transform.position;
            var harvestResult = harvestObj.Harvestable(position, harvestDistance);
            _harvestTime = harvestResult.Item2;

            if (harvestResult.Item1)
            {
                harvestSlider.gameObject.SetActive(true);
                harvestSlider.value = Mathf.Clamp01(harvestSlider.value - (1f / _harvestTime) * Time.deltaTime);

                if (harvestSlider.value <= 0f)
                {
                    Destroy(hitCollider.gameObject);
                    ResetHarvestSlider();
                }
            }
        }
    }

    private void ResetHarvestSlider()
    {
        harvestSlider.value = 1;
        harvestSlider.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        _harvest.Enable();
    }

    private void OnDisable()
    {
        _harvest.Disable();
    }
}
