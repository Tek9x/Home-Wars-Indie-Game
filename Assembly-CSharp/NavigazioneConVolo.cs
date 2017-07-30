using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class NavigazioneConVolo : MonoBehaviour
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x00114AB4 File Offset: 0x00112CB4
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerNavigazione = 524544;
		this.tipoBattaglia = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().tipoBattaglia;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00114AF4 File Offset: 0x00112CF4
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.muoviti = base.GetComponent<PresenzaNemico>().muoviti;
		this.morto = base.GetComponent<PresenzaNemico>().morto;
		this.allontanamentoPerAtt = base.GetComponent<PresenzaNemico>().allontanamentoPerAtt;
		this.bersaglio = base.GetComponent<PresenzaNemico>().bersaglio;
		if (this.bersaglio && this.bersaglio.tag != "ObbiettivoTattico" && !base.GetComponent<PresenzaNemico>().allontanamentoPerAtt)
		{
			this.destPrincipale = this.bersaglio.transform.position;
		}
		else
		{
			this.destPrincipale = base.GetComponent<PresenzaNemico>().destinazione;
		}
		this.SensoriAnteriori();
		if (this.tipoBattaglia == 7 && base.GetComponent<PresenzaNemico>().ricercaAssente)
		{
			if (Vector3.Distance(base.transform.position, this.destPrincipale) < 5f)
			{
				base.GetComponent<PresenzaNemico>().allontanamentoPerAtt = true;
				this.allontanamentoPerAtt = true;
				this.partenzaTimerAllont = true;
			}
			if (this.partenzaTimerAllont)
			{
				this.timerAllontanamento += Time.deltaTime;
				if (this.timerAllontanamento > 8f)
				{
					this.timerAllontanamento = 0f;
					base.GetComponent<PresenzaNemico>().allontanamentoPerAtt = false;
					this.partenzaTimerAllont = false;
				}
			}
		}
		if (this.allontanamentoPerAtt && this.bersaglio != null)
		{
			if (Vector3.Distance(base.transform.position, this.destPrincipale) < 3f)
			{
				this.destPostAttDecisa = false;
			}
			if (!this.destPostAttDecisa)
			{
				if ((this.bersaglio.GetComponent<PresenzaAlleato>() && this.bersaglio.GetComponent<PresenzaAlleato>().volante) || this.bersaglio == this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().satellite)
				{
					this.destPostAtt = new Vector3(this.centroInsetto.x + (float)UnityEngine.Random.Range(-70, 70), this.bersaglio.transform.position.y + (float)UnityEngine.Random.Range(-70, 70), this.centroInsetto.z + (float)UnityEngine.Random.Range(-70, 70));
					this.destPostAttDecisa = true;
					this.destPrincipale = this.destPostAtt;
					base.GetComponent<PresenzaNemico>().destinazione = this.destPrincipale;
				}
				else
				{
					this.destPostAtt = new Vector3(this.centroInsetto.x + (float)UnityEngine.Random.Range(-80, 80), this.bersaglio.transform.position.y + (float)UnityEngine.Random.Range(10, 80), this.centroInsetto.z + (float)UnityEngine.Random.Range(-80, 80));
					this.destPostAttDecisa = true;
					this.destPrincipale = this.destPostAtt;
					base.GetComponent<PresenzaNemico>().destinazione = this.destPrincipale;
				}
			}
		}
		else
		{
			this.destPostAttDecisa = false;
		}
		if (!this.morto)
		{
			this.NavigazioneComune();
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00114E1C File Offset: 0x0011301C
	private void SensoriAnteriori()
	{
		if (Physics.Linecast(this.centroInsetto, this.destPrincipale, this.layerNavigazione))
		{
			this.destinazioneInVista = false;
		}
		else
		{
			this.destinazioneInVista = true;
		}
		float maxDistance = 200f;
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
		Quaternion rotation = Quaternion.identity;
		this.numeroRaggiTrue = 8;
		int num2 = 5;
		while (num2 <= 90 && this.numeroRaggiTrue == 8)
		{
			rotation = Quaternion.AngleAxis((float)num2, base.transform.right);
			this.direzioneRaggioLibero = Vector3.zero;
			float num3 = 99999f;
			this.numeroRaggiTrue = 0;
			for (int i = 0; i < 360; i += 45)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)i, base.transform.forward);
				Ray ray = new Ray(this.centroInsetto, rotation2 * (rotation * base.transform.forward));
				if (!Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, maxDistance, this.layerNavigazione))
				{
					float num4 = Vector3.Distance(this.destPrincipale, ray.GetPoint(50f));
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

	// Token: 0x060007C8 RID: 1992 RVA: 0x00114FF0 File Offset: 0x001131F0
	private void NavigazioneComune()
	{
		if (this.destinazioneInVista)
		{
			Vector3 normalized = (this.destPrincipale - this.centroInsetto).normalized;
			Quaternion to = Quaternion.LookRotation(normalized);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàRotazione * Time.deltaTime);
		}
		else
		{
			Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàRotazione * Time.deltaTime);
		}
		if (this.muoviti)
		{
			base.transform.position += base.transform.forward * this.velocitàTraslazione * Time.deltaTime;
		}
	}

	// Token: 0x04001D3E RID: 7486
	private GameObject infoNeutreTattica;

	// Token: 0x04001D3F RID: 7487
	private Vector3 centroInsetto;

	// Token: 0x04001D40 RID: 7488
	public bool attaccoStazionario;

	// Token: 0x04001D41 RID: 7489
	public bool attaccoACarica;

	// Token: 0x04001D42 RID: 7490
	public float velocitàTraslazione;

	// Token: 0x04001D43 RID: 7491
	public float velocitàRotazione;

	// Token: 0x04001D44 RID: 7492
	private GameObject bersaglio;

	// Token: 0x04001D45 RID: 7493
	private Vector3 destPrincipale;

	// Token: 0x04001D46 RID: 7494
	private int layerNavigazione;

	// Token: 0x04001D47 RID: 7495
	private Animator insettoAnim;

	// Token: 0x04001D48 RID: 7496
	private bool muoviti;

	// Token: 0x04001D49 RID: 7497
	private bool morto;

	// Token: 0x04001D4A RID: 7498
	private bool allontanamentoPerAtt;

	// Token: 0x04001D4B RID: 7499
	private bool destinazioneInVista;

	// Token: 0x04001D4C RID: 7500
	private int numeroRaggiTrue;

	// Token: 0x04001D4D RID: 7501
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001D4E RID: 7502
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001D4F RID: 7503
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001D50 RID: 7504
	private bool destPostAttDecisa;

	// Token: 0x04001D51 RID: 7505
	private Vector3 destPostAtt;

	// Token: 0x04001D52 RID: 7506
	private Vector3 destFitt;

	// Token: 0x04001D53 RID: 7507
	private int tipoBattaglia;

	// Token: 0x04001D54 RID: 7508
	private bool partenzaTimerAllont;

	// Token: 0x04001D55 RID: 7509
	private float timerAllontanamento;
}
