using System;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact(Vector3 transformPosition, float npcDistanceForDialog);
}
public class NpcDistanceCheck : MonoBehaviour
{
    [SerializeField] private float npcDistanceForDialog = 3.0f;
    private int _npcQuestLayerMask;
    
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

    // checking from player to an npc
    void NpcDialogDistance(Vector3 center, float radius, int layerMask)
    {
        var hitColliders = Physics.OverlapSphere(center, radius, layerMask);


        foreach (var hitCollider in hitColliders)
        {
            _interact.performed += ctx =>
            {
                if (hitCollider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact(transform.position, npcDistanceForDialog);
                }
            };
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
