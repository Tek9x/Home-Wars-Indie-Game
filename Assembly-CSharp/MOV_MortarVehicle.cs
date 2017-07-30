using System;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class MOV_MortarVehicle : MonoBehaviour
{
	// Token: 0x06000551 RID: 1361 RVA: 0x000B05E8 File Offset: 0x000AE7E8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x000B0648 File Offset: 0x000AE848
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000B069C File Offset: 0x000AE89C
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000B06D0 File Offset: 0x000AE8D0
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_MortarVehicle>().stopFinito = false;
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

	// Token: 0x06000555 RID: 1365 RVA: 0x000B08C4 File Offset: 0x000AEAC4
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
	}

	// Token: 0x0400140D RID: 5133
	public float velocitàRotazioneMezzo;

	// Token: 0x0400140E RID: 5134
	public float velocitàRotazioneTorretta;

	// Token: 0x0400140F RID: 5135
	public float angCannoniVertMin;

	// Token: 0x04001410 RID: 5136
	public float angCannoniVertMax;

	// Token: 0x04001411 RID: 5137
	public GameObject torretta;

	// Token: 0x04001412 RID: 5138
	private GameObject infoNeutreTattica;

	// Token: 0x04001413 RID: 5139
	private GameObject terzaCamera;

	// Token: 0x04001414 RID: 5140
	private float limiteVelocità;

	// Token: 0x04001415 RID: 5141
	private AudioSource suonoMotore;

	// Token: 0x04001416 RID: 5142
	public AudioClip motoreFermo;

	// Token: 0x04001417 RID: 5143
	public AudioClip motorePartenza;

	// Token: 0x04001418 RID: 5144
	public AudioClip motoreViaggio;

	// Token: 0x04001419 RID: 5145
	public AudioClip motoreStop;

	// Token: 0x0400141A RID: 5146
	private float timerPartenza;

	// Token: 0x0400141B RID: 5147
	private float timerStop;

	// Token: 0x0400141C RID: 5148
	private bool primaPartenza;

	// Token: 0x0400141D RID: 5149
	private bool inPartenza;

	// Token: 0x0400141E RID: 5150
	private bool partenzaFinita;

	// Token: 0x0400141F RID: 5151
	private bool inStop;

	// Token: 0x04001420 RID: 5152
	private bool stopFinito;

	// Token: 0x04001421 RID: 5153
	private Rigidbody corpoRigido;

	// Token: 0x04001422 RID: 5154
	private float rotazioneSuGiù;
}
