using Common;
using EnumCollection;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public static Pointer Instance;

    public Camera rtsCamera;

    private RaycastHit hitInfo;
    private Ray ray;

    #region Properties
    public TargetType TargetType { get; private set; }
    public Vector3 Position
    {
        get
        {
            return Vector3.ProjectOnPlane(transform.position, Vector3.up);
        }
        set
        {
            transform.position = value;
        }
    }
    public GameEntity TargetEntity { get; private set; }

    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(Instance.gameObject);
    }

    public void PutPointer()
    {
        ray = rtsCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray: ray,
                            hitInfo: out hitInfo,
                            maxDistance: Mathf.Infinity,
                            layerMask: LayerMask.GetMask("Place", "NPC", "Construct", "Floor", "UI")))
        {
            Position = hitInfo.point;
            int hitLayer = hitInfo.collider.gameObject.layer;
            TargetType = TargetType.None;
            TargetEntity = null;
            if ((hitLayer == LayerMask.NameToLayer("Place")) || (hitLayer == LayerMask.NameToLayer("Floor")))
            {
                TargetType = TargetType.Place;
            }
            else if (hitLayer == LayerMask.NameToLayer("NPC"))
            {
                TargetType = TargetType.NPC;
                TargetEntity = hitInfo.collider.gameObject.GetComponent<GameEntity>();
            }
            else if (hitLayer == LayerMask.NameToLayer("Construct"))
            {
                Construct targetHit = hitInfo.collider.gameObject.GetComponent<Construct>();
                if (targetHit.Player.Group == Group.NPC)
                {
                    TargetType = TargetType.NPC;
                }
                else
                {
                    TargetType = TargetType.Construct;
                }
                TargetEntity = targetHit.GetComponent<GameEntity>();
            }
            else if (hitLayer == LayerMask.NameToLayer("UI"))
            {
                TargetType = TargetType.None;
            }
        }
    }
}
