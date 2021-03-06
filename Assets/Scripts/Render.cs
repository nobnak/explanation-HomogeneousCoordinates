﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Render : MonoBehaviour {

    [SerializeField]
    protected TextureEvent Changed = new TextureEvent();
    [SerializeField]
    protected int lod = 0;

    protected RenderTexture captured = null;
    protected Camera attachedCam = null;

    private void Update() {
        var c = GetCamera();
        if (c != null) {
            var resolution = new Vector2Int(Screen.width, Screen.height);
            if (lod != 0)
                resolution = resolution.LOD(lod);
            //Debug.Log($"Resolution {resolution}");
            if (captured == null
                || captured.width != resolution.x
                || captured.height != resolution.y) {
                ReleaseCapturedTexture();
                captured = new RenderTexture(resolution.x, resolution.y, 24);
                SetTexture(captured);
            }
        }
    }
    private void OnDisable() {
        ReleaseCapturedTexture();
        attachedCam = null;
    }

    #region methods
    private void ReleaseCapturedTexture() {
        SetTexture(null);
        if (captured != null) {
            captured.DestroySelf();
            captured = null;
        }
    }

    private Camera GetCamera() {
        if (attachedCam == null)
            attachedCam = GetComponent<Camera>();
        return attachedCam;
    }

    private void SetTexture(RenderTexture captured) {
        var c = GetCamera();
        if (c != null) {
            c.targetTexture = captured;
        }

        Changed.Invoke(captured);
    }

    #endregion
}
