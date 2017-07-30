using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class MissileDaCrociera : MonoBehaviour
{
	// Token: 0x06000676 RID: 1654 RVA: 0x000E5A7C File Offset: 0x000E3C7C
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
		this.velocitàTraslazione = this.velocitàTraslazioneIniziale;
		this.velocitàSlittamento = this.velocitàSlittamentoIniziale;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x000E5AD0 File Offset: 0x000E3CD0
	private void Update()
	{
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
			this.Esplosione();
		}
		else
		{
			this.SensoriAnteriori();
			this.SensoriPosteriori();
			this.Navigazione();
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000E5B18 File Offset: 0x000E3D18
	private void SensoriAnteriori()
	{
		if (Physics.Linecast(base.transform.position, this.destMissile, out this.hitSensoreCentrale, 256))
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
		bool flag = false;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out this.hitSensoreCentrale, 256f) && this.destinazioneInVista)
		{
			if (Vector3.Distance(base.transform.position, this.destMissile) < 30f)
			{
				maxDistance = 0f;
			}
			else
			{
				maxDistance = 20f;
			}
		}
		int num = 5;
		while (num <= 90 && this.numeroRaggiTrue == 8)
		{
			rotation = Quaternion.AngleAxis((float)num, base.transform.right);
			this.direzioneRaggioLibero = Vector3.zero;
			float num2 = 99999f;
			this.numeroRaggiTrue = 0;
			for (int i = 0; i < 360; i += 45)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)i, base.transform.forward);
				Ray ray = new Ray(base.transform.position, rotation2 * (rotation * base.transform.forward));
				if (!Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, maxDistance, 256))
				{
					float num3 = Vector3.Distance(this.destMissile, ray.GetPoint(50f));
					if (num3 < num2)
					{
						num2 = num3;
						this.direzioneRaggioLibero = rotation2 * (rotation * base.transform.forward);
					}
				}
				else
				{
					this.numeroRaggiTrue++;
				}
				if (Vector3.Distance(base.transform.position, this.destMissile) < 40f && !flag && num == 45 && i == 0 && Physics.Raycast(ray, out this.hitSensoreCircolareAnteriore, 50f, 256))
				{
					this.VirataDiPericolo();
					flag = true;
				}
			}
			num += 40;
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000E5D50 File Offset: 0x000E3F50
	private void SensoriPosteriori()
	{
		this.slittamentoVerticale1 = 0f;
		this.slittamentoOrizzontale1 = 0f;
		this.slittamentoVerticale2 = 0f;
		this.slittamentoOrizzontale2 = 0f;
		for (int i = 35; i < 95; i += 50)
		{
			Quaternion rotation = Quaternion.AngleAxis((float)i, base.transform.right);
			for (int j = 0; j < 360; j += 90)
			{
				Quaternion rotation2 = Quaternion.AngleAxis((float)j, base.transform.forward);
				Ray ray = new Ray(base.transform.position, rotation2 * (rotation * base.transform.forward));
				if (Physics.Raycast(ray, out this.hitSensoreCircolarePosteriore, 20f, 256))
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

	// Token: 0x0600067A RID: 1658 RVA: 0x000E5EFC File Offset: 0x000E40FC
	private void VirataDiPericolo()
	{
		base.transform.Rotate(base.transform.up * 2000f * Time.deltaTime);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x000E5F34 File Offset: 0x000E4134
	private void Navigazione()
	{
		if (this.destinazioneInVista)
		{
			Quaternion to = Quaternion.LookRotation(this.destMissile - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàAutoRotazione * Time.deltaTime);
		}
		else
		{
			Quaternion to2 = Quaternion.LookRotation(this.direzioneRaggioLibero);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to2, this.velocitàAutoRotazione * Time.deltaTime);
		}
		base.transform.position += base.transform.up * (this.slittamentoVerticale1 + this.slittamentoVerticale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.right * (this.slittamentoOrizzontale1 + this.slittamentoOrizzontale2) * this.velocitàSlittamento * Time.deltaTime;
		base.transform.position += base.transform.forward * Time.deltaTime * this.velocitàTraslazione;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000E6090 File Offset: 0x000E4290
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta && this.timerImpatto > 0f)
		{
			foreach (GameObject current in base.transform.GetChild(0).GetComponent<ColliderAreaEffetto>().ListaAeraEffetto)
			{
				if (current != null && current != this.oggettoColpito)
				{
					if (current.tag == "Nemico")
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
						float num = 0f;
						if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
						{
							num = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
						}
						else if (current.GetComponent<PresenzaNemico>().vita > 0f)
						{
							num = current.GetComponent<PresenzaNemico>().vita;
						}
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
						if (current.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura))
						{
							num += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
						}
						else if (current.GetComponent<PresenzaNemico>().vita > 0f)
						{
							num += current.GetComponent<PresenzaNemico>().vita;
						}
						current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - current.GetComponent<PresenzaNemico>().armatura);
						List<float> listaDanniAlleati;
						List<float> expr_1AF = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int index;
						int expr_1B4 = index = 10;
						float num2 = listaDanniAlleati[index];
						expr_1AF[expr_1B4] = num2 + num;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
					}
					else if (current.tag == "ObbiettivoTattico" && (current.name == "Avamposto Nemico(Clone)" || current.name == "Pane per Convoglio(Clone)"))
					{
						this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
						float num3 = 0f;
						if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
						{
							num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
						}
						else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num3 = current.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
						if (current.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
						{
							num3 += base.GetComponent<DatiGeneraliMunizione>().danno;
						}
						else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num3 += current.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						current.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
						List<float> listaDanniAlleati2;
						List<float> expr_32A = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
						int index;
						int expr_32F = index = 10;
						float num2 = listaDanniAlleati2[index];
						expr_32A[expr_32F] = num2 + num3;
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
					}
				}
			}
			this.esplosioneAvvenuta = true;
		}
		if (this.timerImpatto > 5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x000E6458 File Offset: 0x000E4658
	private void OnCollisionEnter(Collision collisione)
	{
		if (collisione.gameObject.tag == "Ambiente" || collisione.gameObject.tag == "Nemico" || collisione.gameObject.tag == "Nemico Testa" || collisione.gameObject.tag == "Nemico Coll Suppl" || collisione.gameObject.tag == "ObbiettivoTattico")
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
			base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<CapsuleCollider>().enabled = false;
			base.GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
			base.transform.GetChild(2).GetComponent<ParticleSystem>().Clear();
			base.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
			base.GetComponent<AudioSource>().Stop();
			base.transform.GetChild(0).GetComponent<AudioSource>().Play();
			this.avviaTimer = true;
			if (collisione.gameObject.tag == "Nemico")
			{
				this.oggettoColpito = collisione.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num = 0f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura))
				{
					num += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				List<float> listaDanniAlleati;
				List<float> expr_2E8 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_2ED = index = 10;
				float num2 = listaDanniAlleati[index];
				expr_2E8[expr_2ED] = num2 + num;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num;
			}
			else if (collisione.gameObject.tag == "Nemico Testa")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num3 = 0f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f)
				{
					num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * 2f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f)
				{
					num3 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura) * 2f;
				List<float> listaDanniAlleati2;
				List<float> expr_4E5 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_4EA = index = 10;
				float num2 = listaDanniAlleati2[index];
				expr_4E5[expr_4EA] = num2 + num3;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
			}
			else if (collisione.gameObject.tag == "Nemico Coll Suppl")
			{
				this.oggettoColpito = collisione.transform.parent.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num4 = 0f;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num4 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 = this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura))
				{
					num4 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				}
				else if (this.oggettoColpito.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num4 += this.oggettoColpito.GetComponent<PresenzaNemico>().vita;
				}
				this.oggettoColpito.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.oggettoColpito.GetComponent<PresenzaNemico>().armatura);
				List<float> listaDanniAlleati3;
				List<float> expr_6BE = listaDanniAlleati3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_6C3 = index = 10;
				float num2 = listaDanniAlleati3[index];
				expr_6BE[expr_6C3] = num2 + num4;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num4;
			}
			else if ((collisione.gameObject.tag == "ObbiettivoTattico" && collisione.gameObject.name == "Avamposto Nemico(Clone)") || collisione.gameObject.name == "Pane per Convoglio(Clone)")
			{
				this.oggettoColpito = collisione.gameObject;
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num5 = 0f;
				if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num5 = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					num5 = this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
				}
				this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > base.GetComponent<DatiGeneraliMunizione>().danno)
				{
					num5 += base.GetComponent<DatiGeneraliMunizione>().danno;
				}
				else if (this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
				{
					num5 += this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita;
				}
				this.oggettoColpito.GetComponent<ObbiettivoTatticoScript>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno;
				List<float> listaDanniAlleati4;
				List<float> expr_87C = listaDanniAlleati4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int index;
				int expr_881 = index = 10;
				float num2 = listaDanniAlleati4[index];
				expr_87C[expr_881] = num2 + num5;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num5;
			}
		}
	}

	// Token: 0x04001807 RID: 6151
	private GameObject infoNeutreTattica;

	// Token: 0x04001808 RID: 6152
	private GameObject terzaCamera;

	// Token: 0x04001809 RID: 6153
	public float velocitàTraslazioneIniziale;

	// Token: 0x0400180A RID: 6154
	private float velocitàTraslazione;

	// Token: 0x0400180B RID: 6155
	public float velocitàSlittamentoIniziale;

	// Token: 0x0400180C RID: 6156
	private float velocitàSlittamento;

	// Token: 0x0400180D RID: 6157
	public float velocitàAutoRotazione;

	// Token: 0x0400180E RID: 6158
	public Vector3 destMissile;

	// Token: 0x0400180F RID: 6159
	private GameObject oggettoColpito;

	// Token: 0x04001810 RID: 6160
	private bool avviaTimer;

	// Token: 0x04001811 RID: 6161
	private float timerImpatto;

	// Token: 0x04001812 RID: 6162
	private bool esplosioneAvvenuta;

	// Token: 0x04001813 RID: 6163
	private bool destinazioneInVista;

	// Token: 0x04001814 RID: 6164
	private int numeroRaggiTrue;

	// Token: 0x04001815 RID: 6165
	private RaycastHit hitSensoreCentrale;

	// Token: 0x04001816 RID: 6166
	private RaycastHit hitSensoreCircolareAnteriore;

	// Token: 0x04001817 RID: 6167
	private RaycastHit hitSensoreCircolarePosteriore;

	// Token: 0x04001818 RID: 6168
	private Vector3 direzioneRaggioLibero;

	// Token: 0x04001819 RID: 6169
	private float slittamentoVerticale1;

	// Token: 0x0400181A RID: 6170
	private float slittamentoOrizzontale1;

	// Token: 0x0400181B RID: 6171
	private float slittamentoVerticale2;

	// Token: 0x0400181C RID: 6172
	private float slittamentoOrizzontale2;
}
