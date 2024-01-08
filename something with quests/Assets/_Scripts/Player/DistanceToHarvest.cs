using TMPro;
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
    [SerializeField] private TextMeshProUGUI harvestText;
    
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
        CheckForHarvestableDistance(transform.position, harvestDistance, _harvestLayerMask);
        if (_harvest.WasReleasedThisFrame())
        {
            ResetHarvestSlider();
        }
    }

    void CheckForHarvestableDistance(Vector3 center, float radius, int layerMask)
    {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
            harvestText.enabled = hitColliders.Length > 0;
            foreach (var hitCollider in hitColliders)
            {
                if (_harvest.IsPressed())
                {
                    Harvest(hitCollider);
                }
            }
    }
    
    private void Harvest(Collider hitCollider)
    {
        if (!hitCollider.gameObject.TryGetComponent(out IHarvestable harvestObj)) return;
        var position = transform.position;
        var harvestResult = harvestObj.Harvestable(position, harvestDistance);
        _harvestTime = harvestResult.Item2;

        if (harvestResult.Item1)
        {
            harvestSlider.gameObject.SetActive(true);
            harvestSlider.value = Mathf.Clamp01(harvestSlider.value - (1f / _harvestTime) * Time.deltaTime);

            if (harvestSlider.value <= 0f)
            {
                QuestManager.Instance.AdvanceCollectQuest(hitCollider.gameObject.tag);
                Destroy(hitCollider.gameObject);
                ResetHarvestSlider();
            }
        }else
        {
            ResetHarvestSlider();
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
