using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class StudentDetail
    {
        public long Id { get; set; }
        
        [Required]
        public long UserId { get; set; }
        
        public string Nipd { get; set; }
        
        public Gender? Gender { get; set; }
        
        public string Nisn { get; set; }
        
        public string BirthPlace { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public string Nik { get; set; }
        
        public string Religion { get; set; }
        
        public string Address { get; set; }
        
        public string Rt { get; set; }
        
        public string Rw { get; set; }
        
        public string Dusun { get; set; }
        
        public string Kelurahan { get; set; }
        
        public string Kecamatan { get; set; }
        
        public string PostalCode { get; set; }
        
        public string ResidenceType { get; set; }
        
        public string Transportation { get; set; }
        
        public string Phone { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Email { get; set; }
        
        public string Skhun { get; set; }
        
        public bool KpsRecipient { get; set; }
        
        public string KpsNumber { get; set; }
        
        public string ClassGroup { get; set; }
        
        public string UnNumber { get; set; }
        
        public string IjazahNumber { get; set; }
        
        public bool KipRecipient { get; set; }
        
        public string KipNumber { get; set; }
        
        public string KipName { get; set; }
        
        public string KksNumber { get; set; }
        
        public string BirthCertificateNo { get; set; }
        
        public string BankName { get; set; }
        
        public string BankAccountNumber { get; set; }
        
        public string BankAccountHolder { get; set; }
        
        public bool PipEligible { get; set; }
        
        public string PipEligibleReason { get; set; }
        
        public string SpecialNeeds { get; set; }
        
        public string PreviousSchool { get; set; }
        
        public int? ChildOrder { get; set; }
        
        public decimal? Latitude { get; set; }
        
        public decimal? Longitude { get; set; }
        
        public string KkNumber { get; set; }
        
        public decimal? Weight { get; set; }
        
        public decimal? Height { get; set; }
        
        public decimal? HeadCircumference { get; set; }
        
        public int? SiblingsCount { get; set; }
        
        public decimal? DistanceToSchool { get; set; }
        
        // Parent Information
        public string FatherName { get; set; }
        
        public int? FatherBirthYear { get; set; }
        
        public string FatherEducation { get; set; }
        
        public string FatherOccupation { get; set; }
        
        public string FatherIncome { get; set; }
        
        public string FatherNik { get; set; }
        
        public string MotherName { get; set; }
        
        public int? MotherBirthYear { get; set; }
        
        public string MotherEducation { get; set; }
        
        public string MotherOccupation { get; set; }
        
        public string MotherIncome { get; set; }
        
        public string MotherNik { get; set; }
        
        public string GuardianName { get; set; }
        
        public int? GuardianBirthYear { get; set; }
        
        public string GuardianEducation { get; set; }
        
        public string GuardianOccupation { get; set; }
        
        public string GuardianIncome { get; set; }
        
        public string GuardianNik { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public User User { get; set; }
    }
} 