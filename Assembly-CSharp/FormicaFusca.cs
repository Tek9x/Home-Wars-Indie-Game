﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class FormicaFusca : MonoBehaviour
{
	// Token: 0x06000713 RID: 1811 RVA: 0x000FE760 File Offset: 0x000FC960
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

	// Token: 0x06000714 RID: 1812 RVA: 0x000FE7D8 File Offset: 0x000FC9D8
	private void Update()
	{
		this.centroInsetto = base.GetComponent<PresenzaNemico>().centroInsetto;
		this.centroBaseInsetto = base.GetComponent<PresenzaNemico>().centroBaseInsetto;
		this.Morte();
		this.Attacco();
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000FE814 File Offset: 0x000FCA14
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

	// Token: 0x06000716 RID: 1814 RVA: 0x000FE894 File Offset: 0x000FCA94
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

	// Token: 0x04001A6F RID: 6767
	private float danno;

	// Token: 0x04001A70 RID: 6768
	private float frequenzaAttacco;

	// Token: 0x04001A71 RID: 6769
	public float distanzaDiAttacco;

	// Token: 0x04001A72 RID: 6770
	private Animator insettoAnim;

	// Token: 0x04001A73 RID: 6771
	private int morteHash = Animator.StringToHash("insetto-morte");

	// Token: 0x04001A74 RID: 6772
	private int attaccoHash = Animator.StringToHash("insetto-attacco");

	// Token: 0x04001A75 RID: 6773
	private float timerMorte;

	// Token: 0x04001A76 RID: 6774
	private GameObject bersaglio;

	// Token: 0x04001A77 RID: 6775
	private GameObject IANemico;

	// Token: 0x04001A78 RID: 6776
	private GameObject infoNeutreTattica;

	// Token: 0x04001A79 RID: 6777
	private float timerDiAttacco;

	// Token: 0x04001A7A RID: 6778
	private bool attaccoEffettuato;

	// Token: 0x04001A7B RID: 6779
	private RaycastHit hitAttacco;

	// Token: 0x04001A7C RID: 6780
	private float raggioInsettoNav;

	// Token: 0x04001A7D RID: 6781
	private int layerAttacco;

	// Token: 0x04001A7E RID: 6782
	private Vector3 centroInsetto;

	// Token: 0x04001A7F RID: 6783
	private Vector3 centroBaseInsetto;
}
