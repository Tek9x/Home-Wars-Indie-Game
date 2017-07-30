using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class BrachinusBellicosus : MonoBehaviour
{
	// Token: 0x060006FA RID: 1786 RVA: 0x000FBE80 File Offset: 0x000FA080
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerAttacco = 540672;
		this.raggioInsettoNav = base.GetComponent<NavMeshAgent>().radius;
		this.danno = base.GetComponent<PresenzaNemico>().danno1;
		this.frequenzaAttacco = base.GetComponent<PresenzaNemico>().frequenzaAttacco;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x000FBEF8 File Offset: 0x000FA0F8
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000FBF34 File Offset: 0x000FA134
	private void Morte()
	{
		if (base.GetComponent<PresenzaNemico>().morto)
		{
			this.insettoAnim.SetBool(this.attaccoHash, false);
			this.insettoAnim.SetBool(this.morteHash, true);
			if (base.GetComponent<PresenzaNemico>().timerMorte > 3f)
			{
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Remove(base.gameObject);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x000FBFB4 File Offset: 0x000FA1B4
	private void Attacco()
	{
		this.timerDiAttacco += Time.deltaTime;
		if (Physics.Raycast(this.centroBaseInsetto, base.transform.forward, out this.hitAttacco, this.distanzaDiAttacco, this.layerAttacco))
		{
			base.GetComponent<PresenzaNemico>().muoviti = false;
			if (this.timerDiAttacco > this.frequenzaAttacco)
			{
				this.timerDiAttacco = 0f;
				this.attaccoEffettuato = false;
				Vector3 position = this.hitAttacco.collider.gameObject.transform.position;
				base.transform.LookAt(new Vector3(position.x, base.transform.position.y, position.z));
			}
			else if (this.timerDiAttacco > 0.8f && !this.attaccoEffettuato)
			{
				foreach (GameObject current in this.oggettoSpruzzo.GetComponent<ColliderAereaAttNemici>().ListaAeraEffetto)
				{
					if (current != null)
					{
						if (current.tag == "Alleato")
						{
							float num = 0f;
							if (current.GetComponent<PresenzaAlleato>().vita > this.danno)
							{
								num = this.danno;
							}
							else if (current.GetComponent<PresenzaAlleato>().vita > 0f)
							{
								num = current.GetComponent<PresenzaAlleato>().vita;
							}
							current.GetComponent<PresenzaAlleato>().vita -= this.danno;
							List<float> listaDanniNemici;
							List<float> expr_190 = listaDanniNemici = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int tipoInsetto;
							int expr_19E = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
							float num2 = listaDanniNemici[tipoInsetto];
							expr_190[expr_19E] = num2 + num;
						}
						else if (current.tag == "Trappola" && current.GetComponent<PresenzaTrappola>())
						{
							float num3 = 0f;
							if (current.GetComponent<PresenzaTrappola>().vita > this.danno)
							{
								num3 = this.danno;
							}
							else if (current.GetComponent<PresenzaTrappola>().vita > 0f)
							{
								num3 = current.GetComponent<PresenzaTrappola>().vita;
							}
							current.GetComponent<PresenzaTrappola>().vita -= this.danno;
							List<float> listaDanniNemici2;
							List<float> expr_253 = listaDanniNemici2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int tipoInsetto;
							int expr_261 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
							float num2 = listaDanniNemici2[tipoInsetto];
							expr_253[expr_261] = num2 + num3;
						}
						else if (current.name == "Avamposto Alleato(Clone)")
						{
							float num4 = 0f;
							if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
							{
								num4 = this.danno;
							}
							else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
							List<float> listaDanniNemici3;
							List<float> expr_343 = listaDanniNemici3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int tipoInsetto;
							int expr_351 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
							float num2 = listaDanniNemici3[tipoInsetto];
							expr_343[expr_351] = num2 + num4;
						}
						else if (current.name == "Cassa Supply(Clone)")
						{
							float num5 = 0f;
							if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
							{
								num5 = this.danno;
							}
							else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
							List<float> listaDanniNemici4;
							List<float> expr_433 = listaDanniNemici4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int tipoInsetto;
							int expr_441 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
							float num2 = listaDanniNemici4[tipoInsetto];
							expr_433[expr_441] = num2 + num5;
						}
						else if (current.name == "Camion per Convoglio(Clone)")
						{
							float num6 = 0f;
							if (current.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
							{
								num6 = this.danno;
							}
							else if (current.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
							{
								num6 = current.GetComponent<ObbiettivoTatticoScript>().vita;
							}
							current.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
							List<float> listaDanniNemici5;
							List<float> expr_4E7 = listaDanniNemici5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
							int tipoInsetto;
							int expr_4F5 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
							float num2 = listaDanniNemici5[tipoInsetto];
							expr_4E7[expr_4F5] = num2 + num6;
						}
					}
				}
				this.oggettoSpruzzo.GetComponent<ParticleSystem>().Play();
				this.attaccoEffettuato = true;
			}
			if (this.timerDiAttacco < 1.8f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, true);
			}
			else if (this.timerDiAttacco > 1.8f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, false);
			}
		}
		else
		{
			base.GetComponent<PresenzaNemico>().muoviti = true;
			this.insettoAnim.SetBool(this.attaccoHash, false);
		}
	}

	// Token: 0x04001A18 RID: 6680
	private float danno;

	// Token: 0x04001A19 RID: 6681
	private float frequenzaAttacco;

	// Token: 0x04001A1A RID: 6682
	public float distanzaDiAttacco;

	// Token: 0x04001A1B RID: 6683
	private Animator insettoAnim;

	// Token: 0x04001A1C RID: 6684
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001A1D RID: 6685
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001A1E RID: 6686
	private float timerMorte;

	// Token: 0x04001A1F RID: 6687
	private GameObject bersaglio;

	// Token: 0x04001A20 RID: 6688
	private GameObject IANemico;

	// Token: 0x04001A21 RID: 6689
	private GameObject infoNeutreTattica;

	// Token: 0x04001A22 RID: 6690
	private float timerDiAttacco;

	// Token: 0x04001A23 RID: 6691
	private bool attaccoEffettuato;

	// Token: 0x04001A24 RID: 6692
	private RaycastHit hitAttacco;

	// Token: 0x04001A25 RID: 6693
	private float raggioInsettoNav;

	// Token: 0x04001A26 RID: 6694
	private int layerAttacco;

	// Token: 0x04001A27 RID: 6695
	private Vector3 centroInsetto;

	// Token: 0x04001A28 RID: 6696
	private Vector3 centroBaseInsetto;

	// Token: 0x04001A29 RID: 6697
	public GameObject oggettoSpruzzo;
}
