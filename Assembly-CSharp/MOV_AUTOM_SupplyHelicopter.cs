using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class MOV_AUTOM_SupplyHelicopter : MonoBehaviour
{
	// Token: 0x06000470 RID: 1136 RVA: 0x000A55B4 File Offset: 0x000A37B4
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

	// Token: 0x06000471 RID: 1137 RVA: 0x000A5634 File Offset: 0x000A3834
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Inattività();
			if (base.GetComponent<PresenzaAlleato>().destinazioneOrdinata)
			{
				if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 50f)
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
				Quaternion to = Quaternion.LookRotation(this.destinazione - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
			}
			if (this.atterraHeli)
			{
				this.Atterra();
			}
			if (this.muoviti)
			{
				this.NavigazioneSenzaTarget();
			}
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000A57C8 File Offset: 0x000A39C8
	private void RotazionePale()
	{
		base.transform.GetChild(2).GetChild(0).transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
		base.transform.GetChild(2).GetChild(1).transform.Rotate(-Vector3.forward * 1000f * Time.deltaTime);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000A5844 File Offset: 0x000A3A44
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

	// Token: 0x06000474 RID: 1140 RVA: 0x000A5A1C File Offset: 0x000A3C1C
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

	// Token: 0x06000475 RID: 1141 RVA: 0x000A5BAC File Offset: 0x000A3DAC
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

	// Token: 0x06000476 RID: 1142 RVA: 0x000A5E58 File Offset: 0x000A4058
	private void Inattività()
	{
		if (!base.GetComponent<PresenzaAlleato>().attaccoOrdinato && !base.GetComponent<PresenzaAlleato>().destinazioneOrdinata && base.GetComponent<PresenzaAlleato>().unitàBersaglio == null)
		{
			this.muoviti = false;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000A5EE0 File Offset: 0x000A40E0
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

	// Token: 0x0400119D RID: 4509
	public float velocitàTraslazioneIniziale;

	// Token: 0x0400119E RID: 4510
	private float velocitàTraslazione;

	// Token: 0x0400119F RID: 4511
	public float velocitàSlittamentoIniziale;

	// Token: 0x040011A0 RID: 4512
	private float velocitàSlittamento;

	// Token: 0x040011A1 RID: 4513
	public float velocitàAutoRotazione;

	// Token: 0x040011A2 RID: 4514
	private Vector3 origineSensori;

	// Token: 0x040011A3 RID: 4515
	private int layerNavigazione;

	// Token: 0x040011A4 RID: 4516
	private GameObject infoNeutreTattica;

	// Token: 0x040011A5 RID: 4517
	private GameObject terzaCamera;

	// Token: 0x040011A6 RID: 4518
	private GameObject primaCamera;

	// Token: 0x040011A7 RID: 4519
	private RaycastHit hitSensoreCentrale;

	// Token: 0x040011A8 RID: 4520
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x040011A9 RID: 4521
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x040011AA RID: 4522
	private Vector3 destinazione;

	// Token: 0x040011AB RID: 4523
	private bool destinazioneInVista;

	// Token: 0x040011AC RID: 4524
	private int ampiezzaSensoreCircolare;

	// Token: 0x040011AD RID: 4525
	private int numeroRaggiTrue;

	// Token: 0x040011AE RID: 4526
	private float slittamentoVerticale1;

	// Token: 0x040011AF RID: 4527
	private float slittamentoOrizzontale1;

	// Token: 0x040011B0 RID: 4528
	private float slittamentoVerticale2;

	// Token: 0x040011B1 RID: 4529
	private float slittamentoOrizzontale2;

	// Token: 0x040011B2 RID: 4530
	private Vector3 direzioneRaggioLibero;

	// Token: 0x040011B3 RID: 4531
	public bool muoviti;

	// Token: 0x040011B4 RID: 4532
	public bool atterraHeli;
}
