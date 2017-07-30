using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class MOV_AUTOM_AttackHelicopter : MonoBehaviour
{
	// Token: 0x06000452 RID: 1106 RVA: 0x000A2F88 File Offset: 0x000A1188
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.muoviti = true;
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x000A3008 File Offset: 0x000A1208
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Inattività();
			this.target = base.GetComponent<ATT_AttackHelicopter>().unitàBersaglio;
			if (this.target != null && !this.target.GetComponent<PresenzaNemico>().insettoVolante)
			{
				this.destinazione = this.target.transform.position + Vector3.up * 50f;
				Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			}
			if (this.target == null)
			{
				if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
				{
					if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 40f)
					{
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 70f;
					}
					else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 40f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 150f)
					{
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 50f;
					}
					else
					{
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 20f;
					}
					Quaternion to2 = Quaternion.LookRotation(this.destinazione - base.transform.position);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
				}
				if (this.muoviti)
				{
					this.NavigazioneSenzaTarget();
				}
			}
			else if (this.muoviti)
			{
				this.NavigazioneConTarget();
			}
			if (this.atterraHeli)
			{
				this.Atterra();
			}
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x000A3268 File Offset: 0x000A1468
	private void RotazionePale()
	{
		base.transform.GetChild(2).GetChild(0).transform.Rotate(Vector3.forward * 2000f * Time.deltaTime);
		base.transform.GetChild(2).GetChild(1).transform.Rotate(Vector3.right * 2000f * Time.deltaTime);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x000A32E0 File Offset: 0x000A14E0
	private void SensoriAnteriori()
	{
		if (Physics.Linecast(base.transform.position, this.destinazione, this.layerNavigazione))
		{
			this.destinazioneInVista = false;
		}
		else
		{
			this.destinazioneInVista = true;
		}
		float maxDistance = 200f;
		Quaternion rotation = Quaternion.identity;
		this.numeroRaggiTrue = 8;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitSensoreCentrale, (float)this.layerNavigazione))
		{
			if (this.destinazioneInVista)
			{
				maxDistance = 30f;
			}
			else
			{
				float num = Vector3.Distance(this.hitSensoreCentrale.point, base.transform.position);
				if (num < 200f)
				{
					maxDistance = num + 30f;
				}
			}
		}
		int num2 = 10;
		while (num2 <= 90 && this.numeroRaggiTrue == 8)
		{
			rotation = Quaternion.AngleAxis((float)num2, base.transform.right);
			this.direzioneRaggioLibero = Vector3.zero;
			float num3 = 99999f;
			this.numeroRaggiTrue = 0;
			for (int i = 0; i < 360; i += 45)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)i, base.transform.forward);
				Ray ray = new Ray(this.origineSensori, rotation2 * (rotation * base.transform.forward));
				if (!Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, maxDistance, this.layerNavigazione))
				{
					float num4 = Vector3.Distance(this.destinazione, ray.GetPoint(50f));
					if (num4 < num3)
					{
						num3 = num4;
						this.direzioneRaggioLibero = rotation2 * (rotation * base.transform.forward);
					}
				}
				else
				{
					this.numeroRaggiTrue++;
				}
			}
			num2 += 40;
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x000A34B8 File Offset: 0x000A16B8
	private void SensoriPosteriori()
	{
		this.slittamentoVerticale1 = 0f;
		this.slittamentoOrizzontale1 = 0f;
		for (int i = 40; i < 95; i += 45)
		{
			Quaternion rotation = Quaternion.AngleAxis((float)i, base.transform.right);
			for (int j = 0; j < 360; j += 90)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)j, base.transform.forward);
				Ray ray = new Ray(this.origineSensori, rotation2 * (rotation * base.transform.forward));
				if (Physics.Raycast(ray, out this.hitSensoreCircolarePosteriore, 10f, this.layerNavigazione))
				{
					if (i == 40)
					{
						if (j == 0)
						{
							this.slittamentoVerticale1 = this.velocitàSlittamento;
						}
						else if (j == 90)
						{
							this.slittamentoOrizzontale1 = -this.velocitàSlittamento;
						}
						else if (j == 180)
						{
							this.slittamentoVerticale1 = -this.velocitàSlittamento;
						}
						else if (j == 270)
						{
							this.slittamentoOrizzontale1 = this.velocitàSlittamento;
						}
					}
					if (i == 85)
					{
						if (j == 0)
						{
							this.slittamentoVerticale2 = this.velocitàSlittamento;
						}
						else if (j == 90)
						{
							this.slittamentoOrizzontale2 = -this.velocitàSlittamento;
						}
						else if (j == 180)
						{
							this.slittamentoVerticale2 = -this.velocitàSlittamento;
						}
						else if (j == 270)
						{
							this.slittamentoOrizzontale2 = this.velocitàSlittamento;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x000A3648 File Offset: 0x000A1848
	private void NavigazioneSenzaTarget()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		if (this.destinazioneInVista)
		{
			Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			base.transform.position += normalized * this.velocitàTraslazione * Time.deltaTime;
		}
		else
		{
			Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (this.numeroRaggiTrue == 8)
		{
			base.transform.Rotate(base.transform.up * this.velocitàAutoRotazione * Time.deltaTime);
		}
		else
		{
			base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		}
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 15f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (num < 3f)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitSensoreCentrale, 20f, this.layerNavigazione) && num < 70f)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x000A38F4 File Offset: 0x000A1AF4
	private void NavigazioneConTarget()
	{
		Vector3 normalized = (this.destinazione - base.transform.position).normalized;
		if (this.destinazioneInVista)
		{
			Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			base.transform.position += normalized * this.velocitàTraslazione * Time.deltaTime;
		}
		else
		{
			Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		if (this.numeroRaggiTrue == 8)
		{
			base.transform.Rotate(base.transform.up * this.velocitàAutoRotazione * Time.deltaTime);
		}
		else
		{
			base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		}
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 15f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x000A3B28 File Offset: 0x000A1D28
	private void Inattività()
	{
		if (!base.GetComponent<PresenzaAlleato>().attaccoOrdinato && !base.GetComponent<PresenzaAlleato>().destinazioneOrdinata && base.GetComponent<PresenzaAlleato>().unitàBersaglio == null)
		{
			this.muoviti = false;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x000A3BB0 File Offset: 0x000A1DB0
	private void Atterra()
	{
		if (!Physics.Raycast(base.transform.position, -Vector3.up, 3.5f, this.layerNavigazione))
		{
			base.transform.position += -Vector3.up * 15f * Time.deltaTime;
			this.destinazione = base.transform.position;
		}
		else
		{
			this.atterraHeli = false;
		}
	}

	// Token: 0x04001152 RID: 4434
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001153 RID: 4435
	private float velocitàTraslazione;

	// Token: 0x04001154 RID: 4436
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001155 RID: 4437
	private float velocitàSlittamento;

	// Token: 0x04001156 RID: 4438
	public float velocitàAutoRotazione;

	// Token: 0x04001157 RID: 4439
	private Vector3 origineSensori;

	// Token: 0x04001158 RID: 4440
	public GameObject target;

	// Token: 0x04001159 RID: 4441
	private int layerNavigazione;

	// Token: 0x0400115A RID: 4442
	private GameObject infoNeutreTattica;

	// Token: 0x0400115B RID: 4443
	private GameObject terzaCamera;

	// Token: 0x0400115C RID: 4444
	private GameObject primaCamera;

	// Token: 0x0400115D RID: 4445
	private RaycastHit hitSensoreCentrale;

	// Token: 0x0400115E RID: 4446
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x0400115F RID: 4447
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001160 RID: 4448
	private Vector3 destinazione;

	// Token: 0x04001161 RID: 4449
	private bool destinazioneInVista;

	// Token: 0x04001162 RID: 4450
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001163 RID: 4451
	private int numeroRaggiTrue;

	// Token: 0x04001164 RID: 4452
	private float slittamentoVerticale1;

	// Token: 0x04001165 RID: 4453
	private float slittamentoOrizzontale1;

	// Token: 0x04001166 RID: 4454
	private float slittamentoVerticale2;

	// Token: 0x04001167 RID: 4455
	private float slittamentoOrizzontale2;

	// Token: 0x04001168 RID: 4456
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001169 RID: 4457
	public bool muoviti;

	// Token: 0x0400116A RID: 4458
	public bool atterraHeli;
}
