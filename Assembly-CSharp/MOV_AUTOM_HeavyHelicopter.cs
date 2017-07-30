using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class MOV_AUTOM_HeavyHelicopter : MonoBehaviour
{
	// Token: 0x0600045C RID: 1116 RVA: 0x000A3C40 File Offset: 0x000A1E40
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

	// Token: 0x0600045D RID: 1117 RVA: 0x000A3CC0 File Offset: 0x000A1EC0
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Inattività();
			this.target = base.GetComponent<ATT_HeavyHelicopter>().unitàBersaglio;
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

	// Token: 0x0600045E RID: 1118 RVA: 0x000A3F20 File Offset: 0x000A2120
	private void RotazionePale()
	{
		base.transform.GetChild(2).GetChild(0).transform.Rotate(-Vector3.forward * 1000f * Time.deltaTime);
		base.transform.GetChild(2).GetChild(1).transform.Rotate(Vector3.right * 1000f * Time.deltaTime);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x000A3F9C File Offset: 0x000A219C
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

	// Token: 0x06000460 RID: 1120 RVA: 0x000A4174 File Offset: 0x000A2374
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

	// Token: 0x06000461 RID: 1121 RVA: 0x000A4304 File Offset: 0x000A2504
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

	// Token: 0x06000462 RID: 1122 RVA: 0x000A45B0 File Offset: 0x000A27B0
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

	// Token: 0x06000463 RID: 1123 RVA: 0x000A47E4 File Offset: 0x000A29E4
	private void Inattività()
	{
		if (!base.GetComponent<PresenzaAlleato>().attaccoOrdinato && !base.GetComponent<PresenzaAlleato>().destinazioneOrdinata && base.GetComponent<PresenzaAlleato>().unitàBersaglio == null)
		{
			this.muoviti = false;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000A486C File Offset: 0x000A2A6C
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

	// Token: 0x0400116B RID: 4459
	public float velocitàTraslazioneIniziale;

	// Token: 0x0400116C RID: 4460
	private float velocitàTraslazione;

	// Token: 0x0400116D RID: 4461
	public float velocitàSlittamentoIniziale;

	// Token: 0x0400116E RID: 4462
	private float velocitàSlittamento;

	// Token: 0x0400116F RID: 4463
	public float velocitàAutoRotazione;

	// Token: 0x04001170 RID: 4464
	private Vector3 origineSensori;

	// Token: 0x04001171 RID: 4465
	public GameObject target;

	// Token: 0x04001172 RID: 4466
	private int layerNavigazione;

	// Token: 0x04001173 RID: 4467
	private GameObject infoNeutreTattica;

	// Token: 0x04001174 RID: 4468
	private GameObject terzaCamera;

	// Token: 0x04001175 RID: 4469
	private GameObject primaCamera;

	// Token: 0x04001176 RID: 4470
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001177 RID: 4471
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001178 RID: 4472
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001179 RID: 4473
	private Vector3 destinazione;

	// Token: 0x0400117A RID: 4474
	private bool destinazioneInVista;

	// Token: 0x0400117B RID: 4475
	private int ampiezzaSensoreCircolare;

	// Token: 0x0400117C RID: 4476
	private int numeroRaggiTrue;

	// Token: 0x0400117D RID: 4477
	private float slittamentoVerticale1;

	// Token: 0x0400117E RID: 4478
	private float slittamentoOrizzontale1;

	// Token: 0x0400117F RID: 4479
	private float slittamentoVerticale2;

	// Token: 0x04001180 RID: 4480
	private float slittamentoOrizzontale2;

	// Token: 0x04001181 RID: 4481
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001182 RID: 4482
	public bool muoviti;

	// Token: 0x04001183 RID: 4483
	public bool atterraHeli;
}
