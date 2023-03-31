namespace DES.DES
{
    sealed class DES : IDES
    {
        // ------------------------------------------------------------------------------------------------------------
        // fields
        // ------------------------------------------------------------------------------------------------------------
        private ulong Key { get; set; }

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public DES(ulong key)
        {
            Key = key & Constants.keyMask;
        }

        public bool Encrypt(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public bool Decrypt(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private uint GetLeftPart(ulong number)
        {
            return (uint)(number & Constants.leftMask);
        }

        private uint GetRightPart(ulong number)
        {
            return (uint)(number & Constants.RightMask);
        }

    }
}
