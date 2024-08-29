using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.BusinessLogicAcessLayer.Services.PermissionService
{
    public class PermissionService : IPermissionService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public PermissionService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreatePermisssion(PermissionCreateModel permission)
        {
            var permissionModel = new Permission
            {
                Name = permission.Name,
                RolePermissions = new List<RolePermission>()
            };

            if (permissionModel == null)
            {
                return;
            }

            _context.Permissions.Add(permissionModel);
            await _context.SaveChangesAsync(); // Save first to generate the PermissionId

            if (permission.RoleIds != null && permission.RoleIds.Any())
            {
                foreach (var role in permission.RoleIds)
                {
                    var rolePermission = new RolePermission
                    {
                        PermissionId = permissionModel.Id,
                        RoleId = role
                    };
                    _context.RolePermissions.Add(rolePermission);
                }
                await _context.SaveChangesAsync(); // Save the RolePermissions after the PermissionId is set
            }
        }


        public async Task DeletePermission(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return;
            }
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<PermissionGetModel>> GetAllPermissions(int page, int size)
        {
            if (size > PaginationModel.MaxPageSize)
            {
                size = PaginationModel.MaxPageSize; // Ensure page size does not exceed max limit
            }

            try
            {
               

                // Define the query
                var queryable = _context.Permissions
                    .Include(p => p.RolePermissions)
                    .ThenInclude(rp => rp.Role)
                    .AsQueryable();

                // Apply pagination
                var paginatedPermissions = queryable.Paginate(page, size);

                // Map the results
                var mappedPermissions = _mapper.Map<IEnumerable<PermissionGetModel>>(paginatedPermissions.Items);

               

                return new PaginatedResult<PermissionGetModel>
                {
                    TotalItems = paginatedPermissions.TotalItems,
                    Items = mappedPermissions,
                    TotalPages = paginatedPermissions.TotalPages,
                    CurrentPage = paginatedPermissions.CurrentPage,
                    HasPreviousPage = paginatedPermissions.HasPreviousPage,
                    HasNextPage = paginatedPermissions.HasNextPage,
                    FirstPage = paginatedPermissions.FirstPage,
                    LastPage = paginatedPermissions.LastPage
                };
            }
            catch (Exception ex)
            {
               
                throw new Exception("An error occurred while trying to fetch permissions.", ex);
            }
        }


        public async Task UpdatePermission(int id, PermissionUpdateModel permission)
        {
            var permissionModel = await _context.Permissions
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (permissionModel == null)
            {
                return;
            }

            permissionModel.Name = permission.Name;

            if (permissionModel.RolePermissions == null)
            {
                permissionModel.RolePermissions = new List<RolePermission>();
            }

            _context.RolePermissions.RemoveRange(permissionModel.RolePermissions);

            if (permission.RoleIds != null && permission.RoleIds.Any())
            {
                foreach (var role in permission.RoleIds)
                {
                    var rolePermission = new RolePermission
                    {
                        PermissionId = permissionModel.Id,
                        RoleId = role
                    };
                    _context.RolePermissions.Add(rolePermission);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
