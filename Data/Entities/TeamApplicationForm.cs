using System.ComponentModel.DataAnnotations;

namespace Tournament.Data.Entities
{
    public class TeamApplicationForm : BaseEntity
    {
        [Required(ErrorMessage = "Takım adı zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Takım adı en az 3 en fazla 100 karakter olmalıdır.")]
        public string Teamname { get; set; }

        [Required(ErrorMessage = "Kurum adı zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Kurum adı en az 3 en fazla 100 karakter olmalıdır.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        public int FileId { get; set; }

        public FileDocument? File { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Takım boyutu en az 10 olmalıdır.")]
        public int TeamSize { get; set; }

        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();


        public bool Disclaimer { get; set; }

        public string? PhoneNumber { get; set; }
        public bool isMailSendSuccess { get; set; }
    }

    public class TeamMember : BaseEntity
    {
        [Required(ErrorMessage = "Tc zorunludur.")]

        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC kimlik numarası tam olarak 11 karakter olmalıdır.")]
        public string? TC { get; set; }

        [Required(ErrorMessage = "İsim zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İsim en az 2 en fazla 50 karakter olmalıdır.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyisim en az 2 en fazla 50 karakter olmalıdır.")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "İletişim bilgisi zorunludur.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "İletişim bilgisi en az 5 en fazla 20 karakter olmalıdır.")]
        public string? Iletisim { get; set; }

        public bool? isCaptain { get; set; }

        public string? Captain { get; set; }

        public int? TeamApplicationFormId { get; set; }

        public TeamApplicationForm? TeamApplicationForm { get; set; }
    }
}
