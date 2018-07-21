using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlockHealth : MonoBehaviour
{

    [SerializeField] bool destroy = false;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    GridNode gridNode { get; set; }

    Stack<GameObject> m_relayOn = new Stack<GameObject>();

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetGridBlock(GridNode gridNode)
    {
        this.gridNode = gridNode;
    }

    public void SetHealth(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth -= damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyWithDelay());
        }
    }
    IEnumerator DestroyWithDelay()
    {
        foreach (var entry in gridNode.requiredNeighbours)
        {
            foreach (var subEntry in entry.Value.ToArray())
            {
                if (subEntry != null)
                    subEntry.GetComponent<BlockHealth>().SetHealth(100);

                    yield return new WaitForSeconds(0.5f);

            }
        }
        DestroyGridBlock();

    }
    private void Update()
    {
        if (destroy)
        {
            SetHealth(100);
            destroy = false;
        }



    }
    void DestroyGridBlock()
    {
        Destroy(gameObject);
    }
}
