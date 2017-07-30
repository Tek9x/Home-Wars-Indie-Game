using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class MOV_SupplyTruck : MonoBehaviour
{
	// Token: 0x06000564 RID: 1380 RVA: 0x000B171C File Offset: 0x000AF91C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x000B177C File Offset: 0x000AF97C
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x000B1784 File Offset: 0x000AF984
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x000B1798 File Offset: 0x000AF998
	private void ConfermaControllo()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.èInPrimaPersona = true;
		}
		else
		{
			this.èInPrimaPersona = false;
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
		this.GestioneSuoni();
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000B1800 File Offset: 0x000AFA00
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_SupplyTruck>().stopFinito = false;
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

	// Token: 0x06000569 RID: 1385 RVA: 0x000B19F4 File Offset: 0x000AFBF4
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
			base.GetComponent<BoxCollider>().isTrigger = false;
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
		if (Input.GetKey(KeyCode.W))
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.forzaRotazione, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.forzaRotazione, 0f);
			}
		}
		if (Input.GetKey(KeyCode.S))
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, this.forzaRotazione, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.corpoRigido.angularVelocity = new Vector3(0f, -this.forzaRotazione, 0f);
			}
		}
		if (!this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 1.2f;
			this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -30f, 40f);
			this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
		}
		else
		{
			this.terzaCamera.transform.localEulerAngles = Vector3.zero;
		}
	}

	// Token: 0x04001457 RID: 5207
	public float forzaRotazione;

	// Token: 0x04001458 RID: 5208
	private GameObject infoNeutreTattica;

	// Token: 0x04001459 RID: 5209
	private GameObject terzaCamera;

	// Token: 0x0400145A RID: 5210
	private bool èInPrimaPersona;

	// Token: 0x0400145B RID: 5211
	private float rotazioneSuGiù;

	// Token: 0x0400145C RID: 5212
	private float limiteVelocità;

	// Token: 0x0400145D RID: 5213
	private AudioSource suonoMotore;

	// Token: 0x0400145E RID: 5214
	public AudioClip motoreFermo;

	// Token: 0x0400145F RID: 5215
	public AudioClip motorePartenza;

	// Token: 0x04001460 RID: 5216
	public AudioClip motoreViaggio;

	// Token: 0x04001461 RID: 5217
	public AudioClip motoreStop;

	// Token: 0x04001462 RID: 5218
	private float timerPartenza;

	// Token: 0x04001463 RID: 5219
	private float timerStop;

	// Token: 0x04001464 RID: 5220
	private bool primaPartenza;

	// Token: 0x04001465 RID: 5221
	private bool inPartenza;

	// Token: 0x04001466 RID: 5222
	private bool partenzaFinita;

	// Token: 0x04001467 RID: 5223
	private bool inStop;

	// Token: 0x04001468 RID: 5224
	private bool stopFinito;

	// Token: 0x04001469 RID: 5225
	private Rigidbody corpoRigido;
}
