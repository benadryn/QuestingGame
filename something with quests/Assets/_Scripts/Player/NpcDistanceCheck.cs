using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact(Vector3 transformPosition, float npcDistanceForDialog);
    public bool HasQuests();
}
public class NpcDistanceCheck : MonoBehaviour
{
    [SerializeField] private float npcDistanceForDialog = 3.0f;
    [SerializeField] private TextMeshProUGUI interactText;
    private int _npcQuestLayerMask;
    private bool _npcWithQuestsInRange;
    
    private PlayerControls _playerControls;
    private InputAction _interact;

    private void Awake()
    {
        _npcQuestLayerMask = LayerMask.GetMask("QuestNpc");
        _playerControls = new PlayerControls();
        _interact = _playerControls.Player.Interacting;
    }

    private void FixedUpdate()
    {
        NpcDialogDistance(transform.position, npcDistanceForDialog, _npcQuestLayerMask);
    }

    // checking distance from player to an npc
    void NpcDialogDistance(Vector3 center, float radius, int layerMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
        interactText.enabled = _npcWithQuestsInRange;

        if (hitColliders.Length <= 0)
        {
            _npcWithQuestsInRange = false;
        }
        foreach (var hitCollider in hitColliders)
        {
            TryInteractWithNpc(hitCollider);
            CheckNpcQuests(hitCollider);
        }
    }

    void TryInteractWithNpc(Collider collider)
    {
        _interact.performed += ctx =>
        {
            if (collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact(transform.position, npcDistanceForDialog);
            }
        };
    }

    void CheckNpcQuests(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            _npcWithQuestsInRange = interactObj.HasQuests();
        }
        
    }

    private void OnEnable()
    {
        
        _interact.Enable();
        
    }

    private void OnDisable()
    {
        _interact.Disable();
    }

}
