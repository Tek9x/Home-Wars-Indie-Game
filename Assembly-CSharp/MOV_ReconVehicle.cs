using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class MOV_ReconVehicle : MonoBehaviour
{
	// Token: 0x06000557 RID: 1367 RVA: 0x000B0ADC File Offset: 0x000AECDC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
		this.ruote1 = base.transform.GetChild(1).transform.GetChild(1).gameObject;
		this.ruote2 = base.transform.GetChild(1).transform.GetChild(2).gameObject;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000B0B90 File Offset: 0x000AED90
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
		else
		{
			this.GestioneSuoni();
			this.GestioneRuote();
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x000B0BF0 File Offset: 0x000AEDF0
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x000B0C24 File Offset: 0x000AEE24
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_ReconVehicle>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_ReconVehicle>().stopFinito = false;
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

	// Token: 0x0600055B RID: 1371 RVA: 0x000B0E2C File Offset: 0x000AF02C
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
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
			}
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
			}
		}
		float num = Vector3.Dot(this.cannoni.transform.forward, base.transform.up);
		float axis = Input.GetAxis("Mouse X");
		this.torretta.transform.Rotate(0f, axis, 0f);
		float num2 = -Input.GetAxis("Mouse Y");
		if (num > this.angCannoniVertMin && num2 > 0f)
		{
			this.cannoni.transform.Rotate(num2, 0f, 0f);
		}
		if (num < this.angCannoniVertMax && num2 < 0f)
		{
			this.cannoni.transform.Rotate(num2, 0f, 0f);
		}
		if (axis == 0f && num2 == 0f)
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

	// Token: 0x0600055C RID: 1372 RVA: 0x000B1110 File Offset: 0x000AF310
	private void GestioneRuote()
	{
		if (Input.GetKey(KeyCode.W))
		{
			this.ruote1.transform.Rotate(Vector3.right * 4f);
			this.ruote2.transform.Rotate(Vector3.right * 4f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.ruote1.transform.Rotate(-Vector3.right * 4f);
			this.ruote2.transform.Rotate(-Vector3.right * 4f);
		}
	}

	// Token: 0x04001423 RID: 5155
	public float velocitàRotazioneMezzo;

	// Token: 0x04001424 RID: 5156
	public float velocitàRotazioneTorretta;

	// Token: 0x04001425 RID: 5157
	public float velocitàRotazioneCannoni;

	// Token: 0x04001426 RID: 5158
	public float angCannoniVertMin;

	// Token: 0x04001427 RID: 5159
	public float angCannoniVertMax;

	// Token: 0x04001428 RID: 5160
	public GameObject torretta;

	// Token: 0x04001429 RID: 5161
	public GameObject cannoni;

	// Token: 0x0400142A RID: 5162
	private GameObject infoNeutreTattica;

	// Token: 0x0400142B RID: 5163
	private GameObject terzaCamera;

	// Token: 0x0400142C RID: 5164
	private float limiteVelocità;

	// Token: 0x0400142D RID: 5165
	private AudioSource suonoTorretta;

	// Token: 0x0400142E RID: 5166
	private AudioSource suonoMotore;

	// Token: 0x0400142F RID: 5167
	public AudioClip motoreFermo;

	// Token: 0x04001430 RID: 5168
	public AudioClip motorePartenza;

	// Token: 0x04001431 RID: 5169
	public AudioClip motoreViaggio;

	// Token: 0x04001432 RID: 5170
	public AudioClip motoreStop;

	// Token: 0x04001433 RID: 5171
	private float timerPartenza;

	// Token: 0x04001434 RID: 5172
	private float timerStop;

	// Token: 0x04001435 RID: 5173
	private bool primaPartenza;

	// Token: 0x04001436 RID: 5174
	private bool inPartenza;

	// Token: 0x04001437 RID: 5175
	private bool partenzaFinita;

	// Token: 0x04001438 RID: 5176
	private bool inStop;

	// Token: 0x04001439 RID: 5177
	private bool stopFinito;

	// Token: 0x0400143A RID: 5178
	public bool suonoTorrettaPartito;

	// Token: 0x0400143B RID: 5179
	private GameObject ruote1;

	// Token: 0x0400143C RID: 5180
	private GameObject ruote2;

	// Token: 0x0400143D RID: 5181
	private Rigidbody corpoRigido;
}
