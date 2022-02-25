using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Models;

namespace Nop.Plugin.Widgets.MySlider.Services
{
    public class MySliderCustomerService:IMySliderCustomerService
    {
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<MySliderItem> _mysliderItemRepository;
        private readonly IRepository<MySlidersCustomerRole> _mySliderCustomerRoleRepository;
        private readonly IRepository<MySliders> _mysliderRepository;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CustomerCustomerRoleMapping> _customerRepository;

        public MySliderCustomerService(IRepository<MySliders> mysliderRepository,
            IRepository<Domains.MySlidersCustomerRole> mySliderCustomerRoleRepository,
            IRepository<MySliderItem> mysliderItemRepository,
            IStaticCacheManager cacheManager,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher,
            IRepository<CustomerCustomerRoleMapping> customerRepository)
        {
            _mysliderRepository = mysliderRepository;
            _mySliderCustomerRoleRepository = mySliderCustomerRoleRepository;
            _mysliderItemRepository = mysliderItemRepository;
            _catalogSettings = catalogSettings;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _customerRepository = customerRepository;
        }




        //public Domains.MySlidersCustomerRole GetSliderCustomerRoleById(int sliderId)
        //{
        //    var sliderCustomerRole = _mySliderCustomerRoleRepository.Table.Where(x => x.MySlidersId == sliderId);

        //    return sliderCustomerRole;
        //}

        public async Task InsertSliderCustomerAsync(Domains.MySlidersCustomerRole sliderCustomerRole)
        {
            if (sliderCustomerRole == null)
                throw new ArgumentNullException(nameof(sliderCustomerRole));

            await _mySliderCustomerRoleRepository.InsertAsync(sliderCustomerRole);

            await _eventPublisher.EntityInsertedAsync(sliderCustomerRole);
        }

        public async Task DeleteSliderCustomerAsync(Domains.MySlidersCustomerRole sliderCustomerRole)
        {
            if (sliderCustomerRole == null)
                throw new ArgumentNullException(nameof(sliderCustomerRole));

            await _mySliderCustomerRoleRepository.DeleteAsync(sliderCustomerRole);

            await _eventPublisher.EntityInsertedAsync(sliderCustomerRole);
        }

        public async Task UpdateSliderCustomerAsync(Domains.MySlidersCustomerRole sliderCustomerRole)
        {
            if (sliderCustomerRole == null)
                throw new ArgumentNullException(nameof(sliderCustomerRole));

            await _mySliderCustomerRoleRepository.UpdateAsync(sliderCustomerRole);

            await _eventPublisher.EntityInsertedAsync(sliderCustomerRole);
        }

        public async Task InsertCustomerRoleBySliderId(int id, IList<int> selectedCustomerRoleIds)
        {     

            var sliderCustomerRoles = _mySliderCustomerRoleRepository.Table.Where(x => x.MySlidersId == id).ToList();
            

            if (sliderCustomerRoles.Count() != selectedCustomerRoleIds.Count())
            {
                foreach (var sCR in sliderCustomerRoles)
                {
                    await _mySliderCustomerRoleRepository.DeleteAsync(sCR);
                }

                if (!selectedCustomerRoleIds.Contains(0))
                {
                    var sliderCustomerRole = new List<MySlidersCustomerRole>();

                    foreach (var value in selectedCustomerRoleIds)
                    {
                        sliderCustomerRole.Add(new MySlidersCustomerRole()
                        {
                            MySlidersId = id,
                            CustomerRoleId = value
                        });
                    }

                    foreach (var sCR in sliderCustomerRole)
                    {
                        await InsertSliderCustomerAsync(sCR);
                    }
                }
            }
        }

        public List<int> GetCustomerRoleBySliderId(int id)
        {
            var sliderCustomerRoles = _mySliderCustomerRoleRepository.Table.Where(x => x.MySlidersId == id).ToList();

            var list = new List<int>();
            if (sliderCustomerRoles.Count() != 0)
            {
                foreach (var sCR in sliderCustomerRoles)
                {
                    list.Add(sCR.CustomerRoleId);
                }
            }
            else
            {
                list.Add(0);
            }

            return list;

        }

        public IList<int> GetCurrentCustomerRoleIds(int currentCustomerId)
        {
            var customer = _customerRepository.Table.Where(x => x.CustomerId == currentCustomerId).ToList();

            var customerRoleId = new List<int>();

            foreach(var c in customer)
            {
                customerRoleId.Add(c.CustomerRoleId);
            }

            return customerRoleId;
        }

        
    }
}
