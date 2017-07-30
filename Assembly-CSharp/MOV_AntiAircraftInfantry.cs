using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class MOV_AntiAircraftInfantry : MonoBehaviour
{
	// Token: 0x06000497 RID: 1175 RVA: 0x000A8874 File Offset: 0x000A6A74
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x000A88D0 File Offset: 0x000A6AD0
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000A88D8 File Offset: 0x000A6AD8
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000A88FC File Offset: 0x000A6AFC
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

	// Token: 0x0600049B RID: 1179 RVA: 0x000A8974 File Offset: 0x000A6B74
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

	// Token: 0x0600049C RID: 1180 RVA: 0x000A8B00 File Offset: 0x000A6D00
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
		if (base.GetComponent<ATT_AntiAircraftInfantry>().avviaRinculo)
		{
			this.ossoArma.transform.Rotate(-Vector3.right * base.GetComponent<PresenzaAlleato>().ListaValoriArma1[8] * base.GetComponent<ATT_AntiAircraftInfantry>().timerRinculo * Time.deltaTime);
		}
	}

	// Token: 0x0400122B RID: 4651
	public float velocitàRotazOriz;

	// Token: 0x0400122C RID: 4652
	public float velocitàRotazVert;

	// Token: 0x0400122D RID: 4653
	public float angVertMaxPP;

	// Token: 0x0400122E RID: 4654
	public float angVertMinPP;

	// Token: 0x0400122F RID: 4655
	public GameObject ossoArma;

	// Token: 0x04001230 RID: 4656
	private Transform ossoArmaTransform;

	// Token: 0x04001231 RID: 4657
	private GameObject infoNeutreTattica;

	// Token: 0x04001232 RID: 4658
	private GameObject terzaCamera;

	// Token: 0x04001233 RID: 4659
	private Transform terzaCameraTransform;

	// Token: 0x04001234 RID: 4660
	private bool èInPrimaPersona;

	// Token: 0x04001235 RID: 4661
	private float limiteVelocità;

	// Token: 0x04001236 RID: 4662
	private float timerRinculo;

	// Token: 0x04001237 RID: 4663
	private Rigidbody corpoRigido;
}
