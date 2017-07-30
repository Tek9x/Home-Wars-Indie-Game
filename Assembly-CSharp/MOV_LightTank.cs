using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class MOV_LightTank : MonoBehaviour
{
	// Token: 0x0600054B RID: 1355 RVA: 0x000B0064 File Offset: 0x000AE264
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torretta.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000B00D4 File Offset: 0x000AE2D4
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
		this.GestioneSuoni();
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x000B0128 File Offset: 0x000AE328
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x000B015C File Offset: 0x000AE35C
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_LightTank>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_LightTank>().stopFinito = false;
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

	// Token: 0x0600054F RID: 1359 RVA: 0x000B0364 File Offset: 0x000AE564
	private void MovimentoInPrimaPersona()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
			base.gameObject.AddComponent<Rigidbody>();
			base.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)80;
			base.GetComponent<Rigidbody>().mass = 10f;
			base.GetComponent<Rigidbody>().drag = 0.1f;
			base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		}
		float magnitude = base.GetComponent<Rigidbody>().velocity.magnitude;
		float magnitude2 = base.GetComponent<Rigidbody>().angularVelocity.magnitude;
		if (Input.GetKey(KeyCode.W) && magnitude < this.limiteVelocità)
		{
			base.GetComponent<Rigidbody>().AddForce(base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			base.GetComponent<Rigidbody>().AddForce(-base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, -this.velocitàRotazioneMezzo, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, this.velocitàRotazioneMezzo, 0f);
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

	// Token: 0x040013F5 RID: 5109
	public float velocitàRotazioneMezzo;

	// Token: 0x040013F6 RID: 5110
	public float velocitàRotazioneTorretta;

	// Token: 0x040013F7 RID: 5111
	public float velocitàRotazioneCannoni;

	// Token: 0x040013F8 RID: 5112
	public float angCannoniVertMin;

	// Token: 0x040013F9 RID: 5113
	public float angCannoniVertMax;

	// Token: 0x040013FA RID: 5114
	public GameObject torretta;

	// Token: 0x040013FB RID: 5115
	public GameObject cannoni;

	// Token: 0x040013FC RID: 5116
	private GameObject infoNeutreTattica;

	// Token: 0x040013FD RID: 5117
	private GameObject terzaCamera;

	// Token: 0x040013FE RID: 5118
	private float limiteVelocità;

	// Token: 0x040013FF RID: 5119
	private AudioSource suonoTorretta;

	// Token: 0x04001400 RID: 5120
	private AudioSource suonoMotore;

	// Token: 0x04001401 RID: 5121
	public AudioClip motoreFermo;

	// Token: 0x04001402 RID: 5122
	public AudioClip motorePartenza;

	// Token: 0x04001403 RID: 5123
	public AudioClip motoreViaggio;

	// Token: 0x04001404 RID: 5124
	public AudioClip motoreStop;

	// Token: 0x04001405 RID: 5125
	private float timerPartenza;

	// Token: 0x04001406 RID: 5126
	private float timerStop;

	// Token: 0x04001407 RID: 5127
	private bool primaPartenza;

	// Token: 0x04001408 RID: 5128
	private bool inPartenza;

	// Token: 0x04001409 RID: 5129
	private bool partenzaFinita;

	// Token: 0x0400140A RID: 5130
	private bool inStop;

	// Token: 0x0400140B RID: 5131
	private bool stopFinito;

	// Token: 0x0400140C RID: 5132
	public bool suonoTorrettaPartito;
}
