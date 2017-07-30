using System;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class SecondaCamera : MonoBehaviour
{
	// Token: 0x06000695 RID: 1685 RVA: 0x000E8B6C File Offset: 0x000E6D6C
	private void Start()
	{
		this.primaCamera = GameObject.Find("Prima Camera");
		this.canvasManuale = GameObject.FindGameObjectWithTag("CanvasManuale");
		this.raggioLimiteMovimento = this.primaCamera.GetComponent<PrimaCamera>().raggioLimiteMovimento;
		this.questaCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000E8BBC File Offset: 0x000E6DBC
	private void Update()
	{
		this.movimentoABinario = Vector3.ProjectOnPlane(base.transform.up, Vector3.up).normalized;
		if (this.primaCamera.GetComponent<PrimaCamera>().cameraAttiva == 2 && this.canvasManuale.GetComponent<CanvasGroup>().alpha == 0f)
		{
			this.distDaCentroMappa = Vector3.Distance(base.transform.position, Vector3.zero);
			if (this.distDaCentroMappa <= this.raggioLimiteMovimento && !this.primaCamera.GetComponent<PrimaCamera>().fermaVisuale)
			{
				this.MovimentoCamera();
			}
			else
			{
				base.transform.position += (Vector3.zero - base.transform.position).normalized * 100f * Time.deltaTime;
				if (this.distDaCentroMappa > this.raggioLimiteMovimento + 100f)
				{
					base.transform.position = new Vector3(0f, 200f, 0f);
				}
			}
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x000E8CE8 File Offset: 0x000E6EE8
	private void MovimentoCamera()
	{
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.position += this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			base.transform.position += -this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.position += -base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.position += base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (!Input.GetKey(KeyCode.LeftAlt))
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0f && this.questaCamera.orthographicSize < 700f)
			{
				this.questaCamera.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * this.sensibilitàZoom * Time.unscaledDeltaTime;
			}
			else if (Input.GetAxis("Mouse ScrollWheel") > 0f && this.questaCamera.orthographicSize > 50f)
			{
				this.questaCamera.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * this.sensibilitàZoom * Time.unscaledDeltaTime;
			}
		}
	}

	// Token: 0x0400187C RID: 6268
	public float sensibilitàCameraTraslazione;

	// Token: 0x0400187D RID: 6269
	public float sensibilitàCameraRotazione;

	// Token: 0x0400187E RID: 6270
	public float sensibilitàZoom;

	// Token: 0x0400187F RID: 6271
	private Vector3 rotazioneTemporanea;

	// Token: 0x04001880 RID: 6272
	private Vector3 movimentoABinario;

	// Token: 0x04001881 RID: 6273
	private GameObject primaCamera;

	// Token: 0x04001882 RID: 6274
	private GameObject canvasManuale;

	// Token: 0x04001883 RID: 6275
	private float raggioLimiteMovimento;

	// Token: 0x04001884 RID: 6276
	private float distDaCentroMappa;

	// Token: 0x04001885 RID: 6277
	private Camera questaCamera;
}
