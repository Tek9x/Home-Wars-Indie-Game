using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class MOV_Sapper : MonoBehaviour
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x000ABF3C File Offset: 0x000AA13C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
		this.ossoArmaTransform = this.ossoArma.transform;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000ABF98 File Offset: 0x000AA198
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000ABFA0 File Offset: 0x000AA1A0
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x000ABFC4 File Offset: 0x000AA1C4
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

	// Token: 0x060004FB RID: 1275 RVA: 0x000AC03C File Offset: 0x000AA23C
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

	// Token: 0x060004FC RID: 1276 RVA: 0x000AC1C8 File Offset: 0x000AA3C8
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

	// Token: 0x040012E0 RID: 4832
	public float velocitàRotazOriz;

	// Token: 0x040012E1 RID: 4833
	public float velocitàRotazVert;

	// Token: 0x040012E2 RID: 4834
	public float angVertMaxPP;

	// Token: 0x040012E3 RID: 4835
	public float angVertMinPP;

	// Token: 0x040012E4 RID: 4836
	public GameObject ossoArma;

	// Token: 0x040012E5 RID: 4837
	private Transform ossoArmaTransform;

	// Token: 0x040012E6 RID: 4838
	private GameObject infoNeutreTattica;

	// Token: 0x040012E7 RID: 4839
	private GameObject terzaCamera;

	// Token: 0x040012E8 RID: 4840
	private Transform terzaCameraTransform;

	// Token: 0x040012E9 RID: 4841
	private bool èInPrimaPersona;

	// Token: 0x040012EA RID: 4842
	private float limiteVelocità;

	// Token: 0x040012EB RID: 4843
	private float timerRinculo;

	// Token: 0x040012EC RID: 4844
	private Rigidbody corpoRigido;
}
