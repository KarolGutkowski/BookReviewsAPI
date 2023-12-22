using BookReviews.Domain.Models.DataModels;

namespace BookReviews.Domain.Models.DTOs.ExposedDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<BookDTO> LikedBooks { get; set; } = new List<BookDTO>();
        public string? ProfileImage { get; set; }

        public UserDTO(User user, bool mapLikedBooks)
        {
            Id = user.Id;
            UserName = user.UserName;
            ProfileImage = user.ProfileImage;
            if(mapLikedBooks && user.LikedBooks is not null)
            {
                foreach(var likedBook in user.LikedBooks)
                {
                    LikedBooks.Add(new BookDTO(likedBook,mapAuthors: false,mapReviews: false,mappedLikedByUsers: false));
                }
            }

        }
    }
}
