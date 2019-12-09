using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Transform cameraTransform = null;

    [SerializeField]
    private Image foregroundImage;

    private CBaseManager _manager = null;

    private void Awake() {
        if (Camera.main != null) cameraTransform = Camera.main.transform;

        _manager = GetComponentInParent<CBaseManager>();
    }

    private void LateUpdate() {
        if (cameraTransform != null) {
            var cameraPos = cameraTransform.position;
            
            Vector3 targetPosition = new Vector3(transform.position.x,
                cameraPos.y,
                cameraPos.z);
            
            this.transform.LookAt(targetPosition);
        }

        foregroundImage.fillAmount = (float) _manager.Properties.Health / 3.0f;
    }
}