using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class CameraCasa : MonoBehaviour
{
	// Token: 0x0600067F RID: 1663 RVA: 0x000E6D20 File Offset: 0x000E4F20
	private void Start()
	{
		this.luceDirezionale = base.transform.GetChild(0).gameObject;
		this.Schede = GameObject.FindGameObjectWithTag("Schede");
		this.menuStrategico = GameObject.FindGameObjectWithTag("CanvasMenu").transform.FindChild("Menu Strategico").gameObject;
		this.schermataBattaglia = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Battaglia").gameObject;
		this.schermataMissione = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Per Missione").gameObject;
		this.resocontoBattaglia = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Battaglia").gameObject;
		this.resocontoMissione = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Schermata Resoconto Missione").gameObject;
		this.canvasManuale = GameObject.FindGameObjectWithTag("CanvasManuale");
		this.visualizEserAlleato = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito").gameObject;
		this.visualizEserNemico = GameObject.FindGameObjectWithTag("CanvasStrategia").transform.FindChild("Visualizza Esercito Insetti").gameObject;
		this.corpoRigido = base.GetComponent<Rigidbody>();
		this.sensRotCam = PlayerPrefs.GetFloat("sensibilità rotazione camera") / 100f;
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000E6E84 File Offset: 0x000E5084
	private void Update()
	{
		this.movimentoABinario = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
		this.distDaCentroMappa = Vector3.Distance(base.transform.position, Vector3.zero);
		if (this.Schede.GetComponent<CanvasGroup>().alpha == 1f || this.menuStrategico.GetComponent<CanvasGroup>().alpha == 1f || this.cameraèFerma || this.schermataBattaglia.GetComponent<CanvasGroup>().alpha == 1f || this.schermataMissione.GetComponent<CanvasGroup>().alpha == 1f || this.resocontoBattaglia.GetComponent<CanvasGroup>().alpha == 1f || this.resocontoMissione.GetComponent<CanvasGroup>().alpha == 1f || this.canvasManuale.GetComponent<CanvasGroup>().alpha == 1f)
		{
			this.fermaVisuale = true;
		}
		if (this.Schede.GetComponent<CanvasGroup>().alpha == 0f && this.menuStrategico.GetComponent<CanvasGroup>().alpha == 0f && !this.cameraèFerma && this.schermataBattaglia.GetComponent<CanvasGroup>().alpha == 0f && this.schermataMissione.GetComponent<CanvasGroup>().alpha == 0f && this.resocontoBattaglia.GetComponent<CanvasGroup>().alpha == 0f && this.resocontoMissione.GetComponent<CanvasGroup>().alpha == 0f && this.canvasManuale.GetComponent<CanvasGroup>().alpha == 0f)
		{
			this.fermaVisuale = false;
		}
		if (this.distDaCentroMappa <= this.raggioLimiteMovimento)
		{
			if (!this.fermaVisuale && Time.timeSinceLevelLoad > 0.3f)
			{
				this.MovimentoCamera();
			}
		}
		else
		{
			base.transform.position += (Vector3.zero - base.transform.position).normalized * 2f * Time.deltaTime;
			if (this.distDaCentroMappa > this.raggioLimiteMovimento + 100f)
			{
				base.transform.position = new Vector3(0f, 10f, 0f);
			}
		}
		this.luceDirezionale.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x000E714C File Offset: 0x000E534C
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
		if (Input.GetKey(KeyCode.Space))
		{
			base.transform.position += Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			base.transform.position += -Vector3.up * this.sensibilitàCameraTraslazione * Time.unscaledDeltaTime;
		}
		if (Input.mousePosition.x > (float)Screen.width * 0.99f)
		{
			base.transform.Rotate(Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
		}
		if (Input.mousePosition.x < (float)Screen.width * 0.01f)
		{
			base.transform.Rotate(-Vector3.up * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
		}
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		float num = Vector3.Dot(base.transform.up, normalized);
		if (Input.mousePosition.y > (float)Screen.height * 0.99f && num > -this.limiteAngSuGiù)
		{
			base.transform.Rotate(-Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
		}
		if (Input.mousePosition.y < (float)Screen.height * 0.01f && num < this.limiteAngSuGiù)
		{
			base.transform.Rotate(Vector3.right * this.sensibilitàCameraRotazione * Time.unscaledDeltaTime);
		}
		if (!Input.GetMouseButton(0) && this.visualizEserAlleato.GetComponent<CanvasGroup>().alpha == 0f && this.visualizEserNemico.GetComponent<CanvasGroup>().alpha == 0f)
		{
			base.transform.position += base.transform.forward * Input.GetAxis("Mouse ScrollWheel") * this.sensibilitàZoom * Time.unscaledDeltaTime * 1000f;
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

	// Token: 0x0400181D RID: 6173
	public float sensibilitàCameraTraslazione;

	// Token: 0x0400181E RID: 6174
	public float sensibilitàCameraRotazione;

	// Token: 0x0400181F RID: 6175
	public float sensibilitàZoom;

	// Token: 0x04001820 RID: 6176
	public float limiteAngSuGiù;

	// Token: 0x04001821 RID: 6177
	private float rotDestra;

	// Token: 0x04001822 RID: 6178
	private float rotSu;

	// Token: 0x04001823 RID: 6179
	private Vector3 rotazioneTemporanea;

	// Token: 0x04001824 RID: 6180
	private Vector3 movimentoABinario;

	// Token: 0x04001825 RID: 6181
	private GameObject luceDirezionale;

	// Token: 0x04001826 RID: 6182
	private GameObject Schede;

	// Token: 0x04001827 RID: 6183
	private GameObject menuStrategico;

	// Token: 0x04001828 RID: 6184
	private GameObject schermataBattaglia;

	// Token: 0x04001829 RID: 6185
	private GameObject schermataMissione;

	// Token: 0x0400182A RID: 6186
	private GameObject resocontoBattaglia;

	// Token: 0x0400182B RID: 6187
	private GameObject resocontoMissione;

	// Token: 0x0400182C RID: 6188
	private GameObject canvasManuale;

	// Token: 0x0400182D RID: 6189
	private GameObject visualizEserAlleato;

	// Token: 0x0400182E RID: 6190
	private GameObject visualizEserNemico;

	// Token: 0x0400182F RID: 6191
	private Rigidbody corpoRigido;

	// Token: 0x04001830 RID: 6192
	public float raggioLimiteMovimento;

	// Token: 0x04001831 RID: 6193
	private float distDaCentroMappa;

	// Token: 0x04001832 RID: 6194
	private bool fermaVisuale;

	// Token: 0x04001833 RID: 6195
	public bool cameraèFerma;

	// Token: 0x04001834 RID: 6196
	public Vector3 posPrimaDiGestione;

	// Token: 0x04001835 RID: 6197
	public Vector3 rotPrimaDiGestione;

	// Token: 0x04001836 RID: 6198
	public float sensRotCam;
}
