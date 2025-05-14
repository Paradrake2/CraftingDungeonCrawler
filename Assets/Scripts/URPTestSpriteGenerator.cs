using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class URPTestSpriteGenerator : MonoBehaviour
{
    public Camera renderCam;
    public RenderTexture renderTexture;
    public GameObject prefabToRender;
    public UnityEngine.UI.Image targetIcon;
    void Start()
    {
        Sprite generated = GenerateIcon();
        targetIcon.sprite = generated;
    }
    public Sprite GenerateIcon()
    {
        // Clean instantiation at origin
        GameObject visual = Instantiate(prefabToRender, Vector3.zero, Quaternion.identity);
        SetLayerRecursively(visual, LayerMask.NameToLayer("IconCapture"));

        // Ensure the camera sees it
        renderCam.transform.position = new Vector3(0, 0, -10);
        renderCam.orthographic = true;
        renderCam.orthographicSize = 1.0f;
        renderCam.clearFlags = CameraClearFlags.SolidColor;
        renderCam.backgroundColor = new Color(0, 0, 0, 0);
        renderCam.cullingMask = 1 << LayerMask.NameToLayer("IconCapture");

        // Assign targetTexture explicitly
        renderCam.targetTexture = renderTexture;

        // Render and read
        renderCam.Render();

        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Destroy(visual);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
