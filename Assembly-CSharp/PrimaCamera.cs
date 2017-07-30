using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class PrimaCamera : MonoBehaviour
{
	// Token: 0x0600068E RID: 1678 RVA: 0x000E78A4 File Offset: 0x000E5AA4
	private void Start()
	{
		this.secondaCamera = GameObject.Find("Seconda Camera");
		this.terzaCamera = GameObject.Find("Terza Camera");
		this.infoAlleati = GameObject.FindGameObjectWithTag("Info Alleati");
		this.varieMAppaLocale = GameObject.FindGameObjectWithTag("VarieMappaLocale");
		this.canvasManuale = GameObject.FindGameObjectWithTag("CanvasManuale");
		this.impostazioni = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Impostazioni").gameObject;
		this.scrittaRiposiz = GameObject.FindGameObjectWithTag("CanvasComandante").transform.FindChild("Scritte Varie").FindChild("riposizionamento camera").gameObject;
		this.cameraAttiva = 1;
		this.oggettoCameraAttiva = base.gameObject;
		this.secondaCamera.GetComponent<Camera>().enabled = false;
		this.secondaCamera.GetComponent<AudioListener>().enabled = false;
		this.terzaCamera.GetComponent<Camera>().enabled = false;
		this.terzaCamera.GetComponent<AudioListener>().enabled = false;
		this.altezzaPerTrasparimentoOgg = this.varieMAppaLocale.GetComponent<VarieMappaLocale>().altezzaPerTrasparimentoOgg;
		this.ListaOggDaTrasparireConSecondaCam = new List<GameObject>();
		foreach (GameObject current in this.varieMAppaLocale.GetComponent<VarieMappaLocale>().ListaOggDaTrasparireConSecondaCam)
		{
			this.ListaOggDaTrasparireConSecondaCam.Add(current);
		}
		this.sensRotCam = PlayerPrefs.GetFloat("sensibilità rotazione camera") / 100f;
		this.layerAmbiente = 4194560;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x000E7A54 File Offset: 0x000E5C54
	private void Update()
	{
		this.CambioCamera();
		if (this.canvasManuale.GetComponent<CanvasGroup>().alpha == 1f || this.impostazioni.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.fermaVisuale = true;
		}
		if (this.canvasManuale.GetComponent<CanvasGroup>().alpha == 0f && this.impostazioni.GetComponent<CanvasGroup>().alpha == 0f)
		{
			this.fermaVisuale = false;
		}
		if (this.cameraAttiva == 1 && this.canvasManuale.GetComponent<CanvasGroup>().alpha == 0f)
		{
			this.distDaCentroMappa = Vector3.Distance(base.transform.position, Vector3.zero);
			if (this.distDaCentroMappa <= this.raggioLimiteMovimento && Time.timeSinceLevelLoad > 0.3f && !this.fermaVisuale)
			{
				this.MovimentoCamera();
			}
			else
			{
				base.transform.position += (Vector3.zero - base.transform.position).normalized * 100f * Time.deltaTime;
				if (this.distDaCentroMappa > this.raggioLimiteMovimento + 100f)
				{
					base.transform.position = new Vector3(0f, 100f, 0f);
				}
			}
		}
		this.movimentoABinario = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		if (this.oggettoCameraAttiva.transform.position.y > this.altezzaPerTrasparimentoOgg)
		{
			foreach (GameObject current in this.ListaOggDaTrasparireConSecondaCam)
			{
				current.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		else
		{
			foreach (GameObject current2 in this.ListaOggDaTrasparireConSecondaCam)
			{
				current2.GetComponent<MeshRenderer>().enabled = true;
			}
		}
		if (this.morteAvvenuta)
		{
			this.VerificaCameraPostMorte();
			this.morteAvvenuta = false;
		}
		this.Riposizionamento();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x000E7CF8 File Offset: 0x000E5EF8
	private void MovimentoCamera()
	{
		if (Input.GetKey(KeyCode.W))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.GetKey(KeyCode.S))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += -this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += -this.movimentoABinario * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.GetKey(KeyCode.A))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += -base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += -base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += base.transform.right * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.GetKey(KeyCode.Space))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += -Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime * 0.05f;
			}
			else
			{
				base.transform.position += -Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
			}
		}
		if (Input.mousePosition.x > (float)Screen.width * 0.99f)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.Rotate(Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * 0.1f);
			}
			else
			{
				base.transform.Rotate(Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
			}
		}
		if (Input.mousePosition.x < (float)Screen.width * 0.01f)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.Rotate(-Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * 0.1f);
			}
			else
			{
				base.transform.Rotate(-Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
			}
		}
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		float num = Vector3.Dot(base.transform.up, normalized);
		if (Input.mousePosition.y > (float)Screen.height * 0.99f && num > -this.limiteAngSuGiù)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.Rotate(-Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * 0.1f);
			}
			else
			{
				base.transform.Rotate(-Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
			}
		}
		if (Input.mousePosition.y < (float)Screen.height * 0.01f && num < this.limiteAngSuGiù)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.Rotate(Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * 0.1f);
			}
			else
			{
				base.transform.Rotate(Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
			}
		}
		if (!Input.GetKey(KeyCode.LeftAlt))
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				base.transform.position += base.transform.forward * Input.GetAxis("Mouse ScrollWheel") * this.sensibilitàZoom * Time.unscaledDeltaTime * 10f;
			}
			else
			{
				base.transform.position += base.transform.forward * Input.GetAxis("Mouse ScrollWheel") * this.sensibilitàZoom * Time.unscaledDeltaTime * 1000f;
			}
		}
		if (Input.GetMouseButton(2))
		{
			base.transform.Rotate(Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * Input.GetAxis("Mouse X") * 2f * this.sensRotCam);
			base.transform.Rotate(-Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime * Input.GetAxis("Mouse Y") * 1.4f * this.sensRotCam);
		}
		this.rotazioneTemporanea = base.transform.rotation.eulerAngles;
		this.rotazioneTemporanea.z = 0f;
		base.transform.eulerAngles = this.rotazioneTemporanea;
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x000E8494 File Offset: 0x000E6694
	private void CambioCamera()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (this.cameraAttiva == 1)
			{
				this.cameraAttiva = 2;
				this.oggettoCameraAttiva = this.secondaCamera;
			}
			else if (this.cameraAttiva == 3)
			{
				this.cameraAttiva = 1;
				this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = true;
				this.oggettoCameraAttiva = base.gameObject;
			}
			else
			{
				this.cameraAttiva = 1;
				this.oggettoCameraAttiva = base.gameObject;
			}
			if (this.cameraAttiva == 1)
			{
				base.GetComponent<Camera>().enabled = true;
				base.GetComponent<AudioListener>().enabled = true;
				base.gameObject.tag = "MainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = false;
				this.secondaCamera.GetComponent<AudioListener>().enabled = false;
				this.secondaCamera.gameObject.tag = "NoMainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = false;
				this.terzaCamera.GetComponent<AudioListener>().enabled = false;
				this.terzaCamera.gameObject.tag = "NoMainCamera";
			}
			if (this.cameraAttiva == 2)
			{
				base.GetComponent<Camera>().enabled = false;
				base.GetComponent<AudioListener>().enabled = false;
				base.gameObject.tag = "NoMainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = true;
				this.secondaCamera.GetComponent<AudioListener>().enabled = true;
				this.secondaCamera.gameObject.tag = "MainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = false;
				this.terzaCamera.GetComponent<AudioListener>().enabled = false;
				this.terzaCamera.gameObject.tag = "NoMainCamera";
			}
			if (this.cameraAttiva == 3)
			{
				base.GetComponent<Camera>().enabled = false;
				base.GetComponent<AudioListener>().enabled = false;
				base.gameObject.tag = "NoMainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = false;
				this.secondaCamera.GetComponent<AudioListener>().enabled = false;
				this.secondaCamera.gameObject.tag = "NoMainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = true;
				this.terzaCamera.GetComponent<AudioListener>().enabled = true;
				this.terzaCamera.gameObject.tag = "MainCamera";
			}
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.terzaCamera.GetComponent<TerzaCamera>().terzaCameraPosizionata = false;
			this.terzaCamera.GetComponent<TerzaCamera>().èTPS = true;
			if (this.cameraAttiva == 1 || this.cameraAttiva == 2)
			{
				if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count > 0 && !this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento[0].GetComponent<PresenzaAlleato>().èGeniere)
				{
					this.cameraAttiva = 3;
					this.terzaCamera.GetComponent<TerzaCamera>().entraInPrimaPersona = true;
					this.oggettoCameraAttiva = this.terzaCamera;
				}
			}
			else
			{
				this.cameraAttiva = 1;
				this.terzaCamera.GetComponent<TerzaCamera>().esciDaPrimaPersona = true;
				this.oggettoCameraAttiva = base.gameObject;
			}
			if (this.infoAlleati.GetComponent<InfoGenericheAlleati>().ListaSelezionamento.Count <= 0 && this.cameraAttiva == 3)
			{
				this.cameraAttiva = 1;
				this.oggettoCameraAttiva = base.gameObject;
			}
			if (this.cameraAttiva == 1)
			{
				base.GetComponent<Camera>().enabled = true;
				base.GetComponent<AudioListener>().enabled = true;
				base.gameObject.tag = "MainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = false;
				this.secondaCamera.GetComponent<AudioListener>().enabled = false;
				this.secondaCamera.gameObject.tag = "NoMainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = false;
				this.terzaCamera.GetComponent<AudioListener>().enabled = false;
				this.terzaCamera.gameObject.tag = "NoMainCamera";
			}
			if (this.cameraAttiva == 2)
			{
				base.GetComponent<Camera>().enabled = false;
				base.GetComponent<AudioListener>().enabled = false;
				base.gameObject.tag = "NoMainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = true;
				this.secondaCamera.GetComponent<AudioListener>().enabled = true;
				this.secondaCamera.gameObject.tag = "MainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = false;
				this.terzaCamera.GetComponent<AudioListener>().enabled = false;
				this.terzaCamera.gameObject.tag = "NoMainCamera";
			}
			if (this.cameraAttiva == 3)
			{
				base.GetComponent<Camera>().enabled = false;
				base.GetComponent<AudioListener>().enabled = false;
				base.gameObject.tag = "NoMainCamera";
				this.secondaCamera.GetComponent<Camera>().enabled = false;
				this.secondaCamera.GetComponent<AudioListener>().enabled = false;
				this.secondaCamera.gameObject.tag = "NoMainCamera";
				this.terzaCamera.GetComponent<Camera>().enabled = true;
				this.terzaCamera.GetComponent<AudioListener>().enabled = true;
				this.terzaCamera.gameObject.tag = "MainCamera";
			}
			this.terzaCamera.GetComponent<Camera>().fieldOfView = 60f;
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x000E8A1C File Offset: 0x000E6C1C
	private void VerificaCameraPostMorte()
	{
		if (this.cameraAttiva == 1)
		{
			base.GetComponent<Camera>().enabled = true;
			base.GetComponent<AudioListener>().enabled = true;
			base.gameObject.tag = "MainCamera";
			this.secondaCamera.GetComponent<Camera>().enabled = false;
			this.secondaCamera.GetComponent<AudioListener>().enabled = false;
			this.secondaCamera.gameObject.tag = "NoMainCamera";
			this.terzaCamera.GetComponent<Camera>().enabled = false;
			this.terzaCamera.GetComponent<AudioListener>().enabled = false;
			this.terzaCamera.gameObject.tag = "NoMainCamera";
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x000E8ACC File Offset: 0x000E6CCC
	private void Riposizionamento()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitRaggio0, (float)this.layerAmbiente))
		{
			this.scrittaRiposiz.GetComponent<CanvasGroup>().alpha = 0f;
		}
		else
		{
			this.scrittaRiposiz.GetComponent<CanvasGroup>().alpha = 1f;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			base.transform.position = this.posInizialePerRiposiz;
			base.transform.eulerAngles = this.rotInizialePerRiposiz;
		}
	}

	// Token: 0x04001861 RID: 6241
	public float sensibilitàCameraTraslazione;

	// Token: 0x04001862 RID: 6242
	public float sensibilitàCameraRotazione;

	// Token: 0x04001863 RID: 6243
	public float sensibilitàZoom;

	// Token: 0x04001864 RID: 6244
	public float limiteAngSuGiù;

	// Token: 0x04001865 RID: 6245
	private Vector3 rotazioneTemporanea;

	// Token: 0x04001866 RID: 6246
	private Vector3 movimentoABinario;

	// Token: 0x04001867 RID: 6247
	private GameObject secondaCamera;

	// Token: 0x04001868 RID: 6248
	private GameObject terzaCamera;

	// Token: 0x04001869 RID: 6249
	public int cameraAttiva;

	// Token: 0x0400186A RID: 6250
	public GameObject oggettoCameraAttiva;

	// Token: 0x0400186B RID: 6251
	private GameObject infoAlleati;

	// Token: 0x0400186C RID: 6252
	private GameObject varieMAppaLocale;

	// Token: 0x0400186D RID: 6253
	private GameObject canvasManuale;

	// Token: 0x0400186E RID: 6254
	private GameObject impostazioni;

	// Token: 0x0400186F RID: 6255
	private GameObject scrittaRiposiz;

	// Token: 0x04001870 RID: 6256
	private List<GameObject> ListaOggDaTrasparireConSecondaCam;

	// Token: 0x04001871 RID: 6257
	private float altezzaPerTrasparimentoOgg;

	// Token: 0x04001872 RID: 6258
	public float raggioLimiteMovimento;

	// Token: 0x04001873 RID: 6259
	private float distDaCentroMappa;

	// Token: 0x04001874 RID: 6260
	public bool morteAvvenuta;

	// Token: 0x04001875 RID: 6261
	public float sensRotCam;

	// Token: 0x04001876 RID: 6262
	public bool fermaVisuale;

	// Token: 0x04001877 RID: 6263
	private Ray raggio0;

	// Token: 0x04001878 RID: 6264
	private RaycastHit hitRaggio0;

	// Token: 0x04001879 RID: 6265
	private int layerAmbiente;

	// Token: 0x0400187A RID: 6266
	public Vector3 posInizialePerRiposiz;

	// Token: 0x0400187B RID: 6267
	public Vector3 rotInizialePerRiposiz;
}
