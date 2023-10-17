using BookReviews.Domain.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Domain.Models
{
    public interface IUsersRepository
    {
        DbSet<User> Users { get; set; }
        void FinishRegistration();
    }
}
