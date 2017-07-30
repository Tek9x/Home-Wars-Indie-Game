using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class MOV_SupplyHelicopter : MonoBehaviour
{
	// Token: 0x06000491 RID: 1169 RVA: 0x000A80A8 File Offset: 0x000A62A8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000A80E4 File Offset: 0x000A62E4
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x000A80EC File Offset: 0x000A62EC
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona)
		{
			this.voloInverso = base.GetComponent<PresenzaAlleato>().voloInvertito;
			this.MovimentoInPrimaPersonaFisico();
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000A811C File Offset: 0x000A631C
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

	// Token: 0x06000495 RID: 1173 RVA: 0x000A8178 File Offset: 0x000A6378
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

	// Token: 0x04001216 RID: 4630
	public float velocitàMax;

	// Token: 0x04001217 RID: 4631
	public float accTraslAvantiDietro;

	// Token: 0x04001218 RID: 4632
	public float accTraslLaterale;

	// Token: 0x04001219 RID: 4633
	public float accTraslSalita;

	// Token: 0x0400121A RID: 4634
	public float beccheggioVelocitàMax;

	// Token: 0x0400121B RID: 4635
	public float forzaBeccheggio;

	// Token: 0x0400121C RID: 4636
	public float imbardataVelocitàMax;

	// Token: 0x0400121D RID: 4637
	public float forzaImbardata;

	// Token: 0x0400121E RID: 4638
	public float angBeccheggioMax;

	// Token: 0x0400121F RID: 4639
	public float angBeccheggioMin;

	// Token: 0x04001220 RID: 4640
	public PhysicMaterial materialeFisico;

	// Token: 0x04001221 RID: 4641
	public float velocitàTraslLatEffettiva;

	// Token: 0x04001222 RID: 4642
	public float velocitàTraslDavDietroEffettiva;

	// Token: 0x04001223 RID: 4643
	public float velocitàTraslSalitaEffettiva;

	// Token: 0x04001224 RID: 4644
	private float velocitàBecchEffettiva;

	// Token: 0x04001225 RID: 4645
	private float velocitàImbarEffettiva;

	// Token: 0x04001226 RID: 4646
	private GameObject infoNeutreTattica;

	// Token: 0x04001227 RID: 4647
	private GameObject terzaCamera;

	// Token: 0x04001228 RID: 4648
	private bool èInPrimaPersona;

	// Token: 0x04001229 RID: 4649
	private Rigidbody corpoRigido;

	// Token: 0x0400122A RID: 4650
	private int voloInverso;
}
