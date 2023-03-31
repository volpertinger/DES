namespace DES.DES
{
    interface IDES
    {
        abstract public bool Encrypt(string inputFilePath, string outputFilePath);

        abstract public bool Decrypt(string inputFilePath, string outputFilePath);
    }
}
