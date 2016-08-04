namespace BC
{
    public enum Instruction : byte
    {
        // loadVars
        LdI = 0x01,
        LdF = 0x02,
        LdS = 0x03,
        LdB = 0x04,

        // aritmethic
        AddI = 0x08,

        // call stuff
        Call = 0x15,
        Print = 0x16,
        Pause = 0x17,
        Ret = 0x18,

        Local = 0x25,
    }
}