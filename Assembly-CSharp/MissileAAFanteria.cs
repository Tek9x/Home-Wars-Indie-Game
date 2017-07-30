using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class MissileAAFanteria : MonoBehaviour
{
	// Token: 0x060005D2 RID: 1490 RVA: 0x000C87FC File Offset: 0x000C69FC
	private void Start()
	{
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		base.GetComponent<AudioSource>().Play();
		base.GetComponent<ParticleSystem>().Play();
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
		if (GestoreNeutroTattica.èBattagliaVeloce)
		{
			this.moltiplicatoreAttaccoInFPS = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().moltiplicatoreFPSBattVeloce;
		}
		else
		{
			this.moltiplicatoreAttaccoInFPS = PlayerPrefs.GetFloat("moltiplicatore danni PP");
		}
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.terzaCamera = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaCamere[2];
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x000C88A8 File Offset: 0x000C6AA8
	private void Update()
	{
		if (this.ordignoAttivo)
		{
			this.timerDaLancio += Time.deltaTime;
			this.target = base.GetComponent<DatiOrdignoInterno>().bersaglio;
			if (this.target && !this.esplosioneAvvenuta)
			{
				this.posizioneEsplosione = this.target.transform.position;
			}
		}
		if (!base.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
			base.GetComponent<Rigidbody>().useGravity = false;
			base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
		if (this.timerDaLancio > 0f)
		{
			if (!this.esplosioneAvvenuta)
			{
				this.Navigazione();
			}
			if (this.target)
			{
				float num = Vector3.Distance(base.transform.position, this.target.transform.position);
				if (num < this.raggioInnesco && !this.esplosioneAvvenuta)
				{
					base.GetComponent<ParticleSystem>().Clear();
					base.GetComponent<ParticleSystem>().Stop();
					base.GetComponent<AudioSource>().Stop();
					base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
					base.transform.GetChild(1).GetComponent<AudioSource>().Play();
					base.GetComponent<Rigidbody>().isKinematic = true;
					base.GetComponent<CapsuleCollider>().enabled = false;
					base.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
					base.transform.position = this.target.transform.position;
					this.avviaTimer = true;
				}
			}
		}
		if (this.avviaTimer)
		{
			this.timerImpatto += Time.deltaTime;
		}
		if (this.timerImpatto > 0f)
		{
			this.Esplosione();
			this.esplosioneAvvenuta = true;
		}
		if (this.esplosioneAvvenuta)
		{
			this.timerEsplosione += Time.deltaTime;
			if (this.timerEsplosione > 1f && base.GetComponent<MeshRenderer>())
			{
				base.GetComponent<MeshRenderer>().enabled = false;
			}
			if (this.timerEsplosione > 3f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x000C8AF0 File Offset: 0x000C6CF0
	private void Navigazione()
	{
		if (this.timerDaLancio > 0.2f)
		{
			if (this.target)
			{
				Vector3 forward = this.target.transform.position - base.transform.position;
				Quaternion to = Quaternion.LookRotation(forward);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.velocitàRotazione * Time.deltaTime);
			}
			if (!this.audioViaggioAttivo)
			{
				base.GetComponent<AudioSource>().Play();
				this.audioViaggioAttivo = true;
			}
		}
		base.transform.Translate(Vector3.forward * this.velocitàMissile * Time.deltaTime);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x000C8BB0 File Offset: 0x000C6DB0
	private void Esplosione()
	{
		if (!this.esplosioneAvvenuta)
		{
			if (!base.GetComponent<DatiOrdignoInterno>().lanciatoInFPS)
			{
				float num = 0f;
				if (this.target.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione)
				{
					num = base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				}
				else if (this.target.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num = this.target.GetComponent<PresenzaNemico>().vita;
				}
				this.target.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione;
				if (this.target.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura))
				{
					num += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura);
				}
				else if (this.target.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num += this.target.GetComponent<PresenzaNemico>().vita;
				}
				this.target.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura);
				List<float> listaDanniAlleati;
				List<float> expr_176 = listaDanniAlleati = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int truppaDiOrigine;
				int expr_183 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
				float num2 = listaDanniAlleati[truppaDiOrigine];
				expr_176[expr_183] = num2 + num;
			}
			else
			{
				this.terzaCamera.GetComponent<TerzaCamera>().timerVerifTro = 0f;
				float num3 = 0f;
				if (this.target.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS)
				{
					num3 = base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				}
				else if (this.target.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 = this.target.GetComponent<PresenzaNemico>().vita;
				}
				this.target.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().penetrazione * this.moltiplicatoreAttaccoInFPS;
				if (this.target.GetComponent<PresenzaNemico>().vita > base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS)
				{
					num3 += base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				}
				else if (this.target.GetComponent<PresenzaNemico>().vita > 0f)
				{
					num3 += this.target.GetComponent<PresenzaNemico>().vita;
				}
				this.target.GetComponent<PresenzaNemico>().vita -= base.GetComponent<DatiGeneraliMunizione>().danno * (1f - this.target.GetComponent<PresenzaNemico>().armatura) * this.moltiplicatoreAttaccoInFPS;
				List<float> listaDanniAlleati2;
				List<float> expr_336 = listaDanniAlleati2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniAlleati;
				int truppaDiOrigine;
				int expr_344 = truppaDiOrigine = base.GetComponent<DatiGeneraliMunizione>().truppaDiOrigine;
				float num2 = listaDanniAlleati2[truppaDiOrigine];
				expr_336[expr_344] = num2 + num3;
				this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().danniDelGiocatore += num3;
			}
		}
	}

	// Token: 0x040015D9 RID: 5593
	private GameObject infoNeutreTattica;

	// Token: 0x040015DA RID: 5594
	private GameObject terzaCamera;

	// Token: 0x040015DB RID: 5595
	public float velocitàMissile;

	// Token: 0x040015DC RID: 5596
	public float velocitàRotazione;

	// Token: 0x040015DD RID: 5597
	public float raggioInnesco;

	// Token: 0x040015DE RID: 5598
	public bool ordignoAttivo;

	// Token: 0x040015DF RID: 5599
	private float timerDaLancio;

	// Token: 0x040015E0 RID: 5600
	private GameObject target;

	// Token: 0x040015E1 RID: 5601
	private bool esplosioneAvvenuta;

	// Token: 0x040015E2 RID: 5602
	private Vector3 posizioneEsplosione;

	// Token: 0x040015E3 RID: 5603
	private float timerEsplosione;

	// Token: 0x040015E4 RID: 5604
	private bool audioViaggioAttivo;

	// Token: 0x040015E5 RID: 5605
	private bool audioPartenzaAttivo;

	// Token: 0x040015E6 RID: 5606
	private float moltiplicatoreAttaccoInFPS;

	// Token: 0x040015E7 RID: 5607
	private bool avviaTimer;

	// Token: 0x040015E8 RID: 5608
	private float timerImpatto;
}
