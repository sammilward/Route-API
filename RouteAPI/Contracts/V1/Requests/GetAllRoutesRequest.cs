namespace RouteAPI.Contracts.V1.Requests
{
    public class GetAllRoutesRequest
    {
        public string FriendId { get; set; }
        public bool? Popular { get; set; }
    }
}
