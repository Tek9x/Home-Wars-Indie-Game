using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class MOV_HeavyHelicopter : MonoBehaviour
{
	// Token: 0x06000485 RID: 1157 RVA: 0x000A7110 File Offset: 0x000A5310
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000A714C File Offset: 0x000A534C
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000A7154 File Offset: 0x000A5354
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.voloInverso = base.GetComponent<PresenzaAlleato>().voloInvertito;
			this.MovimentoInPrimaPersonaFisico();
		}
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000A7184 File Offset: 0x000A5384
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
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			}
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x000A71E0 File Offset: 0x000A53E0
	private void MovimentoInPrimaPersonaFisico()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<CapsuleCollider>().isTrigger = false;
			base.GetComponent<CapsuleCollider>().material = this.materialeFisico;
			base.gameObject.AddComponent<Rigidbody>();
			this.corpoRigido = base.GetComponent<Rigidbody>();
			this.corpoRigido.constraints = RigidbodyConstraints.FreezeRotationZ;
			this.corpoRigido.useGravity = false;
			this.corpoRigido.mass = 1f;
			this.corpoRigido.drag = 1f;
			this.corpoRigido.angularDrag = 0.3f;
		}
		if (this.velocitàTraslDavDietroEffettiva < this.velocitàMax)
		{
			if (Input.GetKey(KeyCode.W))
			{
				this.velocitàTraslDavDietroEffettiva += this.accTraslAvantiDietro;
			}
		}
		else if (Input.GetKey(KeyCode.W))
		{
			this.velocitàTraslDavDietroEffettiva = this.velocitàMax;
		}
		if (!Input.GetKey(KeyCode.W) && this.velocitàTraslDavDietroEffettiva > 0f)
		{
			this.velocitàTraslDavDietroEffettiva -= 1f;
		}
		if (this.velocitàTraslDavDietroEffettiva > -this.velocitàMax)
		{
			if (Input.GetKey(KeyCode.S))
			{
				this.velocitàTraslDavDietroEffettiva -= this.accTraslAvantiDietro;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			this.velocitàTraslDavDietroEffettiva = -this.velocitàMax;
		}
		if (!Input.GetKey(KeyCode.S) && this.velocitàTraslDavDietroEffettiva < 0f)
		{
			this.velocitàTraslDavDietroEffettiva += 1f;
		}
		if (this.velocitàTraslLatEffettiva < this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.velocitàTraslLatEffettiva += this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.velocitàTraslLatEffettiva = this.velocitàMax / 1.5f;
		}
		if (this.velocitàTraslLatEffettiva > -this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.D))
			{
				this.velocitàTraslLatEffettiva -= this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			this.velocitàTraslLatEffettiva = -this.velocitàMax / 1.5f;
		}
		if (!Input.GetKey(KeyCode.A) && this.velocitàTraslLatEffettiva > 0f)
		{
			this.velocitàTraslLatEffettiva -= 1f;
		}
		if (!Input.GetKey(KeyCode.D) && this.velocitàTraslLatEffettiva < 0f)
		{
			this.velocitàTraslLatEffettiva += 1f;
		}
		if (this.velocitàTraslSalitaEffettiva < this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				this.velocitàTraslSalitaEffettiva += this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			this.velocitàTraslSalitaEffettiva = this.velocitàMax / 1.5f;
		}
		if (this.velocitàTraslSalitaEffettiva > -this.velocitàMax / 1.5f)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.velocitàTraslSalitaEffettiva -= this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.LeftShift))
		{
			this.velocitàTraslSalitaEffettiva = -this.velocitàMax / 1.5f;
		}
		if (!Input.GetKey(KeyCode.Space) && this.velocitàTraslSalitaEffettiva > 0f)
		{
			this.velocitàTraslSalitaEffettiva -= 1f;
		}
		if (!Input.GetKey(KeyCode.LeftShift) && this.velocitàTraslSalitaEffettiva < 0f)
		{
			this.velocitàTraslSalitaEffettiva += 1f;
		}
		this.forzaImbardata = Input.GetAxis("Mouse X");
		this.velocitàImbarEffettiva += this.forzaImbardata;
		if (this.velocitàImbarEffettiva > this.imbardataVelocitàMax)
		{
			this.velocitàImbarEffettiva = this.imbardataVelocitàMax;
		}
		if (this.velocitàImbarEffettiva < -this.imbardataVelocitàMax)
		{
			this.velocitàImbarEffettiva = -this.imbardataVelocitàMax;
		}
		if (this.forzaImbardata == 0f)
		{
			if (this.velocitàImbarEffettiva > 0f)
			{
				this.velocitàImbarEffettiva -= 0.8f;
			}
			else if (this.velocitàImbarEffettiva < 0f)
			{
				this.velocitàImbarEffettiva += 0.8f;
			}
		}
		this.forzaBeccheggio = Input.GetAxis("Mouse Y");
		if (this.forzaBeccheggio > 0f && (base.transform.eulerAngles.x < this.angBeccheggioMin || base.transform.eulerAngles.x > 90f))
		{
			this.velocitàBecchEffettiva += this.forzaBeccheggio;
		}
		if (this.forzaBeccheggio < 0f && (base.transform.eulerAngles.x > this.angBeccheggioMax || base.transform.eulerAngles.x < 90f))
		{
			this.velocitàBecchEffettiva += this.forzaBeccheggio;
		}
		if (this.velocitàBecchEffettiva > this.beccheggioVelocitàMax)
		{
			this.velocitàBecchEffettiva = this.beccheggioVelocitàMax;
		}
		if (this.velocitàBecchEffettiva < -this.beccheggioVelocitàMax)
		{
			this.velocitàBecchEffettiva = -this.beccheggioVelocitàMax;
		}
		if (this.forzaBeccheggio == 0f)
		{
			if (this.velocitàBecchEffettiva > 0f)
			{
				this.velocitàBecchEffettiva -= 0.8f;
			}
			else if (this.velocitàBecchEffettiva < 0f)
			{
				this.velocitàBecchEffettiva += 0.8f;
			}
		}
		this.corpoRigido.AddForce(base.transform.forward * this.velocitàTraslDavDietroEffettiva + -base.transform.right * this.velocitàTraslLatEffettiva + base.transform.up * this.velocitàTraslSalitaEffettiva, ForceMode.Force);
		if (this.voloInverso == 0)
		{
			this.corpoRigido.AddTorque(base.transform.right * this.velocitàBecchEffettiva + Vector3.up * this.velocitàImbarEffettiva, ForceMode.Force);
		}
		else
		{
			this.corpoRigido.AddTorque(-base.transform.right * this.velocitàBecchEffettiva + Vector3.up * this.velocitàImbarEffettiva, ForceMode.Force);
		}
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f);
	}

	// Token: 0x040011EC RID: 4588
	public float velocitàMax;

	// Token: 0x040011ED RID: 4589
	public float accTraslAvantiDietro;

	// Token: 0x040011EE RID: 4590
	public float accTraslLaterale;

	// Token: 0x040011EF RID: 4591
	public float accTraslSalita;

	// Token: 0x040011F0 RID: 4592
	public float beccheggioVelocitàMax;

	// Token: 0x040011F1 RID: 4593
	public float forzaBeccheggio;

	// Token: 0x040011F2 RID: 4594
	public float imbardataVelocitàMax;

	// Token: 0x040011F3 RID: 4595
	public float forzaImbardata;

	// Token: 0x040011F4 RID: 4596
	public float angBeccheggioMax;

	// Token: 0x040011F5 RID: 4597
	public float angBeccheggioMin;

	// Token: 0x040011F6 RID: 4598
	public PhysicMaterial materialeFisico;

	// Token: 0x040011F7 RID: 4599
	public float velocitàTraslLatEffettiva;

	// Token: 0x040011F8 RID: 4600
	public float velocitàTraslDavDietroEffettiva;

	// Token: 0x040011F9 RID: 4601
	public float velocitàTraslSalitaEffettiva;

	// Token: 0x040011FA RID: 4602
	private float velocitàBecchEffettiva;

	// Token: 0x040011FB RID: 4603
	private float velocitàImbarEffettiva;

	// Token: 0x040011FC RID: 4604
	private GameObject infoNeutreTattica;

	// Token: 0x040011FD RID: 4605
	private GameObject terzaCamera;

	// Token: 0x040011FE RID: 4606
	private bool èInPrimaPersona;

	// Token: 0x040011FF RID: 4607
	private Rigidbody corpoRigido;

	// Token: 0x04001200 RID: 4608
	private int voloInverso;
}
