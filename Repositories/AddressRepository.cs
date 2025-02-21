using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Data;
using MormorDagnysDel2.Entities;
using MormorDagnysDel2.Helpers;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Address;

namespace MormorDagnysDel2.Repositories;

public class AddressRepository(DataContext context) : IAddressRepository
{
    private readonly DataContext _context = context;

    public async Task<Address> Add(AddressPostViewModel model)
    {
        var postalAddress = await _context.PostalAddresses.FirstOrDefaultAsync(
              c => c.PostalCode.Replace(" ", "").Trim() == model.PostalCode.Replace(" ", "").Trim());

        if (postalAddress is null)
        {
            postalAddress = new PostalAddress
            {
                PostalCode = model.PostalCode.Replace(" ", "").Trim(),
                City = model.City.Trim()
            };
            await _context.PostalAddresses.AddAsync(postalAddress);
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(
          c => c.AddressLine.Trim().ToLower() == model.AddressLine.Trim().ToLower());

        if (address is null)
        {
            address = new Address
            {
                AddressLine = model.AddressLine,
                AddressTypeId = (int)model.AddressType,
                PostalAddress = postalAddress
            };

            await _context.Addresses.AddAsync(address);
        }

        if (_context.ChangeTracker.HasChanges())
        {
            await _context.SaveChangesAsync();
        }

        return address;
    }

    public async Task<bool> Add(string type)
    {
        {
            var exists = await _context.AddressTypes.FirstOrDefaultAsync(c => c.Value.ToLower()
              == type.ToLower());

            if (exists is not null) throw new DagnysException($"Adress typen {type} finns redan");

            try
            {
                var newType = new AddressType
                {
                    Value = type
                };

                await _context.AddressTypes.AddAsync(newType);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}