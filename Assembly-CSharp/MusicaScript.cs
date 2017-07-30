using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class MusicaScript : MonoBehaviour
{
	// Token: 0x06000862 RID: 2146 RVA: 0x00126FEC File Offset: 0x001251EC
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.musica = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00127008 File Offset: 0x00125208
	private void Start()
	{
		this.volumeIniziale = 0.8f;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00127018 File Offset: 0x00125218
	private void Update()
	{
		if (this.attivitàInMusica)
		{
			this.attivitàInMusica = false;
			this.musica.clip = this.ListaMusiche[this.numeroMusica];
			this.musica.Play();
		}
	}

	// Token: 0x04001FA8 RID: 8104
	public AudioSource musica;

	// Token: 0x04001FA9 RID: 8105
	public List<AudioClip> ListaMusiche;

	// Token: 0x04001FAA RID: 8106
	public bool attivitàInMusica;

	// Token: 0x04001FAB RID: 8107
	public int numeroMusica;

	// Token: 0x04001FAC RID: 8108
	private float volumeIniziale;
}
