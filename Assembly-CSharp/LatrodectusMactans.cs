using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class LatrodectusMactans : MonoBehaviour
{
	// Token: 0x0600072C RID: 1836 RVA: 0x00101608 File Offset: 0x000FF808
	private void Start()
	{
		this.IANemico = GameObject.FindGameObjectWithTag("IANemico");
		this.infoNeutreTattica = GameObject.FindGameObjectWithTag("InfoNeutreTattica");
		this.insettoAnim = base.GetComponent<Animator>();
		this.layerAttacco = 540672;
		this.raggioInsettoNav = base.GetComponent<NavMeshAgent>().radius;
		this.danno1 = base.GetComponent<PresenzaNemico>().danno1;
		this.danno2 = base.GetComponent<PresenzaNemico>().danno2;
		this.frequenzaAttacco = base.GetComponent<PresenzaNemico>().frequenzaAttacco;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00101690 File Offset: 0x000FF890
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x001016CC File Offset: 0x000FF8CC
	private void Morte()
	{
		if (base.GetComponent<PresenzaNemico>().morto)
		{
			this.insettoAnim.SetBool(this.attacco1Hash, false);
			this.insettoAnim.SetBool(this.attacco2Hash, false);
			this.insettoAnim.SetBool(this.morteHash, true);
			if (base.GetComponent<PresenzaNemico>().timerMorte > 3f)
			{
				this.IANemico.GetComponent<InfoGenericheNemici>().ListaNemici.Remove(base.gameObject);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0010175C File Offset: 0x000FF95C
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
			else if (this.timerDiAttacco > 2.2f && !this.attaccoEffettuato)
			{
				if (!this.turnoAttacco2)
				{
					if (this.hitAttacco.collider.gameObject.tag == "Alleato")
					{
						GameObject gameObject = this.hitAttacco.collider.gameObject;
						float num = 0f;
						if (gameObject.GetComponent<PresenzaAlleato>().vita > this.danno1)
						{
							num = this.danno1;
						}
						else if (gameObject.GetComponent<PresenzaAlleato>().vita > 0f)
						{
							num = gameObject.GetComponent<PresenzaAlleato>().vita;
						}
						gameObject.GetComponent<PresenzaAlleato>().vita -= this.danno1;
						List<float> listaDanniNemici;
						List<float> expr_18C = listaDanniNemici = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_19A = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici[tipoInsetto];
						expr_18C[expr_19A] = num2 + num;
					}
					else if (this.hitAttacco.collider.gameObject.tag == "Trappola" && this.hitAttacco.collider.gameObject.GetComponent<PresenzaTrappola>())
					{
						GameObject gameObject2 = this.hitAttacco.collider.gameObject;
						float num3 = 0f;
						if (gameObject2.GetComponent<PresenzaTrappola>().vita > this.danno1)
						{
							num3 = this.danno1;
						}
						else if (gameObject2.GetComponent<PresenzaTrappola>().vita > 0f)
						{
							num3 = gameObject2.GetComponent<PresenzaTrappola>().vita;
						}
						gameObject2.GetComponent<PresenzaTrappola>().vita -= this.danno1;
						List<float> listaDanniNemici2;
						List<float> expr_27E = listaDanniNemici2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_28C = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici2[tipoInsetto];
						expr_27E[expr_28C] = num2 + num3;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Avamposto Alleato(Clone)")
					{
						float num4 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num4 = this.danno1;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici3;
						List<float> expr_37D = listaDanniNemici3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_38B = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici3[tipoInsetto];
						expr_37D[expr_38B] = num2 + num4;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Cassa Supply(Clone)")
					{
						float num5 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num5 = this.danno1;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici4;
						List<float> expr_47C = listaDanniNemici4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_48A = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici4[tipoInsetto];
						expr_47C[expr_48A] = num2 + num5;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Camion per Convoglio(Clone)")
					{
						GameObject gameObject3 = this.hitAttacco.collider.gameObject;
						float num6 = 0f;
						if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > this.danno1)
						{
							num6 = this.danno1;
						}
						else if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num6 = gameObject3.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						gameObject3.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno1;
						List<float> listaDanniNemici5;
						List<float> expr_555 = listaDanniNemici5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_563 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici5[tipoInsetto];
						expr_555[expr_563] = num2 + num6;
					}
				}
				else
				{
					if (this.hitAttacco.collider.gameObject.tag == "Alleato")
					{
						GameObject gameObject4 = this.hitAttacco.collider.gameObject;
						float num7 = 0f;
						if (gameObject4.GetComponent<PresenzaAlleato>().vita > this.danno2)
						{
							num7 = this.danno2;
						}
						else if (gameObject4.GetComponent<PresenzaAlleato>().vita > 0f)
						{
							num7 = gameObject4.GetComponent<PresenzaAlleato>().vita;
						}
						gameObject4.GetComponent<PresenzaAlleato>().vita -= this.danno2;
						List<float> listaDanniNemici6;
						List<float> expr_62E = listaDanniNemici6 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_63C = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici6[tipoInsetto];
						expr_62E[expr_63C] = num2 + num7;
					}
					else if (this.hitAttacco.collider.gameObject.tag == "Trappola" && this.hitAttacco.collider.gameObject.GetComponent<PresenzaTrappola>())
					{
						GameObject gameObject5 = this.hitAttacco.collider.gameObject;
						float num8 = 0f;
						if (gameObject5.GetComponent<PresenzaTrappola>().vita > this.danno2)
						{
							num8 = this.danno2;
						}
						else if (gameObject5.GetComponent<PresenzaTrappola>().vita > 0f)
						{
							num8 = gameObject5.GetComponent<PresenzaTrappola>().vita;
						}
						gameObject5.GetComponent<PresenzaTrappola>().vita -= this.danno2;
						List<float> listaDanniNemici7;
						List<float> expr_726 = listaDanniNemici7 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_734 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici7[tipoInsetto];
						expr_726[expr_734] = num2 + num8;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Avamposto Alleato(Clone)")
					{
						float num9 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
						{
							num9 = this.danno2;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num9 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().avampostoAlleato.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
						List<float> listaDanniNemici8;
						List<float> expr_825 = listaDanniNemici8 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_833 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici8[tipoInsetto];
						expr_825[expr_833] = num2 + num9;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Cassa Supply(Clone)")
					{
						float num10 = 0f;
						if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
						{
							num10 = this.danno2;
						}
						else if (this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num10 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().cassaSupply.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
						List<float> listaDanniNemici9;
						List<float> expr_924 = listaDanniNemici9 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_932 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici9[tipoInsetto];
						expr_924[expr_932] = num2 + num10;
					}
					else if (this.hitAttacco.collider.gameObject.name == "Camion per Convoglio(Clone)")
					{
						GameObject gameObject6 = this.hitAttacco.collider.gameObject;
						float num11 = 0f;
						if (gameObject6.GetComponent<ObbiettivoTatticoScript>().vita > this.danno2)
						{
							num11 = this.danno2;
						}
						else if (gameObject6.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
						{
							num11 = gameObject6.GetComponent<ObbiettivoTatticoScript>().vita;
						}
						gameObject6.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno2;
						List<float> listaDanniNemici10;
						List<float> expr_9FD = listaDanniNemici10 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
						int tipoInsetto;
						int expr_A0B = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
						float num2 = listaDanniNemici10[tipoInsetto];
						expr_9FD[expr_A0B] = num2 + num11;
					}
					GameObject gameObject7 = this.hitAttacco.collider.gameObject;
					int index = 0;
					bool flag = false;
					if (gameObject7 != null && gameObject7.tag == "Alleato")
					{
						for (int i = 0; i < gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count; i++)
						{
							if (gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == base.GetComponent<PresenzaNemico>().dannoVeleno)
							{
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][1] = gameObject7.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								break;
							}
							if (!flag && gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[i][0] == 0f)
							{
								index = i;
								flag = true;
							}
							if (i == gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento.Count - 1)
							{
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][0] = base.GetComponent<PresenzaNemico>().dannoVeleno;
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][1] = gameObject7.GetComponent<PresenzaAlleato>().durataAvvelenamento;
								gameObject7.GetComponent<PresenzaAlleato>().ListaAvvelenamento[index][2] = (float)base.GetComponent<PresenzaNemico>().tipoInsetto;
							}
						}
					}
				}
				this.attaccoEffettuato = true;
				this.cambiaAttacco = true;
			}
			if (this.timerDiAttacco < 3f)
			{
				if (!this.turnoAttacco2)
				{
					this.insettoAnim.SetBool(this.attacco1Hash, true);
				}
				else
				{
					this.insettoAnim.SetBool(this.attacco2Hash, true);
				}
			}
			else if (this.timerDiAttacco > 3f)
			{
				this.insettoAnim.SetBool(this.attacco1Hash, false);
				this.insettoAnim.SetBool(this.attacco2Hash, false);
				if (this.cambiaAttacco)
				{
					this.cambiaAttacco = false;
					this.turnoAttacco2 = !this.turnoAttacco2;
				}
			}
		}
		else
		{
			base.GetComponent<PresenzaNemico>().muoviti = true;
			this.insettoAnim.SetBool(this.attacco1Hash, false);
		}
	}

	// Token: 0x04001AC8 RID: 6856
	private float danno1;

	// Token: 0x04001AC9 RID: 6857
	private float danno2;

	// Token: 0x04001ACA RID: 6858
	private float frequenzaAttacco;

	// Token: 0x04001ACB RID: 6859
	public float distanzaDiAttacco;

	// Token: 0x04001ACC RID: 6860
	private Animator insettoAnim;

	// Token: 0x04001ACD RID: 6861
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001ACE RID: 6862
	private int attacco1Hash = Animator.StringToHash("insetto-attacco1");

	// Token: 0x04001ACF RID: 6863
	private int attacco2Hash = Animator.StringToHash("insetto-attacco2");

	// Token: 0x04001AD0 RID: 6864
	private float timerMorte;

	// Token: 0x04001AD1 RID: 6865
	private GameObject bersaglio;

	// Token: 0x04001AD2 RID: 6866
	private GameObject IANemico;

	// Token: 0x04001AD3 RID: 6867
	private GameObject infoNeutreTattica;

	// Token: 0x04001AD4 RID: 6868
	private float timerDiAttacco;

	// Token: 0x04001AD5 RID: 6869
	private bool attaccoEffettuato;

	// Token: 0x04001AD6 RID: 6870
	private RaycastHit hitAttacco;

	// Token: 0x04001AD7 RID: 6871
	private float raggioInsettoNav;

	// Token: 0x04001AD8 RID: 6872
	private int layerAttacco;

	// Token: 0x04001AD9 RID: 6873
	private Vector3 centroInsetto;

	// Token: 0x04001ADA RID: 6874
	private Vector3 centroBaseInsetto;

	// Token: 0x04001ADB RID: 6875
	private bool turnoAttacco2;

	// Token: 0x04001ADC RID: 6876
	private bool cambiaAttacco;
}
