using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class MOV_AUTOM_LightHelicopter : MonoBehaviour
{
	// Token: 0x06000466 RID: 1126 RVA: 0x000A48FC File Offset: 0x000A2AFC
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

	// Token: 0x06000467 RID: 1127 RVA: 0x000A497C File Offset: 0x000A2B7C
	private void Update()
	{
		this.origineSensori = base.transform.position;
		this.RotazionePale();
		if (base.gameObject != this.terzaCamera.GetComponent<TerzaCamera>().ospiteCamera)
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Inattività();
			this.target = base.GetComponent<ATT_LightHelicopter>().unitàBersaglio;
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

	// Token: 0x06000468 RID: 1128 RVA: 0x000A4BDC File Offset: 0x000A2DDC
	private void RotazionePale()
	{
		base.transform.GetChild(2).GetChild(0).transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
		base.transform.GetChild(2).GetChild(1).transform.Rotate(Vector3.right * 1000f * Time.deltaTime);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x000A4C54 File Offset: 0x000A2E54
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

	// Token: 0x0600046A RID: 1130 RVA: 0x000A4E2C File Offset: 0x000A302C
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

	// Token: 0x0600046B RID: 1131 RVA: 0x000A4FBC File Offset: 0x000A31BC
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

	// Token: 0x0600046C RID: 1132 RVA: 0x000A5268 File Offset: 0x000A3468
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

	// Token: 0x0600046D RID: 1133 RVA: 0x000A549C File Offset: 0x000A369C
	private void Inattività()
	{
		if (!base.GetComponent<PresenzaAlleato>().attaccoOrdinato && !base.GetComponent<PresenzaAlleato>().destinazioneOrdinata && base.GetComponent<PresenzaAlleato>().unitàBersaglio == null)
		{
			this.muoviti = false;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x000A5524 File Offset: 0x000A3724
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

	// Token: 0x04001184 RID: 4484
	public float velocitàTraslazioneIniziale;

	// Token: 0x04001185 RID: 4485
	private float velocitàTraslazione;

	// Token: 0x04001186 RID: 4486
	public float velocitàSlittamentoIniziale;

	// Token: 0x04001187 RID: 4487
	private float velocitàSlittamento;

	// Token: 0x04001188 RID: 4488
	public float velocitàAutoRotazione;

	// Token: 0x04001189 RID: 4489
	private Vector3 origineSensori;

	// Token: 0x0400118A RID: 4490
	public GameObject target;

	// Token: 0x0400118B RID: 4491
	private int layerNavigazione;

	// Token: 0x0400118C RID: 4492
	private GameObject infoNeutreTattica;

	// Token: 0x0400118D RID: 4493
	private GameObject terzaCamera;

	// Token: 0x0400118E RID: 4494
	private GameObject primaCamera;

	// Token: 0x0400118F RID: 4495
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001190 RID: 4496
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001191 RID: 4497
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001192 RID: 4498
	private Vector3 destinazione;

	// Token: 0x04001193 RID: 4499
	private bool destinazioneInVista;

	// Token: 0x04001194 RID: 4500
	private int ampiezzaSensoreCircolare;

	// Token: 0x04001195 RID: 4501
	private int numeroRaggiTrue;

	// Token: 0x04001196 RID: 4502
	private float slittamentoVerticale1;

	// Token: 0x04001197 RID: 4503
	private float slittamentoOrizzontale1;

	// Token: 0x04001198 RID: 4504
	private float slittamentoVerticale2;

	// Token: 0x04001199 RID: 4505
	private float slittamentoOrizzontale2;

	// Token: 0x0400119A RID: 4506
	private Vector3 direzioneRaggioLibero;

	// Token: 0x0400119B RID: 4507
	public bool muoviti;

	// Token: 0x0400119C RID: 4508
	public bool atterraHeli;
}
