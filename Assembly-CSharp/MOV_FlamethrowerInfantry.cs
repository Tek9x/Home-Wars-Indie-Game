using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class MOV_FlamethrowerInfantry : MonoBehaviour
{
	// Token: 0x060004A5 RID: 1189 RVA: 0x000A9054 File Offset: 0x000A7254
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x000A90B0 File Offset: 0x000A72B0
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x000A90B8 File Offset: 0x000A72B8
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000A90DC File Offset: 0x000A72DC
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

	// Token: 0x060004A9 RID: 1193 RVA: 0x000A9154 File Offset: 0x000A7354
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

	// Token: 0x060004AA RID: 1194 RVA: 0x000A92E0 File Offset: 0x000A74E0
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
	}

	// Token: 0x04001245 RID: 4677
	public float velocitàRotazOriz;

	// Token: 0x04001246 RID: 4678
	public float velocitàRotazVert;

	// Token: 0x04001247 RID: 4679
	public float angVertMaxPP;

	// Token: 0x04001248 RID: 4680
	public float angVertMinPP;

	// Token: 0x04001249 RID: 4681
	public GameObject ossoArma;

	// Token: 0x0400124A RID: 4682
	private Transform ossoArmaTransform;

	// Token: 0x0400124B RID: 4683
	private GameObject infoNeutreTattica;

	// Token: 0x0400124C RID: 4684
	private GameObject terzaCamera;

	// Token: 0x0400124D RID: 4685
	private Transform terzaCameraTransform;

	// Token: 0x0400124E RID: 4686
	private bool èInPrimaPersona;

	// Token: 0x0400124F RID: 4687
	private float limiteVelocità;

	// Token: 0x04001250 RID: 4688
	private float timerRinculo;

	// Token: 0x04001251 RID: 4689
	private Rigidbody corpoRigido;
}
