using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class MOV_RocketArtillery : MonoBehaviour
{
	// Token: 0x0600055E RID: 1374 RVA: 0x000B11C4 File Offset: 0x000AF3C4
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x000B1234 File Offset: 0x000AF434
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x000B1288 File Offset: 0x000AF488
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x000B12BC File Offset: 0x000AF4BC
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_RocketArtillery>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_RocketArtillery>().stopFinito = false;
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

	// Token: 0x06000562 RID: 1378 RVA: 0x000B14C4 File Offset: 0x000AF6C4
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

	// Token: 0x0400143E RID: 5182
	public float velocitàRotazioneMezzo;

	// Token: 0x0400143F RID: 5183
	public float velocitàRotazioneTorretta;

	// Token: 0x04001440 RID: 5184
	public float angCannoniVertMin;

	// Token: 0x04001441 RID: 5185
	public float angCannoniVertMax;

	// Token: 0x04001442 RID: 5186
	public GameObject torretta;

	// Token: 0x04001443 RID: 5187
	public GameObject lanciamissili;

	// Token: 0x04001444 RID: 5188
	private GameObject infoNeutreTattica;

	// Token: 0x04001445 RID: 5189
	private GameObject terzaCamera;

	// Token: 0x04001446 RID: 5190
	private float limiteVelocità;

	// Token: 0x04001447 RID: 5191
	private AudioSource suonoTorretta;

	// Token: 0x04001448 RID: 5192
	private AudioSource suonoMotore;

	// Token: 0x04001449 RID: 5193
	public AudioClip motoreFermo;

	// Token: 0x0400144A RID: 5194
	public AudioClip motorePartenza;

	// Token: 0x0400144B RID: 5195
	public AudioClip motoreViaggio;

	// Token: 0x0400144C RID: 5196
	public AudioClip motoreStop;

	// Token: 0x0400144D RID: 5197
	private float timerPartenza;

	// Token: 0x0400144E RID: 5198
	private float timerStop;

	// Token: 0x0400144F RID: 5199
	private bool primaPartenza;

	// Token: 0x04001450 RID: 5200
	private bool inPartenza;

	// Token: 0x04001451 RID: 5201
	private bool partenzaFinita;

	// Token: 0x04001452 RID: 5202
	private bool inStop;

	// Token: 0x04001453 RID: 5203
	private bool stopFinito;

	// Token: 0x04001454 RID: 5204
	public bool suonoTorrettaPartito;

	// Token: 0x04001455 RID: 5205
	private Rigidbody corpoRigido;

	// Token: 0x04001456 RID: 5206
	private float rotazioneSuGiù;
}
