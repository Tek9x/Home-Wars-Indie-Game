using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class MOV_GrenadeLauncher : MonoBehaviour
{
	// Token: 0x060004AC RID: 1196 RVA: 0x000A93E8 File Offset: 0x000A75E8
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000A9434 File Offset: 0x000A7634
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000A943C File Offset: 0x000A763C
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x000A9460 File Offset: 0x000A7660
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

	// Token: 0x060004B0 RID: 1200 RVA: 0x000A94D8 File Offset: 0x000A76D8
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

	// Token: 0x060004B1 RID: 1201 RVA: 0x000A9664 File Offset: 0x000A7864
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
		this.terzaCamera.transform.Rotate(-num * this.velocitàRotazVert * Time.deltaTime, 0f, 0f);
	}

	// Token: 0x04001252 RID: 4690
	public float velocitàRotazOriz;

	// Token: 0x04001253 RID: 4691
	public float velocitàRotazVert;

	// Token: 0x04001254 RID: 4692
	public float angVertMaxPP;

	// Token: 0x04001255 RID: 4693
	public float angVertMinPP;

	// Token: 0x04001256 RID: 4694
	private GameObject infoNeutreTattica;

	// Token: 0x04001257 RID: 4695
	private GameObject terzaCamera;

	// Token: 0x04001258 RID: 4696
	private Transform terzaCameraTransform;

	// Token: 0x04001259 RID: 4697
	private bool èInPrimaPersona;

	// Token: 0x0400125A RID: 4698
	private float limiteVelocità;

	// Token: 0x0400125B RID: 4699
	private float timerRinculo;

	// Token: 0x0400125C RID: 4700
	private Rigidbody corpoRigido;
}
