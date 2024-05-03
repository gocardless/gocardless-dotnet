namespace GoCardless.Internals
{
    public interface IHasIdempotencyKey
    {
        string IdempotencyKey { get; set; }
    }
}
