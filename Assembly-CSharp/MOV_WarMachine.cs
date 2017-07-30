using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class MOV_WarMachine : MonoBehaviour
{
	// Token: 0x0600056B RID: 1387 RVA: 0x000B1C7C File Offset: 0x000AFE7C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.suonoTorretta = this.torrettaFPS4.GetComponent<AudioSource>();
		this.suonoMotore = base.GetComponent<AudioSource>();
		this.primaPartenza = true;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x000B1CEC File Offset: 0x000AFEEC
	private void Update()
	{
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
		else if (!Physics.Raycast(base.transform.position, -Vector3.up, out this.hitTerreno, 3f, 256) && base.GetComponent<Rigidbody>())
		{
			this.corpoRigido.AddForce(-Vector3.up * 50f, ForceMode.VelocityChange);
		}
		this.GestioneSuoni();
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000B1DA4 File Offset: 0x000AFFA4
	private void FixedUpdate()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.MovimentoInPrimaPersona();
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000B1DD8 File Offset: 0x000AFFD8
	private void GestioneSuoni()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.suonoMotore.volume = base.GetComponent<ATT_WarMachine>().volumeMotoreIniziale;
			if (Input.GetKeyUp(KeyCode.Q))
			{
				this.suonoMotore.clip = this.motoreFermo;
				this.suonoMotore.Play();
				base.GetComponent<ATT_WarMachine>().stopFinito = false;
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

	// Token: 0x0600056F RID: 1391 RVA: 0x000B1FE0 File Offset: 0x000B01E0
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
		float deltaTime = Time.deltaTime;
		float num = -Input.GetAxis("Mouse Y");
		float num2 = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èTPS)
		{
			this.terzaCamera.transform.Rotate(num * deltaTime * this.velocitàRotazioneTPS, 0f, 0f);
			this.terzaCamera.transform.Rotate(0f, num2 * deltaTime * this.velocitàRotazioneTPS, 0f);
			this.terzaCamera.transform.eulerAngles = new Vector3(this.terzaCamera.transform.eulerAngles.x, this.terzaCamera.transform.eulerAngles.y, 0f);
			this.terzaCamera.transform.localEulerAngles = new Vector3(this.terzaCamera.transform.localEulerAngles.x, this.terzaCamera.transform.localEulerAngles.y, 0f);
		}
		else
		{
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 0)
			{
				float magnitude3 = Vector3.Project(this.torrettaFPS1.transform.forward, base.transform.right).magnitude;
				float num3 = Vector3.Dot(base.transform.forward, this.torrettaFPS1.transform.right);
				if (magnitude3 > 0.94f && num3 < 0f && num2 > 0f)
				{
					num2 = 0f;
				}
				else if (magnitude3 > 0.94f && num3 > 0f && num2 < 0f)
				{
					num2 = 0f;
				}
				else
				{
					this.torrettaFPS1.transform.Rotate(new Vector3(0f, num2 * this.velocitàRotazioneTorretta1, 0f) * Time.deltaTime);
				}
				float magnitude4 = Vector3.Project(this.cannoneFPS1.transform.forward, base.transform.up).magnitude;
				float num4 = Vector3.Dot(base.transform.forward, this.cannoneFPS1.transform.up);
				if (magnitude4 > 0.28f && num4 < 0f && num < 0f)
				{
					num = 0f;
				}
				else if (magnitude4 > 0.1f && num4 > 0f && num > 0f)
				{
					num = 0f;
				}
				else
				{
					this.cannoneFPS1.transform.Rotate(new Vector3(num * this.velocitàRotazioneCannoni1, 0f, 0f) * Time.deltaTime);
				}
				this.cannoneFPS1.transform.localEulerAngles = new Vector3(this.cannoneFPS1.transform.localEulerAngles.x, this.cannoneFPS1.transform.localEulerAngles.y, 0f);
			}
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 1)
			{
				float num5 = Vector3.Angle(this.torrettaFPS2.transform.forward, -base.transform.right);
				float num6 = Vector3.Dot(base.transform.forward, this.torrettaFPS2.transform.forward);
				if (num5 > 100f && num6 > 0f && num2 > 0f)
				{
					num2 = 0f;
				}
				else if (num5 > 100f && num6 < 0f && num2 < 0f)
				{
					num2 = 0f;
				}
				else
				{
					this.torrettaFPS2.transform.Rotate(new Vector3(0f, num2 * this.velocitàRotazioneTorretta2, 0f) * Time.deltaTime);
				}
				float magnitude5 = Vector3.Project(this.cannoneFPS2.transform.forward, base.transform.up).magnitude;
				float num7 = Vector3.Dot(this.cannoneFPS2.transform.forward, this.torrettaFPS2.transform.up);
				if (magnitude5 > 0.27f && num7 > 0f && num < 0f)
				{
					num = 0f;
				}
				else if (magnitude5 > 0.32f && num7 < 0f && num > 0f)
				{
					num = 0f;
				}
				else
				{
					this.cannoneFPS2.transform.Rotate(new Vector3(num * this.velocitàRotazioneCannoni2, 0f, 0f) * Time.deltaTime);
				}
				this.cannoneFPS2.transform.localEulerAngles = new Vector3(this.cannoneFPS2.transform.localEulerAngles.x, this.cannoneFPS2.transform.localEulerAngles.y, 0f);
			}
			if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 2)
			{
				float num8 = Vector3.Angle(this.torrettaFPS3.transform.forward, base.transform.right);
				float num9 = Vector3.Dot(base.transform.forward, this.torrettaFPS3.transform.forward);
				if (num8 > 100f && num9 < 0f && num2 > 0f)
				{
					num2 = 0f;
				}
				else if (num8 > 100f && num9 > 0f && num2 < 0f)
				{
					num2 = 0f;
				}
				else
				{
					this.torrettaFPS3.transform.Rotate(new Vector3(0f, num2 * this.velocitàRotazioneTorretta3, 0f) * Time.deltaTime);
				}
				float magnitude6 = Vector3.Project(this.cannoneFPS3.transform.forward, base.transform.up).magnitude;
				float num10 = Vector3.Dot(this.cannoneFPS3.transform.forward, this.torrettaFPS3.transform.up);
				if (magnitude6 > 0.27f && num10 > 0f && num < 0f)
				{
					num = 0f;
				}
				else if (magnitude6 > 0.32f && num10 < 0f && num > 0f)
				{
					num = 0f;
				}
				else
				{
					this.cannoneFPS3.transform.Rotate(new Vector3(num * this.velocitàRotazioneCannoni3, 0f, 0f) * Time.deltaTime);
				}
				this.cannoneFPS3.transform.localEulerAngles = new Vector3(this.cannoneFPS3.transform.localEulerAngles.x, this.cannoneFPS3.transform.localEulerAngles.y, 0f);
			}
			else if (base.GetComponent<PresenzaAlleato>().armaAttivaInFPS == 3)
			{
				this.torrettaFPS4.transform.Rotate(new Vector3(0f, num2 * this.velocitàRotazioneTorretta4, 0f) * Time.deltaTime);
				float num11 = Vector3.Dot(this.cannoneFPS4.transform.up, this.torrettaFPS4.transform.forward);
				if (num11 > 0.25f && num > 0f)
				{
					num = 0f;
				}
				else if (num11 < -0.5f && num < 0f)
				{
					num = 0f;
				}
				else
				{
					this.cannoneFPS4.transform.Rotate(new Vector3(num * this.velocitàRotazioneCannoni4, 0f, 0f) * Time.deltaTime);
				}
				this.cannoneFPS4.transform.localEulerAngles = new Vector3(this.cannoneFPS4.transform.localEulerAngles.x, this.cannoneFPS4.transform.localEulerAngles.y, 0f);
			}
			if (num2 == 0f && num == 0f)
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
	}

	// Token: 0x0400146A RID: 5226
	public float velocitàRotazioneMezzo;

	// Token: 0x0400146B RID: 5227
	public float velocitàRotazioneTPS;

	// Token: 0x0400146C RID: 5228
	public float velocitàRotazioneTorretta1;

	// Token: 0x0400146D RID: 5229
	public float velocitàRotazioneCannoni1;

	// Token: 0x0400146E RID: 5230
	public float velocitàRotazioneTorretta2;

	// Token: 0x0400146F RID: 5231
	public float velocitàRotazioneCannoni2;

	// Token: 0x04001470 RID: 5232
	public float velocitàRotazioneTorretta3;

	// Token: 0x04001471 RID: 5233
	public float velocitàRotazioneCannoni3;

	// Token: 0x04001472 RID: 5234
	public float velocitàRotazioneTorretta4;

	// Token: 0x04001473 RID: 5235
	public float velocitàRotazioneCannoni4;

	// Token: 0x04001474 RID: 5236
	public GameObject torrettaFPS1;

	// Token: 0x04001475 RID: 5237
	public GameObject torrettaFPS2;

	// Token: 0x04001476 RID: 5238
	public GameObject torrettaFPS3;

	// Token: 0x04001477 RID: 5239
	public GameObject torrettaFPS4;

	// Token: 0x04001478 RID: 5240
	public GameObject cannoneFPS1;

	// Token: 0x04001479 RID: 5241
	public GameObject cannoneFPS2;

	// Token: 0x0400147A RID: 5242
	public GameObject cannoneFPS3;

	// Token: 0x0400147B RID: 5243
	public GameObject cannoneFPS4;

	// Token: 0x0400147C RID: 5244
	private GameObject infoNeutreTattica;

	// Token: 0x0400147D RID: 5245
	private GameObject terzaCamera;

	// Token: 0x0400147E RID: 5246
	private bool èInPrimaPersona;

	// Token: 0x0400147F RID: 5247
	private float limiteVelocità;

	// Token: 0x04001480 RID: 5248
	private AudioSource suonoTorretta;

	// Token: 0x04001481 RID: 5249
	private AudioSource suonoMotore;

	// Token: 0x04001482 RID: 5250
	public AudioClip motoreFermo;

	// Token: 0x04001483 RID: 5251
	public AudioClip motorePartenza;

	// Token: 0x04001484 RID: 5252
	public AudioClip motoreViaggio;

	// Token: 0x04001485 RID: 5253
	public AudioClip motoreStop;

	// Token: 0x04001486 RID: 5254
	private float timerPartenza;

	// Token: 0x04001487 RID: 5255
	private float timerStop;

	// Token: 0x04001488 RID: 5256
	private bool primaPartenza;

	// Token: 0x04001489 RID: 5257
	private bool inPartenza;

	// Token: 0x0400148A RID: 5258
	private bool partenzaFinita;

	// Token: 0x0400148B RID: 5259
	private bool inStop;

	// Token: 0x0400148C RID: 5260
	private bool stopFinito;

	// Token: 0x0400148D RID: 5261
	public bool suonoTorrettaPartito;

	// Token: 0x0400148E RID: 5262
	private Rigidbody corpoRigido;

	// Token: 0x0400148F RID: 5263
	private RaycastHit hitTerreno;
}
