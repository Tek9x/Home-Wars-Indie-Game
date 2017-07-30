using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class ColliderLimitiStanze : MonoBehaviour
{
	// Token: 0x0600083E RID: 2110 RVA: 0x00121250 File Offset: 0x0011F450
	private void OnTriggerEnter(Collider collisore)
	{
		if (collisore.gameObject.layer == 14)
		{
			if (collisore.GetComponent<PresenzaAlleato>().tipoTruppa != 32)
			{
				if (!collisore.GetComponent<PresenzaAlleato>().èAereo)
				{
					collisore.GetComponent<PresenzaAlleato>().vita = 0f;
				}
				else
				{
					collisore.GetComponent<PresenzaAlleato>().ritornoEffettuato = true;
				}
			}
		}
		else if (collisore.gameObject.layer == 10)
		{
			if (collisore.GetComponent<Collider>().tag != "ObbiettivoTattico")
			{
				collisore.GetComponent<PresenzaNemico>().vita = 0f;
			}
		}
		else if (collisore.gameObject.layer == 12)
		{
			UnityEngine.Object.Destroy(collisore.gameObject);
		}
	}
}
