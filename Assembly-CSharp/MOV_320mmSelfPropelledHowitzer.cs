using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class MOV_320mmSelfPropelledHowitzer : MonoBehaviour
{
	// Token: 0x06000515 RID: 1301 RVA: 0x000AD0B8 File Offset: 0x000AB2B8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x000AD118 File Offset: 0x000AB318
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x000AD120 File Offset: 0x000AB320
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000AD134 File Offset: 0x000AB334
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

	// Token: 0x06000519 RID: 1305 RVA: 0x000AD19C File Offset: 0x000AB39C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_320mmSelfPropelledHowitzer>().volumeMotoreIniziale / 3f;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_320mmSelfPropelledHowitzer>().stopFinito = false;
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

	// Token: 0x0600051A RID: 1306 RVA: 0x000AD3AC File Offset: 0x000AB5AC
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
		this.rotazioneSuGiù += Input.GetAxis("Mouse Y") * 0.7f;
		this.rotazioneSuGiù = Mathf.Clamp(this.rotazioneSuGiù, -50f, 40f);
		this.terzaCamera.transform.localEulerAngles = new Vector3(-this.rotazioneSuGiù, 0f, 0f);
	}

	// Token: 0x04001320 RID: 4896
	public float velocitàRotazioneMezzo;

	// Token: 0x04001321 RID: 4897
	public float velocitàRotazioneCannoni;

	// Token: 0x04001322 RID: 4898
	public float angCannoniVertMin;

	// Token: 0x04001323 RID: 4899
	public float angCannoniVertMax;

	// Token: 0x04001324 RID: 4900
	public GameObject cannoni;

	// Token: 0x04001325 RID: 4901
	private GameObject infoNeutreTattica;

	// Token: 0x04001326 RID: 4902
	private GameObject terzaCamera;

	// Token: 0x04001327 RID: 4903
	private bool èInPrimaPersona;

	// Token: 0x04001328 RID: 4904
	private float limiteVelocità;

	// Token: 0x04001329 RID: 4905
	private AudioSource suonoMotore;

	// Token: 0x0400132A RID: 4906
	public AudioClip motoreFermo;

	// Token: 0x0400132B RID: 4907
	public AudioClip motorePartenza;

	// Token: 0x0400132C RID: 4908
	public AudioClip motoreViaggio;

	// Token: 0x0400132D RID: 4909
	public AudioClip motoreStop;

	// Token: 0x0400132E RID: 4910
	private float timerPartenza;

	// Token: 0x0400132F RID: 4911
	private float timerStop;

	// Token: 0x04001330 RID: 4912
	private bool primaPartenza;

	// Token: 0x04001331 RID: 4913
	private bool inPartenza;

	// Token: 0x04001332 RID: 4914
	private bool partenzaFinita;

	// Token: 0x04001333 RID: 4915
	private bool inStop;

	// Token: 0x04001334 RID: 4916
	private bool stopFinito;

	// Token: 0x04001335 RID: 4917
	private Rigidbody corpoRigido;

	// Token: 0x04001336 RID: 4918
	private float rotazioneSuGiù;
}
