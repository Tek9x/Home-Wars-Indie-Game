using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class TitanusGiganteus : MonoBehaviour
{
	// Token: 0x06000781 RID: 1921 RVA: 0x0010C8B4 File Offset: 0x0010AAB4
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

	// Token: 0x06000782 RID: 1922 RVA: 0x0010C92C File Offset: 0x0010AB2C
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0010C968 File Offset: 0x0010AB68
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

	// Token: 0x06000784 RID: 1924 RVA: 0x0010C9E8 File Offset: 0x0010ABE8
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
			else if (this.timerDiAttacco > 2f && !this.attaccoEffettuato)
			{
				if (this.hitAttacco.collider.gameObject.tag == "Alleato")
				{
					GameObject gameObject = this.hitAttacco.collider.gameObject;
					float num = 0f;
					if (gameObject.GetComponent<PresenzaAlleato>().vita > this.danno)
					{
						num = this.danno;
					}
					else if (gameObject.GetComponent<PresenzaAlleato>().vita > 0f)
					{
						num = gameObject.GetComponent<PresenzaAlleato>().vita;
					}
					gameObject.GetComponent<PresenzaAlleato>().vita -= this.danno;
					List<float> listaDanniNemici;
					List<float> expr_181 = listaDanniNemici = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_18F = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici[tipoInsetto];
					expr_181[expr_18F] = num2 + num;
				}
				else if (this.hitAttacco.collider.gameObject.tag == "Trappola" && this.hitAttacco.collider.gameObject.GetComponent<PresenzaTrappola>())
				{
					GameObject gameObject2 = this.hitAttacco.collider.gameObject;
					float num3 = 0f;
					if (gameObject2.GetComponent<PresenzaTrappola>().vita > this.danno)
					{
						num3 = this.danno;
					}
					else if (gameObject2.GetComponent<PresenzaTrappola>().vita > 0f)
					{
						num3 = gameObject2.GetComponent<PresenzaTrappola>().vita;
					}
					gameObject2.GetComponent<PresenzaTrappola>().vita -= this.danno;
					List<float> listaDanniNemici2;
					List<float> expr_273 = listaDanniNemici2 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_281 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici2[tipoInsetto];
					expr_273[expr_281] = num2 + num3;
				}
				else if (this.hitAttacco.collider.gameObject.name == "Avamposto Alleato(Clone)")
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
					List<float> expr_372 = listaDanniNemici3 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_380 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici3[tipoInsetto];
					expr_372[expr_380] = num2 + num4;
				}
				else if (this.hitAttacco.collider.gameObject.name == "Cassa Supply(Clone)")
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
					List<float> expr_471 = listaDanniNemici4 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_47F = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici4[tipoInsetto];
					expr_471[expr_47F] = num2 + num5;
				}
				else if (this.hitAttacco.collider.gameObject.name == "Camion per Convoglio(Clone)")
				{
					GameObject gameObject3 = this.hitAttacco.collider.gameObject;
					float num6 = 0f;
					if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > this.danno)
					{
						num6 = this.danno;
					}
					else if (gameObject3.GetComponent<ObbiettivoTatticoScript>().vita > 0f)
					{
						num6 = gameObject3.GetComponent<ObbiettivoTatticoScript>().vita;
					}
					gameObject3.GetComponent<ObbiettivoTatticoScript>().vita -= this.danno;
					List<float> listaDanniNemici5;
					List<float> expr_54A = listaDanniNemici5 = this.infoNeutreTattica.GetComponent<GestoreNeutroTattica>().ListaDanniNemici;
					int tipoInsetto;
					int expr_558 = tipoInsetto = base.GetComponent<PresenzaNemico>().tipoInsetto;
					float num2 = listaDanniNemici5[tipoInsetto];
					expr_54A[expr_558] = num2 + num6;
				}
				this.attaccoEffettuato = true;
			}
			if (this.timerDiAttacco < 3f)
			{
				this.insettoAnim.SetBool(this.attaccoHash, true);
			}
			else if (this.timerDiAttacco > 3f)
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

	// Token: 0x04001C07 RID: 7175
	private float danno;

	// Token: 0x04001C08 RID: 7176
	private float frequenzaAttacco;

	// Token: 0x04001C09 RID: 7177
	public float distanzaDiAttacco;

	// Token: 0x04001C0A RID: 7178
	private Animator insettoAnim;

	// Token: 0x04001C0B RID: 7179
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001C0C RID: 7180
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001C0D RID: 7181
	private float timerMorte;

	// Token: 0x04001C0E RID: 7182
	private GameObject bersaglio;

	// Token: 0x04001C0F RID: 7183
	private GameObject IANemico;

	// Token: 0x04001C10 RID: 7184
	private GameObject infoNeutreTattica;

	// Token: 0x04001C11 RID: 7185
	private float timerDiAttacco;

	// Token: 0x04001C12 RID: 7186
	private bool attaccoEffettuato;

	// Token: 0x04001C13 RID: 7187
	private RaycastHit hitAttacco;

	// Token: 0x04001C14 RID: 7188
	private float raggioInsettoNav;

	// Token: 0x04001C15 RID: 7189
	private int layerAttacco;

	// Token: 0x04001C16 RID: 7190
	private Vector3 centroInsetto;

	// Token: 0x04001C17 RID: 7191
	private Vector3 centroBaseInsetto;
}
