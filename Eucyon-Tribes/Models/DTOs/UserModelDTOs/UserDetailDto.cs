namespace Eucyon_Tribes.Models.UserModels
{
    public class UserDetailDto
    {
        public int ID { get; }
        public string Name { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public int KingdomID { get; }
        public int LocationID { get; }
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public DateTime CreatedDate { get; }
        public DateTime VerifiedAt { get; }
        public int WorldID { get; }
        public string KingdomName { get; }
        public string VerificationToken { get; set; } = null!;
        public string ForgottenPasswordToken { get; set; } = null!;

        public UserDetailDto(int iD, string name, string email, string passwordHash,
            int kingdomID, int locationID, int xCoordinate, int yCoordinate,
            DateTime createdDate, DateTime verifiedAt, int worldID, string kingdomName
            , string verificationToken, string forgottenPasswordToken)
        {
            ID = iD;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            KingdomID = kingdomID;
            LocationID = locationID;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            CreatedDate = createdDate;
            VerifiedAt = verifiedAt;
            WorldID = worldID;
            KingdomName = kingdomName;
            VerificationToken = verificationToken;
            ForgottenPasswordToken = forgottenPasswordToken;    
        }
    }
}
