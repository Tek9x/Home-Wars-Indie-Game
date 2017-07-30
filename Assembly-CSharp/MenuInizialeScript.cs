using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C7 RID: 199
public class MenuInizialeScript : MonoBehaviour
{
	// Token: 0x060006E5 RID: 1765 RVA: 0x000F7E54 File Offset: 0x000F6054
	private void Start()
	{
		this.sfondo = GameObject.FindGameObjectWithTag("CanvasMenuIniz").transform.FindChild("sfondo foto").gameObject;
		if (GameObject.FindGameObjectWithTag("Musica"))
		{
			this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
		}
		else
		{
			this.oggettoMusica = (UnityEngine.Object.Instantiate(this.oggettoMusicaPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		}
		if (!PlayerPrefs.HasKey("non è primo avvio assoluto"))
		{
			PlayerPrefs.SetInt("non è primo avvio assoluto", 1);
			AudioListener.volume = 0.5f;
			PlayerPrefs.SetFloat("volume globale", 0.5f);
			PlayerPrefs.SetFloat("volume musica strategia", 0.5f);
			PlayerPrefs.SetFloat("volume musica tattica", 0.2f);
			PlayerPrefs.SetInt("max nemici", 400);
			PlayerPrefs.SetInt("max alleati", 80);
			PlayerPrefs.SetFloat("vita nemici", 100f);
			PlayerPrefs.SetFloat("attacco nemici", 100f);
			PlayerPrefs.SetFloat("sensibilità aerei", 100f);
			PlayerPrefs.SetInt("tipo truppa volante 1 0", 3);
			PlayerPrefs.SetInt("tipo truppa volante 1 1", 4);
			PlayerPrefs.SetInt("tipo truppa volante 2 0", 5);
			PlayerPrefs.SetInt("tipo truppa volante 2 1", 6);
			PlayerPrefs.SetInt("tipo truppa volante 2 2", 5);
			PlayerPrefs.SetInt("tipo truppa volante 3 0", 8);
			PlayerPrefs.SetInt("tipo truppa volante 3 1", 7);
			PlayerPrefs.SetInt("tipo truppa volante 3 2", 9);
			PlayerPrefs.SetInt("tipo truppa volante 4 0", 11);
			PlayerPrefs.SetInt("tipo truppa volante 4 1", 12);
			PlayerPrefs.SetInt("tipo truppa volante 4 2", 38);
			PlayerPrefs.SetInt("tipo truppa volante 5 0", 15);
			PlayerPrefs.SetInt("tipo truppa volante 5 1", 16);
			PlayerPrefs.SetInt("tipo truppa volante 5 2", 45);
			PlayerPrefs.SetInt("tipo truppa volante 5 3", 51);
			PlayerPrefs.SetInt("tipo truppa volante 6 0", 1);
			PlayerPrefs.SetInt("tipo truppa volante 7 0", 23);
			PlayerPrefs.SetInt("tipo truppa volante 7 1", 24);
			PlayerPrefs.SetInt("tipo truppa volante 11 0", 8);
			PlayerPrefs.SetInt("tipo truppa volante 11 1", 9);
			PlayerPrefs.SetInt("tipo truppa volante 12 0", 5);
			PlayerPrefs.SetInt("tipo truppa volante 12 1", 6);
			PlayerPrefs.SetInt("tipo truppa volante 13 0", 30);
			PlayerPrefs.SetInt("tipo truppa volante 13 1", 31);
			PlayerPrefs.SetInt("tipo truppa volante 13 2", 39);
			PlayerPrefs.SetInt("tipo truppa volante 13 3", 47);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 1 0", 25);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 1 1", 27);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 2 0", 29);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 2 1", 29);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 3 0", 34);
			PlayerPrefs.SetInt("tipo truppa terr con ordigno 3 1", 35);
		}
		if (!this.nonPrimoAvvioRelativo)
		{
			this.nonPrimoAvvioRelativo = true;
			AudioListener.volume = PlayerPrefs.GetFloat("volume globale");
		}
		if (PlayerPrefs.GetFloat("sensibilità aerei") == 0f)
		{
			PlayerPrefs.SetFloat("sensibilità aerei", 150f);
		}
		if (PlayerPrefs.GetFloat("sensibilità rotazione camera") == 0f)
		{
			PlayerPrefs.SetFloat("sensibilità rotazione camera", 100f);
		}
		if (!PlayerPrefs.HasKey("ordini aerei aria"))
		{
			PlayerPrefs.SetInt("ordini aerei aria", 1);
			PlayerPrefs.SetInt("ordini aerei terra", 1);
			PlayerPrefs.SetInt("ordini aerei bomb", 1);
		}
		if (!PlayerPrefs.HasKey("fattore diff fresh food"))
		{
			PlayerPrefs.SetFloat("fattore diff fresh food", 1f);
			PlayerPrefs.SetFloat("fattore diff rotten food", 1f);
			PlayerPrefs.SetFloat("fattore diff high protein food", 1f);
			PlayerPrefs.SetInt("fattore diff spawn gruppi", 1);
		}
		if (!PlayerPrefs.HasKey("durata battaglia"))
		{
			PlayerPrefs.SetInt("durata battaglia", 0);
		}
		if (!PlayerPrefs.HasKey("colore rosso unità"))
		{
			PlayerPrefs.SetFloat("colore rosso unità", 0.06f);
			PlayerPrefs.SetFloat("colore verde unità", 0.49f);
			PlayerPrefs.SetFloat("colore blu unità", 0.1f);
		}
		if (!MenuInizialeScript.filmatoFinito)
		{
			this.filmatoImmagine = this.oggettoFilmato.GetComponent<RawImage>();
			this.filmato = (this.oggettoFilmato.GetComponent<RawImage>().texture as MovieTexture);
			this.filmatoAudio = this.oggettoFilmato.GetComponent<AudioSource>();
			this.filmatoAudio.clip = this.filmato.audioClip;
			this.filmatoAudio.Play();
			this.filmato.Play();
		}
		int index = UnityEngine.Random.Range(0, this.ListaImmaginiSfondo.Count);
		this.sfondo.GetComponent<Image>().sprite = this.ListaImmaginiSfondo[index];
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x000F82B4 File Offset: 0x000F64B4
	private void Update()
	{
		if (!MenuInizialeScript.filmatoFinito)
		{
			this.timerFilmato += Time.deltaTime;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
			{
				this.timerFilmato = 10f;
			}
			if (this.timerFilmato > 7.3f)
			{
				this.oggettoFilmato.GetComponent<CanvasGroup>().interactable = false;
				this.oggettoFilmato.GetComponent<CanvasGroup>().blocksRaycasts = false;
				if (this.oggettoFilmato.GetComponent<CanvasGroup>().alpha > 0f)
				{
					this.oggettoFilmato.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * 4f;
				}
				else
				{
					this.oggettoFilmato.GetComponent<CanvasGroup>().alpha = 0f;
					MenuInizialeScript.filmatoFinito = true;
				}
			}
		}
		else
		{
			this.oggettoFilmato.GetComponent<CanvasGroup>().alpha = 0f;
			this.oggettoFilmato.GetComponent<CanvasGroup>().interactable = false;
			this.oggettoFilmato.GetComponent<CanvasGroup>().blocksRaycasts = false;
			if (!this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 0;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
			}
			else if (this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying && this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 0)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 0;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
			}
		}
	}

	// Token: 0x040019D2 RID: 6610
	private bool nonPrimoAvvioRelativo;

	// Token: 0x040019D3 RID: 6611
	public List<Sprite> ListaImmaginiSfondo;

	// Token: 0x040019D4 RID: 6612
	private GameObject sfondo;

	// Token: 0x040019D5 RID: 6613
	public GameObject oggettoFilmato;

	// Token: 0x040019D6 RID: 6614
	private RawImage filmatoImmagine;

	// Token: 0x040019D7 RID: 6615
	private MovieTexture filmato;

	// Token: 0x040019D8 RID: 6616
	private AudioSource filmatoAudio;

	// Token: 0x040019D9 RID: 6617
	public static bool filmatoFinito;

	// Token: 0x040019DA RID: 6618
	private float timerFilmato;

	// Token: 0x040019DB RID: 6619
	public GameObject oggettoMusicaPrefab;

	// Token: 0x040019DC RID: 6620
	private GameObject oggettoMusica;
}
