﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class ReticleShownArea : MonoBehaviour {

    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    public GameController gameController;

    private void OnEnable() {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }


    private void OnDisable() {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    private void HandleOver() {
        //Debug.Log("Over Transit: ");
        //gameController.FillSelectionBar(3.0f);
        //gameController.PlayBlinkEffect();
        gameController.HideReticleDot(false);
    }

    private void HandleOut() {
        //gameController.CancelSelectionBar();
        //gameController.CancelBlinkTransit();
        gameController.HideReticleDot(true);
    }
}
