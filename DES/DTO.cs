#pragma warning disable CS8618
namespace DES
{
    public class Settings
    {
        public ulong Key { get; set; }
        public AtomicOperation[] Operations { get; set; }
    }
    public class AtomicOperation
    {
        public string PathInput { get; set; }
        public string PathOutput { get; set; }

        public string Operation { get; set; }
    }

    public static class Operations
    {
        public const string Encrypt = "Encrypt";
        public const string Decrypt = "Decrypt";
    }
}
#pragma warning restore CS8618
