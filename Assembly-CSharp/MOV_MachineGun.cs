using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class MOV_MachineGun : MonoBehaviour
{
	// Token: 0x060004CF RID: 1231 RVA: 0x000AA72C File Offset: 0x000A892C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.limiteVelocità = base.GetComponent<NavMeshAgent>().speed;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x000AA778 File Offset: 0x000A8978
	private void Update()
	{
		this.ConfermaControllo();
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000AA780 File Offset: 0x000A8980
	private void FixedUpdate()
	{
		if (this.èInPrimaPersona && !base.GetComponent<PresenzaAlleato>().èParà)
		{
			this.MovimentoTraslazioni();
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000AA7A4 File Offset: 0x000A89A4
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

	// Token: 0x060004D3 RID: 1235 RVA: 0x000AA81C File Offset: 0x000A8A1C
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

	// Token: 0x060004D4 RID: 1236 RVA: 0x000AA9A8 File Offset: 0x000A8BA8
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

	// Token: 0x04001291 RID: 4753
	public float velocitàRotazOriz;

	// Token: 0x04001292 RID: 4754
	public float velocitàRotazVert;

	// Token: 0x04001293 RID: 4755
	public float angVertMaxPP;

	// Token: 0x04001294 RID: 4756
	public float angVertMinPP;

	// Token: 0x04001295 RID: 4757
	private GameObject infoNeutreTattica;

	// Token: 0x04001296 RID: 4758
	private GameObject terzaCamera;

	// Token: 0x04001297 RID: 4759
	private Transform terzaCameraTransform;

	// Token: 0x04001298 RID: 4760
	private bool èInPrimaPersona;

	// Token: 0x04001299 RID: 4761
	private float limiteVelocità;

	// Token: 0x0400129A RID: 4762
	private float timerRinculo;

	// Token: 0x0400129B RID: 4763
	private Rigidbody corpoRigido;
}
