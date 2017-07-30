using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000102 RID: 258
public class CaricamentoDaScenaCaricamento : MonoBehaviour
{
	// Token: 0x0600082B RID: 2091 RVA: 0x0011BEDC File Offset: 0x0011A0DC
	private void Start()
	{
		this.scritta = GameObject.FindGameObjectWithTag("CanvasPerCaricam").transform.FindChild("sfondo scritta").GetChild(0).gameObject;
		this.sfondo = GameObject.FindGameObjectWithTag("CanvasPerCaricam").transform.FindChild("sfondo immagine").gameObject;
		this.oggettoMusica = GameObject.FindGameObjectWithTag("Musica");
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		int index = UnityEngine.Random.Range(0, this.ListaConsigli.Count);
		this.scritta.GetComponent<Text>().text = this.ListaConsigli[index].GetComponent<Text>().text;
		GestoreNeutroStrategia.valoreRandomSeed++;
		UnityEngine.Random.InitState(GestoreNeutroStrategia.valoreRandomSeed);
		index = UnityEngine.Random.Range(0, this.ListaImmagini.Count);
		this.sfondo.GetComponent<Image>().sprite = this.ListaImmagini[index];
		if (this.oggettoMusica)
		{
			if (!this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 1;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
			}
			else if (this.oggettoMusica.GetComponent<MusicaScript>().musica.isPlaying && this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica != 1)
			{
				this.oggettoMusica.GetComponent<MusicaScript>().numeroMusica = 1;
				this.oggettoMusica.GetComponent<MusicaScript>().attivitàInMusica = true;
			}
		}
		SceneManager.LoadScene(CaricaScene.nomeScenaDaCaricare);
	}

	// Token: 0x04001E59 RID: 7769
	private GameObject scritta;

	// Token: 0x04001E5A RID: 7770
	private GameObject sfondo;

	// Token: 0x04001E5B RID: 7771
	public List<GameObject> ListaConsigli;

	// Token: 0x04001E5C RID: 7772
	public List<Sprite> ListaImmagini;

	// Token: 0x04001E5D RID: 7773
	private GameObject oggettoMusica;
}
