using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class MOV_HeavyTank : MonoBehaviour
{
	// Token: 0x06000539 RID: 1337 RVA: 0x000AEFB4 File Offset: 0x000AD1B4
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x000AF024 File Offset: 0x000AD224
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x000AF078 File Offset: 0x000AD278
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000AF0AC File Offset: 0x000AD2AC
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_HeavyTank>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_HeavyTank>().stopFinito = false;
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

	// Token: 0x0600053D RID: 1341 RVA: 0x000AF2B4 File Offset: 0x000AD4B4
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

	// Token: 0x040013AA RID: 5034
	public float velocitàRotazioneMezzo;

	// Token: 0x040013AB RID: 5035
	public float velocitàRotazioneTorretta;

	// Token: 0x040013AC RID: 5036
	public float velocitàRotazioneCannoni;

	// Token: 0x040013AD RID: 5037
	public float angCannoniVertMin;

	// Token: 0x040013AE RID: 5038
	public float angCannoniVertMax;

	// Token: 0x040013AF RID: 5039
	public GameObject torretta;

	// Token: 0x040013B0 RID: 5040
	public GameObject cannoni;

	// Token: 0x040013B1 RID: 5041
	private GameObject infoNeutreTattica;

	// Token: 0x040013B2 RID: 5042
	private GameObject terzaCamera;

	// Token: 0x040013B3 RID: 5043
	private float limiteVelocità;

	// Token: 0x040013B4 RID: 5044
	private AudioSource suonoTorretta;

	// Token: 0x040013B5 RID: 5045
	private AudioSource suonoMotore;

	// Token: 0x040013B6 RID: 5046
	public AudioClip motoreFermo;

	// Token: 0x040013B7 RID: 5047
	public AudioClip motorePartenza;

	// Token: 0x040013B8 RID: 5048
	public AudioClip motoreViaggio;

	// Token: 0x040013B9 RID: 5049
	public AudioClip motoreStop;

	// Token: 0x040013BA RID: 5050
	private float timerPartenza;

	// Token: 0x040013BB RID: 5051
	private float timerStop;

	// Token: 0x040013BC RID: 5052
	private bool primaPartenza;

	// Token: 0x040013BD RID: 5053
	private bool inPartenza;

	// Token: 0x040013BE RID: 5054
	private bool partenzaFinita;

	// Token: 0x040013BF RID: 5055
	private bool inStop;

	// Token: 0x040013C0 RID: 5056
	private bool stopFinito;

	// Token: 0x040013C1 RID: 5057
	public bool suonoTorrettaPartito;

	// Token: 0x040013C2 RID: 5058
	private Rigidbody corpoRigido;
}
