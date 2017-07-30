using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class MOV_LightHelicopter : MonoBehaviour
{
	// Token: 0x0600048B RID: 1163 RVA: 0x000A78DC File Offset: 0x000A5ADC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000A7918 File Offset: 0x000A5B18
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000A7920 File Offset: 0x000A5B20
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.voloInverso = base.GetComponent<PresenzaAlleato>().voloInvertito;
			this.MovimentoInPrimaPersonaFisico();
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000A7950 File Offset: 0x000A5B50
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

	// Token: 0x0600048F RID: 1167 RVA: 0x000A79AC File Offset: 0x000A5BAC
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
		if (this.velocitàTraslLatEffettiva < this.velocitàMax / 1.2f)
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.velocitàTraslLatEffettiva += this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.velocitàTraslLatEffettiva = this.velocitàMax / 1.2f;
		}
		if (this.velocitàTraslLatEffettiva > -this.velocitàMax / 1.2f)
		{
			if (Input.GetKey(KeyCode.D))
			{
				this.velocitàTraslLatEffettiva -= this.accTraslLaterale;
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			this.velocitàTraslLatEffettiva = -this.velocitàMax / 1.2f;
		}
		if (!Input.GetKey(KeyCode.A) && this.velocitàTraslLatEffettiva > 0f)
		{
			this.velocitàTraslLatEffettiva -= 1f;
		}
		if (!Input.GetKey(KeyCode.D) && this.velocitàTraslLatEffettiva < 0f)
		{
			this.velocitàTraslLatEffettiva += 1f;
		}
		if (this.velocitàTraslSalitaEffettiva < this.velocitàMax / 1.2f)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				this.velocitàTraslSalitaEffettiva += this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			this.velocitàTraslSalitaEffettiva = this.velocitàMax / 1.2f;
		}
		if (this.velocitàTraslSalitaEffettiva > -this.velocitàMax / 1.2f)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.velocitàTraslSalitaEffettiva -= this.accTraslSalita;
			}
		}
		else if (Input.GetKey(KeyCode.LeftShift))
		{
			this.velocitàTraslSalitaEffettiva = -this.velocitàMax / 1.2f;
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

	// Token: 0x04001201 RID: 4609
	public float velocitàMax;

	// Token: 0x04001202 RID: 4610
	public float accTraslAvantiDietro;

	// Token: 0x04001203 RID: 4611
	public float accTraslLaterale;

	// Token: 0x04001204 RID: 4612
	public float accTraslSalita;

	// Token: 0x04001205 RID: 4613
	public float beccheggioVelocitàMax;

	// Token: 0x04001206 RID: 4614
	public float forzaBeccheggio;

	// Token: 0x04001207 RID: 4615
	public float imbardataVelocitàMax;

	// Token: 0x04001208 RID: 4616
	public float forzaImbardata;

	// Token: 0x04001209 RID: 4617
	public float angBeccheggioMax;

	// Token: 0x0400120A RID: 4618
	public float angBeccheggioMin;

	// Token: 0x0400120B RID: 4619
	public PhysicMaterial materialeFisico;

	// Token: 0x0400120C RID: 4620
	public float velocitàTraslLatEffettiva;

	// Token: 0x0400120D RID: 4621
	public float velocitàTraslDavDietroEffettiva;

	// Token: 0x0400120E RID: 4622
	public float velocitàTraslSalitaEffettiva;

	// Token: 0x0400120F RID: 4623
	private float velocitàBecchEffettiva;

	// Token: 0x04001210 RID: 4624
	private float velocitàImbarEffettiva;

	// Token: 0x04001211 RID: 4625
	private GameObject infoNeutreTattica;

	// Token: 0x04001212 RID: 4626
	private GameObject terzaCamera;

	// Token: 0x04001213 RID: 4627
	private bool èInPrimaPersona;

	// Token: 0x04001214 RID: 4628
	private Rigidbody corpoRigido;

	// Token: 0x04001215 RID: 4629
	private int voloInverso;
}
