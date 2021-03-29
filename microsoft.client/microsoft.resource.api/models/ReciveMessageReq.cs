using System.ComponentModel.DataAnnotations;

namespace microsoft.resource.api.models
{ 
    public class SendMessageReq
    {
        [Required]
        [Phone]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public string PhoneNumberTo { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address; instead of sending the empty field set it to null")]
        public string Mail { get; set; }
        public string campaing { get; set; }
        [Required]
        [Phone]
        [StringLength(maximumLength:20, MinimumLength =5)]
        public string PhoneNumber { get; set; }
    }
}
