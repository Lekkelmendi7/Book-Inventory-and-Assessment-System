namespace BookInventory.BusinessLogicAcessLayer.Models.AccountModels
{
    public class AuthenticationResponseModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
