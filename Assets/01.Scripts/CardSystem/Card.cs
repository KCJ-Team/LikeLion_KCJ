using UnityEngine;

[System.Serializable]
public class Card
{
    public string Name;
    public int Id = -1;

    public Card()
    {
        Name = "";
        Id = -1;
    }

    public Card(CardObject card)
    {
        Name = card.name;
        Id = card.data.Id;
    }

}