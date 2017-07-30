using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class MOV_AUTOM_ArmoredAirship : MonoBehaviour
{
	// Token: 0x06000448 RID: 1096 RVA: 0x000A2258 File Offset: 0x000A0458
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.primaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[0];
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
		this.layerNavigazione = 256;
		this.muoviti = true;
		this.elicaDestra = base.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
		this.elicaSinistra = base.transform.GetChild(2).GetChild(0).GetChild(1).gameObject;
		this.elicaGrande = base.transform.GetChild(2).GetChild(0).GetChild(2).gameObject;
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x000A2340 File Offset: 0x000A0540
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Inattività();
			this.target = base.GetComponent<ATT_ArmoredAirship>().unitàBersaglio;
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
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 150f;
					}
					else if (base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y >= 40f && base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh.y < 150f)
					{
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 100f;
					}
					else
					{
						this.destinazione = base.GetComponent<PresenzaAlleato>().destinazioneSenzaNavMesh + Vector3.up * 40f;
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

	// Token: 0x0600044A RID: 1098 RVA: 0x000A25A0 File Offset: 0x000A07A0
	private void RotazionePale()
	{
		this.elicaDestra.transform.Rotate(Vector3.up * 180f * Time.deltaTime);
		this.elicaSinistra.transform.Rotate(Vector3.up * 180f * Time.deltaTime);
		this.elicaGrande.transform.Rotate(Vector3.up * 70f * Time.deltaTime);
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000A2628 File Offset: 0x000A0828
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
				maxDistance = 50f;
			}
			else
			{
				float num = Vector3.Distance(this.hitSensoreCentrale.point, base.transform.position);
				if (num < 200f)
				{
					maxDistance = num + 50f;
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

	// Token: 0x0600044C RID: 1100 RVA: 0x000A2800 File Offset: 0x000A0A00
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

	// Token: 0x0600044D RID: 1101 RVA: 0x000A2990 File Offset: 0x000A0B90
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
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 20f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
		float num = Vector3.Distance(base.transform.position, this.destinazione);
		if (num < 3f)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitSensoreCentrale, 20f, this.layerNavigazione) && num < 100f)
		{
			base.GetComponent<PresenzaAlleato>().destinazioneOrdinata = false;
		}
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x000A2C3C File Offset: 0x000A0E3C
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
		if (Physics.Raycast(this.origineSensori, base.transform.forward, 20f, this.layerNavigazione))
		{
			base.transform.Rotate(base.transform.up * 1000f * Time.deltaTime);
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x000A2E70 File Offset: 0x000A1070
	private void Inattività()
	{
		if (!base.GetComponent<PresenzaAlleato>().attaccoOrdinato && !base.GetComponent<PresenzaAlleato>().destinazioneOrdinata && base.GetComponent<PresenzaAlleato>().unitàBersaglio == null)
		{
			this.muoviti = false;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x000A2EF8 File Offset: 0x000A10F8
	private void Atterra()
	{
		if (!Physics.Raycast(base.transform.position, -Vector3.up, 26f, this.layerNavigazione))
		{
			base.transform.position += -Vector3.up * 15f * Time.deltaTime;
			this.destinazione = base.transform.position;
		}
		else
		{
			this.atterraHeli = false;
		}
	}

	// Token: 0x04001136 RID: 4406
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001137 RID: 4407
	private float velocitàTraslazione;

	// Token: 0x04001138 RID: 4408
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001139 RID: 4409
	private float velocitàSlittamento;

	// Token: 0x0400113A RID: 4410
	public float velocitàAutoRotazione;

	// Token: 0x0400113B RID: 4411
	private Vector3 origineSensori;

	// Token: 0x0400113C RID: 4412
	public GameObject target;

	// Token: 0x0400113D RID: 4413
	private int layerNavigazione;

	// Token: 0x0400113E RID: 4414
	private GameObject infoNeutreTattica;

	// Token: 0x0400113F RID: 4415
	private GameObject terzaCamera;

	// Token: 0x04001140 RID: 4416
	private GameObject primaCamera;

	// Token: 0x04001141 RID: 4417
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001142 RID: 4418
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001143 RID: 4419
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001144 RID: 4420
	private Vector3 destinazione;

	// Token: 0x04001145 RID: 4421
	private bool destinazioneInVista;

	// Token: 0x04001146 RID: 4422
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001147 RID: 4423
	private int numeroRaggiTrue;

	// Token: 0x04001148 RID: 4424
	private float slittamentoVerticale1;

	// Token: 0x04001149 RID: 4425
	private float slittamentoOrizzontale1;

	// Token: 0x0400114A RID: 4426
	private float slittamentoVerticale2;

	// Token: 0x0400114B RID: 4427
	private float slittamentoOrizzontale2;

	// Token: 0x0400114C RID: 4428
	private Vector3 direzioneRaggioLibero;

	// Token: 0x0400114D RID: 4429
	public bool muoviti;

	// Token: 0x0400114E RID: 4430
	private GameObject elicaDestra;

	// Token: 0x0400114F RID: 4431
	private GameObject elicaSinistra;

	// Token: 0x04001150 RID: 4432
	private GameObject elicaGrande;

	// Token: 0x04001151 RID: 4433
	public bool atterraHeli;
}
