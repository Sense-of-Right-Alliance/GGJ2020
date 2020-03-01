using System;

[Serializable]
public class ShipBonusStack
{
    public float Amount = 1;
    public float Duration = 0;

    public ShipBonusStack(float amount, float duration)
    {
        this.Amount = amount;
        this.Duration = duration;
    }
}
