namespace MinimalApi.Domain.ModelViews
{
    public struct Home
    {
        public string Message
        {
            get => "Welcome to the Vehicle API - Minimal API";
        }
        public string Doc
        {
            get => "/swagger";
        }
    }
}
