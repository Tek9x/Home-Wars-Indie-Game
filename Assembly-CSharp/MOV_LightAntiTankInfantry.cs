using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class MOV_LightAntiTankInfantry : MonoBehaviour
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x000A9F4C File Offset: 0x000A814C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000A9FA8 File Offset: 0x000A81A8
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000A9FB0 File Offset: 0x000A81B0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x000A9FD4 File Offset: 0x000A81D4
	private void ConfermaControllo()
	{
		if (base.gameObject == this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.èInPrimaPersona = true;
			this.terzaCameraTransform = this.terzaCamera.transform;
			this.MovimentoRotazioni();
		}
		else
		{
			this.èInPrimaPersona = false;
			if (base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
			}
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000AA04C File Offset: 0x000A824C
	private void MovimentoTraslazioni()
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
		if (Input.GetKey(KeyCode.W) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.S) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.forward * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.A) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(-base.transform.right * 500f, ForceMode.Force);
		}
		if (Input.GetKey(KeyCode.D) && magnitude < this.limiteVelocità)
		{
			this.corpoRigido.AddForce(base.transform.right * 500f, ForceMode.Force);
		}
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x000AA1D8 File Offset: 0x000A83D8
	private void MovimentoRotazioni()
	{
		float axis = Input.GetAxis("Mouse X");
		if (this.terzaCamera.GetComponent<TerzaCamera>().èFPS)
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazOriz / 1.5f * Time.deltaTime, 0f);
		}
		else
		{
			base.transform.Rotate(0f, axis * this.velocitàRotazOriz * Time.deltaTime, 0f);
		}
		float num = 0f;
		float axis2 = Input.GetAxis("Mouse Y");
		float num2 = Vector3.Dot(this.terzaCameraTransform.forward, Vector3.up);
		if (axis2 > 0f && num2 < this.angVertMaxPP)
		{
			num = axis2;
		}
		if (axis2 < 0f && num2 > this.angVertMinPP)
		{
			num = axis2;
		}
		this.ossoArma.transform.Rotate(-num * this.velocitàRotazVert * Time.deltaTime, 0f, 0f);
		if (base.GetComponent<ATT_LightAntiTankInfantry>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_LightAntiTankInfantry>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x04001277 RID: 4727
	public float velocitàRotazOriz;

	// Token: 0x04001278 RID: 4728
	public float velocitàRotazVert;

	// Token: 0x04001279 RID: 4729
	public float angVertMaxPP;

	// Token: 0x0400127A RID: 4730
	public float angVertMinPP;

	// Token: 0x0400127B RID: 4731
	public GameObject ossoArma;

	// Token: 0x0400127C RID: 4732
	private Transform ossoArmaTransform;

	// Token: 0x0400127D RID: 4733
	private GameObject infoNeutreTattica;

	// Token: 0x0400127E RID: 4734
	private GameObject terzaCamera;

	// Token: 0x0400127F RID: 4735
	private Transform terzaCameraTransform;

	// Token: 0x04001280 RID: 4736
	private bool èInPrimaPersona;

	// Token: 0x04001281 RID: 4737
	private float limiteVelocità;

	// Token: 0x04001282 RID: 4738
	private float timerRinculo;

	// Token: 0x04001283 RID: 4739
	private Rigidbody corpoRigido;
}
