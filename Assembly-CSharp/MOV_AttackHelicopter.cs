using System;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class MOV_AttackHelicopter : MonoBehaviour
{
	// Token: 0x0600047F RID: 1151 RVA: 0x000A6944 File Offset: 0x000A4B44
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x000A6980 File Offset: 0x000A4B80
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000A6988 File Offset: 0x000A4B88
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.voloInverso = base.GetComponent<PresenzaAlleato>().voloInvertito;
			this.MovimentoInPrimaPersonaFisico();
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x000A69B8 File Offset: 0x000A4BB8
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

	// Token: 0x06000483 RID: 1155 RVA: 0x000A6A14 File Offset: 0x000A4C14
	private void MovimentoInPrimaPersonaFisico()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.GetComponent<BoxCollider>().isTrigger = false;
			base.GetComponent<BoxCollider>().material = this.materialeFisico;
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

	// Token: 0x040011D7 RID: 4567
	public float velocitàMax;

	// Token: 0x040011D8 RID: 4568
	public float accTraslAvantiDietro;

	// Token: 0x040011D9 RID: 4569
	public float accTraslLaterale;

	// Token: 0x040011DA RID: 4570
	public float accTraslSalita;

	// Token: 0x040011DB RID: 4571
	public float beccheggioVelocitàMax;

	// Token: 0x040011DC RID: 4572
	public float forzaBeccheggio;

	// Token: 0x040011DD RID: 4573
	public float imbardataVelocitàMax;

	// Token: 0x040011DE RID: 4574
	public float forzaImbardata;

	// Token: 0x040011DF RID: 4575
	public float angBeccheggioMax;

	// Token: 0x040011E0 RID: 4576
	public float angBeccheggioMin;

	// Token: 0x040011E1 RID: 4577
	public PhysicMaterial materialeFisico;

	// Token: 0x040011E2 RID: 4578
	public float velocitàTraslLatEffettiva;

	// Token: 0x040011E3 RID: 4579
	public float velocitàTraslDavDietroEffettiva;

	// Token: 0x040011E4 RID: 4580
	public float velocitàTraslSalitaEffettiva;

	// Token: 0x040011E5 RID: 4581
	private float velocitàBecchEffettiva;

	// Token: 0x040011E6 RID: 4582
	private float velocitàImbarEffettiva;

	// Token: 0x040011E7 RID: 4583
	private GameObject infoNeutreTattica;

	// Token: 0x040011E8 RID: 4584
	private GameObject terzaCamera;

	// Token: 0x040011E9 RID: 4585
	private bool èInPrimaPersona;

	// Token: 0x040011EA RID: 4586
	private Rigidbody corpoRigido;

	// Token: 0x040011EB RID: 4587
	private int voloInverso;
}
