using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    public Camera mainCamera;
    public Camera backgroundCamera;

    [SerializeField]
    private Vector2 scrollSpeed = new Vector2(0.01f, 0);
    private Material[] layers;
    private Vector2[] layerSpeeds;

    void Awake() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        layers = meshRenderer.materials;
        layerSpeeds = new Vector2[] { scrollSpeed, 1.28f * scrollSpeed, 1.66f * scrollSpeed, 1.89f * scrollSpeed, 2f * scrollSpeed };

        float width = meshRenderer.material.mainTexture.width * 1f;
        float height = meshRenderer.material.mainTexture.height * 1f;
        float ratio = width / height;

        //backgroundCamera.aspect = ratio;
        float cameraHeight = backgroundCamera.orthographicSize * 2;
        Vector3 scale = new Vector3(ratio * cameraHeight, cameraHeight, 1);

        transform.position = backgroundCamera.transform.position;
        transform.Translate(new Vector3(0,0,1));
        transform.localScale = scale;
    }
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < layers.Length; i++) {
            Vector2 offset = new Vector2(mainCamera.transform.position.z * layerSpeeds[i].x, 0 * layerSpeeds[i].y);
            layers[i].mainTextureOffset = offset;
        }
	}
}
