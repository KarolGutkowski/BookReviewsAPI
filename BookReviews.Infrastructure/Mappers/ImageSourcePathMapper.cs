using BookReviews.Domain.Models.DataModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Infrastructure.Mappers
{
    public class ImageSourcePathMapper
    {
        private readonly IConfiguration _configuration;
        public ImageSourcePathMapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void MapBookImageSourceToEndpointPath(Book book)
        {
            if (book.Img is null)
                book.Img = "placeholder";

            var fileName = book.Img + ".jpeg";

            book.Img = MapFileNameToEndpointDataPath(fileName);
        }

        private string MapFileNameToEndpointDataPath(string fileName)
        {
            return _configuration.GetSection("ImageEndpointPrefix").Value + fileName;
        }

        public void MapUsersLikedBooksSourcePahts(User user)
        {
            if (user.LikedBooks is null)
                return;

            foreach(var book in user.LikedBooks)
            {
                MapBookImageSourceToEndpointPath(book);
            }
        }

        public void MapUserProfilePictureToPath(User user)
        {
            if (string.IsNullOrEmpty(user.ProfileImage))
                user.ProfileImage = "male.png";

            user.ProfileImage = MapFileNameToEndpointDataPath(user.ProfileImage);
        }

        public string MapToServerPath(string fileName)
        {
            return $"./Resources/Images/{fileName}";
        }
    }
}
