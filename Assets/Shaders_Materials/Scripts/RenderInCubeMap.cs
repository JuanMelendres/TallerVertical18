using UnityEngine;
using System.Collections;

// Attach this script to an object that uses a Reflective shader.
// Realtime reflective cubemaps!

[ExecuteInEditMode]
public class RenderInCubeMap : MonoBehaviour
{	
	public int CubeMapSize = 128;
	public bool OneFacePerFrame = false;
	public bool CreateStaticCubeMap = false;

	private Camera cam;
	private RenderTexture rtex;

	void Start()
	{
		// render all six faces at startup
		this.UpdateCubemap(63);
    }

	void LateUpdate()
	{
		if (this.CreateStaticCubeMap == true)
			return;

		if (this.OneFacePerFrame)
		{
			int faceToRender = Time.frameCount % 6;
			int faceMask = 1 << faceToRender;
			this.UpdateCubemap(faceMask);
		}
		else
		{
			// We render the six faces in one frame.
			this.UpdateCubemap(63); 
		}
	}

	void UpdateCubemap(int faceMask)
	{
		if (!this.cam)
		{
			GameObject go = new GameObject("CubemapCamera", typeof(Camera));
			go.hideFlags = HideFlags.HideAndDontSave;
			go.transform.position = transform.position;
			go.transform.rotation = Quaternion.identity;
			this.cam = go.GetComponent<Camera>();
			this.cam.farClipPlane = 100; // don't render very far into cubemap
			this.cam.enabled = false;
		}

		if (!this.rtex)
		{
			this.rtex = new RenderTexture(this.CubeMapSize, this.CubeMapSize, 16);
			this.rtex.isCubemap = true;
			this.rtex.hideFlags = HideFlags.HideAndDontSave;
			this.GetComponent<Renderer>().sharedMaterial.SetTexture("_CubeMap", this.rtex);
		}

		this.cam.transform.position = this.transform.position;
		this.cam.RenderToCubemap(this.rtex, faceMask);
	}

	void OnDisable()
	{
		DestroyImmediate(this.cam);
		DestroyImmediate(this.rtex);
	}
}
