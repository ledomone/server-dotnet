namespace server_dotnet.Controllers.Validators
{
    public static class CommonRules
    {
        public static bool BeAValidDate(DateTime date)
        {
            return date <= DateTime.Now;
        }
    }
}
