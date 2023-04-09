namespace Medilive.Assessment.Affiliate.Dto
{
    public class Response<T>
    {
        public T Data { get; set; }
        public  bool IsSuccessful { get; set; }
        public List<ResponseMessageDto> Messages { get; set; }
    }
}
