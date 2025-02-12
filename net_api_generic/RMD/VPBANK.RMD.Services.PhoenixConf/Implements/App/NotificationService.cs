using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Entities.POCOs.App;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.App;
using VPBANK.RMD.Services.PhoenixConf.Interfaces.App;

namespace VPBANK.RMD.Services.PhoenixConf.Implements.App
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork<PhoenixConfContext> _unitOfWork;
        private readonly ISubscriberInfoRepository _subcriberRepository;
        private readonly INotificationCountRepository _notiCountrepository;
        private readonly IGenericRepository<PhoenixConfContext, SubscriberInfo, decimal> _subsGenericRepository;

        public NotificationService(IUnitOfWork<PhoenixConfContext> unitOfWork,
            ISubscriberInfoRepository subcriberRepository, INotificationCountRepository notiCountrepository,
            IGenericRepository<PhoenixConfContext, SubscriberInfo, decimal> subsGenericRepository)
        {
            _unitOfWork = unitOfWork;
            _subcriberRepository = subcriberRepository;
            _notiCountrepository = notiCountrepository;
            _subsGenericRepository = subsGenericRepository;
        }

        public int GetNotiCount(string username)
        {
            try
            {
                var notiCount = _notiCountrepository.FindFirstByUsername(username);
                return notiCount != null ? notiCount.Quantity : 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task ResetNotiCountAsync(string username)
        {
            try
            {
                var notiCount = _notiCountrepository.FindFirstByUsername(username);
                if (notiCount == null)
                {
                    _notiCountrepository.Insert(new NotificationCount
                    {
                        Pk_Id = 0,
                        Username = username,
                        Quantity = 0
                    });
                }
                else
                {
                    notiCount.Quantity += 1;
                    _notiCountrepository.Update(notiCount);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public void IncrementCount(string username)
        {
            try
            {
                var notiCount = _notiCountrepository.FindFirstByUsername(username);
                if (notiCount == null)
                {
                    notiCount = new NotificationCount
                    {
                        Pk_Id = 0,
                        Username = username,
                        Quantity = 1
                    };
                    _notiCountrepository.Insert(notiCount);
                }
                else
                {
                    notiCount.Quantity += 1;
                    _notiCountrepository.Update(notiCount);
                }
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public void IncrementCount(List<string> usernames)
        {
            try
            {
                foreach (var username in usernames)
                {
                    var notiCount = _notiCountrepository.FindFirstByUsername(username);
                    if (notiCount == null)
                    {
                        notiCount = new NotificationCount
                        {
                            Pk_Id = 0,
                            Username = username,
                            Quantity = 1
                        };
                        _notiCountrepository.Insert(notiCount);
                    }
                    else
                    {
                        notiCount.Quantity += 1;
                        _notiCountrepository.Update(notiCount);
                    }
                }

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task IncrementCountAsync(string username)
        {
            try
            {
                var notiCount = _notiCountrepository.FindFirstByUsername(username);
                if (notiCount == null)
                {
                    notiCount = new NotificationCount
                    {
                        Pk_Id = 0,
                        Username = username,
                        Quantity = 1
                    };
                    _notiCountrepository.Insert(notiCount);
                }
                else
                {
                    notiCount.Quantity += 1;
                    _notiCountrepository.Update(notiCount);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task AddSubscriberAsync(string username, string routingKey)
        {
            try
            {
                var subscriber = new SubscriberInfo
                {
                    Pk_Id = 0,
                    Username = username,
                    Routing_Key = routingKey
                };

                _subcriberRepository.Insert(subscriber);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task RemoveSubscriberAsync(string username)
        {
            try
            {
                var subscribers = _subcriberRepository.FindAllSubscriberByUsername(username);
                if (subscribers == null || subscribers.Count == 0) return;
                var data = new List<object>();
                data.AddRange(subscribers);
                await _subsGenericRepository.BulkDeleteAsync(data);
                //await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task RemoveSubscriberAsync(string username, string routingKey)
        {
            try
            {
                var subscribers = _subcriberRepository.FindAllSubscriberByUsernameAndRoutingKey(username, routingKey);
                if (subscribers == null || subscribers.Count == 0) return;
                var data = new List<object>();
                data.AddRange(subscribers);
                await _subsGenericRepository.BulkDeleteAsync(data);
                //await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.Message);
                throw;
            }
        }

        public List<string> FindAllRoutingKeyByUsername(string username)
        {
            var subcs = _subcriberRepository
                .FindAllSubscriberByUsername(username)
                .Select(c => c.Routing_Key)
                .ToList();
            return subcs;
        }

        public List<string> FindAllSubscribersByRoutekey(string routingKey)
        {
            var subcs = _subcriberRepository
                .FindAllSubscriberByRoutingKey(routingKey)
                .Select(c => c.Username)
                .ToList();
            return subcs;
        }

        public bool IsMatchSubscribe(SubscriberInfo subscriberInfo)
        {
            throw new NotImplementedException();
        }
    }
}
