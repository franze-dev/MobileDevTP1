using System.Collections.Generic;
using UnityEngine;

public class EstadisticasUI : MonoBehaviour
{
    //cantidad de bolsas recogidas
    private int CantBolsasMax = 3;

    //cooldown para el flash entre verde y azul
    private float CooldownFlash = 0.2f;
    //dinero ganado
    //jugador correspondiente
    [SerializeField] private Player Camion;

    [SerializeField] private List<GameObject> ProgresoBolsas;
    [SerializeField] private GameObject FinalBolsas;
    private float TimerFlash = 0f;

    private void Awake()
    {
        CantBolsasMax = Camion.Bolsas.Length;
    }

    private void Update()
    {
        if (Camion.CantBolsAct == CantBolsasMax)
        {
            if (TimerFlash >= CooldownFlash)
            {
                TimerFlash = 0f;
                FinalBolsas.SetActive(!FinalBolsas.activeSelf);
            }

            TimerFlash += Time.deltaTime;
        }
        else
            FinalBolsas.SetActive(false);

        if (Camion.CantBolsAct > 0)
        {
            ProgresoBolsas[Camion.CantBolsAct - 1].SetActive(false);
            ProgresoBolsas[Camion.CantBolsAct].SetActive(true);
        }
        else
        {
            if (!ProgresoBolsas[0].activeSelf)
            {
                foreach (var item in ProgresoBolsas)
                {
                    item.SetActive(false);
                }
                ProgresoBolsas[0].SetActive(true);
            }
        }
    }

}
