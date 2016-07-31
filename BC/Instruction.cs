namespace BC
{
    public enum Instruction : byte
    {
        // loadVars
        ld_i = 0x01,
        ld_f = 0x02,
        ld_s = 0x03,

        // aritmethic
        add_i = 0x08,

        // call stuff
        call = 0x15,
        print = 0x16,
        pause = 0x17,
        ret = 0x18,
        
    }
}