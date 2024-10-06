namespace PE.Core.Commons
{
    public sealed class JwtSettings
    {
        /// <summary>
        /// Issuer of the token
        /// </summary>
        public string Issuer { get; set; } = null!;

        /// <summary>
        /// Audience of the token
        /// </summary>
        public string Audience { get; set; } = null!;

        /// <summary>
        /// Signature key
        /// </summary>
        public string Key { get; set; } = null!;
        
        /// <summary>
        /// Token's lifetimw
        /// </summary>
        public int DurationInMinutes { get; set; }
    }
}
