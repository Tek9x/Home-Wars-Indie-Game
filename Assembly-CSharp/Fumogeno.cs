using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class Fumogeno : MonoBehaviour
{
	// Token: 0x06000670 RID: 1648 RVA: 0x000E5788 File Offset: 0x000E3988
	private void Start()
	{
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x000E5798 File Offset: 0x000E3998
	private void Update()
	{
		this.timerVita += Time.deltaTime;
		this.EffettoFumo();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x000E57B4 File Offset: 0x000E39B4
	private void EffettoFumo()
	{
		if (this.ListaAreaFumo.Count > 0)
		{
			for (int i = 0; i < this.ListaAreaFumo.Count; i++)
			{
				if (this.ListaAreaFumo[i] == null && i < this.ListaAreaFumo.Count - 1)
				{
					this.ListaAreaFumo[i] = this.ListaAreaFumo[i + 1];
					this.ListaAreaFumo[i + 1] = null;
				}
			}
			for (int j = 0; j < this.ListaAreaFumo.Count; j++)
			{
				if (this.ListaAreaFumo[j] == null)
				{
					this.ListaAreaFumo.RemoveAt(j);
				}
			}
			foreach (GameObject current in this.ListaAreaFumo)
			{
				if (current != null && current.GetComponent<NavigazioneConCamminata>())
				{
					current.GetComponent<NavigazioneConCamminata>().rallDaTrappola = true;
					current.GetComponent<NavigazioneConCamminata>().effettoRallDaTrappola = 0.4f;
				}
			}
		}
		if (this.timerVita > 20f)
		{
			foreach (GameObject current2 in this.ListaAreaFumo)
			{
				if (current2 != null && current2.GetComponent<NavigazioneConCamminata>())
				{
					current2.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x000E59A4 File Offset: 0x000E3BA4
	private void OnTriggerEnter(Collider altri)
	{
		if (altri.tag == "Nemico" && !this.ListaAreaFumo.Contains(altri.gameObject))
		{
			this.ListaAreaFumo.Add(altri.gameObject);
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x000E59F0 File Offset: 0x000E3BF0
	private void OnTriggerExit(Collider altri)
	{
		if (altri.tag == "Nemico" && this.ListaAreaFumo.Contains(altri.gameObject))
		{
			this.ListaAreaFumo.Remove(altri.gameObject);
			if (altri.gameObject != null && altri.gameObject.GetComponent<NavigazioneConCamminata>())
			{
				altri.gameObject.GetComponent<NavigazioneConCamminata>().rallDaTrappola = false;
			}
		}
	}

	// Token: 0x04001805 RID: 6149
	public List<GameObject> ListaAreaFumo;

	// Token: 0x04001806 RID: 6150
	private float timerVita;
}
