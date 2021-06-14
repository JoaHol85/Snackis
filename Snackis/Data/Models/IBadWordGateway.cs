using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public interface IBadWordGateway
    {
        Task<string> CheckForBadWords(string message);
        Task DeleteBadWordAsync(int id);
        Task<List<BadWord>> GetAllBadWordsAsync();
        Task<BadWord> PostBadWordAsync(BadWord badWord);
    }
}
