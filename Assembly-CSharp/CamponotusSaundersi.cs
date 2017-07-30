using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class CamponotusSaundersi : MonoBehaviour
{
	// Token: 0x060006FF RID: 1791 RVA: 0x000FC5C0 File Offset: 0x000FA7C0
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

	// Token: 0x06000700 RID: 1792 RVA: 0x000FC638 File Offset: 0x000FA838
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000FC674 File Offset: 0x000FA874
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

	// Token: 0x06000702 RID: 1794 RVA: 0x000FC6F4 File Offset: 0x000FA8F4
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
				foreach (GameObject current in this.oggettoEsplosione.GetComponent<ColliderAereaAttNemici>().ListaAeraEffetto)
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
				this.oggettoEsplosione.GetComponent<ParticleSystem>().Play();
				this.attaccoEffettuato = true;
				foreach (GameObject current2 in this.oggettoEsplosione.GetComponent<ColliderAereaAttNemici>().ListaAeraEffetto)
				{
					if (current2 != null && current2.tag == "Alleato")
					{
						int index = 0;
						bool flag = false;
						for (int i = 0; i < current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count; i++)
						{
							if (current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == base.GetComponent<PresenzaNemico>().dannoVeleno)
							{
								current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][1] = current2.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								break;
							}
							if (!flag && current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == 0f)
							{
								index = i;
								flag = true;
							}
							if (i == current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count - 1)
							{
								current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][0] = base.GetComponent<PresenzaNemico>().dannoVeleno;
								current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][1] = current2.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								current2.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][2] = (float)base.GetComponent<PresenzaNemico>().tipoInsetto;
							}
						}
					}
				}
			}
			if (this.timerDiAttacco < 1.8f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, true);
			}
			if (this.timerDiAttacco > 1.2f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, false);
				base.GetComponent<PresenzaNemico>().vita = 0f;
				base.GetComponent<PresenzaNemico>().morto = true;
			}
		}
		else
		{
			base.GetComponent<PresenzaNemico>().muoviti = true;
			this.insettoAnim.SetBool(this.attaccoHash, false);
		}
	}

	// Token: 0x04001A2A RID: 6698
	private float danno;

	// Token: 0x04001A2B RID: 6699
	private float frequenzaAttacco;

	// Token: 0x04001A2C RID: 6700
	public float distanzaDiAttacco;

	// Token: 0x04001A2D RID: 6701
	private Animator insettoAnim;

	// Token: 0x04001A2E RID: 6702
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001A2F RID: 6703
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001A30 RID: 6704
	private float timerMorte;

	// Token: 0x04001A31 RID: 6705
	private GameObject bersaglio;

	// Token: 0x04001A32 RID: 6706
	private GameObject IANemico;

	// Token: 0x04001A33 RID: 6707
	private GameObject infoNeutreTattica;

	// Token: 0x04001A34 RID: 6708
	private float timerDiAttacco;

	// Token: 0x04001A35 RID: 6709
	private bool attaccoEffettuato;

	// Token: 0x04001A36 RID: 6710
	private RaycastHit hitAttacco;

	// Token: 0x04001A37 RID: 6711
	private float raggioInsettoNav;

	// Token: 0x04001A38 RID: 6712
	private int layerAttacco;

	// Token: 0x04001A39 RID: 6713
	private Vector3 centroInsetto;

	// Token: 0x04001A3A RID: 6714
	private Vector3 centroBaseInsetto;

	// Token: 0x04001A3B RID: 6715
	public GameObject oggettoEsplosione;
}
