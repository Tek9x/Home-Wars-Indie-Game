using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class AreaFiloSpinato : MonoBehaviour
{
	// Token: 0x060007FE RID: 2046 RVA: 0x00119E9C File Offset: 0x0011809C
	private void Start()
	{
		this.tempoFreqDanno = base.GetComponent<PresenzaTrappola>().frequenzaAttacco;
		this.suonoArea = base.GetComponent<AudioSource>();
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00119EBC File Offset: 0x001180BC
	private void Update()
	{
		if (base.GetComponent<PresenzaTrappola>().disposta)
		{
			this.EffettoFiloSpinato();
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00119ED4 File Offset: 0x001180D4
	private void EffettoFiloSpinato()
	{
		if (this.ListaAreaTrappola.Count > 0)
		{
			for (int i = 0; i < this.ListaAreaTrappola.Count; i++)
			{
				if (this.ListaAreaTrappola[i] == null && i < this.ListaAreaTrappola.Count - 1)
				{
					this.ListaAreaTrappola[i] = this.ListaAreaTrappola[i + 1];
					this.ListaAreaTrappola[i + 1] = null;
				}
			}
			for (int j = 0; j < this.ListaAreaTrappola.Count; j++)
			{
				if (this.ListaAreaTrappola[j] == null)
				{
					this.ListaAreaTrappola.RemoveAt(j);
				}
			}
			if (!this.partenzaSuono)
			{
				this.suonoArea.Play();
				this.partenzaSuono = true;
			}
			this.timerDannoFilo += Time.deltaTime;
			foreach (GameObject current in this.ListaAreaTrappola)
			{
				if (current != null)
				{
					if (current.GetComponent<NavigazioneConCamminata>())
					{
						current.GetComponent<NavigazioneConCamminata>().rallDaTrappola = true;
						current.GetComponent<NavigazioneConCamminata>().effettoRallDaTrappola = base.GetComponent<PresenzaTrappola>().percDiRallentamento;
					}
					if (this.timerDannoFilo > this.tempoFreqDanno)
					{
						float num = current.GetComponent<PresenzaNemico>().armatura - base.GetComponent<PresenzaTrappola>().penetrazione;
						if (num <= 0f)
						{
							current.GetComponent<PresenzaNemico>().vita -= base.GetComponent<PresenzaTrappola>().danno;
						}
						else
						{
							float num2 = num - base.GetComponent<PresenzaTrappola>().danno;
							if (num2 < 0f)
							{
								current.GetComponent<PresenzaNemico>().vita += num2;
							}
						}
						base.GetComponent<PresenzaTrappola>().vita -= base.GetComponent<PresenzaTrappola>().dannoSubitoPerAzione;
					}
				}
			}
			if (this.timerDannoFilo > this.tempoFreqDanno)
			{
				this.timerDannoFilo = 0f;
			}
		}
		else
		{
			this.suonoArea.Stop();
			this.partenzaSuono = false;
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0011A138 File Offset: 0x00118338
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Add(altri.gameObject);
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0011A184 File Offset: 0x00118384
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaAreaTrappola.Contains(altri.gameObject))
		{
			this.ListaAreaTrappola.Remove(altri.gameObject);
			if (altri.gameObject != null && altri.gameObject.GetComponent<NavigazioneConCamminata>())
			{
				altri.gameObject.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
			}
		}
	}

	// Token: 0x04001E08 RID: 7688
	public List<GameObject> ListaAreaTrappola;

	// Token: 0x04001E09 RID: 7689
	private AudioSource suonoArea;

	// Token: 0x04001E0A RID: 7690
	private bool partenzaSuono;

	// Token: 0x04001E0B RID: 7691
	private float timerDannoFilo;

	// Token: 0x04001E0C RID: 7692
	private float tempoFreqDanno;
}
