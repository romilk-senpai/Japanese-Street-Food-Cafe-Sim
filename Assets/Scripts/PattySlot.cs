using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PattySlot : MonoBehaviour
{
    public event Action<PattySlot> OnPattyPlaced;
    public event Action<PattySlot> OnPattyRemoved;

    [SerializeField] private CanvasGroup highlightCanvasGroup;
    [SerializeField] private Transform snapTransform;

    private List<Patty> _pattiesInTrigger = new List<Patty>();
    private bool _hasPatty = false;
    private Patty _patty;

    public bool HasPatty => _hasPatty;
    public Patty Patty => _patty;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasPatty)
        {
            return;
        }

        var patty = other.GetComponent<Patty>();

        if (patty != null && !_pattiesInTrigger.Contains(patty) && !patty.Done)
        {
            _pattiesInTrigger.Add(patty);

            highlightCanvasGroup.DOFade(1f, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var patty = other.GetComponent<Patty>();

        if (patty != null && _pattiesInTrigger.Contains(patty))
        {
            _pattiesInTrigger.Remove(patty);

            if (_pattiesInTrigger.Count == 0)
            {
                highlightCanvasGroup.DOFade(0f, 0.5f);
            }

            if (patty == _patty)
            {
                _patty = null;
                _hasPatty = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_hasPatty)
            return;

        var patty = other.GetComponent<Patty>();

        if (patty != null && _pattiesInTrigger.Contains(patty))
        {
            if (!patty.Grabbable.BeingHeld)
            {
                SnapPatty(patty);
            }
        }
    }

    private void Update()
    {
        foreach (var patty in _pattiesInTrigger)
        {
            var pattyCollider = patty.GetComponent<Collider>();

            if (!pattyCollider.enabled)
                OnTriggerExit(pattyCollider);
        }
    }

    private void SnapPatty(Patty patty)
    {
        _hasPatty = true;
        _patty = patty;

        _patty.Grabbable.enabled = false;
        _patty.PattyRigidbody.isKinematic = true;
        _patty.transform.SetParent(snapTransform);
        _patty.transform.localPosition = Vector3.zero;
        _patty.transform.localRotation = Quaternion.identity;

        highlightCanvasGroup.DOFade(0f, 0.5f);
    }
}
