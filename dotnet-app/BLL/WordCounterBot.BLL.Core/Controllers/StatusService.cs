using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class StatusService
    {
        private readonly ICounterDao _counterDao;

        public StatusService(ICounterDao counterDao)
        {
            _counterDao = counterDao;
        }

        public async Task<long> GetTotalWords()
        {
            return await _counterDao.GetTotalCount();
        }
    }
}
