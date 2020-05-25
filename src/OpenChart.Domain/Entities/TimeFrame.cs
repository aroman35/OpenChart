namespace OpenChart.Domain.Entities
{
    public enum TimeFrame
    {
        Undefined = 0,

        M1 = 60,

        M5 = 300,

        M15 = 900,

        M30 = 1_800,

        H1 = 3_600,

        H4 = 14_400,

        D = 86_400,

        W = 604_800,

        MN = 259_2000,
    }
}