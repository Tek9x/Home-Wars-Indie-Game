using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class MOV_150mmSelfPropelledHowitzer : MonoBehaviour
{
	// Token: 0x0600050F RID: 1295 RVA: 0x000ACB58 File Offset: 0x000AAD58
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000ACBC8 File Offset: 0x000AADC8
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000ACC1C File Offset: 0x000AAE1C
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x000ACC50 File Offset: 0x000AAE50
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_150mmSelfPropelledHowitzer>().volumeMotoreIniziale / 2f;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_150mmSelfPropelledHowitzer>().stopFinito = false;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			{
				this.timerPartenza += Time.deltaTime;
				this.timerStop = 0f;
				this.primaPartenza = false;
				this.inStop = false;
				this.stopFinito = false;
			}
			if (!this.inPartenza && this.timerPartenza > 0f)
			{
				this.suonoMotore.clip = this.motorePartenza;
				this.suonoMotore.Play();
				this.inPartenza = true;
			}
			if (!this.partenzaFinita && this.timerPartenza > this.motorePartenza.length)
			{
				this.suonoMotore.clip = this.motoreViaggio;
				this.suonoMotore.Play();
				this.partenzaFinita = true;
			}
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
			{
				this.timerStop += Time.deltaTime;
				this.timerPartenza = 0f;
				this.inPartenza = false;
				this.partenzaFinita = false;
			}
			if (!this.inStop && this.timerStop > 0f)
			{
				this.suonoMotore.clip = this.motoreStop;
				this.suonoMotore.Play();
				this.inStop = true;
			}
			if (!this.stopFinito && this.timerStop > this.motoreStop.length)
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				this.stopFinito = true;
			}
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x000ACE60 File Offset: 0x000AB060
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.constraints = (RigidbodyConstraints)80;
			this.corpoRigido.mass = 10f;
			this.corpoRigido.drag = 0.1f;
			this.corpoRigido.angularDrag = 0.1f;
		}
		float magnitude = this.corpoRigido.velocity.magnitude;
		float magnitude2 = this.corpoRigido.angularVelocity.magnitude;
		if (Input.GetKey(KeyCode.W) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
		}
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis * this.velocitàRotazioneTorretta * Time.deltaTime, 0f);
		this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 0.3f;
		this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -50f, 40f);
		this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
		if (axis == 0f)
		{
			this.suonoTorretta.Stop();
			this.suonoTorrettaPartito = false;
		}
		else if (!this.suonoTorrettaPartito)
		{
			this.suonoTorretta.Play();
			this.suonoTorrettaPartito = true;
		}
	}

	// Token: 0x04001307 RID: 4871
	public float velocitàRotazioneMezzo;

	// Token: 0x04001308 RID: 4872
	public float velocitàRotazioneTorretta;

	// Token: 0x04001309 RID: 4873
	public float angCannoniVertMin;

	// Token: 0x0400130A RID: 4874
	public float angCannoniVertMax;

	// Token: 0x0400130B RID: 4875
	public GameObject torretta;

	// Token: 0x0400130C RID: 4876
	public GameObject cannoni;

	// Token: 0x0400130D RID: 4877
	private GameObject infoNeutreTattica;

	// Token: 0x0400130E RID: 4878
	private GameObject terzaCamera;

	// Token: 0x0400130F RID: 4879
	private float limiteVelocità;

	// Token: 0x04001310 RID: 4880
	private AudioSource suonoTorretta;

	// Token: 0x04001311 RID: 4881
	private AudioSource suonoMotore;

	// Token: 0x04001312 RID: 4882
	public AudioClip motoreFermo;

	// Token: 0x04001313 RID: 4883
	public AudioClip motorePartenza;

	// Token: 0x04001314 RID: 4884
	public AudioClip motoreViaggio;

	// Token: 0x04001315 RID: 4885
	public AudioClip motoreStop;

	// Token: 0x04001316 RID: 4886
	private float timerPartenza;

	// Token: 0x04001317 RID: 4887
	private float timerStop;

	// Token: 0x04001318 RID: 4888
	private bool primaPartenza;

	// Token: 0x04001319 RID: 4889
	private bool inPartenza;

	// Token: 0x0400131A RID: 4890
	private bool partenzaFinita;

	// Token: 0x0400131B RID: 4891
	private bool inStop;

	// Token: 0x0400131C RID: 4892
	private bool stopFinito;

	// Token: 0x0400131D RID: 4893
	public bool suonoTorrettaPartito;

	// Token: 0x0400131E RID: 4894
	private Rigidbody corpoRigido;

	// Token: 0x0400131F RID: 4895
	private float rotazioneSuGiù;
}
