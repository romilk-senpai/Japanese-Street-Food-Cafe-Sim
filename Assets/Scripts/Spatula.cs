using BNG;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spatula : MonoBehaviour
{
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private SpatulaTrigger spatulaTrigger;
    [SerializeField] private Transform receiverRaycastTransform;
    [SerializeField] private Transform snapTransform;
    [SerializeField] private InputActionReference leftGrabAction;
    [SerializeField] private InputActionReference rightGrabAction;

    private SpatulaGrabbable _grabbedGrabbable;
    private ISpatulaGrabbableReceiver _receiver;
    private bool _grabbing;

    private void Start()
    {
        leftGrabAction.action.Enable();
        rightGrabAction.action.Enable();
    }

    private void Update()
    {
        if (!grabbable.BeingHeld)
        {
            if (_grabbing)
            {
                ReleaseGrabbable();
            }

            return;
        }

        var inputValue = 0f;

        if (grabbable.GetPrimaryGrabber().HandSide == ControllerHand.Left)
        {
            inputValue = leftGrabAction.action.ReadValue<float>();
        }
        else
        {
            inputValue = rightGrabAction.action.ReadValue<float>();
        }

        if (inputValue > .5f)
        {
            if (!_grabbing)
            {
                TryGrabGrabbable();
            }
        }
        else
        {
            if (_grabbing)
            {
                ReleaseGrabbable();
            }
        }

        if (_grabbing)
        {
            if (Physics.Raycast(receiverRaycastTransform.position, receiverRaycastTransform.forward, out var info, 1f))
            {
                var receiver = info.collider.GetComponent<ISpatulaGrabbableReceiver>();

                if (receiver != null)
                {
                    if (_receiver != null)
                    {
                        if (_receiver != receiver)
                        {
                            _receiver.HideHighlight();
                        }
                    }
                    else
                    {
                        _receiver = receiver;

                        _receiver.ShowHighlight();
                    }
                }
            }
            else
            {
                if (_receiver != null)
                {
                    _receiver.HideHighlight();

                    _receiver = null;
                }
            }
        }
    }

    private void TryGrabGrabbable()
    {
        if (spatulaTrigger.NearestGrabbable != null)
        {
            _grabbedGrabbable = spatulaTrigger.NearestGrabbable;

            _grabbedGrabbable.transform.SetParent(snapTransform);
            _grabbedGrabbable.transform.localPosition = Vector3.zero;
            _grabbedGrabbable.transform.localRotation = Quaternion.identity;

            _grabbedGrabbable.OnGrabbed();

            _grabbing = true;
        }
    }

    public void ForceReleaseGrabbable()
    {
        ReleaseGrabbable();
    }

    private void ReleaseGrabbable()
    {
        _grabbedGrabbable.transform.SetParent(null);

        _grabbedGrabbable.OnRelease();

        _grabbedGrabbable = null;

        _grabbing = false;
    }
}
